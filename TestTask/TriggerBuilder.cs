namespace Benlai.AutoPublish.Utils.Task
{
    public class TriggerBuilder
    {
        public static DailyExecution WithDailyExecution(int hour, int minute, int second)
        {
            return new DailyExecution(hour,minute,second);
        }

        public static DailyExecution WithDailyExecution(string time)
        {
            return new DailyExecution(time);
        }
    }
}
