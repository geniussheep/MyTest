using System;
using System.Collections.Generic;
using System.Diagnostics;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace ConsoleApp
{

    public partial class Program
    {
        public static void MainTestArray()
        {
            var names = new[] {1, 2, 0, 5, 4};
            var namesdes = new int[names.Length];
            System.Buffer.BlockCopy(names, 0, namesdes, 1, names.Length);
            var des = new string[namesdes.Length];
            foreach (var val in namesdes)
            {
                //Console.WriteLine(val);
            }

            var arr = new object[10];

            var arrs = new object[10, 8];

            //Console.WriteLine(namesdes.Aggregate((a, b) => a.ToString() + "," + b.ToString()));
            //Console.WriteLine(FileAttributes.Archive | FileAttributes.Hidden | FileAttributes.Normal);
            //Console.WriteLine(FileAttributes.Archive & FileAttributes.Archive);

            decimal[,] darr = (decimal[,]) Array.CreateInstance(typeof (decimal), new int[] {5, 4}, new[] {2005, 1});
            Console.WriteLine("{0,4},{1,9},{2,9},{3,9},{4,9}", "Year", "Q1", "Q2", "Q3", "Q4");
            int firstYear = darr.GetLowerBound(0);
            int lastYear = darr.GetUpperBound(0);
            int firstQuarter = darr.GetLowerBound(1);
            int lastQuarter = darr.GetUpperBound(1);

            for (int i = firstYear; i <= lastYear; i++)
            {
                Console.Write(i + " ");
                for (int j = firstQuarter; j <= lastQuarter; j++)
                {
                    Console.Write("{0,1:C} ", darr[i, j]);
                }
                Console.WriteLine();
            }

            int[,] a1 = new int[10000,10000];
            int[][] a2 = new int[10000][];
            for (int i = 0; i < 10000; i++)
            {
                a2[i] = new int[10000];
            }
            Stopwatch sw=Stopwatch.StartNew();
            for (int i = 0; i < 10; i++)
            {
                Safe2DimArrayAccess(a1);
            }
            Console.WriteLine("{0}:Safe2DimArrayAccess",sw.Elapsed);

            sw = Stopwatch.StartNew();
            for (int i = 0; i < 10; i++)
            {
                SafeJaggedArrayAccess(a2);
            }
            Console.WriteLine("{0}:SafeJaggedArrayAccess", sw.Elapsed);

            sw = Stopwatch.StartNew();
            for (int i = 0; i < 10; i++)
            {
                UnSafe2DimArrayAccess(a1);
            }
            Console.WriteLine("{0}:UnSafe2DimArrayAccess", sw.Elapsed);

            StackallocDemo();
            InlineArrayDemo();
            Console.Read();
        }

        private static int Safe2DimArrayAccess(int[,] a)
        {
            int sum = 0;
            for (int i = 0; i < 10000; i++)
            {
                for (int j = 0; j < 10000; j++)
                {
                    sum += a[i, j];
                }
            }
            return sum;
        }

        private static int SafeJaggedArrayAccess(int[][] a)
        {
            int sum = 0;
            for (int i = 0; i < 10000; i++)
            {
                for (int j = 0; j < 10000; j++)
                {
                    sum += a[i][j];
                }
            }
            return sum;
        }

        private static unsafe int UnSafe2DimArrayAccess(int[,] a)
        {
            int sum = 0;
            fixed (int* pi = a)
            {
                for (int i = 0; i < 10000; i++)
                {
                    int baseofdim = i*10000;
                    for (int j = 0; j < 10000; j++)
                    {
                        sum += pi[baseofdim + j];
                    }
                }
            }
            return sum;
        }


        private static void StackallocDemo()
        {
            unsafe
            {
                const int width = 20;
                char* pc = stackalloc char[width];

                string s = "Jeffrev Richter";

                for (int i = 0; i < width; i++)
                {
                    pc[width - i - 1] = (i < s.Length) ? s[i] : '.';
                }

                Console.Write(new string(pc, 0, width));
            }
        }

        private static void InlineArrayDemo()
        {
            unsafe
            {
                CharArray ca;
                int widthInBytes = sizeof (CharArray);
                int width = widthInBytes/2;
                string s = "Jeffrev Richter";

                for (int i = 0; i < width; i++)
                {
                    ca.characters[width - i - 1] = (i < s.Length) ? s[i] : '.';
                }
                Console.Write(new string(ca.characters, 0, width));
            }
        }


    }

    internal unsafe struct CharArray
    {
        public fixed char characters [20];
    }
}
