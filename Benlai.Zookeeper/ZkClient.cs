using System;
using System.Collections.Concurrent;
using System.Collections.Generic;
using System.Threading;
using System.Linq;
using Org.Apache.Zookeeper.Data;
using ZooKeeperNet;
using Benlai.Zookeeper.Interface;
using Benlai.Zookeeper.Exceptions;
using Benlai.Zookeeper.Utils;
using Benlai.Zookeeper.Extensions;
using Benlai.Common;

namespace Benlai.Zookeeper
{
    public class ZkClient : IWatcher, IDisposable
    {
        private static readonly LogInfoWriter Logger = LogInfoWriter.GetInstance("ZookeeperLog");

        protected IZkConnection _connection;

        /// <summary>
        /// ZK子连接的监听列表
        /// </summary>
        private readonly IDictionary<string, ConcurrentHashSet<IZkChildListener>> _childListener = new ConcurrentDictionary<string, ConcurrentHashSet<IZkChildListener>>();

        /// <summary>
        /// zk数据监听列表
        /// </summary>
        private readonly ConcurrentDictionary<String, ConcurrentHashSet<IZkDataListener>> _dataListener = new ConcurrentDictionary<string, ConcurrentHashSet<IZkDataListener>>();

        /// <summary>
        /// zk状态连接监听列表
        /// </summary>
        private readonly ConcurrentHashSet<IZkStateListener> _stateListener = new ConcurrentHashSet<IZkStateListener>();

        private KeeperState _currentState;

        /// <summary>
        /// Zookeeper当前状态
        /// </summary>
        public KeeperState CurrentState
        {
            get
            {
                return _currentState;
            }

            set
            {
                this.EventLock.Lock();
                try
                {
                    this._currentState = value;
                }
                finally
                {
                    this.EventLock.Unlock();
                }
            }
        }

        public ZkLock EventLock { get; private set; }

        public bool ShutdownTrigger { get; set; }

        private ZkEventThread _eventThread;

        private Thread _zookeeperEventThread;

        public IZkSerializer ZkSerializer { get; set; }

        public ZkClient(string zkServers, int sessionTimeout, int connectionTimeout, IZkSerializer zkSerializer)
            : this(new ZkConnection(zkServers, TimeSpan.FromMilliseconds(sessionTimeout)), connectionTimeout, zkSerializer)
        {
        }

        public ZkClient(IZkConnection connection)
            : this(connection, int.MaxValue)
        {
        }

        public ZkClient(IZkConnection connection, int connectionTimeout)
            : this(connection, connectionTimeout, null)
        {
        }

        public ZkClient(IZkConnection zkConnection, int connectionTimeout, IZkSerializer zkSerializer)
        {
            this._connection = zkConnection;
            this.ZkSerializer = zkSerializer;
            this.EventLock = new ZkLock();
            this.Connect(connectionTimeout, this);
        }

        /// <summary>
        /// 订阅指定路径下的子路径的变化
        /// </summary>
        /// <param name="path">待监听的nodePath</param>
        /// <param name="childListener">子路径的监听实例</param>
        /// <returns></returns>
        public IEnumerable<string> SubscribeChildChanges(string path, IZkChildListener childListener)
        {
            lock (_childListener)
            {
                ConcurrentHashSet<IZkChildListener> listeners = _childListener.Get(path);

                if (listeners == null)
                {
                    listeners = new ConcurrentHashSet<IZkChildListener>();
                    _childListener[path] = listeners;
                }

                listeners.Add(childListener);
            }

            return WatchForChilds(path);
        }

        /// <summary>
        /// 取消订阅指定路径下的子路径的变化
        /// </summary>
        /// <param name="path">待监听的nodePath</param>
        /// <param name="childListener">子路径的监听实例</param>
        public void UnsubscribeChildChanges(string path, IZkChildListener childListener)
        {
            lock (_childListener)
            {
                ConcurrentHashSet<IZkChildListener> listeners = _childListener.Get(path);
                if (listeners != null)
                {
                    listeners.TryRemove(childListener);
                }
            }
        }

        /// <summary>
        /// 订阅指定路径的数据变化
        /// </summary>
        /// <param name="path">待监听的nodePath</param>
        /// <param name="dataListener">指定路径的监听实例</param>
        public void SubscribeDataChanges(string path, IZkDataListener dataListener)
        {
            ConcurrentHashSet<IZkDataListener> listeners;
            lock (_dataListener)
            {
                listeners = _dataListener.Get(path);
                if (listeners == null)
                {
                    listeners = new ConcurrentHashSet<IZkDataListener>();
                    _dataListener[path] = listeners;
                }

                listeners.Add(dataListener);
            }
            WatchForData(path);

            System.Diagnostics.Debug.WriteLine("Subscribed Data changes for " + path);
        }

        /// <summary>
        /// 取消订阅指定路径的数据变化
        /// </summary>
        /// <param name="path">待监听的nodePath</param>
        /// <param name="dataListener">指定路径的监听实例</param>
        public void UnsubscribeDataChanges(string path, IZkDataListener dataListener)
        {
            lock (_dataListener)
            {
                ConcurrentHashSet<IZkDataListener> listeners = _dataListener.Get(path);
                if (listeners != null)
                {
                    listeners.TryRemove(dataListener);
                }

                if (listeners == null || listeners.Count == 0)
                {
                    ConcurrentHashSet<IZkDataListener> _;
                    _dataListener.TryRemove(path, out _);
                }
            }
        }

        /// <summary>
        /// 订阅zookeeper状态变化
        /// </summary>
        /// <param name="stateListener">状态监听实例</param>
        public void SubscribeStateChanges(IZkStateListener stateListener)
        {
            lock (_stateListener)
            {
                _stateListener.Add(stateListener);
            }
        }

        /// <summary>
        /// 取消订阅zookeeper状态变化
        /// </summary>
        /// <param name="stateListener">状态监听实例</param>
        public void UnsubscribeStateChanges(IZkStateListener stateListener)
        {
            lock (_stateListener)
            {
                _stateListener.TryRemove(stateListener);
            }
        }

        /// <summary>
        /// 取消所有当前已订阅的zookeeper的变化监听实例
        /// </summary>
        public void UnsubscribeAll()
        {
            lock (_childListener)
            {
                _childListener.Clear();
            }

            lock (_dataListener)
            {
                _dataListener.Clear();
            }

            lock (_stateListener)
            {
                _stateListener.Clear();
            }
        }

        /// <summary>
        /// 根据指定path建立永久的Node
        /// </summary>
        /// <param name="path">待创建的nodePath</param>
        /// <param name="createParents">是否建立父节点</param>
        public void CreatePersistent(String path, bool createParents = false)
        {
            try
            {
                Create(path, null, CreateMode.Persistent);
            }
            catch (ZkNodeExistsException)
            {
                if (!createParents)
                {
                    throw;
                }
            }
            catch (ZkNoNodeException)
            {
                if (!createParents)
                {
                    throw;
                }

                var parentDir = path.Substring(0, path.LastIndexOf('/'));
                // ReSharper disable ConditionIsAlwaysTrueOrFalse
                CreatePersistent(parentDir, createParents);
                CreatePersistent(path, createParents);
                // ReSharper restore ConditionIsAlwaysTrueOrFalse
            }
        }

        /// <summary>
        /// 根据指定path建立永久的Node，同时设定其值为Data
        /// </summary>
        /// <param name="path">待创建的nodePath</param>
        /// <param name="data">nodeData</param>
        public void CreatePersistent(string path, object data)
        {
            this.Create(path, data, CreateMode.Persistent);
        }

        /// <summary>
        /// 根据指定path建立永久有序的Node，同时设定其值为Data
        /// </summary>
        /// <param name="path">待创建的nodePath</param>
        /// <param name="data">nodeData</param>
        /// <returns>创建好实际的nodePath</returns>
        public string CreatePersistentSequential(string path, object data)
        {
            return this.Create(path, data, CreateMode.PersistentSequential);
        }

        /// <summary>
        /// 根据指定的nodePath，建立一个临时的Node
        /// </summary>
        /// <param name="path">待创建的nodePath</param>
        public void CreateEphemeral(string path)
        {
            this.Create(path, null, CreateMode.Ephemeral);
        }

        /// <summary>
        /// 根据指定的nodePath以及指定的Mode，建立一个Node，同时为新建的Node赋值data
        /// </summary>
        /// <param name="path">待创建的nodePath</param>
        /// <param name="data">nodeData</param>
        /// <param name="mode">新建的Node的类型(永久，短暂等)</param>
        /// <returns>创建好实际的nodePath</returns>
        public String Create(string path, object data, CreateMode mode)
        {
            if (path == null)
            {
                throw new NullReferenceException("path must not be null.");
            }

            var bytes = data == null ? null : this.Serialize(data);
            return RetryUntilConnected(() => _connection.Create(path, bytes, mode));
        }

        /// <summary>
        /// 根据指定的nodePath，建立一个临时的Node
        /// </summary>
        /// <param name="path">待创建的nodePath</param>
        /// <param name="data">nodeData</param>
        public void CreateEphemeral(string path, object data)
        {
            this.Create(path, data, CreateMode.Ephemeral);
        }

        /// <summary>
        /// 根据指定的nodePath，建立一个临时有序的Node
        /// </summary>
        /// <param name="path">待创建的nodePath</param>
        /// <param name="data">nodeData</param>
        /// <returns>创建好实际的nodePath</returns>
        public string CreateEphemeralSequential(string path, object data)
        {
            return this.Create(path, data, CreateMode.EphemeralSequential);
        }

        /// <summary>
        /// 添加的监听后被触发的方法
        /// </summary>
        /// <param name="watchedEvent"></param>
        public void Process(WatchedEvent watchedEvent)
        {
            System.Diagnostics.Debug.WriteLine("Received event: " + watchedEvent);
            _zookeeperEventThread = Thread.CurrentThread;
            var stateChanged = watchedEvent.Path == null;
            var znodeChanged = watchedEvent.Path != null;
            var dataChanged = watchedEvent.Type == EventType.NodeDataChanged || watchedEvent.Type == EventType.NodeDeleted || watchedEvent.Type == EventType.NodeCreated
                    || watchedEvent.Type == EventType.NodeChildrenChanged;

            this.EventLock.Lock();
            try
            {
                // We might have to install child change event listener if a new node was created
                if (this.ShutdownTrigger)
                {
                    System.Diagnostics.Debug.WriteLine("ignoring event '{" + watchedEvent.Type + " | " + watchedEvent.Path + "}' since shutdown triggered");
                    return;
                }

                if (stateChanged)
                {
                    this.ProcessStateChanged(watchedEvent);
                }

                if (dataChanged)
                {
                    this.ProcessDataOrChildChange(watchedEvent);
                }
            }
            finally
            {
                if (stateChanged)
                {
                    this.EventLock.StateChangedCondition.SignalAll();

                    // If the session expired we have to signal all conditions, because watches might have been removed and
                    // there is no guarantee that those
                    // conditions will be signaled at all after an Expired event
                    if (watchedEvent.State == KeeperState.Expired)
                    {
                        this.EventLock.ZNodeEventCondition.SignalAll();
                        this.EventLock.DataChangedCondition.SignalAll();

                        // We also have to notify all listeners that something might have changed
                        this.FireAllEvents();
                    }
                }

                if (znodeChanged)
                {
                    this.EventLock.ZNodeEventCondition.SignalAll();
                }

                if (dataChanged)
                {
                    this.EventLock.DataChangedCondition.SignalAll();
                }

                this.EventLock.Unlock();
                System.Diagnostics.Debug.WriteLine("Leaving process event");
            }
        }

        /// <summary>
        /// 执行所有注册好的事件
        /// </summary>
        private void FireAllEvents()
        {
            foreach (var entry in _childListener)
            {
                this.FireChildChangedEvents(entry.Key, entry.Value);
            }

            foreach (var entry in _dataListener)
            {
                this.FireDataChangedEvents(entry.Key, entry.Value);
            }
        }

        /// <summary>
        /// 获取指定的nodePath下所有的childPath
        /// </summary>
        /// <param name="path">指定的NodePath</param>
        /// <returns>ChildPath的集合</returns>
        public IEnumerable<string> GetChildren(string path)
        {
            return this.GetChildren(path, this.HasListeners(path));
        }

        /// <summary>
        /// 获取指定的nodePath下所有的childPath
        /// </summary>
        /// <param name="path">指定的NodePath</param>
        /// <param name="watch">是否加入监听</param>
        /// <returns>ChildPath的集合</returns>
        protected IEnumerable<string> GetChildren(string path, bool watch)
        {
            return RetryUntilConnected(() => _connection.GetChildren(path, watch));
        }

        /// <summary>
        /// 获取指定的nodePath下所有的child的个数
        /// </summary>
        /// <param name="path">指定的NodePath</param>
        /// <returns>所有的child的个数</returns>
        public int CountChildren(string path)
        {
            try
            {
                return GetChildren(path).Count();
            }
            catch (ZkNoNodeException)
            {
                return 0;
            }
        }

        /// <summary>
        /// 判断给定的NodePath是否存在
        /// </summary>
        /// <param name="path">指定的NodePath</param>
        /// <param name="watch">是否加入监听</param>
        /// <returns>bool</returns>
        protected bool Exists(string path, bool watch)
        {
            return RetryUntilConnected(() => _connection.Exists(path, watch));
        }

        /// <summary>
        /// 判断给定的NodePath是否存在
        /// </summary>
        /// <param name="path">指定的NodePath</param>
        /// <returns>bool</returns>
        public bool Exists(string path)
        {
            return Exists(path, this.HasListeners(path));
        }

        /// <summary>
        /// 执行Zookeeper的状态变化方法
        /// </summary>
        /// <param name="watchedEvent">监视事件</param>
        private void ProcessStateChanged(WatchedEvent watchedEvent)
        {
            System.Diagnostics.Debug.WriteLine("zookeeper state changed ({0})", watchedEvent.State);
            CurrentState = watchedEvent.State;
            if (ShutdownTrigger)
            {
                return;
            }

            try
            {
                this.FireStateChangedEvent(watchedEvent.State);

                if (watchedEvent.State == KeeperState.Expired)
                {
                    Reconnect();
                    FireNewSessionEvents();
                }
            }
            catch (Exception e)
            {
                throw new Exception("Exception while restarting zk client", e);
            }
        }


        /// <summary>
        /// 执行新建Session后的事件
        /// </summary>
        private void FireNewSessionEvents()
        {
            foreach (var s in this._stateListener)
            {
                var stateListener = s;
                _eventThread.Send(
                    new ZkEvent("New session event sent to " + stateListener)
                    {
                        RunAction =
                                () =>
                                stateListener.HandleNewSession()
                    });
            }
        }

        /// <summary>
        /// 执行Zookeeper的状态变化事件
        /// </summary>
        /// <param name="state">zookeeper的状态</param>
        private void FireStateChangedEvent(KeeperState state)
        {
            foreach (var s in this._stateListener)
            {
                var stateListener = s;
                _eventThread.Send(new ZkEvent("State changed to " + state + " sent to " + stateListener)
                {
                    RunAction = () => stateListener.HandleStateChanged(state)
                });
            }
        }

        /// <summary>
        /// 是否存在指定的nodepath 的数据监控
        /// </summary>
        /// <param name="path">nodepath</param>
        /// <returns>bool</returns>
        private bool HasListeners(string path)
        {
            var dataListeners = _dataListener.Get(path);
            if (dataListeners != null && dataListeners.Count > 0)
            {
                return true;
            }

            var childListeners = _childListener.Get(path);
            if (childListeners != null && childListeners.Count > 0)
            {
                return true;
            }

            return false;
        }

        /// <summary>
        /// 删除指定NodePath下所有的Child
        /// </summary>
        /// <param name="path">nodePath</param>
        /// <returns>是否删除成功</returns>
        public bool DeleteRecursive(String path)
        {
            IEnumerable<String> children;
            try
            {
                children = GetChildren(path, false);
            }
            catch (ZkNoNodeException)
            {
                return true;
            }

            foreach (var subPath in children)
            {
                if (!DeleteRecursive(path + "/" + subPath))
                {
                    return false;
                }
            }

            return Delete(path);
        }

        /// <summary>
        /// 执行数据变化方法
        /// </summary>
        /// <param name="watchedEvent">监视事件</param>
        private void ProcessDataOrChildChange(WatchedEvent watchedEvent)
        {
            var path = watchedEvent.Path;

            if (watchedEvent.Type == EventType.NodeChildrenChanged || watchedEvent.Type == EventType.NodeCreated || watchedEvent.Type == EventType.NodeDeleted)
            {
                var childListeners = _childListener.Get(path);
                if (childListeners != null && childListeners.Count > 0)
                {
                    this.FireChildChangedEvents(path, childListeners);
                }
            }

            if (watchedEvent.Type == EventType.NodeDataChanged || watchedEvent.Type == EventType.NodeDeleted || watchedEvent.Type == EventType.NodeCreated)
            {
                var listeners = _dataListener.Get(path);
                if (listeners != null && listeners.Count > 0)
                {
                    this.FireDataChangedEvents(watchedEvent.Path, listeners);
                }
            }
        }

        /// <summary>
        /// 执行给定的NodePath的数据变化事件
        /// </summary>
        /// <param name="path">nodePath</param>
        /// <param name="listeners">数据变化监听器集合</param>
        private void FireDataChangedEvents(string path, IEnumerable<IZkDataListener> dataListeners)
        {
            foreach (var l in dataListeners)
            {
                var listener = l;
                _eventThread.Send(new ZkEvent("Data of " + path + " changed sent to " + listener)
                {
                    RunAction = () =>
                    {
                        // reinstall watch
                        this.Exists(path, true);
                        try
                        {
                            Object data = this.ReadData<object>(path, null, true);
                            listener.HandleDataChange(path, data);
                        }
                        catch (ZkNoNodeException)
                        {
                            listener.HandleDataDeleted(path);
                        }
                    }
                });
            }
        }

        /// <summary>
        /// 执行给定的NodePath的child变化事件
        /// </summary>
        /// <param name="path">nodePath</param>
        /// <param name="childListeners">child变化监听器集合</param>
        private void FireChildChangedEvents(string path, ConcurrentHashSet<IZkChildListener> childListeners)
        {
            try
            {
                //重新注册子节点变化监听器
                foreach (var l in childListeners)
                {
                    var listener = l;
                    _eventThread.Send(new ZkEvent("Children of " + path + " changed sent to " + listener)
                    {
                        RunAction = () =>
                        {
                            try
                            {
                                // if the node doesn't exist we should listen for the root node to reappear
                                Exists(path);
                                var children = this.GetChildren(path);
                                listener.HandleChildChange(path, children);
                            }
                            catch (ZkNoNodeException)
                            {
                                listener.HandleChildChange(path, null);
                            }
                        }
                    });
                }
            }
            catch (Exception e)
            {
                Logger.Error("Failed to fire child changed event. Unable to getChildren.  ", e);
            }
        }

        /// <summary>
        /// 等待指定时间后判断指定nodePath是否存在 返回 true，超过时间则返回 false
        /// </summary>
        /// <param name="path">nodePath</param>
        /// <param name="time">超时时间</param>
        /// <returns>bool</returns>
        public bool WaitUntilExists(String path, TimeSpan time)
        {
            DateTime timeout = DateTime.Now + time;
            System.Diagnostics.Debug.WriteLine("Waiting until znode '" + path + "' becomes available.");
            if (Exists(path))
            {
                return true;
            }

            AcquireEventLock();
            try
            {
                while (!Exists(path, true))
                {
                    var gotSignal = EventLock.ZNodeEventCondition.AwaitUntil(timeout);
                    if (!gotSignal)
                    {
                        return false;
                    }
                }

                return true;
            }
            catch (ThreadInterruptedException e)
            {
                throw new ZkInterruptedException("Thread interrupted", e);
            }
            finally
            {
                EventLock.Unlock();
            }
        }

        /// <summary>
        /// 获取指定NodePath所有的数据变化的监听器
        /// </summary>
        /// <param name="path">nodePath</param>
        /// <returns>监听器集合</returns>
        protected ConcurrentHashSet<IZkDataListener> GetDataListener(string path)
        {
            return _dataListener.Get(path);
        }

        /// <summary>
        /// 等待直到再次连接到Zookeeper
        /// </summary>
        public void WaitUntilConnected()
        {
            this.WaitUntilConnected(TimeSpan.FromMilliseconds(int.MaxValue));
        }

        /// <summary>
        /// 等待指定时间直到连接完成 返回 true，超过时间则返回 false
        /// </summary>
        /// <param name="timeout">超时时间</param>
        /// <returns>bool</returns>
        public bool WaitUntilConnected(TimeSpan timeout)
        {
            return WaitForKeeperState(KeeperState.SyncConnected, timeout);
        }

        /// <summary>
        /// 等待指定时间直到zookeeper的连接状态与指定的连接状态相等 返回 true，超过时间则返回 false
        /// </summary>
        /// <param name="keeperState">连接状态</param>
        /// <param name="timeout">超时时间</param>
        /// <returns>bool</returns>
        public bool WaitForKeeperState(KeeperState keeperState, TimeSpan timeout)
        {
            if (_zookeeperEventThread != null && Thread.CurrentThread == _zookeeperEventThread)
            {
                throw new Exception("Must not be done in the zookeeper event thread.");
            }

            System.Diagnostics.Debug.WriteLine("Waiting for keeper state " + keeperState);
            this.AcquireEventLock();
            try
            {
                bool stillWaiting = true;
                while (CurrentState != keeperState)
                {
                    if (!stillWaiting)
                    {
                        return false;
                    }

                    stillWaiting = EventLock.StateChangedCondition.Await(timeout);
                }

                System.Diagnostics.Debug.WriteLine("State is " + CurrentState);
                return true;
            }
            catch (ThreadInterruptedException e)
            {
                throw new ZkInterruptedException(e);
            }
            finally
            {
                EventLock.Unlock();
            }
        }

        /// <summary>
        /// 获取事件锁
        /// </summary>
        private void AcquireEventLock()
        {
            try
            {
                EventLock.LockInterruptibly();
            }
            catch (ThreadInterruptedException e)
            {
                throw new ZkInterruptedException(e);
            }
        }

        /// <summary>
        /// 重试连接Zk，直到连接成功，执行回掉函数并返回回掉函数的返回值
        /// </summary>
        /// <typeparam name="TResult">返回值</typeparam>
        /// <param name="callable">回掉函数</param>
        /// <returns>TResult</returns>
        public TResult RetryUntilConnected<TResult>(Func<TResult> callable)
        {
            if (_zookeeperEventThread != null && Thread.CurrentThread == _zookeeperEventThread)
            {
                throw new Exception("Must not be done in the zookeeper event thread.");
            }

            while (true)
            {
                try
                {
                    return callable();
                }
                catch (KeeperException.ConnectionLossException)
                {
                    // we give the event thread some time to update the status to 'Disconnected'
                    Thread.Yield();
                    WaitUntilConnected();
                }
                catch (KeeperException.SessionExpiredException)
                {
                    // we give the event thread some time to update the status to 'Expired'
                    Thread.Yield();
                    WaitUntilConnected();
                }
                catch (KeeperException e)
                {
                    throw ZkException.Create(e);
                }
                catch (ThreadInterruptedException e)
                {
                    throw new ZkInterruptedException(e);
                }
            }
        }

        /// <summary>
        /// 删除指定NodePath的Node，删除成功返回 true 否则返回false
        /// </summary>
        /// <param name="path">nodePath</param>
        /// <returns>bool</returns>
        public bool Delete(string path)
        {
            try
            {
                RetryUntilConnected<object>(() => { _connection.Delete(path); return null; });
                return true;
            }
            catch (ZkNoNodeException)
            {
                return false;
            }
        }

        /// <summary>
        /// 将给定数据序列化成byte[]
        /// </summary>
        /// <param name="data">给定data</param>
        /// <returns>byte[]</returns>
        private byte[] Serialize(Object data)
        {
            return ZkSerializer.Serialize(data);
        }

        /// <summary>
        /// 将给定byte[]的数据反序列化指定类型的数据
        /// </summary>
        /// <typeparam name="TResult">TResult</typeparam>
        /// <param name="data">byte[]</param>
        /// <returns>TResult实例</returns>
        private TResult Derializable<TResult>(byte[] data)
        {
            if (data == null)
            {
                return default(TResult);
            }

            return (TResult)ZkSerializer.Deserialize(data);
        }

        /// <summary>
        /// 读取指定NodePath的数据
        /// </summary>
        /// <typeparam name="TResult">TResult</typeparam>
        /// <param name="path">nodePath</param>
        /// <param name="returnNullIfPathNotExists">当nodePath不存在是否返回null</param>
        /// <returns>TResult</returns>
        public TResult ReadData<TResult>(string path, bool returnNullIfPathNotExists = false)
        {
            TResult data = default(TResult);
            try
            {
                data = ReadData<TResult>(path, null);
            }
            catch (ZkNoNodeException)
            {
                if (!returnNullIfPathNotExists)
                {
                    throw;
                }
            }

            return data;
        }

        /// <summary>
        /// 读取指定状态的NodePath的数据
        /// </summary>
        /// <typeparam name="TResult">TResult</typeparam>
        /// <param name="path">nodePath</param>
        /// <param name="stat">nodeStat</param>
        /// <returns>TResult</returns>
        public TResult ReadData<TResult>(string path, Stat stat)
        {
            return ReadData<TResult>(path, stat, HasListeners(path));
        }

        /// <summary>
        /// 读取指定状态的NodePath的数据
        /// </summary>
        /// <typeparam name="TResult">TResult</typeparam>
        /// <param name="path">nodePath</param>
        /// <param name="stat">nodeStat</param>
        /// <param name="watch">是否监听</param>
        /// <returns>TResult</returns>
        protected TResult ReadData<TResult>(string path, Stat stat, bool watch)
        {
            var data = RetryUntilConnected(
                () => _connection.ReadData(path, stat, watch)
            );

            return Derializable<TResult>(data);
        }

        /// <summary>
        /// 向指定NodePath内写入数据
        /// </summary>
        /// <param name="path">nodePath</param>
        /// <param name="data">data</param>
        public void WriteData(string path, object data)
        {
            this.WriteData(path, data, -1);
        }

        public void UpdateDataSerialized<TResult>(string path, IDataUpdater<TResult> updater)
        {
            var stat = new Stat();
            bool retry;
            do
            {
                retry = false;
                try
                {
                    var oldData = ReadData<TResult>(path, stat);
                    var newData = updater.Update(oldData);
                    WriteData(path, newData, stat.Version);
                }
                catch (ZkBadVersionException)
                {
                    retry = true;
                }
            }
            while (retry);
        }

        /// <summary>
        /// 向指定NodePath内写入数据，可以指定预期版本
        /// </summary>
        /// <param name="path">nodePath</param>
        /// <param name="data">data</param>
        /// <param name="expectedVersion">预期的版本</param>
        public void WriteData(string path, object data, int expectedVersion)
        {
            this.WriteDataReturnStat(path, data, expectedVersion);
        }

        /// <summary>
        /// 向给定nodePath写入给定数据，并返回指定Node的状态
        /// </summary>
        /// <param name="path"></param>
        /// <param name="data"></param>
        /// <param name="expectedVersion"></param>
        /// <returns></returns>
        public Stat WriteDataReturnStat(string path, object data, int expectedVersion)
        {
            var datas = this.Serialize(data);
            return RetryUntilConnected(
                () => _connection.WriteDataReturnStat(path, datas, expectedVersion)
            );
        }

        /// <summary>
        /// 为给定的NodePath添加数据变化的监听
        /// </summary>
        /// <param name="path">nodePath</param>
        public void WatchForData(string path)
        {
            RetryUntilConnected<object>(
                () =>
                {
                    _connection.Exists(path, true);
                    return null;
                }

            );
        }

        /// <summary>
        /// 为给定的NodePath添加child变化的监听
        /// </summary>
        /// <param name="path">nodePath</param>
        /// <returns>ChildPath的集合</returns>
        public IEnumerable<String> WatchForChilds(string path)
        {
            if (_zookeeperEventThread != null && Thread.CurrentThread == _zookeeperEventThread)
            {
                throw new Exception("Must not be done in the zookeeper event thread.");
            }

            return RetryUntilConnected(
                () =>
                {
                    Exists(path, true);
                    try
                    {
                        return GetChildren(path, true);
                    }
                    catch (ZkNoNodeException)
                    {
                        // ignore, the "exists" watch will listen for the parent node to appear
                    }

                    return null;
                });
        }

        /// <summary>
        /// 连接zookeeper
        /// </summary>
        /// <param name="maxMsToWaitUntilConnected">等待连接成功的最大时间 单位：毫秒</param>
        /// <param name="watcher">监听对象</param>
        public void Connect(long maxMsToWaitUntilConnected, IWatcher watcher)
        {
            bool started = false;
            try
            {
                EventLock.LockInterruptibly();
                ShutdownTrigger = false;
                _eventThread = new ZkEventThread(_connection.Servers);
                _eventThread.Start();
                _connection.Connect(watcher);

                System.Diagnostics.Debug.WriteLine("Awaiting connection to Zookeeper server");
                if (!WaitUntilConnected(TimeSpan.FromMilliseconds(maxMsToWaitUntilConnected)))
                {
                    throw new ZkTimeoutException("Unable to connect to zookeeper server within timeout: " + maxMsToWaitUntilConnected);
                }

                started = true;
            }
            catch (ThreadInterruptedException)
            {
                ZooKeeper.States state = _connection.ZookeeperState;
                throw new Exception("Not connected with zookeeper server yet. Current state is " + state);
            }
            finally
            {
                EventLock.Unlock();

                // we should close the zookeeper instance, otherwise it would keep
                // on trying to connect
                if (!started)
                {
                    this.Dispose();
                }
            }
        }

        /// <summary>
        /// 获取给定NodePath的创建时间
        /// </summary>
        /// <param name="path">nodePath</param>
        /// <returns>时间戳</returns>
        public long GetCreationTime(String path)
        {
            try
            {
                EventLock.LockInterruptibly();
                return _connection.GetCreateTime(path);
            }
            catch (KeeperException e)
            {
                throw ZkException.Create(e);
            }
            catch (ThreadInterruptedException e)
            {
                throw new ZkInterruptedException(e);
            }
            finally
            {
                EventLock.Unlock();
            }
        }

        /// <summary>
        /// 关闭Zookeeper连接，释放被占用所有资源
        /// </summary>
        public void Dispose()
        {
            if (_connection == null)
            {
                return;
            }

            System.Diagnostics.Debug.WriteLine("Closing ZkClient...");
            EventLock.Lock();
            try
            {
                ShutdownTrigger = true;
                _eventThread.Interrupt();
                _eventThread.Join(2000);

            }
            catch (ThreadInterruptedException e)
            {
                throw new ZkInterruptedException(e);
            }
            finally
            {
                EventLock.Unlock();
            }

            _connection.Dispose();
            _connection = null;

            System.Diagnostics.Debug.WriteLine("Closing ZkClient...done");
        }

        /// <summary>
        /// 重新连接zookeeper
        /// </summary>
        private void Reconnect()
        {
            EventLock.Lock();
            try
            {
                _connection.Dispose();
                _connection.Connect(this);
            }
            catch (ThreadInterruptedException e)
            {
                throw new ZkInterruptedException(e);
            }
            finally
            {
                EventLock.Unlock();
            }
        }

        /// <summary>
        /// 当前已注册的监听器个数
        /// </summary>
        public int NumberOfListeners
        {
            get
            {
                var listeners = 0;
                foreach (var childListeners in _childListener.Values)
                {
                    listeners += childListeners.Count;
                }

                foreach (var dataListeners in _dataListener.Values)
                {
                    listeners += dataListeners.Count;
                }

                listeners += _stateListener.Count;

                return listeners;
            }
        }
    }
}
