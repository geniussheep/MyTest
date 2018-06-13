using System;
using System.Threading;

namespace TestTask
{
    public class ScheduleTask : IDisposable
    {

        public delegate void Job();

        private ISchedule _schedule;

        public Job ExecuteJob { get; set; }

        private readonly object _runningLock = new object();

        private bool _isRunFirstTime;
        private bool _isStopTaskWithBusiness;

        private Thread _task;

        /// <summary> 
        /// 任务名称 
        /// </summary> 
        private string _name;

        public string Name
        {
            get { return _name; }
            set { _name = value; }
        }

        /// <summary> 
        /// 任务描述 
        /// </summary> 
        private string _description;

        public string Description
        {
            get { return _description; }
            set { _description = value; }
        }

        private DateTime _lastExecuteTime;

        public DateTime LastExecuteTime => _lastExecuteTime;

        public ScheduleTask(ISchedule schedule, Job executeJob)
        {
            if (schedule == null)
            {
                throw new ArgumentNullException(nameof(schedule));
            }

            _schedule = schedule;
            _isRunFirstTime = true;
            ExecuteJob = executeJob;
            _isStopTaskWithBusiness = false;
        }

        public void Start()
        {
            if (ExecuteJob == null)
            {
                throw new ArgumentNullException(nameof(ExecuteJob));
            }
            if (_task != null)
            {
                return;
            }

            lock (_runningLock)
            {
                if (_task != null)
                {
                    return;
                }

                _task = new Thread(ExecTask) {IsBackground = true};
                _task.Start();
            }
        }

        public void Stop()
        {
            if (_task == null)
            {
                return;
            }
            lock (_runningLock)
            {
                if (_task == null)
                {
                    return;
                }
                try
                {
                    if (!_task.Join(TimeSpan.FromSeconds(1)))
                    {
                        _task.Abort();
                    }
                }
                catch (Exception)
                {
                    // ignored
                }
                finally
                {
                    _task = null;
                }
            }
        }

        public void StopWithBusiness()
        {
            _isStopTaskWithBusiness = true;
        }

        private void ExecTask()
        {
            while (true)
            {
                try
                {
                    if (_isRunFirstTime)
                    {
                        Thread.Sleep(ExecutePeriod());
                    }

                    ExecuteJob?.Invoke();
                    if (_isStopTaskWithBusiness)
                    {
                        LogInfoWriter.GetInstance().Info("the schedule task has been stopped!");
                        break;
                    }

                    Thread.Sleep(ExecutePeriod());
                }
                catch (Exception e)
                {
                    LogInfoWriter.GetInstance().Warn("run schedule task warning!", e);
                }
            }
        }

        private TimeSpan ExecutePeriod()
        {
            if (_isRunFirstTime)
            {
                _lastExecuteTime = _schedule.ExecutionTime;
                _isRunFirstTime = false;
                return TimeSpan.FromMilliseconds(_schedule.DueTime);
            }
            _lastExecuteTime = DateTime.Now;
            return TimeSpan.FromMilliseconds(_schedule.Period);
        }

        public void Dispose()
        {
            Stop();
            _schedule = null;
            _name = null;
            _description = null;
        }
    }
}
