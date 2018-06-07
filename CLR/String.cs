using System;
using System.Globalization;
using System.Runtime.InteropServices;
using System.Security;
using System.Text;
using System.Threading;
using System.Xml;

namespace ConsoleApp
{
    public partial class Program
    {
        public static void MainTestString()
        {
            //StringBuilder sb = new StringBuilder();

            //sb.AppendFormat(new BoldInt32s(), "{0} {1} {2:M}", "jeff", 123, DateTime.Now);

            //Console.WriteLine(sb);

            //int x = int.Parse(" 123", NumberStyles.None, null);

            //foreach (EncodingInfo ei in Encoding.GetEncodings())
            //{
            //    Encoding e = ei.GetEncoding();
            //    Console.WriteLine("{1}{0}\tCodePage={2},WindowCodePage={3}{0}" +
            //                      "\tWebName={4},HeaderName={5},BodyName={6}{0}" +
            //                      "\tIsBrowserDisplay={7},IsBrowserSave={8}{0}" +
            //                      "\tIsMailNewsDisplay={9},IsMailNewsSave={10}{0}",
            //        Environment.NewLine, e.EncodingName, e.CodePage, e.WindowsCodePage, e.WebName, e.HeaderName,
            //        e.BodyName, e.IsBrowserDisplay, e.IsBrowserSave, e.IsMailNewsDisplay, e.IsMailNewsSave
            //        );
            //}

            using (SecureString ss=new SecureString())
            {
                Console.WriteLine("please enter password:");

                while (true)
                {
                    ConsoleKeyInfo cki = Console.ReadKey(true);
                    if (cki.Key == ConsoleKey.Enter) break;
                    ss.AppendChar(cki.KeyChar);
                    Console.Write("*");
                    DisplaySecureString(ss);
                }
            }

            Console.Read();
        }

        private unsafe static void DisplaySecureString(SecureString ss)
        {
            char* pc = null;
            try
            {
                pc = (char*) Marshal.SecureStringToCoTaskMemUnicode(ss);

                for (int ind = 0; pc[ind] != 0; ind++)
                {
                    Console.Write(pc[ind]);
                }
                Console.WriteLine(Environment.NewLine);
            }
            finally
            {
                if (pc != null) Marshal.ZeroFreeCoTaskMemUnicode((IntPtr)pc);
            }
        }
    }

    internal sealed class BoldInt32s : IFormatProvider, ICustomFormatter
    {
        public object GetFormat(Type formtaType)
        {
            return formtaType == typeof (ICustomFormatter)
                ? this
                : Thread.CurrentThread.CurrentCulture.GetFormat(formtaType);
        }

        public string Format(string format, object arg, IFormatProvider formatProvider)
        {
            string s;
            IFormattable formattable = arg as IFormattable;

            if (formattable == null) s = arg.ToString();
            else s = formattable.ToString(format, formatProvider);

            if (arg is Int32)
                return "<B>" + s + "</B>";
            return s;
        }
    }
}
