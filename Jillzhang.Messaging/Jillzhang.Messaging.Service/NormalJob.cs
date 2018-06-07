using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using Jillzhang.Messaging.Contract;

namespace Jillzhang.Messaging.Service
{
    public class NormalJob:INormalJob
    {
        public string Do(string jobName)
        {
            try
            {              
                System.Diagnostics.Stopwatch watcher = new System.Diagnostics.Stopwatch();
                watcher.Start();
                System.Threading.Thread.Sleep(1000);
                Console.WriteLine("服务" + AppDomain.CurrentDomain.FriendlyName + "执行任务:" + jobName);
                watcher.Stop();              
                return "成功";
            }
            catch
            {
                return "失败";
            }
        }
    }
}
