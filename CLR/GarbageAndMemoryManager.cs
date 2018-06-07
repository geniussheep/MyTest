using System;
using System.Diagnostics;
using System.IO;
using System.Runtime;
using System.Runtime.CompilerServices;
using System.Runtime.InteropServices;
using System.Threading;
using Microsoft.Win32.SafeHandles;

namespace ConsoleApp
{
    public static class GCNotification
    {
        private static Action<int> _sGcDone = null;//GC完成事件的字段

        public static event Action<int> GCDone
        {
            add
            {
                if (_sGcDone == null)
                {
                    new GenObject(0);
                    new GenObject(1);
                }
                _sGcDone += value;
            }
            remove { _sGcDone -= value; }
        }
 
        private sealed class GenObject
        {
            private int m_generation;

            public GenObject(int generation)
            {
                m_generation = generation;
            }

            ~GenObject()
            {
                //若果这个对象在我们希望的（或更高的）代中，就通知委托一次GC刚刚完成
                if (GC.GetGeneration(this) >= m_generation)
                {
                    Action<int> temp = Interlocked.CompareExchange(ref _sGcDone, null, null);
                    if (temp != null)
                    {
                        temp(m_generation);
                    }
                    //如果至少还有一个已登记的委托，且appdomain并未正在卸载，且进程并非正在关闭，就就继续通知GC
                    if (_sGcDone != null 
                        && !AppDomain.CurrentDomain.IsFinalizingForUnload() 
                        && Environment.HasShutdownStarted)
                    {
                        //对第0代对象，创建一个新对象，对第2代，复活对象
                        if (m_generation == 0) new GenObject(0);
                        else GC.ReRegisterForFinalize(this);
                    }
                }
                else
                {
                    //什么也不做，让GC回收
                }
            }
        }
    }


   

    public partial class Program
    {
        //在程序中用一个计时器，每隔几秒钟调用一次该函数，打开任务管理器，你会有惊奇的发现

        public static void MainGarbageTest()
        {
            //Test1();
            //Test2();
            //Test3();//打开优化代码 和关闭优化代码 不同结果
            //Test4();
            //Console.ReadLine();

            //new GCBeep();

            //for (int i = 0; i < 100000; i++)
            //{
            //    //Console.WriteLine(i);
            //    var a = new { A = int.MaxValue };
            //    var b = new { A = int.MaxValue };
            //    var c = new { A = int.MaxValue };
            //    var d = new { A = int.MaxValue };
            //    var e = new { A = int.MaxValue };
            //    var f = new { A = int.MaxValue };
            //    var g = new { A = int.MaxValue };
            //    var q = new { A = int.MaxValue };
            //    var p = new { A = int.MaxValue };
            //    var h = new { A = int.MaxValue };
            //    var u = new { A = int.MaxValue };
            //}
            //Console.WriteLine("done");

           // var tempFile = new TempFile("123456");

            //Test5();
            //Test6();

            GCNotification.GCDone += g =>
            {
                Console.WriteLine(g);
                Console.Beep();
                //获得当前工作进程
                Process proc = Process.GetCurrentProcess();

                long usedMemory = proc.PrivateMemorySize64;
                Console.WriteLine("Current Memory Size {0}", (decimal)usedMemory/1024);
                Console.Read();
            };

            //MemoryPressureDemo(0);
            MemoryPressureDemo(10*1024*1024);

            //HandleCollectionDemo();
            var obj = new object();
            GCHandle.Alloc(obj,GCHandleType.Weak);//用于监控对象的生存期

            var isServerGC = GCSettings.IsServerGC;//是否是服务器模式的回收
            //服务器GC模式的默认值，在工作站GC模式中，这个延迟模式关闭并发GC，在服务器模式中，这是唯一的有效延迟模式
            GCSettings.LatencyMode = GCLatencyMode.Batch;
            //工作站GC模式的默认值，在工作站GC模式中，这个延迟模式打开并发GC，在服务器模式中，这是无效延迟模式
            GCSettings.LatencyMode = GCLatencyMode.Interactive;

            //在工作站GC模式中，短期的、时间敏感的操作中（比如动画绘制）使用这个延迟模式；但这种模式下可能会对第二代回收造成混乱，在服务器模式中，这是无效延迟模式
            GCSettings.LatencyMode = GCLatencyMode.LowLatency;
            GCSettings.LatencyMode = GCLatencyMode.SustainedLowLatency;

            //垃圾回收监视
            GC.GetTotalMemory(true);//检查托管堆中当前使用了多少内存
            GC.CollectionCount(0);//检查某一代发生了多少次垃圾回收
            Console.ReadLine();
        }

        /// <summary>
        /// 如何正确使用LowLatency模式
        /// </summary>
        private static void LowLatencyDemo()
        {
            GCLatencyMode oldMode = GCSettings.LatencyMode;
            RuntimeHelpers.PrepareConstrainedRegions(); //将代码指定为受约束的区域
            try
            {
                GCSettings.LatencyMode = GCLatencyMode.LowLatency;
            }
            finally
            {
                GCSettings.LatencyMode = oldMode;
            }
        }

        private static void MemoryPressureDemo(int size)
        {
            Console.WriteLine();
            Console.WriteLine("MemoryPressureDemo,Size = {0}",size);
            for (int count = 0; count < 15; count++)
            {
                new BigNativeResource(size);
                //出于演示目的，强制回收
                GC.Collect();
                GC.WaitForPendingFinalizers();
            }
        }

        private sealed class BigNativeResource
        {
            private int m_size;

            public BigNativeResource(int size)
            {
                m_size = size;
                if (m_size>0)
                {
                    GC.AddMemoryPressure(m_size);
                }
                Console.WriteLine("BigNativeResource created");
            }

            ~BigNativeResource()
            {
                if (m_size>0)
                {
                    GC.RemoveMemoryPressure(m_size);
                }
                Console.WriteLine("BigNativeResource destoried");
            }
        }

        private static void HandleCollectionDemo()
        {
            Console.WriteLine();
            Console.WriteLine("HandleCollectionDemo");

            for (int count = 0; count < 10; count++)
            {
                new LimitedResource();
            }
            GC.Collect();
            GC.WaitForPendingFinalizers();
        }

        private sealed class LimitedResource
        {
            private static HandleCollector s_hc=new HandleCollector("LimitedResource",2);

            public LimitedResource()
            {
                //告诉HandleCollector堆中又增加了一个LimitedResource
                s_hc.Add();
                Console.WriteLine("LimitedResource count={0}",s_hc.Count);
            }

            ~LimitedResource()
            {
                //告诉HandleCollector堆中又增减少了一个LimitedResource
                s_hc.Remove();
                Console.WriteLine("LimitedResource count={0}", s_hc.Count);
            }

        }


        private static void Test1()
        {
            Timer t = new Timer(TimeCallBack, "Test1", 0, 2000);

            Console.ReadLine();
        }

        private static void Test2()
        {
            Timer t = new Timer(TimeCallBack, "Test2", 0, 2000);
            GC.Collect();

            Console.ReadLine();
        }


        private static void Test3()
        {
            Timer t = new Timer(TimeCallBack, "Test3", 0, 2000);

            Console.ReadLine();
           
            t = null;
        }

        private static void Test4()
        {
            Timer t = new Timer(TimeCallBack, "Test4", 0, 2000);

            Console.ReadLine();

            t.Dispose();

        }

        private static void TimeCallBack(object o)
        {
            Console.WriteLine("In {0} timecallback:{1}",o.ToString(), DateTime.Now);
            GC.Collect();
            Console.WriteLine("GCCollect count:"+GC.CollectionCount(0));
        }

        unsafe public static void Test5()
        {
            for (int i = 0; i < 10000; i++)
            {
                new object();
            }
            IntPtr originalMemoryAddress;
            byte[] bytes = new byte[1000];

            fixed (byte* pbytes = bytes)
            {
                originalMemoryAddress = (IntPtr) pbytes;
            }

            GC.Collect();

            fixed (byte* pbytes = bytes)
            {
                Console.WriteLine("the byte[] did{0} move during th gc,originalMemoryAddress = {1} pbytes={2}",
                    originalMemoryAddress == (IntPtr)pbytes ? "not" : null, originalMemoryAddress, (IntPtr)pbytes);
            }
        }

        public static void Test6()
        {
            object o = new object().GCWatch(String.Format("my object created at {0}", DateTime.Now));

            o = null;
            Thread.Sleep(2000);
            GC.Collect();

            o = new object().GCWatch(String.Format("my object created at {0}", DateTime.Now));
            GC.KeepAlive(o);

            o = null;
            GC.Collect();
        }

    }

    internal static class GCWatcher
    {
        private readonly static ConditionalWeakTable<object,NotifyWhenGCd<string>> s_cwt = new ConditionalWeakTable<object, NotifyWhenGCd<string>>();
        private sealed class NotifyWhenGCd<T>
        {
            private readonly T m_value;
            internal NotifyWhenGCd(T value)
            {
                m_value = value;
            }

            public override string ToString()
            {
                return m_value.ToString();
            }

            ~NotifyWhenGCd()
            {
                Console.WriteLine("GC'd:{0}", m_value);
            }
        }

        public static T GCWatch<T>(this T @object, string tag) where T : class
        {
            s_cwt.Add(@object,new NotifyWhenGCd<string>(tag));
            return @object;
        }
    }

    internal static class SomeType
    {
        //这个原型不健壮
        [DllImport("Kernel32", CharSet = CharSet.Unicode, EntryPoint = "CreateEvent")]
        private static extern IntPtr CreateEventBad(
            IntPtr pSecurityAttributes, bool manualReset, 
            bool initialState,string name);

        //这个原型是健壮的
        [DllImport("Kernel32", CharSet = CharSet.Unicode, EntryPoint = "CreateEvent")]
        private static extern SafeWaitHandle CreateEventGood(
            IntPtr pSecurityAttributes, bool manualReset,
            bool initialState, string name);

        public static void SomeMethod()
        {
            IntPtr handle = CreateEventBad(IntPtr.Zero, false, false, null);
            SafeWaitHandle swh = CreateEventGood(IntPtr.Zero, false, false, null);
        }
    }

    internal sealed class GCBeep
    {
        ~GCBeep()
        {
            //在垃圾回收的时候会发出beep的声音
            Console.Beep();

            if (!AppDomain.CurrentDomain.IsFinalizingForUnload()&& !Environment.HasShutdownStarted)
            {
                new GCBeep();
            }
        }
    }

    internal sealed class TempFile
    {
        private string m_fileName;
        private FileStream m_fs;

        public TempFile(string fileName)
        {
            try
            {
                m_fs = new FileStream(fileName,FileMode.Create);
                //throw new Exception("故意出错");
                m_fileName = fileName;
            }
            catch
            {
                //发生错误，告诉GC不要调用Finalize方法
                //GC.SuppressFinalize(this);
            }
        }

        ~TempFile()
        {
            //Console.Beep();
            if (!String.IsNullOrEmpty(m_fileName))
            {
                File.Delete(m_fileName);
            }
        }
    }

    
}
