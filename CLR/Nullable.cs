using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace ConsoleApp
{
    class Nullable
    {
    }

    public partial class Program
    {
        [CLSCompliant(true)]
        [STAThread]
        public static void MainNullableTest()
        {
            Console.WriteLine("null | false = {0}", (null | false).ToString());
            Console.WriteLine("null & false = {0}", (null & false).ToString());

            Console.WriteLine("null | true = {0}", (null | true).ToString());
            Console.WriteLine("null & true = {0}", (null & true).ToString());


            string temp = null,temp1 = null;
            Console.WriteLine(temp ?? temp1 ?? "123");

            object o = 5;
            Console.WriteLine((int)o);
            Console.WriteLine((int?)o);

            o = null;
            try
            {
                Console.WriteLine((int)o);
            }
            catch (Exception)
            {
                throw;
            }
            Console.WriteLine((int?)o);


            Console.Read();
        }
    }

}
