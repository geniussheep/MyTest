using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading;
using System.Threading.Tasks;
using Benlai.Common;

namespace ConsoleTaskPool.TaskPoolService
{
    public class TaskPool : IDisposable
    {
        /// <summary>
        /// 所有任务信息集合
        /// </summary>
        private List<TaskModel> _taskInfoList;

        /// <summary>
        /// 运行中任务信息的集合
        /// </summary>
        private readonly Dictionary<int, TaskModel> _runningTaskInfoDic;

        /// <summary>
        /// 已完成的任务列表
        /// </summary>
        private readonly List<TaskModel> _hasCompleteTaskInfoList;

        /// <summary>
        /// 执行任务的线程集合
        /// </summary>
        private readonly List<Task> _executeTaskList;

        /// <summary>
        /// 最大可并发任务数
        /// </summary>
        private readonly int _concurrencyRunTaskMaxcount;

        /// <summary>
        /// 所有任务允许的最大失败数
        /// </summary>
        private readonly int _taskFailedMaxCount;

        /// <summary>
        /// 失败的任务数量
        /// </summary>
        private int _failedTaskCount;

        /// <summary>
        /// 成功的任务数量
        /// </summary>
        private int _successTaskCount;

        /// <summary>
        /// 完成任务数量
        /// </summary>
        private int _completedTaskCount;

        /// <summary>
        /// 所有任务是否已经完成  = 所有任务完成 或者 任务失败数>=最大可失败数
        /// </summary>
        private bool _isFinished;

        /// <summary>
        /// 取消任务操作
        /// </summary>
        private readonly CancellationTokenSource _cts = new CancellationTokenSource();

        private readonly object _lockObj = new object();

        /// <summary>
        /// 任务主逻辑开始前的逻辑
        /// </summary>
        /// <returns></returns>
        public OnExecuteTaskEventHandler OnBeforeStart { get; set; }

        /// <summary>
        /// 任务主逻辑结束后的逻辑
        /// </summary>
        /// <returns></returns>
        public OnExecuteTaskEventHandler OnAfterStart { get; set; }

        /// <summary>
        /// 任务主逻辑结束后的逻辑
        /// </summary>
        /// <returns></returns>
        public OnExecuteTaskEventHandler OnCompleted { get; set; }

        /// <summary>
        /// 当发生异常是的处理逻辑
        /// </summary>
        public OnExceptionOccurredEventHandler OnExceptionOccurred { get; set; }

        /// <summary>
        /// 当任务停止时的处理逻辑
        /// </summary>
        public OnStopTaskEventHandler OnStoped { get; set; }

        /// <summary>
        /// 当任务取消时的处理逻辑
        /// </summary>
        public OnStopTaskEventHandler OnCancelled { get; set; }

        public int FailedTaskCount => _failedTaskCount;
        public int SuccessTaskCount => _successTaskCount;
        public int CompletedTaskCount => _completedTaskCount;

        public TaskPool(int concurrencyRunTaskMaxcount, int taskFailedMaxCount = 0)
        {
            _concurrencyRunTaskMaxcount = concurrencyRunTaskMaxcount;
            _taskFailedMaxCount = taskFailedMaxCount;
            _failedTaskCount = 0;
            _runningTaskInfoDic = new Dictionary<int, TaskModel>();
            _executeTaskList = new List<Task>();
            _hasCompleteTaskInfoList = new List<TaskModel>();
        }

        public TaskPool(List<TaskModel> taskInfoList, int concurrencyRunTaskMaxcount, int taskFailedMaxCount = 0)
        {
            _taskInfoList = taskInfoList;
            _concurrencyRunTaskMaxcount = concurrencyRunTaskMaxcount;
            _taskFailedMaxCount = taskFailedMaxCount;
            _failedTaskCount = 0;
            _runningTaskInfoDic = new Dictionary<int, TaskModel>();
            _executeTaskList = new List<Task>();
            _hasCompleteTaskInfoList = new List<TaskModel>();
        }

        public void SetTaskInfoList(List<TaskModel> taskInfoList)
        {
            _taskInfoList = taskInfoList;
        }

        public void Start()
        {
            if (_taskInfoList == null || !_taskInfoList.Any())
            {
                throw new ArgumentNullException(nameof(_taskInfoList), "待执行的任务列表信息为空或没有任务可执行任务！");
            }

            for (int i = 0; i < _concurrencyRunTaskMaxcount; i++)
            {
                _executeTaskList.Add(Task.Run(() => { ExecuteTask(); }).ContinueWith(t => { }));
            }

            Task.WaitAll(_executeTaskList.ToArray());
            Clear();
        }

        public void Stop()
        {
            if (_isFinished) return;
            _isFinished = true;
            Task.WaitAll(_executeTaskList.ToArray());
            Thread.Sleep(1);
            OnStoped?.Invoke();
            Clear();
        }

        public void Cancel()
        {
            var currentTaskId = Thread.CurrentThread.ManagedThreadId;
            string debugStr = $"Thread -- {currentTaskId}";
            LogInfoWriter.GetInstance().Info($"{debugStr} --Check Cancel");

            if (_cts.IsCancellationRequested) return;
            LogInfoWriter.GetInstance().Info($"{debugStr} --Start Cancel");

            _cts.Cancel(); //设置为取消
            _cts.Token.Register(() =>
            {
                OnCancelled?.Invoke();
                Clear();
                LogInfoWriter.GetInstance().Info($"{debugStr} --End Cancel");
            });
        }

        private void Clear()
        {
            _executeTaskList?.Clear();
            _runningTaskInfoDic?.Clear();
            _hasCompleteTaskInfoList?.Clear();
        }

        private void ExecuteTask()
        {
            var currentTaskId = Thread.CurrentThread.ManagedThreadId;
            string debugStr = $"Thread -- {currentTaskId}";
            while (!_isFinished && !_cts.IsCancellationRequested)
            {
                LogInfoWriter.GetInstance().Info($"{debugStr} --Check Start [taskInfoList:{_taskInfoList.Count()}, hasCompleteTaskInfoList:{_hasCompleteTaskInfoList.Count()}, runningTaskInfoDic:{_runningTaskInfoDic.Count()}]");
                Thread.Sleep(1000);
                lock (_lockObj)
                {
                    //所有任务已完成
                    if (_taskInfoList.Count() <= 0)
                    {
                        LogInfoWriter.GetInstance().Info($"{debugStr} --Check taskInfoList.Count() <= 100 \r\n");
                        _isFinished = true;
                        return;
                    }

                    //任务失败数超过任务要求的最大可失败数
                    LogInfoWriter.GetInstance().Info($"{debugStr} --Check hasCompleteTaskInfoList.Count(s => s.IsFailed):{_hasCompleteTaskInfoList.Count(s => s.IsFailed)}");
                    if (_hasCompleteTaskInfoList.Count(s => s.IsFailed) >= _taskFailedMaxCount)
                    {
                        LogInfoWriter.GetInstance().Info($"{debugStr} --Check hasCompleteTaskInfoList.Count(s => s.IsFailed) >= {_taskFailedMaxCount} \r\n");
                        _isFinished = true;
                        return;
                    }

                    //正在进行的任务数 = 最大可并发任务 则跳过获取新任务  继续等待当前运行的任务完成
                    LogInfoWriter.GetInstance().Info($"{debugStr} --Check runningTaskInfoDic.Count():{_runningTaskInfoDic.Count()}");
                    if (_runningTaskInfoDic.Count() == _concurrencyRunTaskMaxcount)
                    {
                        LogInfoWriter.GetInstance().Info($"{debugStr} --Check runningTaskInfoDic.Count() == {_concurrencyRunTaskMaxcount} \r\n");
                        continue;
                    }

                    //正在运行的任务中存在失败的任务且还有其他任务未结束 则不去获取新任务
                    LogInfoWriter.GetInstance().Info($"{debugStr} --Check runningTaskInfoDic.Values.Any(s => s.IsFailed):{_runningTaskInfoDic.Values.Any(s => s.IsFailed)}");
                    LogInfoWriter.GetInstance().Info($"{debugStr} --Check runningTaskInfoDic.Values.Any(s => !s.IsCompleted):{_runningTaskInfoDic.Values.Any(s => !s.IsCompleted)}");
                    if (_runningTaskInfoDic.Values.Any(s => s.IsFailed) &&
                        _runningTaskInfoDic.Values.Any(s => !s.IsCompleted))
                    {
                        LogInfoWriter.GetInstance().Info($"{debugStr} --Check runningTaskInfoDic.Values.Any(s => s.IsFailed) == _runningTaskInfoDic.Values.Any(s => !s.IsCompleted) == true \r\n");
                        continue;
                    }
                    //如果当前正在运行任务小于最大 并发任务数 且当前线程没有正在运行的任务  则去拉取新任务执行
                    if (_runningTaskInfoDic.Count < _concurrencyRunTaskMaxcount &&
                       !_runningTaskInfoDic.ContainsKey(currentTaskId))
                    {
                        var t = _taskInfoList.First(s => !s.IsCompleted);
                        LogInfoWriter.GetInstance().Info($"{debugStr} --Get GetTask Success :{t.TaskName}");
                        _runningTaskInfoDic.Add(currentTaskId, t);
                        _taskInfoList.Remove(t);
                        LogInfoWriter.GetInstance().Info($"{debugStr} --Get taskInfoList.Count():{_taskInfoList.Count()}");
                        LogInfoWriter.GetInstance().Info($"{debugStr} --Get runningTaskInfoDic.Count():{_runningTaskInfoDic.Count()}\r\n");
                    }
                }
                if (_runningTaskInfoDic.ContainsKey(currentTaskId))
                {
                    var task = _runningTaskInfoDic[currentTaskId];

                    try
                    {
                        LogInfoWriter.GetInstance().Info($"{debugStr} --Execute Start");
                        OnBeforeStart?.Invoke(task);
                        task.TaskAction?.Invoke();
                        Interlocked.Increment(ref _successTaskCount);
                        OnAfterStart?.Invoke(task);
                    }
                    catch (Exception e)
                    {
                        task.ExecuteFailed(e);
                        Interlocked.Increment(ref _failedTaskCount);
                        OnExceptionOccurred?.Invoke(e);
                        LogInfoWriter.GetInstance().Info($"{debugStr} --Execute Failed");
                    }
                    finally
                    {
                        task.HasCompleted();
                        Interlocked.Increment(ref _completedTaskCount);
                        OnCompleted?.Invoke(task);
                        LogInfoWriter.GetInstance().Info($"{debugStr} -- Execute runningTaskInfoDic.Values.Any(s => s.IsFailed)：{_runningTaskInfoDic.Values.Any(s => s.IsFailed)}!");
                        LogInfoWriter.GetInstance().Info($"{debugStr} -- Execute hasCompleteTaskInfoList.Any(s => s.IsFailed))：{_hasCompleteTaskInfoList.Any(s => s.IsFailed)}!");
                        LogInfoWriter.GetInstance().Info($"{debugStr} -- Execute runningTaskInfoDic.Values.Any(s=>!s.IsCompleted)：{_runningTaskInfoDic.Values.Any(s => !s.IsCompleted)}!");

                        while ((_runningTaskInfoDic.Values.Any(s => s.IsFailed) || _hasCompleteTaskInfoList.Any(s => s.IsFailed)) && _runningTaskInfoDic.Values.Any(s=>!s.IsCompleted))
                        {
                            LogInfoWriter.GetInstance().Info($"{debugStr} -- Execute Wait [taskInfoList:{_taskInfoList.Count()}, hasCompleteTaskInfoList:{_hasCompleteTaskInfoList.Count()}, runningTaskInfoDic:{_runningTaskInfoDic.Count()}]");
                            LogInfoWriter.GetInstance().Info($"{debugStr} -- Execute Wait there has some task failed ,so wait all current running task has finished!");
                            LogInfoWriter.GetInstance().Info($"{debugStr} -- Execute Wait [runningTaskInfoDic:{_runningTaskInfoDic.Values.Select(s=>s.TaskName + ":[" +s.IsFailed+","+s.IsCompleted+"]").Aggregate((s,r)=>s+" - "+r)}]");
                            Thread.Sleep(1000);
                        }
                        if(_runningTaskInfoDic.ContainsKey(currentTaskId))
                        {
                            LogInfoWriter.GetInstance().Info($"{debugStr} -- Execute Add [runningTaskInfoDic:{_runningTaskInfoDic.Values.Select(s => s.TaskName + ":[" + s.IsFailed + "," + s.IsCompleted + "]").Aggregate((s, r) => s + " - " + r)}]");
                            LogInfoWriter.GetInstance().Info($"{debugStr} --Execute Add {task.TaskName} HasComplete and Remove running");
                            _runningTaskInfoDic.Remove(currentTaskId);
                            _hasCompleteTaskInfoList.Add(task);
                        }
                        LogInfoWriter.GetInstance().Info($"{debugStr} --Execute End");
                    }
                }

                LogInfoWriter.GetInstance().Info($"{debugStr} --Check End [taskInfoList:{_taskInfoList.Count()}, hasCompleteTaskInfoList:{_hasCompleteTaskInfoList.Count()}, runningTaskInfoDic:{_runningTaskInfoDic.Count()}]");
            }
        }


        public void Dispose()
        {
            if (!_isFinished)
            {
                _isFinished = true;
            }
            Task.WaitAll(_executeTaskList.ToArray());
            Thread.Sleep(1);
            Clear();
            _cts?.Dispose();
            _taskInfoList?.Clear();
        }
    }
}
