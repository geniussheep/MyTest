using System;
using System.Collections.Generic;
using System.Linq;
using System.Net;
using System.Text;
using System.Threading;
using System.Threading.Tasks;

namespace ConsoleApp
{
    public partial class Program
    {
        private static void Main基元线程同步(string[] args)
        {
            //StrangeBehavior.MainRun();

            //以下三个结果都可能与预期不同呀？？？？
            //new ThreadsSharingData().MainRun();
            //Console.Read();

            //new ThreadsSharingData_VolatileReadAndWrite().MainRun();
            //Console.Read();

            //new ThreadsSharingData_VolatileField().MainRun();
            //Console.Read();

            //new ThreadsSharingData_InterLocked().MainRun();
            //Console.Read();

            new MultiWebRequests();
            Console.Read();
        }
    }

    #region 易失构造

    /// <summary>
    /// X86的release下运行
    /// </summary>
    public static class StrangeBehavior
    {
        //易失字段
        //private volatile static bool s_stopWorker = false;
        private static bool s_stopWorker = false;

        public static void MainRun()
        {
            Console.WriteLine("StrangeBehavior.MainRun : letting worker run for 2 seconds");
            Thread t = new Thread(Worker);
            t.Start();
            Thread.Sleep(2000);
            s_stopWorker = true;
            Console.WriteLine("StrangeBehavior.MainRun :waiting for worker to stop");
            t.Join();
            Console.WriteLine("StrangeBehavior.MainRun finished");
        }

        private static void Worker(object o)
        {
            int x = 0;
            while (!s_stopWorker)
            {
                x++;
            }
            Console.WriteLine("Worker : stopped when x={0}", x);
        }

    }

    /// <summary>
    /// X86的release下运行
    /// </summary>
    public sealed class ThreadsSharingData
    {
        private int flag = 0;
        private int value = 0;

        public void Thread1()
        {
            Console.WriteLine("Thread1 : set value and flag");
            value = 5;
            flag = 1;
            unsafe
            {
                fixed (int* address = &flag)
                {
                    System.Console.WriteLine("The address stored in flag: {0}", (int)address);
                }
            }
        }

        public void Thread2()
        {
            unsafe
            {
                fixed (int* address = &flag)
                {
                    System.Console.WriteLine("The address stored in flag: {0}", (int)address);
                }
            }
            if (flag == 1)
            {
                Console.WriteLine("Thread2 : value={0}", value);
            }
        }

        public void MainRun()
        {
            Thread t1 = new Thread(Thread1);
            Thread t2 = new Thread(Thread2);
            t1.Start();
            t2.Start();
            Console.WriteLine("ThreadsSharingData.MainRun finished");
        }
    }

    /// <summary>
    /// X86的release下运行
    /// </summary>
    public sealed class ThreadsSharingData_VolatileReadAndWrite
    {
        private int flag = 0;
        private int value = 0;

        public void Thread1()
        {
            Console.WriteLine("Thread1 : set value and flag");
            value = 5;
            Thread.VolatileWrite(ref flag, 1);
            unsafe
            {
                fixed (int* address = &flag)
                {
                    System.Console.WriteLine("The address stored in flag: {0}", (int)address);
                }
            }
        }

        public void Thread2()
        {
            unsafe
            {
                fixed (int* address = &flag)
                {
                    System.Console.WriteLine("The address stored in flag: {0}", (int)address);
                }
            }
            if (Thread.VolatileRead(ref flag) == 1)
            {
                Console.WriteLine("Thread2 : value={0}", value);
            }
        }

        public void MainRun()
        {
            Thread t1 = new Thread(Thread1);
            Thread t2 = new Thread(Thread2);
            t1.Start();
            t2.Start();
            Console.WriteLine("ThreadsSharingData_VolatileReadAndWrite.MainRun finished");
        }
    }

    /// <summary>
    /// X86的release下运行
    /// </summary>
    public sealed class ThreadsSharingData_VolatileField
    {
        private volatile int flag = 0;
        private int value = 0;

        public void Thread1()
        {
            Console.WriteLine("Thread1 : set value and flag");
            value = 5;
            flag = 1;
            unsafe
            {
                fixed (int* address = &flag)
                {
                    System.Console.WriteLine("The address stored in flag: {0}", (int) address);
                }
            }
        }

        public void Thread2()
        {
            unsafe
            {
                fixed (int* address = &flag)
                {
                    System.Console.WriteLine("The address stored in flag: {0}", (int)address);
                }
            }
            if (flag == 1)
            {
                Console.WriteLine("Thread2 : value={0}", value);
            }
        }

        public void MainRun()
        {
            Thread t1 = new Thread(Thread1);
            Thread t2 = new Thread(Thread2);
            t1.Start();
            t2.Start();
            Console.WriteLine("ThreadsSharingData_VolatileField.MainRun finished");
        }
    }

    #endregion

    #region 互锁构造
    public sealed class ThreadsSharingData_InterLocked
    {
        private long flag = 0;
        private int value = 0;

        public void Thread1()
        {
            Console.WriteLine("Thread1 : set value and flag");
            value = 5;
            Interlocked.Exchange(ref flag, 1);
            unsafe
            {
                fixed (long* address = &flag)
                {
                    System.Console.WriteLine("The address stored in flag: {0}", (int)address);
                }
            }
        }

        public void Thread2()
        {
            unsafe
            {
                fixed (long* address = &flag)
                {
                    System.Console.WriteLine("The address stored in flag: {0}", (int)address);
                }
            }
            if (Interlocked.Read(ref flag) == 1)
            {
                Console.WriteLine("Thread2 : value={0}", value);
            }
        }

        public void MainRun()
        {
            Thread t1 = new Thread(Thread1);
            Thread t2 = new Thread(Thread2);
            t1.Start();
            Thread.Sleep(0);
            t2.Start();
            Console.WriteLine("ThreadsSharingData_InterLocked.MainRun finished");
        }
    }

    public sealed class MultiWebRequests
    {
        //辅助类用于协助所有异步操作
        private AsyncCoordinator m_ac= new AsyncCoordinator();

        //想要查询的所有web服务器集合
        private WebRequest[] m_requests = new WebRequest[]
        {
            WebRequest.Create("http://www.163.com"),
            WebRequest.Create("http://www.qq.com"),
            WebRequest.Create("http://www.baidu.com"),
            WebRequest.Create("http://www.sohu.com"),
        };

        //响应数组，每个请求一个响应
        private WebResponse[] m_results = new WebResponse[3];

        public MultiWebRequests(int timeout = Timeout.Infinite)
        {
            //以异步方式一次行发起所有请求
            for (int i = 0; i < m_requests.Length; i++)
            {
                m_ac.AboutToBegin(1);
                m_requests[i].BeginGetResponse(_endGetResponse, i);
            }
            //告诉辅助类所有操作已发起，并在所有操作完成、调用Cancel或者超时的时候调用AllDone
            m_ac.AllBegin(_allDone, timeout);
        }

        public void Cancel(){ m_ac.Cancel();}

        private void _endGetResponse(IAsyncResult result)
        {
            //获取请求的索引
            int ind = (int) result.AsyncState;

            Console.WriteLine("{0},Thread Id:{1}", m_requests[ind].RequestUri.AbsoluteUri, Thread.CurrentThread.ManagedThreadId);
            //将响应保存在请求的相同结果索引里
            m_results[ind] = m_requests[ind].EndGetResponse(result);

            //告诉辅助类一个Web请求已响应
            m_ac.JustEnded();
        }

        private void _allDone(CoordinationStatus status)
        {
            switch (status)
            {
                case CoordinationStatus.AllDone:
                    Console.WriteLine("here are the results from all the web servers");
                    for (int i = 0; i < m_requests.Length; i++)
                    {
                        Console.WriteLine("{0} returned {1} bytes", m_results[i].ResponseUri, m_results[i].ContentLength);
                    }
                    break;
                case CoordinationStatus.Cancel:
                    Console.WriteLine("the operatio was canceled");
                    break;
                case CoordinationStatus.Timeout:
                    Console.WriteLine("the operatio time-out");
                    break;
            }
        }
    }

    public class AsyncCoordinator
    {
        /// <summary>
        /// AllBegin内部调用JustEnd来多减去一次,一开始设置为1 保证AllDone不会执行
        /// </summary>
        private int m_opCount = 1;

        /// <summary>
        /// 0=false 1=true
        /// </summary>
        private int m_statusReported = 0;
        private Action<CoordinationStatus> m_callback = null;
        private Timer m_timer;

        /// <summary>
        /// 必须在所有BeginXXX方法之前调用
        /// </summary>
        /// <param name="opsToAdd"></param>
        public void AboutToBegin(int opsToAdd = 1)
        {
            Interlocked.Add(ref m_opCount, opsToAdd);
        }

        /// <summary>
        /// 必须在调用一个EndXXX后调用该方法
        /// </summary>
        public void JustEnded()
        {
            if (Interlocked.Decrement(ref m_opCount) == 0)
            {
                ReportStatus(CoordinationStatus.AllDone);
            }
        }

        /// <summary>
        /// 必须在调用一个BeginXXX之后调用这个方法
        /// </summary>
        /// <param name="callback"></param>
        /// <param name="timeout"></param>
        public void AllBegin(Action<CoordinationStatus> callback, int timeout = Timeout.Infinite)
        {
            m_callback = callback;
            if(timeout !=Timeout.Infinite)
                m_timer=new Timer(TimeExpired,null,timeout,Timeout.Infinite);
            JustEnded();
        }

        public void TimeExpired(object o)
        {
            ReportStatus(CoordinationStatus.Timeout);
        }

        public void Cancel()
        {
            ReportStatus(CoordinationStatus.Cancel);
        }

        public void ReportStatus(CoordinationStatus status)
        {
            //如果状态从未报告过就报告，已报告过就忽略
            if (Interlocked.Exchange(ref m_statusReported,1)==0)
            {
                m_callback(status);
            }
        }
    }

    public enum CoordinationStatus
    {
        AllDone,
        Timeout,
        Cancel
    }
    #endregion

}
