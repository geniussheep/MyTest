using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading;
using System.Threading.Tasks;
using Ibms.Utility.Task;

namespace TestTask
{
    /// <summary>
    /// 计划在某一未来的时间执行一个操作一次，如果这个时间比现在的时间小，就变成了立即执行的方式
    /// </summary>

    public class ScheduleExecutionOnce : ISchedule
    {
        /// <summary>
        /// 构造函数
        /// </summary>
        /// <param name="schedule">计划开始执行的时间</param>
        public ScheduleExecutionOnce(DateTime schedule)
        {
            m_schedule = schedule;
        }


        private DateTime m_schedule;

        #region ISchedule 成员
        public DateTime ExecutionTime
        {
            get
            {
                // TODO:  添加 ScheduleExecutionOnce.ExecutionTime getter 实现
                return m_schedule;
            }
            set
            {
                m_schedule = value;
            }
        }


        /// <summary>
        /// 得到该计划还有多久才能运行
        /// </summary>

        public long DueTime

        {

            get

            {

                long ms = (m_schedule.Ticks - DateTime.Now.Ticks) / 10000;



                if (ms < 0 ) ms = 0;

                return ms;

            }

        }





        public long Period

        {

            get

            {

                // TODO:  添加 ScheduleExecutionOnce.Period getter 实现

                return Timeout.Infinite;

            }

        }
        #endregion

    }

}
