using System;
using System.Collections;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace ConsoleApp
{
    public partial class Program
    {
        class MyClass
        {
            public static int Count = 0;

            static MyClass()
            {
                Count++;
            }

            public MyClass ()
            {
                Count++;
            }
        }

        public class MyClassA
        {
            public MyClassA()
            {
                Console.WriteLine("A");
            }

            public virtual void Fun()
            {
                Console.WriteLine("A.Fun");
            }
        }

        public class MyClassB:MyClassA
        {
            public MyClassB()
            {
                Console.WriteLine("B");
            }

            //public new void Fun()
            //{
            //    Console.WriteLine("B.Fun");
            //}

            public override void Fun()
            {
                Console.WriteLine("B.Fun");
            }
        }
        private static void MainTest(string[] args)
        {
            //int a = 1,b = 0;
            //switch (a)
            //{
            //    case 1:
            //        switch (b)
            //        {
            //            case 0:Console.WriteLine("0");
            //                break;
            //            case 1: Console.WriteLine("1"); break;
            //        }
            //        break;

            //    case 2:
            //        Console.WriteLine("2");
            //        break;
            //}
            //MyClass o1=new MyClass();
            //MyClass o2 = new MyClass();
            //Console.WriteLine(MyClass.Count);

            //MyClassA a=new MyClassB();
            //a.Fun();
            Console.Read();

        }

       
    }

    public interface A
    {
        void me(string ma);

        void me(string me, string med);
    }
}
