using System;
using TestTask;

namespace Benlai.AutoPublish.Utils.Task
{
    public class DailyExecution : ISchedule
    {
        private DateTime _executionTime;

        public DateTime ExecutionTime
        {
            get { return _executionTime; }
            set { _executionTime = value; }
        }

        /// <summary> 
        /// 得到该计划首次还有多久才能运行 单位：毫秒 当前值：指定时间与当前时间的差
        /// </summary> 
        public long DueTime
        {
            get
            {
                long ms = (_executionTime.Ticks - DateTime.Now.Ticks) / 10000;
                if (ms < 0)
                    ms = 0;
                return ms;
            }
        }

        /// <summary> 
        /// 循环的周期 一天 单位：毫秒
        /// </summary>
        public long Period => TimeSpan.FromDays(1).Ticks / 10000;

        public DailyExecution(int hour, int minute, int second)
        {
            if (hour < 0 || hour > 23)
            {
                throw new ArgumentException("Hour must be from 0 to 23");
            }

            if (minute < 0 || minute > 59)
            {
                throw new ArgumentException("Minute must be from 0 to 59");
            }

            if (second < 0 || second > 59)
            {
                throw new ArgumentException("Second must be from 0 to 59");
            }

            _executionTime = DateTime.Parse($"{DateTime.Now.ToShortDateString()} {hour}:{minute}:{second}");
        }

        public DailyExecution(string executeTime)
        {
            if (!DateTime.TryParse($"{DateTime.Now.ToShortDateString()} {executeTime}", out _executionTime))
            {
                throw new ArgumentException($"the value of executeTime:{executeTime} is not right time", nameof(executeTime));
            }
        }

        public DailyExecution(DateTime executionTime)
        {
            _executionTime = executionTime;
        }

        public DailyExecution()
        {
            _executionTime = DateTime.Now;
        }
    }
}
