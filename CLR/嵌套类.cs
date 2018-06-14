using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace ConsoleApp
{
    public class ClassA
    {
        public static string StaticString = GetString();

        public ClassA()
        {
            Console.WriteLine("ClassA Instance Inilizlized");
        }


        public static string GetString()
        {
            Console.WriteLine("ClassA Static field Inilizlized ");
            return "xxxxxxxxxxxxxxxxxxx";
        }
    }

    public class ClassB
    {
        public static string StaticString = GetString();

        public ClassB()
        {
            Console.WriteLine("ClassB Instance Inilizlized");
        }

        static ClassB()
        {

        }


        public static string GetString()
        {
            Console.WriteLine("ClassB Static field Inilizlized ");
            return "xxxxxxxxxxxxxxxxxxx";
        }
    }

    public partial class Program
    {
        static void Main嵌套类Test(string[] args)
        {

            Console.WriteLine("Start Main");

            ClassB.StaticString = "";
            ClassA.StaticString = "";

            Console.WriteLine("Finish Main");
        }
    }


    //Output
    //ClassA Static field Inilizlized
    //Start Main
    //ClassB Static field Inilizlized 
    //Finish Main

    /// <summary>
    /// 我认为延迟初始化的确不是嵌套类天生的能力，而是后天养成的。这也是为什么我在延迟初始化这个小标题上加了引号，
    /// 这里的延迟初始化其实是因为调用的时机比较晚而已，再通过静态构造器的这种方式才让一个类保证了初始化的时机只会在用到的时候才初始化。
    /// 嵌套类能提供这个作用是在单例模式中被发现的，所以我就拿单例来举例子，但是本文不会重点介绍单例模式
    ///
    /// 这是对单例模式很简单的实现，上述实现由 CLR 保证线程安全，执行速度快，
    /// 唯一缺点是无法延迟初始化，在第一次调用这个类型的任何成员时，就会自动执行构造函数然后进行初始化。
    /// 如果初始化的动作很耗时，或者要占用大量资源，那这个实现可能就会有问题了。
    /// </summary>
    public class Singleton1
    {
        private static readonly Singleton1 _instance = new Singleton1();

        // 静态构造器用于保证 _instance 会在类型第一次使用的时候初始化
        static Singleton1() { }

        private Singleton1() { }

        public static Singleton1 Instance
        {
            get
            {
                return _instance;
            }
        }
    }

    /// <summary>
    /// 换另一种实现，下面的实现使用了著名的双检索机制，不过这段代码会有一点小问题
    /// 上面的代码，只有当获取 Instance 实例的时候才会去执行创建一个实例化的动作。因为需要加锁，所以不会有线程安全的问题，但是执行速度慢。
    /// </summary>
    public class Singleton2
    {
        private static Singleton2 _instance;
        private static readonly object _lck = new object();

        private Singleton2() { }

        public static Singleton2 Instance
        {
            get
            {
                if (_instance == null)
                {
                    lock (_lck)
                    {
                        if (_instance == null)
                        {
                            _instance = new Singleton2(); //这句话的执行会出现问题
                        }
                    }
                }
                return _instance;
            }
        }
    }


    /// <summary>
    /// 如果又希望安全，又希望速度快，同时还想延迟初始化，那就可以利用本文提供的嵌套类来实现：
    /// Nested 类中的显示静态构造器保证了 _instance 会在 Nested 类型第一次被使用的时候初始化，而且由 CLR 来保证线程安全。
    /// 又因为 Nested 是嵌套类，对于何时使用嵌套类由外部类决定，因此只需要外部类在需要使用的时候再调用嵌套类即可。
    /// 上面的代码只有在 Insatnce 这个属性里才有对 Nested 类的调用，因此保证了在没有调用 Instance 这个属性的时候永远不会初始化这个实例。
    /// </summary>
    public class Singleton3
    {
        private Singleton3() { }

        public static Singleton3 Instance
        {
            get
            {
                return Nested._instance;
            }
        }

        public static void SomeMethod()
        {
            Console.WriteLine("SomeMethod");
        }

        private class Nested
        {
            static Nested()
            {
                Console.WriteLine("Nested.cctor");
            }

            internal static Singleton3 _instance = new Singleton3();
        }

    }
}
