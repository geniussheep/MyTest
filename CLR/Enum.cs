using System;
using System.Collections.Generic;
using System.IO;
using System.Linq;
using System.Runtime;
using System.Runtime.Remoting.Metadata.W3cXsd2001;
using System.Text;
using System.Threading.Tasks;

namespace ConsoleApp
{
    public enum TestEnum1
    {
        A = 0x0001,
        B = 0x0002,
        C = 0x0004,
    }

    [Flags]
    public enum TestEnum2
    {
        A = 0x0001,
        B = 0x0002,
        C = 0x0004,
    }

    public partial class Program
    {
        public static void MainEnum()
        {
            Console.WriteLine(TestEnum1.A | TestEnum1.B);

            Console.WriteLine(TestEnum2.A | TestEnum2.B);

            FileAttributes fa = FileAttributes.System;

            fa = fa.Set(FileAttributes.Hidden);
            Console.WriteLine("{0} {1}",fa,fa.IsSet(FileAttributes.Hidden));
            fa = fa.Set(FileAttributes.ReadOnly);
            Console.WriteLine("{0} {1}", fa, fa.IsSet(FileAttributes.ReadOnly));
            fa = fa.Set(FileAttributes.Archive);
            Console.WriteLine("{0} {1}", fa, fa.IsSet(FileAttributes.Archive));
            fa = fa.Clear(FileAttributes.Hidden);
            Console.WriteLine("{0} {1}", fa, fa.IsClear(FileAttributes.Hidden));
            fa.ForEach(f=>Console.WriteLine(f));
            //Console.WriteLine(FileAttributes.Archive | FileAttributes.Hidden | FileAttributes.Normal);
            //Console.WriteLine(FileAttributes.Archive & FileAttributes.Archive);
            Console.Read();
        }
    }

    public static class FileAttributesExtensionMethods
    {
        public static bool IsSet(this FileAttributes flags, FileAttributes flagToTest)
        {
            if (flagToTest == 0)
            {
                throw new ArgumentOutOfRangeException("");
            }
            return (flags & flagToTest) == flagToTest;
        }

        public static bool IsClear(this FileAttributes flags, FileAttributes flagToTest)
        {
            if (flagToTest == 0)
            {
                throw new ArgumentOutOfRangeException("");
            }
            return (flags & flagToTest) != flagToTest;
        }

        public static bool AnyFlagsSet(this FileAttributes flags, FileAttributes testFlags)
        {
            return (flags & testFlags) != 0;
        }

        public static FileAttributes Set(this FileAttributes flags, FileAttributes setFlags)
        {
            return flags | setFlags;
        }

        public static FileAttributes Clear(this FileAttributes flags, FileAttributes clearFlags)
        {
            return flags & ~clearFlags;
        }

        public static void ForEach(this FileAttributes flags, Action<FileAttributes> processFlag)
        {
            if (processFlag == null)
            {
                throw new ArgumentNullException();
            }
            for (uint bit = 1; bit != 0; bit <<= 1)
            {
                uint temp = ((uint) flags) & bit;
                if (temp != 0) processFlag((FileAttributes) temp);
                {
                    
                }
            }
        }
    }
}
