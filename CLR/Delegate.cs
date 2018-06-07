using System;
using System.Collections.Generic;
using System.Diagnostics;
using System.IO;
using System.Linq;
using System.Reflection;
using System.Text;
using System.Threading;
using System.Threading.Tasks;
using System.Windows.Forms;

namespace ConsoleApp
{
    internal sealed class Light
    {
        public string SwitchPosition()
        {
            return "the light is off -- \r\n";
        }
    }

    internal sealed class Fan
    {
        public string Speed()
        {
            return "the Fan is Speed -- ";
//            throw new InvalidOperationException("the fan broke due to overheating");
        }
    }

    internal sealed class Speaker
    {
        public string Volume()
        {
            return "the volume is load -- ";
        }
    }

    public delegate object TwoInt32(int n1, int n2);
    public partial class Program
    {
        public delegate void Feedback(int val);


        public delegate object MyCallback(FileStream s);

        public delegate string GetStatus();

        public static void MainDelegateTest()
        {
//            StaticDelegateDemo();
//            InstanceDelegateDemo();
//            ChainDelegateDemo1();
//            ChainDelegateDemo2();
            MyCallback mcb = new MyCallback(SomeMethod);
            GetStatus getStatus = null;

            getStatus += new GetStatus(new Light().SwitchPosition);
            getStatus += new GetStatus(new Fan().Speed);
            getStatus += new GetStatus(new Speaker().Volume);

            Console.WriteLine(getStatus());

            UsingLocalVariablesInTheCallbackCode(10);

            UsingCreateDelegateOrDynamicInvoke("ConsoleApp.TwoInt32", "Add");

            Console.Read();
        }

        private static void UsingCreateDelegateOrDynamicInvoke(string delegateType,string method)
        {
            Type delType = Type.GetType(delegateType);
            if (delType == null)
            {
                
            }

            Delegate d;
            try
            {
                MethodInfo mi = typeof (Program).GetMethod(method,BindingFlags.Public|BindingFlags.Static);

                d = Delegate.CreateDelegate(delType,mi);

                var result = d.DynamicInvoke(1, 2);
            }
            catch (Exception)
            {
                
                throw;
            }
        }

        public static object Add(int n1, int n2)
        {
            return n1 + n2;
        }

        public static void UsingLocalVariablesInTheCallbackCode(int numToDo)
        {
            int[] squares = new int[numToDo];

            AutoResetEvent done =new AutoResetEvent(false);

            for (int i = 0; i < squares.Length; i++)
            {
                ThreadPool.QueueUserWorkItem(obj =>
                {
                    int num = (int) obj;
                    squares[num] = num*num;
                    Thread.Sleep(100);
                    Console.WriteLine("Thread:index {0},square{1}", num, squares[num]);
                    if (Interlocked.Decrement(ref numToDo) == 0)
                    {
                        done.Set();
                    }
                },i);
            }
            done.WaitOne();

            for (int i = 0; i < squares.Length; i++)
            {
                Console.WriteLine("index {0},square{1}",i,squares[i]);
            }
        }

        private static void CallbackWithoutNewingADelegateObject()
        {
            ThreadPool.QueueUserWorkItem(SomeAsyncTask, 6);

            ThreadPool.QueueUserWorkItem(obj => Console.WriteLine(obj), 7);
        }

        private static void SomeAsyncTask(object o)
        {
            Console.WriteLine(o);
        }

        private static string GetComponentStatusReport(GetStatus getStatus)
        {
            if (getStatus == null) return "delegate is null";

            StringBuilder sb = new StringBuilder();

            Delegate[] arrayOfDelegates = getStatus.GetInvocationList();

            foreach (GetStatus status in arrayOfDelegates)
            {
                try
                {
                    sb.AppendFormat("{0}{1}{1}", status(),Environment.NewLine);
                }
                catch (Exception e)
                {
                    object component = status.Target;
                    sb.AppendFormat("failed to get status from {0}{2}{0}   error:{3}{0}{0}", Environment.NewLine,
                        component == null ? "" : component.GetType() + ".",status.Method.Name,e.Message);
                }
            }
            return sb.ToString();
        }

        public static string SomeMethod(Stream fs)
        {
            StreamWriter sw = new StreamWriter("Status.txt", true);
            sw.WriteLine("item = {0}", 123);
            sw.Close();
            return "success";
        }

        private static void StaticDelegateDemo()
        {
            Console.WriteLine("---Static Delegate Demo---");
            Counter(1, 3, null);
            Counter(1, 3, new Feedback(Other.FeedbackToConsole));
            Counter(1, 3, new Feedback(FeedbackToMsgBox));
            Console.WriteLine();
        }

        private static void InstanceDelegateDemo()
        {
            Console.WriteLine("---Instance Delegate Demo---");
            Program p = new Program();
            
            Counter(1, 3, new Feedback(p.FeedbackToFile));
            Counter(1, 3, new Feedback(FeedbackToMsgBox));
            Console.WriteLine();
        }

        private static void ChainDelegateDemo1()
        {
            Console.WriteLine("---Chain Delegate Demo---");
            Program p = new Program();
            Feedback fb1 = new Feedback(Other.FeedbackToConsole);
            Feedback fb2 = new Feedback(FeedbackToMsgBox);
            Feedback fb3 = new Feedback(p.FeedbackToFile);

            Feedback fbChain = null;
            fbChain = (Feedback)Delegate.Combine(fbChain, fb1);
            fbChain = (Feedback)Delegate.Combine(fbChain, fb2);
            fbChain = (Feedback)Delegate.Combine(fbChain, fb3);
            Counter(1, 3, fbChain);
            Console.WriteLine();

            fbChain = (Feedback) Delegate.Remove(fbChain, new Feedback(FeedbackToMsgBox));
            Counter(1,3,fbChain);
        }

        private static void ChainDelegateDemo2()
        {
            Console.WriteLine("---Chain Delegate Demo---");
            Program p = new Program();
            Feedback fb1 = new Feedback(Other.FeedbackToConsole);
            Feedback fb2 = new Feedback(FeedbackToMsgBox);
            Feedback fb3 = new Feedback(p.FeedbackToFile);

            Feedback fbChain = null;
            fbChain += fb1;
            fbChain += fb2;
            fbChain +=fb3;
            Counter(1, 3, fbChain);
            Console.WriteLine();

            fbChain -= new Feedback(FeedbackToMsgBox);
            Counter(1, 3, fbChain);
        }

        private static void Counter(int a, int b, Feedback fb)
        {
            for (int val = a;val<=b;val++)
            {
                if (fb!=null)
                {
                    fb(val);
                }
            }
        }


        private static void FeedbackToMsgBox(int val)
        {
            MessageBox.Show(String.Format("item = {0}", val));
        }

        private void FeedbackToFile(int val)
        {
            StreamWriter sw = new StreamWriter("Status.txt",true);
            sw.WriteLine("item = {0}", val);
            sw.Close();
        }

        bool DelegateRefersToInstanceMethodOfType(MulticastDelegate d, Type type)
        {
            return d.Target != null && d.Target.GetType() == type;
        }

        bool DelegateRefersToMethodOfName(MulticastDelegate d, string methodName)
        {
            return d.Method.Name == methodName;
        }
    }

    internal class Other
    {

        public static void FeedbackToConsole(int val)
        {
            Console.WriteLine("item = {0}", val);
        }
    }
}
