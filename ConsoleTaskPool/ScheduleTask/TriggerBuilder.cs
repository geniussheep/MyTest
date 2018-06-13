
using System;

namespace ConsoleTaskPool.ScheduleTask
{
    public class TriggerBuilder
    {
        /// <summary>
        /// 新建每天的运行触发器<see cref="DailyExecution"/>
        /// 指定每天的几点几分几秒运行
        /// </summary>
        /// <param name="hour">时</param>
        /// <param name="minute">分</param>
        /// <param name="second">秒</param>
        /// <returns>DailyExecution</returns>
        public static DailyExecution WithDailyExecution(int hour, int minute, int second)
        {
            return new DailyExecution(hour,minute,second);
        }

        /// <summary>
        /// 新建每天的运行触发器<see cref="DailyExecution"/>
        /// 指定每天的几点几分几秒运行
        /// </summary>
        /// <param name="time">运行时间 格式：HH:mm:ss</param>
        /// <returns>DailyExecution</returns>
        public static DailyExecution WithDailyExecution(string time)
        {
            return new DailyExecution(time);
        }

        /// <summary>
        /// 新建循环运行触发器<see cref="CycExecution"/>
        /// </summary>
        /// <param name="executeTime">首次运行时间</param>
        /// <param name="period">运行间隔时间</param>
        /// <returns>CycExecution</returns>
        public static CycExecution WithCycExecution(DateTime executeTime, TimeSpan period)
        {
            return new CycExecution(executeTime, period);
        }

        /// <summary>
        /// 新建循环运行触发器<see cref="CycExecution"/>
        /// 首次运行时 是DateTime.Now即 当前时间
        /// </summary>
        /// <param name="period">运行间隔时间</param>
        /// <returns>CycExecution</returns>
        public static CycExecution WithCycExecutionNow(TimeSpan period)
        {
            return new CycExecution(period);
        }
    }
}
