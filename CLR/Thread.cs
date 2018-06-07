using System;
using System.Collections.Generic;
using System.Diagnostics;
using System.IO;
using System.Linq;
using System.Reflection;
using System.Runtime.InteropServices;
using System.Text;
using System.Threading;
using System.Threading.Tasks;

namespace ConsoleApp
{
     class ThreadTest
     {
         public static void Run(object cts)
         {
             while (!(cts as CancellationTokenSource).IsCancellationRequested)
             
             {
                 Thread.Sleep(1000);
                 Console.WriteLine(System.Threading.Thread.CurrentThread.ManagedThreadId + " running");
             }
             Console.WriteLine(System.Threading.Thread.CurrentThread.ManagedThreadId + " stop");
         }
     }



     public partial class Program
     {
         static readonly CancellationTokenSource Cts = new CancellationTokenSource();
         static void MainThreadTest(string[] args)
         {
            //Task.Factory.StartNew(() =>
            //{
            //    Start(null);
            //});

            //Task.Factory.StartNew(() =>
            //{
            //    Thread.Sleep(10000);
            //    Stop();
            //});

            //CallContext.LogicalSetData("FName", "Jeffrey");
            //CallContext.LogicalSetData("LName", "Sheep");
            //ThreadPool.QueueUserWorkItem(state =>
            //{
            //    Console.WriteLine("FName {0},LName:{1},{2} running ", CallContext.LogicalGetData("FName"), CallContext.LogicalGetData("LName"),Thread.CurrentThread.ManagedThreadId);
            //});

            //ExecutionContext.SuppressFlow();
            //ThreadPool.QueueUserWorkItem(state =>
            //{
            //    Console.WriteLine("FName {0},LName:{1},{2} running ", CallContext.LogicalGetData("FName"), CallContext.LogicalGetData("LName"),Thread.CurrentThread.ManagedThreadId);
            //});

            //ExecutionContext.RestoreFlow();







            //协作取消();
            //取消任务();
            //任务按顺序执行();
            //子任务();

            任务工厂();

            //Parallel

            //TimerDemo();

            //TestThreadProptery();
             Console.ReadLine();
         }

         private static void 协作取消()
         {
            CancellationTokenSource cts = new CancellationTokenSource();
            //协作取消
            ThreadPool.QueueUserWorkItem(o =>
            {
                for (int i = 0; i < 1000; i++)
                {
                    if (cts.IsCancellationRequested)
                    {
                        Console.WriteLine(" thread {0} is stop!", Thread.CurrentThread.ManagedThreadId);
                        break;
                    }
                    Console.WriteLine(Thread.CurrentThread.ManagedThreadId.ToString() + ":" + i);
                    Thread.Sleep(200);
                }
                Console.WriteLine(" thread {0} is done!", Thread.CurrentThread.ManagedThreadId);
            });
            Console.WriteLine("press <Enter> to cancel the operation");
            Console.ReadLine();
            cts.Token.Register(s => { Console.WriteLine(s); }, "134");
            cts.Token.Register(s => { Console.WriteLine(s); }, "4554");
            cts.Token.Register(s => { Console.WriteLine(s); }, "7894");
            cts.Cancel();

        }

         private static void 取消任务()
         {
            CancellationTokenSource cts = new CancellationTokenSource();

            //取消任务
            Task<int> task_cancel = new Task<int>(() =>
            {
                int n = 10000;
                int sum = 0;
                for (; n > 0; n--)
                {
                    cts.Token.ThrowIfCancellationRequested();
                    checked
                    {
                        sum += n;
                        Console.WriteLine("sum is {0}", sum);
                    }
                }
                return sum;
            }, cts.Token);
            task_cancel.Start();
            Thread.Sleep(100);
            cts.Cancel();
            try
            {
                Console.WriteLine("thr sum is :{0}", task_cancel.Result);
            }
            catch (AggregateException ex)
            {
                Console.WriteLine("sum was canceled");
            }
        }

        private static void 任务按顺序执行()
         {
            //一个任务完成时自动启动一个新任务
            CancellationTokenSource cts = new CancellationTokenSource();
            Task<int> task_continue = new Task<int>(n =>
            {
                int temp = Convert.ToInt32(n);
                int sum = 0;
                for (; temp > 0; temp--)
                {
                    if (cts.Token.IsCancellationRequested) break;
                    checked
                    {
                        sum += temp;
                        Thread.Sleep(10);
                        Console.WriteLine("sum is {0}", sum);
                    }
                }
                return sum;
            }, 10000000);
            task_continue.Start();
            task_continue.ContinueWith(task => Console.WriteLine("finish the sum is:" + task.Result), TaskContinuationOptions.OnlyOnRanToCompletion);
            task_continue.ContinueWith(task =>
            {
                try
                {
                    Console.WriteLine("the sum is:" + task.Result);
                }
                catch (Exception)
                {
                    Console.WriteLine("exception {0}", task.Exception);
                }
            }, TaskContinuationOptions.OnlyOnFaulted);
            task_continue.ContinueWith(task => Console.WriteLine("sum is cancel!"), TaskContinuationOptions.OnlyOnCanceled);
            Thread.Sleep(100);
            cts.Cancel();
        }

         private static void 子任务()
         {
            Task<int[]> parent = new Task<int[]>(() =>
            {
                var results = new int[3];
                new Task(() => { results[0] = Sum(10); Thread.Sleep(1500); Console.WriteLine("task1"); }, TaskCreationOptions.AttachedToParent).Start();
                new Task(() => { results[1] = Sum(20); Thread.Sleep(5000); Console.WriteLine("task2"); }, TaskCreationOptions.AttachedToParent).Start();
                new Task(() => { results[2] = Sum(30); Thread.Sleep(2000); Console.WriteLine("task3"); }, TaskCreationOptions.AttachedToParent).Start();
                return results;
            });
            var cwt = parent.ContinueWith(parentTask => Array.ForEach(parentTask.Result, Console.WriteLine));
            parent.Start();

        }

         public static void 任务工厂()
         {
            CancellationTokenSource cts = new CancellationTokenSource();

            Task parentTaskFactory = new Task(() =>
            {
                var cts_TaskFactory = new CancellationTokenSource();
                var tf = new TaskFactory<int>(cts.Token, TaskCreationOptions.AttachedToParent,
                    TaskContinuationOptions.ExecuteSynchronously, TaskScheduler.Default);
                var childTasks = new[]
                {
                    tf.StartNew(() => Sum(10000, cts_TaskFactory.Token)),
                    tf.StartNew(() => Sum(20000, cts_TaskFactory.Token)),
                    tf.StartNew(() => Sum(30000, cts_TaskFactory.Token)),
                };

                for (int i = 0; i < childTasks.Length; i++)
                {
                    childTasks[i].ContinueWith(T => cts.Cancel(), TaskContinuationOptions.OnlyOnFaulted);
                }
                tf.ContinueWhenAll(
                    childTasks,
                    completedTasks => completedTasks
                                       .Where(t => !t.IsCanceled && !t.IsFaulted)
                                       .Max(t => t.Result),
                   CancellationToken.None)
                  .ContinueWith(t => Console.WriteLine("the maxinum is {0}", t.Result), TaskContinuationOptions.ExecuteSynchronously);
            });
            parentTaskFactory.ContinueWith(p =>
            {
                StringBuilder sb = new StringBuilder("the following eception occurred:" + Environment.NewLine);
                foreach (var e in p.Exception.Flatten().InnerExceptions)
                {
                    sb.AppendLine(" " + e.ToString());
                    Console.WriteLine(sb.ToString());
                }
            }, TaskContinuationOptions.OnlyOnFaulted);
            parentTaskFactory.Start();

        }

        public static int Sum(int n)
         {
             int sum = 0;
             for (; n > 0; n--)
             {
                 checked
                 {
                     sum += n;
                     Thread.Sleep(10);
                     Console.WriteLine("sum is {0}", sum);
                 }
             }
             return sum;
         }


         public static int Sum(int n,CancellationToken ct)
         {
             int sum = 0;
             for (; n > 0; n--)
             {
                 if(ct.IsCancellationRequested) break;
                 checked
                 {
                     sum += n;
                     //Thread.Sleep(10);
                     //Console.WriteLine("sum is {0}", sum);
                 }
             }
             Console.WriteLine(Thread.CurrentThread.ManagedThreadId + ":" +sum);
             return sum;
         }

         public static long DirectoryBytes(string path, string searchPattern,SearchOption searchOption)
         {
             var files = Directory.EnumerateFiles(path, searchPattern, searchOption);
             long masterTotal = 0;
             ParallelLoopResult result = Parallel.ForEach<string, long>
                 (
                     files, 
                     () =>//LocalInit 每个任务开始之前调用，初始化总值为0
                     {
                         return 0;//将taskLocalTotal值设为0
                     }, 
                     (file, loopState, index, taskLocalTotal) =>//Body，每个任务调用一次
                     {
                         //loopState.
                         long fileLength = 0;
                         FileStream fs = null;
                         try
                         {
                             fs = File.OpenRead(file);
                             fileLength = fs.Length;
                         }
                         catch (IOException)
                         {
                         }
                         finally
                         {
                             if (fs!=null)
                             {
                                 fs.Dispose();
                             }
                         }
                         return taskLocalTotal + fileLength;
                     },
                     taskLocalTotal =>//每个任务完成是调用
                     {
                         //线程同步的更新masterTotal的值做累加
                         Interlocked.Add(ref masterTotal, taskLocalTotal);
                     }
                 );
             return masterTotal;
         }

         //Timer
         public static Timer timer;

         public static void TimerDemo()
         {
             Console.WriteLine("Main therad:starting a timer");

             using (timer = new Timer(ComputeBoundOp,5,0,Timeout.Infinite))
             {
                 Console.WriteLine("Main therad:doing other work here...");
                 Thread.Sleep(10000);
                 Console.WriteLine("Main therad:other work done");
             }
         }

         public static void ComputeBoundOp(object state)
         {
             Console.WriteLine("in computeBoundOp:state={0}",state);
             Thread.Sleep(200);
             timer.Change(2000, Timeout.Infinite);
         }

         /// <summary>
         /// 并行Linq 可提高效率
         /// </summary>
         public static void PLinq(Assembly assembly)
         {
             var query = from type in assembly.GetExportedTypes().AsParallel()//转为并行执行查询
                 from method in type.GetMethods(BindingFlags.Public | BindingFlags.Instance | BindingFlags.Static)
                 let obsoletAttrtype = typeof (ObsoleteAttribute)
                 where Attribute.IsDefined(method, obsoletAttrtype)
                 orderby type.FullName
                 let obsoleteAttrObj = (ObsoleteAttribute) Attribute.GetCustomAttribute(method, obsoletAttrtype)
                 select
                     String.Format("type = {0}\nMethod={1}\nMessage={2}", type.FullName, method, obsoleteAttrObj.Message);

         }

         //测试性能
         [StructLayout(LayoutKind.Explicit)]
         public class TestThreadProptery_Data
         {
             [FieldOffset(0)]
             public int field1;
             [FieldOffset(64)]
             public int field2;
         }

         //public class TestThreadProptery_Data
         //{
         //    public int field1;
         //    public int field2;
         //}

         private const int iterations = 1000000000;
         private static int s_operations = 2;
         private static float s_startTime;

         public static void TestThreadProptery()
         {
             TestThreadProptery_Data data = new TestThreadProptery_Data();
             s_startTime = Stopwatch.GetTimestamp();

             ThreadPool.QueueUserWorkItem(o => TestThreadProptery_AccessData(data, 0));
             ThreadPool.QueueUserWorkItem(o => TestThreadProptery_AccessData(data, 1));
         }

         public static void TestThreadProptery_AccessData(TestThreadProptery_Data data, int field)
         {
             for (int x = 0; x < iterations; x++)
             {
                 if (field == 0) data.field1 ++;
                 else data.field2++;
             }
             if (Interlocked.Decrement(ref s_operations) == 0)
             {
                 Console.WriteLine("access time :{0}",
                     (Stopwatch.GetTimestamp() - s_startTime)/Stopwatch.Frequency/1000.00);
             }
         }

         public static void Start(object sender)
         {
             Console.WriteLine("Service Start");
             ThreadPool.SetMaxThreads(5, 10);
             ThreadPool.SetMinThreads(1, 10);
             for (int i = 0; i < 10; i++)
             {
                 ThreadPool.QueueUserWorkItem(ThreadTest.Run, Cts);
             }
         }

         public static void Stop()
         {
             Cts.Token.Register(() =>
             {
                 Console.WriteLine("Service Stop");
             });
             Cts.Cancel();
         }
     }
}
