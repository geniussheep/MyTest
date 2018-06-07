using System;
using System.Diagnostics;
using System.Linq.Expressions;
using System.Reflection.Emit;


//文章地址 http://www.cnblogs.com/ldp615/archive/2013/03/31/2991304.html

namespace ConsoleApp
{

    public class MyMath
    {
        public int Add(int a, int b)
        {
            return a + b;
        }
    }

    public partial class Program
    {
        private static void Main替代反射调用的几种方式及性能测试(string[] args)
        {
            int result;
            var math = new MyMath();
            var count = 1000000;

            Console.WriteLine("数据量：" + count);
            Console.WriteLine("-----------------------------r\n");

            using (Profiler.Step("循环：{0} ms"))
            {
                for (var i = 0; i < count; i++)
                    result = 1;
            }
            using (Profiler.Step("直接调用 ：{0} ms"))
            {
                for (var i = 0; i < count; i++)
                    result = math.Add(i, i);
            }
            using (Profiler.Step("反射发出：{0} ms"))
            {
                var emitAdd = BuildEmitAddFunc();
                for (var i = 0; i < count; i++)
                    result = emitAdd(math, i, i);
            }
            using (Profiler.Step("表达式树：{0} ms"))
            {
                var expressionAdd = BuildExpressionAddFunc();
                for (var i = 0; i < count; i++)
                    result = expressionAdd(math, i, i);
            }
            using (Profiler.Step("dynamic 调用：{0} ms"))
            {
                dynamic d = math;
                for (var i = 0; i < count; i++)
                    result = d.Add(i, i);
            }
            using (Profiler.Step("反射调用：{0} ms"))
            {
                var add = typeof(MyMath).GetMethod("Add");
                for (var i = 0; i < count; i++)
                    result = (int) add.Invoke(math, new object[] {i, i});
            }

            Console.WriteLine("\r\n\r\n测试完成，任意键退出...");
            Console.ReadKey();
        }

        private static Func<MyMath, int, int, int> BuildExpressionAddFunc()
        {
            var add = typeof(MyMath).GetMethod("Add");
            var math = Expression.Parameter(typeof(MyMath));
            var a = Expression.Parameter(typeof(int), "a");
            var b = Expression.Parameter(typeof(int), "b");
            var body = Expression.Call(math, add, a, b);
            var lambda = Expression.Lambda<Func<MyMath, int, int, int>>(body, math, a, b);
            return lambda.Compile();
        }

        private static Func<MyMath, int, int, int> BuildEmitAddFunc()
        {
            var add = typeof(MyMath).GetMethod("Add");
            var dynamicMethod = new DynamicMethod("", typeof(int), new[] {typeof(MyMath), typeof(int), typeof(int)});
            var il = dynamicMethod.GetILGenerator();
            il.Emit(OpCodes.Ldarg_0);
            il.Emit(OpCodes.Ldarg_1);
            il.Emit(OpCodes.Ldarg_2);
            il.Emit(OpCodes.Callvirt, add);
            il.Emit(OpCodes.Ret);
            return (Func<MyMath, int, int, int>) dynamicMethod.CreateDelegate(typeof(Func<MyMath, int, int, int>));
        }
    }

    public class Profiler : IDisposable
    {
        private readonly string _message;
        private readonly Stopwatch _watch;

        private Profiler(string message)
        {
            _watch = new Stopwatch();
            _watch.Start();
            _message = message;
        }

        public void Dispose()
        {
            _watch.Stop();
            Console.WriteLine(_message, _watch.ElapsedMilliseconds);
            Console.WriteLine();
        }

        public static IDisposable Step(string message)
        {
            return new Profiler(message);
        }

        public static T Inline<T>(string message, Func<T> func)
        {
            using (new Profiler(message))
            {
                return func();
            }
        }
    }
}