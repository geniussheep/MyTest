using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using Jillzhang.Messaging.Contract;
using System.ServiceModel;

namespace Jillzhang.Messaging.Service
{
   [ServiceBehavior(ConcurrencyMode=ConcurrencyMode.Multiple)]
    public class Job:IJob
    {
        public string Do(string jobName)
        {
            try
            {
                ICallback callback = OperationContext.Current.GetCallbackChannel<ICallback>();
                System.Diagnostics.Stopwatch watcher = new System.Diagnostics.Stopwatch();
                watcher.Start();
                System.Threading.Thread.Sleep(1000);
                Console.WriteLine("服务" + AppDomain.CurrentDomain.FriendlyName + "执行任务:" + jobName);
                watcher.Stop();
                callback.Done((int)watcher.ElapsedMilliseconds);
                return "成功";
            }
            catch 
            {
                return "失败";
            }
        }
    }
}
