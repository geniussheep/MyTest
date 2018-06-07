using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading;
using System.Threading.Tasks;
using ConsoleTaskPool.TaskPoolService;
using Benlai.Common;

namespace ConsoleTaskPool.TaskPoolService
{
    public class TaskPool
    {
        /// <summary>
        /// 所有任务信息集合
        /// </summary>
        private readonly List<TaskModel> _taskInfoList;

        /// <summary>
        /// 运行中任务信息的集合
        /// </summary>
        private Dictionary<int, TaskModel> _runningTaskInfoDic;

        /// <summary>
        /// 已完成的任务列表
        /// </summary>
        private List<TaskModel> _hasCompleteTaskInfoList;

        /// <summary>
        /// 执行任务的线程集合
        /// </summary>
        private readonly List<Task> _executeTaskList;

        private readonly Mutex mutex = new Mutex();

        /// <summary>
        /// 最大可并发任务数
        /// </summary>
        private readonly int _concurrencyRunTaskMaxcount;

        /// <summary>
        /// 所有任务允许的最大失败数
        /// </summary>
        private readonly int _taskFailedMaxCount;

        /// <summary>
        /// 所有任务是否已经完成  = 所有任务完成 或者 任务失败数>=最大可失败数
        /// </summary>
        private bool _isFinished;

        private readonly object _lockObj = new object();

        /// <summary>
        /// 任务主逻辑开始前的逻辑
        /// </summary>
        /// <returns></returns>
        public OnBeforeStartTaskEventHandler OnBeforeStart { get; set; }

        /// <summary>
        /// 任务主逻辑结束后的逻辑
        /// </summary>
        /// <returns></returns>
        public OnAfterStartTaskEventHandler OnAfterStart { get; set; }

        /// <summary>
        /// 当发生异常是的处理逻辑
        /// </summary>
        public OnExceptionOccurredEventHandler OnExceptionOccurred { get; set; }

        public TaskPool(List<TaskModel> taskInfoList, int concurrencyRunTaskMaxcount, int taskFailedMaxCount)
        {
            _taskInfoList = taskInfoList;
            _concurrencyRunTaskMaxcount = concurrencyRunTaskMaxcount;
            _taskFailedMaxCount = taskFailedMaxCount;
            _runningTaskInfoDic = new Dictionary<int, TaskModel>();
            _executeTaskList = new List<Task>();
            _hasCompleteTaskInfoList = new List<TaskModel>();
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
        }

        private void ExecuteTask()
        {
            var currentTaskId = Thread.CurrentThread.ManagedThreadId;
            string debugStr = $"Thread -- {currentTaskId}";
            while (!_isFinished)
            {
                LogInfoWriter.GetInstance().Info($"{debugStr} --Check Start [taskInfoList:{_taskInfoList.Count()}, hasCompleteTaskInfoList:{_hasCompleteTaskInfoList.Count()}, runningTaskInfoDic:{_runningTaskInfoDic.Count()}]");
                Thread.Sleep(1000);
                lock (_runningTaskInfoDic)
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
                        task.ExecuteAction();
                        OnAfterStart?.Invoke(task);
                    }
                    catch (Exception e)
                    {
                        task.ExecuteFailed(e);
                        OnExceptionOccurred?.Invoke(e);
                        LogInfoWriter.GetInstance().Info($"{debugStr} --Execute Failed");
                    }
                    finally
                    {
                        task.HasCompleted();
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
    }
}
