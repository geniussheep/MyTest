using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading;
using System.Threading.Tasks;

namespace ConsoleTaskPool
{
    public class TracingThreadPool
    {
        // 线程池是否关闭
        private bool isClosed = false;
        // 表示工作队列
        private LinkedList<Task> workQueue;
        // 表示线程池ID
        private static int threadPoolID;
        // 表示工作线程ID
        private int threadID;

        // poolSize指定线程池中的工作线程数目
        public TracingThreadPool(int poolSize)
        {
//            super("ThreadPool-" + (threadPoolID++));
//            SetDaemon(true);
            // 创建工作队列
            workQueue = new LinkedList<Task>();
            // 打印起始时间
            Console.WriteLine("start time:" + DateTime.Now);

            for (int i = 0; i < poolSize; i++)
                // 创建并启动工作线程
                new WorkThread().start();
        }


        public void AddTask(Task task)
        {
            // 线程池被关则抛出IllegalStateException异常
            if (isClosed)
            {
                throw new IllegalStateException();
            }
            if (task != null)
            {
                workQueue.AddLast(task);
                // 唤醒正在getTask()方法中等待任务的工作线程
                Notify();
            }
        }


      protected Task getTask() //throws InterruptedException
        {
  while (!workQueue.Any()) {
                if (isClosed)
                    return null;
                // 如果工作队列中没有任务，就等待任务
                Wait();
            }
  var result = workQueue.First();
            workQueue.RemoveFirst();
            return result;
        }


        public void Close()
        {
            if (!isClosed)
            {
                isClosed = true;
                workQueue.Clear(); // 清空工作队列
                Interrupt(); // 中断所有的工作线程，该方法继承自ThreadGroup类
            }
        }


        public void Join()
        {
            synchronized(this) {
                isClosed = true;
                // 唤醒还在getTask()方法中等待任务的工作线程
                NotifyAll();
            }

            Thread[] threads = new Thread[ActiveCount()];
            //获得线程组中当前所有活着的工作线程
            int count = Enumerate(threads);
            // 等待所有工作线程运行结束
            for (int i = 0; i < count; i++)
            {
                try
                {
                    // 等待工作线程运行结束
                    threads[i].Join();
                }
                catch (InterruptedException ex)
                {
                }
            }
            //打印结束时间
            Console.WriteLine("end time:" + (new Date()));
        }


        private class WorkThread : Thread
        {
  public WorkThread():base()
        {
            // 加入到当前ThreadPool线程组中
           TracingThreadPool.this, "WorkThread-" + (threadID++));
        }

        public void run()
        {
            while (!IsAlive())
            { // isInterrupted()方法继承自Thread类，判断线程是否被中断
                Runnable task = null;
                try
                {
                    // 得到任务
                    task = getTask();
                }
                catch (InterruptedException ex)
                {
                }

                // 如果getTask()返回null或者线程执行getTask()时被中断，则结束此线程
                if (task == null)
                    return;

                try
                {
                    // 运行任务，捕获异常
                    task.run();
                }
                catch (Throwable t)
                {
                    t.printStackTrace();
                }
            }
        }
    }
}
