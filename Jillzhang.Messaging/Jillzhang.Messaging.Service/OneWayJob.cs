﻿using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using Jillzhang.Messaging.Contract;

namespace Jillzhang.Messaging.Service
{
    public class OneWayJob : IOneWayJob
    {
        public void Do(string jobName)
        {
            System.Diagnostics.Stopwatch watcher = new System.Diagnostics.Stopwatch();
            watcher.Start();
            System.Threading.Thread.Sleep(1000);
            Console.WriteLine("服务" + AppDomain.CurrentDomain.FriendlyName + "执行任务:" + jobName);
            watcher.Stop();
        }
    }
}
