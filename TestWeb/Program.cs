using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using TestClassLibrary;
namespace TestWeb
{
    class Program
    {
        private static void Main()
        {
            Console.WriteLine("Press 1 to use - Explicit Hosting");
            Console.WriteLine("Press 2 to use - SelfHost Helper");
            var testSheep = new TestSheep();
            Console.ReadKey();
        }
    }
}
