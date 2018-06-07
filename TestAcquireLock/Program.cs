using Benlai.Common.Redis;
using System;
using System.Collections;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading;
using System.Threading.Tasks;
using Benlai.Common.Redis.Units;
using ServiceStack.Common.Extensions;

namespace TestAcquireLock
{

    public class TestModel
    {
        public int value { get; set; }

        public List<String> Comm;
        public List<String> Gc;

        public override string ToString()
        {
            return
                $"Value:{value}, Comm:{Comm.Aggregate((r, s) => r + "," + s)}, Gc:{Gc.Aggregate((r, s) => r + "," + s)}";
        }
    }

    public class TestClass
    {
        private static TestClass _instance;

        public static TestClass SingleInstance() => _instance ?? (_instance = new TestClass());

        public TestModel TestMethod(int value, List<string> commList, List<string> gcList)
        {
            value +=new Random(10).Next(1,100);

            commList = commList.Select(s => s + value).ToList();

            gcList = gcList.Select(s => s + s.Length).ToList();

            var result = new TestModel
            {
                value = value,
                Comm = commList,
                Gc = gcList
            };

            Task.Factory.StartNew(() =>
            {
                Console.WriteLine($"SubThread:{Thread.CurrentThread.ManagedThreadId}:{result}");
            });

            return result;
        }
    }


    class Program
    {

//        private static readonly object LockObj = new object();
//
//        public static void AcquireLock(string localKey, int sleep)
//        {
//            while (true)
//            {
//                bool isGetLock = false;
//                Monitor.TryEnter(LockObj, ref isGetLock);
//                if (isGetLock)
//                {
//                        Console.WriteLine(@"ThreadId--" + Thread.CurrentThread.ManagedThreadId + @"-" + localKey + "\t get lock");
//                    try
//                    {
//                        Console.WriteLine(@"ThreadId--" + Thread.CurrentThread.ManagedThreadId + @"-" + localKey+ "\t will go AcquireLock");
//                        using (RedisEntities.Cache.AcquireLock(localKey, TimeSpan.FromSeconds(2)))
//                        {
//                            Console.WriteLine(@"ThreadId--" + Thread.CurrentThread.ManagedThreadId + @"-" + localKey + "\t in AcquireLock");
//                        }
//                        Thread.Sleep(sleep);
//                    }
//                    catch (Exception ex)
//                    {
//                        Console.WriteLine(Thread.CurrentThread.ManagedThreadId + " " + ex.ToString());
//                    }
//                    finally
//                    {
//                        Console.WriteLine(@"ThreadId--" + Thread.CurrentThread.ManagedThreadId + @"-" + localKey + "\t out AcquireLock");
//
//                    }
//                }
//                else
//                {
//                    Thread.Sleep(sleep - 500);
//                    Console.WriteLine(@"ThreadId--" + Thread.CurrentThread.ManagedThreadId + @"-" + localKey + "\t can not get lock");
//                }
//            }
//        }
//
//        static void Main(string[] args)
//        {
//            for (int i = 0; i < 3; i++)
//            {
//                var time = DateTime.Now.ToString("yyyyMMddHHmms");
//                Task testTask = new Task(() =>
//                {
//                    AcquireLock(time, 500 * (i+2));
//                });
//                testTask.Start();
//            }
//
//            Console.ReadKey();
//        }

        private static HashSet<string> hsHashSet = new HashSet<string>();

        private static readonly ReaderWriterLockSlim MappersLock = new ReaderWriterLockSlim();

        [STAThread]
        static void Main(string[] args)
        {
            //            ArrayList list = new ArrayList();
            //            list.Add("aaaa");
            //            list.Add("bbbb");
            //
            //            var gcList = new List<string>() {"1324g", "23423234d"};
            //
            //            Task.Factory.StartNew(() =>
            //            {
            //                var result = TestClass.SingleInstance().TestMethod(Thread.CurrentThread.ManagedThreadId,
            //                    list.ToList<string>(),
            //                    gcList);
            //                Thread.Sleep(2000);
            //                Console.WriteLine($"{Thread.CurrentThread.ManagedThreadId}:{result}");
            //            });
            //
            //            Task.Factory.StartNew(() =>
            //            {
            //                var result = TestClass.SingleInstance().TestMethod(Thread.CurrentThread.ManagedThreadId,
            //                    list.ToList<string>(),
            //                    gcList);
            //                Thread.Sleep(1300);
            //                Console.WriteLine($"{Thread.CurrentThread.ManagedThreadId}:{result}");
            //            });
            //
            //            Task.Factory.StartNew(() =>
            //            {
            //                var result = TestClass.SingleInstance().TestMethod(Thread.CurrentThread.ManagedThreadId,
            //                    list.ToList<string>(),
            //                    gcList);
            //                Thread.Sleep(3400);
            //                Console.WriteLine($"{Thread.CurrentThread.ManagedThreadId}:{result}");
            //            });

            var random  = new Random(1);

            Task.Factory.StartNew(() =>
            {
                while (true)
                {
                    MappersLock.EnterWriteLock();
                    Console.WriteLine($"enter Write{Thread.CurrentThread.ManagedThreadId}...");
                    try
                    {
                        var num = random.Next(1, 40).ToString();
                        hsHashSet.Add(num);
                        Console.BackgroundColor = ConsoleColor.Blue;
                        Console.WriteLine($"Write{Thread.CurrentThread.ManagedThreadId}:Add --{num}");
                        Thread.Sleep(100);
                        num = random.Next(1, 10).ToString();
                        if (hsHashSet.Contains(num))
                        {
                            hsHashSet.Remove(num);
                            Console.BackgroundColor = ConsoleColor.Yellow;
                            Console.WriteLine($"Write{Thread.CurrentThread.ManagedThreadId}:----------------------- Del --{num}");
                        }
                    }
                    catch (Exception e)
                    {
                        Console.WriteLine($"Write{Thread.CurrentThread.ManagedThreadId} error: --{e}");
                    }
                    finally
                    {
                        MappersLock.ExitWriteLock();
                    }
                    Thread.Sleep(100);
                }
            });


            for (int i = 0; i < 4; i++)
            {
                Task.Factory.StartNew(() =>
                {
                    while (true)
                    {
                        MappersLock.EnterReadLock();
                        Console.WriteLine($"enter Read{Thread.CurrentThread.ManagedThreadId}...");
                        try
                        {

                            foreach (var item in hsHashSet)
                            {
                                Console.BackgroundColor = ConsoleColor.Red;
                                Console.WriteLine($"Read{Thread.CurrentThread.ManagedThreadId}: -- {item}");
                            }

                        }
                        catch (Exception e)
                        {
                            Console.WriteLine($"Read{Thread.CurrentThread.ManagedThreadId} error: --{e}");
                        }
                        finally
                        {
                            MappersLock.ExitReadLock();
                        }
                        Thread.Sleep(10);

                    }
                });
            }

            Console.ReadLine();
        }
    }
}
