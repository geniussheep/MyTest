//C#语言有很多值得学习的地方，这里我们主要介绍C# Mutex对象，包括介绍控制好多个线程相互之间的联系等方面。

//如何控制好多个线程相互之间的联系，不产生冲突和重复，这需要用到互斥对象，即：System.Threading 命名空间中的 Mutex 类。

//我们可以把Mutex看作一个出租车，乘客看作线程。乘客首先等车，然后上车，最后下车。当一个乘客在车上时，其他乘客就只有等他下车以后才可以上车。
//而线程与C# Mutex对象的关系也正是如此，线程使用Mutex.WaitOne()方法等待C# Mutex对象被释放，如果它等待的C# Mutex对象被释放了，它就自动拥有这个对象，
//直到它调用Mutex.ReleaseMutex()方法释放这个对象，而在此期间，其他想要获取这个C# Mutex对象的线程都只有等待。

//下面这个例子使用了C# Mutex对象来同步四个线程，主线程等待四个线程的结束，而这四个线程的运行又是与两个C# Mutex对象相关联的。

//其中还用到AutoResetEvent类的对象，可以把它理解为一个信号灯。这里用它的有信号状态来表示一个线程的结束。

using System;
using System.Threading;

namespace ConsoleApp
{
    public class MutexSample
    {
        private static Mutex gM1;
        private static Mutex gM2;
        private const int ITERS = 100;
        private static readonly AutoResetEvent Event1 = new AutoResetEvent(false);
        private static readonly AutoResetEvent Event2 = new AutoResetEvent(false);
        private static readonly AutoResetEvent Event3 = new AutoResetEvent(false);
        private static readonly AutoResetEvent Event4 = new AutoResetEvent(false);

        public static void MainMutexSample(String[] args)
        {
            Console.WriteLine("Mutex Sample ");
            //创建一个Mutex对象，并且命名为MyMutex  
            gM1 = new Mutex(true, "MyMutex");
            //创建一个未命名的Mutex 对象.  
            gM2 = new Mutex(true);
            Console.WriteLine(" - Main Owns gM1 and gM2");

            var evs = new AutoResetEvent[4];
            evs[0] = Event1; //为后面的线程t1,t2,t3,t4定义AutoResetEvent对象  
            evs[1] = Event2;
            evs[2] = Event3;
            evs[3] = Event4;

            var tm = new MutexSample();
            var t1 = new Thread(tm.t1Start);
            var t2 = new Thread(tm.t2Start);
            var t3 = new Thread(tm.t3Start);
            var t4 = new Thread(tm.t4Start);
            t1.Start(); // 使用Mutex.WaitAll()方法等待一个Mutex数组中的对象全部被释放  
            t2.Start(); // 使用Mutex.WaitOne()方法等待gM1的释放  
            t3.Start(); // 使用Mutex.WaitAny()方法等待一个Mutex数组中任意一个对象被释放  
            t4.Start(); // 使用Mutex.WaitOne()方法等待gM2的释放  

            Thread.Sleep(10000);
            Console.WriteLine(" - Main releases gM2");
            gM2.ReleaseMutex(); //线程t3,t4结束条件满足  

            Thread.Sleep(1000);
            Console.WriteLine(" - Main releases gM1");
            gM1.ReleaseMutex(); //线程t1,t2结束条件满足  

//等待所有四个线程结束  
            WaitHandle.WaitAll(evs);
            Console.WriteLine(" Mutex Sample");
            Console.ReadLine();
        }

        public void t1Start()
        {
            Console.WriteLine("t1Start started, Mutex.WaitAll(Mutex[])");
            var gMs = new Mutex[2];
            gMs[0] = gM1; //创建一个Mutex数组作为Mutex.WaitAll()方法的参数  
            gMs[1] = gM2;
            WaitHandle.WaitAll(gMs); //等待gM1和gM2都被释放  
            Thread.Sleep(2000);
            Console.WriteLine("t1Start finished, Mutex.WaitAll(Mutex[]) satisfied");
            Event1.Set(); //线程结束，将Event1设置为有信号状态  
        }

        public void t2Start()
        {
            Console.WriteLine("t2Start started, gM1.WaitOne( )");
            gM1.WaitOne(); //等待gM1的释放  
            Console.WriteLine("t2Start finished, gM1.WaitOne( ) satisfied");
            Event2.Set(); //线程结束，将Event2设置为有信号状态  
        }

        public void t3Start()
        {
            Console.WriteLine("t3Start started, Mutex.WaitAny(Mutex[])");
            var gMs = new Mutex[2];
            gMs[0] = gM1; //创建一个Mutex数组作为Mutex.WaitAny()方法的参数  
            gMs[1] = gM2;
            WaitHandle.WaitAny(gMs); //等待数组中任意一个Mutex对象被释放  
            Console.WriteLine("t3Start finished, Mutex.WaitAny(Mutex[])");
            Event3.Set(); //线程结束，将Event3设置为有信号状态  
        }

        public void t4Start()
        {
            Console.WriteLine("t4Start started, gM2.WaitOne( )");
            gM2.WaitOne(); //等待gM2被释放  
            Console.WriteLine("t4Start finished, gM2.WaitOne( )");
            Event4.Set(); //线程结束，将Event4设置为有信号状态  
        }
    }
} 