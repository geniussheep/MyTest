using System;
using System.Collections.Generic;
using System.Text;

namespace ConsoleApp
{
    public static class Extensions
    {
        /// <summary>
        /// </summary>
        /// <typeparam name="T"></typeparam>
        /// <param name="source"></param>
        /// <param name="action"></param>
        public static void ForEach<T>(this IEnumerable<T> source, Action<T> action)
        {
            foreach (T element in source)
                action(element);
        }


        public static int GetLength(string str)
        {
            if (str.Length == 0) return 0;
            ASCIIEncoding ascii = new ASCIIEncoding();
            int tempLen = 0;
            byte[] s = ascii.GetBytes(str);
            for (int i = 0; i < s.Length; i++)
            {
                if ((int)s[i] == 63)
                {
                    tempLen += 2;
                }
                else
                {
                    tempLen += 1;
                }
            }
            return tempLen;
        }
    }

    public partial class Program
    {
        static void MainExtensionsTest(string[] args)
        {
            string str = "fd地方地方大幅度";

            Console.WriteLine(Extensions.GetLength(str));

            Console.WriteLine(Encoding.Default.GetBytes(str).Length);

            Console.ReadLine();
        }
    }
}
