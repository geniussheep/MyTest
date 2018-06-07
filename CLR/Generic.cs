using System;
using System.Collections;
using System.Collections.Generic;
using System.Diagnostics;
using System.Linq;
using System.Text;
using System.Threading;
using System.Threading.Tasks;

namespace ConsoleApp
{
    class Generic
    {
    }

    internal sealed class OperationTime : IDisposable
    {
        private Int64 m_startTime;

        private String m_text;

        private Int32 m_collectionCount;

        private Stopwatch stwch = new Stopwatch();

        public OperationTime(String text)
        {
            Stopwatch.StartNew();
            stwch.Start();
            PreareForOperation();
            m_text = text;
            m_collectionCount = GC.CollectionCount(0);

            m_startTime = Stopwatch.GetTimestamp();
        }

        private static void PreareForOperation()
        {
            GC.Collect();
            GC.WaitForPendingFinalizers();
            GC.Collect();
        }

        public void Dispose()
        {
            stwch.Stop();
            Console.Write("RunTime:{0} seconds", stwch.Elapsed.TotalMilliseconds);

            Console.WriteLine("{0,6:###.00} seconds (GCs={1,3}) {2} ,\r\nm_startTime:{3},m_endTime:{4},m_timeFrequency:{5}",
                (Stopwatch.GetTimestamp() - m_startTime)/(Double) Stopwatch.Frequency,
                GC.CollectionCount(0) - m_collectionCount, m_text, m_startTime, Stopwatch.GetTimestamp(),Stopwatch.Frequency);
        }
    }

    public partial class Program
    {
        static void MainGenericTest(string[] args)
        {
            const Int32 count = 10000000;

            using (new OperationTime("List<Int32>"))
            {
                List<Int32> l=new List<int>();
                for (int i = 0; i < count; i++)
                {
                    l.Add(i);
                    Int32 x = l[i];
                }
                l = null;
            }

            using (new OperationTime("ArrayList of Int32"))
            {
                ArrayList a = new ArrayList();
                for (int i = 0; i < count; i++)
                {
                    a.Add(i);
                    Int32 x = (Int32)a[i];
                }
                a = null;
            }

            Console.ReadLine();
        }
    }
}
