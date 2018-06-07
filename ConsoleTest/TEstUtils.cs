using System;
using System.Collections.Generic;
using System.Diagnostics;
using System.Linq;
using System.Reflection;
using System.Text;

namespace ConsoleTest
{



    public class TestModel
    {
        public string Name { get; set; }

        public int Value { get; set; }
    }

    public class TestUtils
    {
        private const int TestTimes = 1000000;

        private const int TestMax = 5;

        public static void ConsoleResult(Func<double> func, string methodName)
        {
            double sum = 0;
            for (int i = 0; i < TestMax; i++)
            {
                sum += func();
            }
            Console.WriteLine($"{methodName} 平均耗时 :{sum / 5.0}");
        }

        public static void ConsoleResult(double result, string methodName)
        {
            double sum = 0;
            for (int i = 0; i < TestMax; i++)
            {
                sum += result;
            }
            Console.WriteLine($"{methodName} 平均耗时 :{sum / 5.0}");
        }

        public static double TestMethodUseTime(Action testMethod, string methodName)
        {
            var stopWatch = new Stopwatch();
            stopWatch.Start();
            for (int i = 0; i < TestTimes; i++)
            {
                testMethod();
            }
            stopWatch.Stop();
            Console.WriteLine("{0}: 执行{3}次耗时 {1} s, {2} ms.", methodName, stopWatch.Elapsed.Seconds,
                stopWatch.Elapsed.Milliseconds, TestTimes);
            return stopWatch.Elapsed.TotalMilliseconds;
        }

        public static double TestMethodUseTime(Action<string> testMethod, string str, string methodName)
        {
            var stopWatch = new Stopwatch();
            stopWatch.Start();
            for (int i = 0; i < TestTimes; i++)
            {
                testMethod(str);
            }
            stopWatch.Stop();
            Console.WriteLine("{0}: 执行{3}次耗时 {1} s, {2} ms.", methodName, stopWatch.Elapsed.Seconds,
                stopWatch.Elapsed.Milliseconds, TestTimes);
            return stopWatch.Elapsed.TotalMilliseconds;
        }


        public static double TestMethodUseTime(Action<string, string> testMethod, string str, string str2,
            string methodName)
        {
            var stopWatch = new Stopwatch();
            stopWatch.Start();
            for (int i = 0; i < TestTimes; i++)
            {
                testMethod(str, str2);
            }
            stopWatch.Stop();
            Console.WriteLine("{0}: 执行{3}次耗时 {1} s, {2} ms.", methodName, stopWatch.Elapsed.Seconds,
                stopWatch.Elapsed.Milliseconds, TestTimes);
            return stopWatch.Elapsed.TotalMilliseconds;
        }

        public static double TestMethodUseTime(Action<string, string, string> testMethod, string str, string str2,
            string str3, string methodName)
        {
            var stopWatch = new Stopwatch();
            stopWatch.Start();
            for (int i = 0; i < TestTimes; i++)
            {
                testMethod(str, str2, str3);
            }
            stopWatch.Stop();
            Console.WriteLine("{0}: 执行{3}次耗时 {1} s, {2} ms.", methodName, stopWatch.Elapsed.Seconds,
                stopWatch.Elapsed.Milliseconds, TestTimes);
            return stopWatch.Elapsed.TotalMilliseconds;
        }

        public static double TestMethodUseTime(Func<List<string>, string> testMethod, List<string> strList, string methodName)
        {
            var stopWatch = new Stopwatch();
            stopWatch.Start();
            for (int i = 0; i < TestTimes; i++)
            {
                testMethod(strList);
            }
            stopWatch.Stop();
            Console.WriteLine("{0}: 执行{3}次耗时 {1} s, {2} ms.", methodName, stopWatch.Elapsed.Seconds,
                stopWatch.Elapsed.Milliseconds, TestTimes);
            return stopWatch.Elapsed.TotalMilliseconds;
        }
    }








}
