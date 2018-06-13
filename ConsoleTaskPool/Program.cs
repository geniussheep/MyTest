using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading;
using System.Threading.Tasks;
using Benlai.Common;
using ConsoleTaskPool.QueuePool;
using ConsoleTaskPool.ScheduleTask;
using ConsoleTaskPool.TaskPoolService;

namespace ConsoleTaskPool
{
    class Program
    {
//
//        int threadPollMaxNUM = 3;   //最大线程数
//        Thread thread3;
//        Mutex mutex;
//        object lockObject = new object();
//
//        public Program()
//        {
//            mutex = new Mutex();
//        }
//
//        static void Main(string[] args)
//        {
//            Program p = new Program();
//            p.RunThread();
//
//        }
//        public void RunThread()
//        {
//            for (int i = 0; i < threadPollMaxNUM; i++)
//            {
//                thread3 = new Thread(new ThreadStart(testFunc_three));
//                thread3.Name = "thread--" + i;
//                thread3.Start();
//            }
//            string re = Console.ReadLine();
//            if (re == "exit")
//            {
//                running = false;
//            }
//        }
//
//        //以下是弹性处理任务的模型
//
//        bool running = true;
//
//        private void testFunc_three()
//        {
//
//            while (running)
//            {
//
//                mutex.WaitOne();
//                //bool _havaTask = false;
//                //lock (lockObject)
//                //{
//                int ix = 0; //仅作为自动退出用，实际用时不需要
//
//                bool _havaTask = false;
//                while (_havaTask == false && running)
//                {
//                    Console.WriteLine("{0} checking...", Thread.CurrentThread.Name);
//                    _havaTask = getOneTask();
//                    if (_havaTask == false)
//                    {
//                        //real sleep
//                        Thread.Sleep(4000);
//
//                        ix++;
//                        if (ix > 5) { running = false; }//仅作为自动退出用，实际用时不需要
//                    }
//                }
//                //}
//                //'' ""
//                mutex.ReleaseMutex();
//
//                if (_havaTask)
//                {
//                    Console.WriteLine("{0} working...", Thread.CurrentThread.Name);
//
//                    //do some thing....
//                    Thread.Sleep(5000);
//
//                    Console.WriteLine("{0} end !", Thread.CurrentThread.Name);
//                }
//
//            }
//
//
//
//            Console.WriteLine("{0} exit !", Thread.CurrentThread.Name);
//        }
//
//        int u = 0;
//        /// <summary>
//        /// 假设这是一个拉任务的方法
//        /// </summary>
//        /// <returns></returns>
//        private bool getOneTask()
//        {
//            if (u < 6)
//            {
//                u++;
//                Thread.Sleep(1000);
//                return true;
//            }
//            else
//            {
//                //Thread.Sleep(30 * 1000);
//                //running = false;
//                return false;
//            }
//        }


    static void Main(string[] args)
        {
//            TaskPoolServiceRun();
            ScheduleTaskRun();
        }

        static void TaskPoolServiceRun()
        {
            var rnd = new Random();
            var lst = new List<TaskModel>();
            var taskPool = new TaskPool(5, 4);
            taskPool.OnCancelled += () =>
            {
                LogInfoWriter.GetInstance().Info($"Thread -- {Thread.CurrentThread.ManagedThreadId} -- On Cancelled");
            };
            for (var i = 0; i < 100; i++)
            {
                var s = rnd.Next(4,10);

                var j = i;
                var isException = false;//s % 4 == 0;
                var isCancel = i == 10;
                var testTaskModel = new TaskModel(new Action(() =>
                {
                    if (isCancel)
                    {
                        taskPool.Cancel();
                    }
                    LogInfoWriter.GetInstance().Info($"Thread -- {Thread.CurrentThread.ManagedThreadId} -- 第{j}个任务（用时{s}秒）{(isException ? "有异常" : "")}  已经开始 " );
                    Thread.Sleep(s * 1000);
                    LogInfoWriter.GetInstance().Info($"Thread -- {Thread.CurrentThread.ManagedThreadId} -- 第{j}个任务（用时{s}秒）{(isException ? "有异常" : "")}  已经结束");
                    if (isException)
                    {
                        throw new Exception("this task is failed");
                    }
                }))
                {
                    TaskName = $"第{j}个任务（用时{s}秒）{(isException ? "有异常" : "")}",
                };
                lst.Add(testTaskModel);
            }

            taskPool.SetTaskInfoList(lst);
            taskPool.Start();

            Console.WriteLine();
            Console.ReadLine();
        }

        static void ScheduleTaskRun()
        {
            var 测试任务 = new Action(() =>
            {
                var j = 1;
                var s = 2;
                Console.WriteLine($"ThreadId{Thread.CurrentThread.ManagedThreadId}--第{j}个任务（用时{s}秒）已经开始 ");
                Thread.Sleep(s * 1000);
                Console.WriteLine($"ThreadId{Thread.CurrentThread.ManagedThreadId}--第{j}个任务（用时{s}秒）已经结束");
            });

            var scheduleTask =
                new ScheduleTask.ScheduleTask(TriggerBuilder.WithDailyExecution(1,0,0), new ScheduleTask.ScheduleTask.Job(测试任务));

            Task.Run(() =>
            {
                while (true)
                {
                    Thread.Sleep(1000);
                Console.WriteLine($"ThreadId{Thread.CurrentThread.ManagedThreadId}-- DT:{DateTime.Now:HH:mm:ss} ");

                }
            });
            scheduleTask.Start();
            Console.ReadLine();

           
        }

        static void TaskPoolRun()
        {
            var rnd = new Random();
            var lst = new MyTaskList();
            for (var i = 0; i < 100; i++)
            {
                var s = rnd.Next(10);
                var j = i;
                var 测试任务 = new Action(() =>
                {
                    var isException = s % 5 == 0;
                    Console.WriteLine($"ThreadId{Thread.CurrentThread.ManagedThreadId}--第{j}个任务（用时{s}秒）已经开始 "+(isException?"":"有异常"));
                    Thread.Sleep(s * 1000);
                    Console.WriteLine($"ThreadId{Thread.CurrentThread.ManagedThreadId}--第{j}个任务（用时{s}秒）已经结束");
                    if (isException)
                    {
                        throw new Exception("this task is failed");
                    }
                });
                lst.Tasks.Add(测试任务);
            }
            lst.Completed += () => Console.WriteLine("____________________没有更多的任务了！");
            lst.Start();

            Console.ReadLine();
        }

        static void QueuePoolRun()
        {
            QueueServicePool.Instacne.Init(3);//初始化3条线程&队列
            Process p = new Process();
            Thread t1 = new Thread(new ThreadStart(p.Run));
            Thread t2 = new Thread(new ThreadStart(p.Run));
            Thread t3 = new Thread(new ThreadStart(p.Run));
            Thread t4 = new Thread(new ThreadStart(p.Run));
            Thread t5 = new Thread(new ThreadStart(p.Run));
            Thread t6 = new Thread(new ThreadStart(p.Run));
            t1.Start();
            t2.Start();
            t3.Start();
            t4.Start();
            t5.Start();
            t6.Start();
            Console.ReadLine();
        }

        public class Process
        {
            public void Run()
            {
                for (int i = 0; i < 2; i++)
                {
                    QueueModel m = new QueueModel();
                    Random ran = new Random();
                    m.id = ran.Next(0, 10001);
                    m.p1 = Guid.NewGuid().ToString("N");
                    QueueServicePool.Instacne.AddTask(m);
                }
            }
        }
    }
}
