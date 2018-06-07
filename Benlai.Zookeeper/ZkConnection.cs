using System;
using System.Collections.Generic;
using System.Threading;
using Org.Apache.Zookeeper.Data;
using ZooKeeperNet;
using Benlai.Zookeeper.Interface;
using Benlai.Zookeeper.Exceptions;

namespace Benlai.Zookeeper
{
    public class ZkConnection : IZkConnection
    {
        /** It is recommended to use quite large sessions timeouts for ZooKeeper. */
        private static readonly TimeSpan DefaultSessionTimeout = TimeSpan.FromSeconds(30);

        private ZooKeeper _zk;

        private object _zookeeperLock = new object();

        private string _servers;

        private TimeSpan _sessionTimeOut;

        public ZkConnection(string zkServers)
            : this(zkServers, DefaultSessionTimeout)
        {
        }

        public ZkConnection(string zkServers, TimeSpan sessionTimeOut)
        {
            this._servers = zkServers;
            this._sessionTimeOut = sessionTimeOut;
        }

        public void Connect(IWatcher watcher)
        {
            Monitor.Enter(this._zookeeperLock);
            try
            {
                if (this._zk != null)
                {
                    throw new Exception("zk client has already been started");
                }

                try
                {
                    System.Diagnostics.Debug.WriteLine("Creating new ZookKeeper instance to connect to " + this._servers + ".");
                    this._zk = new ZooKeeper(this._servers, this._sessionTimeOut, watcher);
                }
                catch (Exception e)
                {
                    throw new ZkException("Unable to connect to " + this._servers, e);
                }
            }
            finally
            {
                Monitor.Exit(this._zookeeperLock);
            }
        }

        public void Dispose()
        {
            Monitor.Enter(this._zookeeperLock);
            try
            {
                if (this._zk != null)
                {
                    System.Diagnostics.Debug.WriteLine("Closing ZooKeeper connected to " + this._servers);
                    this._zk.Dispose();
                    this._zk = null;
                }
            }
            finally
            {
                Monitor.Exit(this._zookeeperLock);
            }
        }

        public string Create(string path, byte[] data, CreateMode mode)
        {
            return this._zk.Create(path, data, Ids.OPEN_ACL_UNSAFE, mode);
        }

        public void Delete(string path)
        {
            this._zk.Delete(path, -1);
        }

        public bool Exists(string path, bool watch)
        {
            var result = this._zk.Exists(path, watch) != null;
            return result;
        }

        public IEnumerable<string> GetChildren(string path, bool watch)
        {
            return this._zk.GetChildren(path, watch);
        }

        public byte[] ReadData(string path, Stat stat, bool watch)
        {
            return this._zk.GetData(path, watch, stat);
        }

        public void WriteData(string path, byte[] data)
        {
            this.WriteData(path, data, -1);
        }

        public void WriteData(string path, byte[] data, int version)
        {
            this._zk.SetData(path, data, version);
        }

        public Stat WriteDataReturnStat(string path, byte[] data, int version)
        {
            return this._zk.SetData(path, data, version);
        }

        public ZooKeeper.States ZookeeperState
        {
            get
            {
                return this._zk != null ? this._zk.State : null;
            }
        }

        public ZooKeeper Zookeeper
        {
            get
            {
                return this._zk;
            }
        }

        public long GetCreateTime(string path)
        {
            var stat = this._zk.Exists(path, false);
            if (stat != null)
            {
                return stat.Ctime;
            }

            return -1;
        }

        public string Servers
        {
            get
            {
                return this._servers;
            }
        }
    }
}
