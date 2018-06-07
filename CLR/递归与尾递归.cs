using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace ConsoleApp
{
    public partial class Program
    {
        /// <summary>
        /// 尾递归
        /// </summary>
        /// <typeparam name="T1"></typeparam>
        /// <typeparam name="T2"></typeparam>
        /// <typeparam name="T3"></typeparam>
        /// <typeparam name="T4"></typeparam>
        /// <typeparam name="TResult"></typeparam>
        /// <param name="func"></param>
        /// <returns></returns>
        public static Func<T1, T2, T3, T4, TResult> TailRecursive<T1, T2, T3, T4, TResult>
            (Func<Func<T1, T2, T3, T4, TResult>, T1, T2, T3, T4, TResult> func)
        {
            return (p1, p2, p3, p4) =>
            {
                bool callback = false;
                Func<T1, T2, T3, T4, TResult> self = (q1, q2, q3, q4) =>
                {
                    p1 = q1;
                    p2 = q2;
                    p3 = q3;
                    p4 = q4;
                    callback = true;
                    return default(TResult);
                };
                do
                {
                    var result = func(self, p1, p2, p3, p4);
                    if (!callback)
                    {
                        return result;
                    }
                    callback = false;
                } while (true);
            };
        }

        /// <summary>
        /// 另外还补充了普通递归的包装方法：
        /// </summary>
        /// <typeparam name="T1"></typeparam>
        /// <typeparam name="T2"></typeparam>
        /// <typeparam name="T3"></typeparam>
        /// <typeparam name="T4"></typeparam>
        /// <typeparam name="TResult"></typeparam>
        /// <param name="func"></param>
        /// <returns></returns>
        public static Func<T1, T2, T3, T4, TResult> Recursive<T1, T2, T3, T4, TResult>
    (Func<Func<T1, T2, T3, T4, TResult>, T1, T2, T3, T4, TResult> func)
        {
            Func<T1, T2, T3, T4, TResult> self = null;
            self = (p1, p2, p3, p4) => func(self, p1, p2, p3, p4);
            return self;
        }

        static void MainRecursive(string[] args)
        {
            Console.WriteLine("Normal");
            int x = 10000;
            var s = x.ToString();
            GC.Collect();
            GC.WaitForPendingFinalizers();
            Func<int, int, int, int, int> fib1 = null;
            fib1 = (i, n, a, b) =>
            {
                x = -x;
                for (int j = 0; j < Math.Abs(x); j++)
                {
                    s = (s + x.ToString()).Substring(0, 5);
                }
                return (n < 3 ? 1 : (i == n ? a + b : fib1(i + 1, n, b, a + b)));
            };
            Action<int> Fib1 = n =>
            {
                Console.Write("Fib({0}) = ", n);
                var sw = System.Diagnostics.Stopwatch.StartNew();
                var result = fib1(3, n, 1, 1);
                sw.Stop();
                Console.WriteLine("{0} ({1} ms)", result, sw.ElapsedMilliseconds);
            };
            Fib1(100);
            Fib1(1000);
            Fib1(10000);
            Console.WriteLine("/////////////////////////////////////");
            Console.WriteLine("Recursive");
            GC.Collect();
            GC.WaitForPendingFinalizers();
            var fib2 = Recursive<int, int, int, int, int>((self, i, n, a, b) =>
            {
                x = -x;
                for (int j = 0; j < Math.Abs(x); j++)
                {
                    s = (s + x.ToString()).Substring(0, 3);
                }
                return (n < 3 ? 1 : (i == n ? a + b : self(i + 1, n, b, a + b)));
            });
            Action<int> Fib2 = n =>
            {
                Console.Write("Fib({0}) = ", n);
                var sw = System.Diagnostics.Stopwatch.StartNew();
                var result = fib2(3, n, 1, 1);
                sw.Stop();
                Console.WriteLine("{0} ({1} ms)", result, sw.ElapsedMilliseconds);
            };
            Fib2(100);
            Fib2(1000);
            Fib2(10000);
            Console.WriteLine("/////////////////////////////////////");
            Console.WriteLine("TailRecursive");
            GC.Collect();
            GC.WaitForPendingFinalizers();
            var fib3 = TailRecursive<int, int, int, int, int>((self, i, n, a, b) =>
            {
                x = -x;
                for (int j = 0; j < Math.Abs(x); j++)
                {
                    s = (s + x.ToString()).Substring(0, 3);
                }
                return (n < 3 ? 1 : (i == n ? a + b : self(i + 1, n, b, a + b)));
            });
            Action<int> Fib3 = n =>
            {
                Console.Write("Fib({0}) = ", n);
                var sw = System.Diagnostics.Stopwatch.StartNew();
                var result = fib3(3, n, 1, 1);
                sw.Stop();
                Console.WriteLine("{0} ({1} ms)", result, sw.ElapsedMilliseconds);
            };
            Fib3(100);
            Fib3(1000);
            Fib3(10000);
        }
    }
}
