using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace TestTask
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
            m_schedule = shedule;
            m_period = period;
        }


        /// <summary>;
        /// 构造函数,马上开始运行
        /// </summary>;
        /// <param name="period">;周期时间</param>;
        public CycExecution(TimeSpan period)
        {
            m_schedule = DateTime.Now;
            m_period = period;
        }


        private DateTime m_schedule;

        private TimeSpan m_period;

        #region ISchedule 成员

        public long DueTime
        {
            get
            {
                long ms = (m_schedule.Ticks - DateTime.Now.Ticks) / 10000;
                if (ms < 0) ms = 0;
                return ms;
            }
        }

        public DateTime ExecutionTime
        {
            get
            {
                // TODO:  添加 CycExecution.ExecutionTime getter 实现
                return m_schedule;
            }
            set
            {
                m_schedule = value;
            }
        }

        public long Period
        {
            get
            {
                // TODO:  添加 CycExecution.Period getter 实现
                return m_period.Ticks / 10000;
            }
        }
        #endregion
    }

}
