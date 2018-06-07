using System;
using System.Collections.Generic;
using System.Linq;

namespace ConsoleTaskPool.QueuePool
{
    public class QueueServicePool
    {
        public static QueueServicePool Instacne { get; } = new QueueServicePool();


        private QueueServicePool()
        {
            this.ServiceList = new List<QueueService>();
        }


        private readonly List<QueueService> ServiceList;


        public void Init(int threadNumbers)
        {
            for (int i = 0; i < threadNumbers; i++)
            {
                ServiceList.Add(new QueueService());
            }
        }


        public void AddTask(QueueModel model)
        {
            List<int> taskNumberList = new List<int>();
            foreach (QueueService serv in this.ServiceList)
            {
                taskNumberList.Add(serv.CurrentQueueCount);
            }


            ServiceList[taskNumberList.IndexOf(taskNumberList.Min())].AddToQueue(model);
        }
    }
}
