using System;
using System.Threading;
using Benlai.AutoPublish.Utils.Task;

namespace ConsoleTaskPool.ScheduleTask
{
    /// <summary>;

    /// 计划立即执行任务

    /// </summary>;

    public class ImmediateExecution : ISchedule

    {

        #region ISchedule 成员



        public DateTime ExecutionTime

        {

            get

            {

                // TODO:  添加 ImmediatelyShedule.ExecutionTime getter 实现

                return DateTime.Now;

            }

            set { ; }

        }



        public long DueTime

        {

            get { return 0; }

        }





        public long Period
        {
            get

            {
                // TODO:  添加 ImmediatelyShedule.Period getter 实现

                return Timeout.Infinite;

            }
        }
        #endregion
    }

}
