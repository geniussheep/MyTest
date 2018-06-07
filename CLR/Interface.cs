using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace ConsoleApp
{
    internal class BaseInterface : IDisposable
    {
        public void Dispose()
        {
            Console.WriteLine("Base a Dispose");
        }
    }

    internal class DerivedInterface : BaseInterface
    {
        public new void Dispose()
        {
            Console.WriteLine("Derived a Dispose");

            base.Dispose();
        }
    }

    public partial class Program
    {
        public static void MainInterfaceTest()
        {
            BaseInterface b = new BaseInterface();

            b.Dispose();

            ((IDisposable)b).Dispose();

            DerivedInterface d = new DerivedInterface();

            d.Dispose();

            ((IDisposable)d).Dispose();

            b = new BaseInterface();

            b.Dispose();

            ((IDisposable)b).Dispose();

            Console.ReadLine();

        }
    }
}
