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


namespace ConsoleTest
{

    public class Bb
    {
        public int Bd { get; set; }
        public string BS { get; set; }
    }


    public class Aa
    {


        public string cc { get; set; }


        public int bb { get; set; }

        public List<Bb> BbList { get; set; }

        protected bool Equals(Aa other)
        {
            return bb == other.bb && string.Equals(cc, other.cc);
        }

        public override int GetHashCode()
        {
            unchecked
            {
                return (bb * 397) ^ (cc != null ? cc.GetHashCode() : 0);
            }
        }

        public override bool Equals(object right)
        {
            if (ReferenceEquals(null, right)) return false;
            if (ReferenceEquals(this, right)) return true;
            if (right.GetType() != this.GetType()) return false;
            return Equals((Aa) right);
        }
    }


    class Program
    {

        public static List<Aa> GetAAs(int start, int end)
        {
            var result = new List<Aa>();
            for (int i = start; i <= end; i++)
            {
                result.Add(new Aa() {bb = i, cc = i.ToString()});
            }
            return result;
        }



        [ThreadStatic] static int number = 0;

        static void Main(string[] args)
        {
            for (int i = 0; i < 10; i++)
            {
                T1(i);
                T2(i);
                T3(i);
            }

            Console.ReadKey();
        }

        private static async Task<int> T1(int i)
        {
            Console.WriteLine($"T1 - {i} before: {Thread.CurrentThread.ManagedThreadId}");
            await Task.Delay(1000);
            Console.WriteLine($"T1 - {i} after: {Thread.CurrentThread.ManagedThreadId}");
            return 1;
        }

        private static async Task<int> T2(int i)
        {
            Console.WriteLine($"T2 - {i} before: {Thread.CurrentThread.ManagedThreadId}");
            var result = await Task.FromResult(1);
            Console.WriteLine($"T2 - {i} after: {Thread.CurrentThread.ManagedThreadId}");
            return result;
        }

        private static async Task<int> T3(int i)
        {
            Console.WriteLine($"T3 - {i} before: {Thread.CurrentThread.ManagedThreadId}");
            var result = await Task.FromResult(T31(i));
            Console.WriteLine($"T3 - {i} after: {Thread.CurrentThread.ManagedThreadId}");
            return result;
        }

        static int T31(int i)
        {
            Thread.Sleep(1000);
            Console.WriteLine($"T31 - {i}: {Thread.CurrentThread.ManagedThreadId}");
            return 1;
        }
    }
}
