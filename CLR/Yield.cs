using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace ConsoleApp
{
    class Yield
    {
        static void MainYield()
        {
            // Display powers of 2 up to the exponent of 8:
            foreach (int i in Power(2, 8))
            {
                Console.Write("{0} ", i);
            }

            foreach (var x in Fib(10))
            {
                Console.Write("{0} ", x);
            }
        }

        public static IEnumerable<int> Power(int number, int exponent)
        {
            int result = 1;

            for (int i = 0; i < exponent; i++)
            {
                result = result * number;
                yield return result;
            }
        }

        public static IEnumerable<int> Fib(int max)
        {
            int count = 1;
            int x = 0, y = 1;
            while (count<max)
            {
                yield return x;
                x = y;
                y = x + y;
            }
        } 
    }
}
