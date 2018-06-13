using System;

namespace ConsoleTaskPool.ScheduleTask
{
    /// <summary>;
    /// 周期性的执行计划
    /// </summary>;

    public class CycExecution : ISchedule
    {
        /// <summary>;
        /// 构造函数，在一个将来时间开始运行
        /// </summary>;
        /// <param name="shedule">;计划执行的时间</param>;
        /// <param name="period">;周期时间</param>;
        public CycExecution(DateTime shedule, TimeSpan period)
        {
            _schedule = shedule;
            _period = period;
        }


        /// <summary>;
        /// 构造函数,马上开始运行
        /// </summary>;
        /// <param name="period">;周期时间</param>;
        public CycExecution(TimeSpan period)
        {
            _schedule = DateTime.Now;
            _period = period;
        }


        private DateTime _schedule;

        private TimeSpan _period;

        #region ISchedule 成员

        public long DueTime
        {
            get
            {
                long ms = (_schedule.Ticks - DateTime.Now.Ticks) / 10000;
                if (ms < 0) ms = 0;
                return ms;
            }
        }

        public DateTime ExecutionTime
        {
            get
            {
                return _schedule;
            }
            set
            {
                _schedule = value;
            }
        }

        public long Period => _period.Ticks / 10000;
        #endregion
    }

}
