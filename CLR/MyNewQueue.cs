using System;
using System.Messaging;

namespace ConsoleApp
{
    /// <summary>
    /// Provides a container class for the example.
    /// </summary>
    public class MyNewQueue
    {

        //**************************************************
        // Provides an entry point into the application.
        //         
        // This example demonstrates several ways to set
        // a queue's path.
        //**************************************************



        public static void MainMyNewQueueTest()
        {

            //Point p = new Point(1,1);
            //Console.WriteLine(p);

            //p.Change(2,2);
            //Console.WriteLine(p);

            //object o = p;
            //Console.WriteLine(o);

            //((Point)o).Change(3,3);
            //Console.WriteLine(o);

            //((IChangePointBox)p).Change(4, 4);
            //Console.WriteLine(p);

            //((IChangePointBox)o).Change(5, 5);
            //Console.WriteLine(o);

            //Int32 y = (Int32) 6.8;
            //Console.WriteLine(y);
            //checked
            //{
            //    Byte b = 100;
            //    Console.WriteLine(Byte.MaxValue);
            //    Console.WriteLine(b);
            //    b = (Byte) (b + 200);
            //    Console.WriteLine(b);
            //}
            //Employee e;
            //Int32 year;
            //e = new Manager();
            //e = Employee.Lookup("joe");
            //year = e.GetYearsEmployed();
            //Console.WriteLine(e.Name + year);
            //e.GenProgressReport();
            // Create a new instance of the class.
            //MyNewQueue myNewQueue = new MyNewQueue();

            //myNewQueue.SendPublic();
            //myNewQueue.SendPrivate();
            //myNewQueue.SendByLabel();
            //myNewQueue.SendByFormatName();
            //myNewQueue.MonitorComputerJournal();
            //myNewQueue.MonitorQueueJournal();
            //myNewQueue.MonitorDeadLetter();
            //myNewQueue.MonitorTransactionalDeadLetter();

            //var uri = new Uri("http://jtlc.sgzb2.com/Contribution/Add?userId={userId}&roleId={roleId}&value={value}&signature={signature}");

            //Console.WriteLine(uri.Scheme);
            //Console.WriteLine(uri.Query);

            //var dict = new Dictionary<string, string>
            //{
            //    {"userId", "1"},
            //    {"roleId", "2"},
            //    {"value", "3"},
            //    {"key", "4"}
            //};

            //string param = dict.OrderBy(m => m.Key)
            //    .Aggregate("",
            //        (current, source) =>
            //            current + source.Key + "=" + source.Value + ","
            //    );

            //Console.WriteLine(param);
            //dynamic args = new DynamicModel();
            //args.userId = "order.Uid";
            //args.roleId = 0;
            //args.value = "rebatepoint";
            //args.key = "app.AuthorizationCode";
            //args.signature = "{signature}";

            //Console.WriteLine(param);
            //Console.WriteLine(args.JsonSerializer());

            //DynamicModel args2 = new DynamicModel(args.JsonSerializer());

            //Dictionary<string, object> cusargs = args2.Clone();
            //(cusargs).ForEach(s =>
            //{
            //    //判断参数是否要从AppModel内获取指定属性值
            //    if (s.Value.ToString().ToLower().Contains("app."))
            //    {
            //        args2.Dictionary[s.Key] = "app" + s.Key;
            //    }
            //    //判断参数是否要从OrderModel内获取指定属性值
            //    else if (s.Value.ToString().ToLower().Contains("order."))
            //    {

            //        args2.Dictionary[s.Key] = "order" + s.Key;
            //    }
            //    else if (s.Value.ToString().ToLower() == "rebatepoint")
            //    {
            //        args2.Dictionary[s.Key] = 1000;
            //    }
            //    else
            //    {
            //        args2.Dictionary[s.Key] = s.Value;
            //    }
            //});
            //Console.WriteLine(args2.JsonSerializer());
            Console.WriteLine(123456789.ToString("X8"));
            Console.WriteLine(1.ToString("X8"));
            Console.Read();
            //var signatureStr = Md5Encrypt.GetMd5Utf8(param);
            //return signature == signatureStr;
            return;
        }

        // References public queues.
        public void SendPublic()
        {
            MessageQueue myQueue = new MessageQueue(".//myQueue");
            myQueue.Send("Public queue by path name.");

            return;
        }

        // References private queues.
        public void SendPrivate()
        {
            MessageQueue myQueue = new
                MessageQueue(".//Private$//myQueue");
            myQueue.Send("Private queue by path name.");

            return;
        }

        // References queues by label.
        public void SendByLabel()
        {
            MessageQueue myQueue = new MessageQueue("Label:TheLabel");
            myQueue.Send("Queue by label.");

            return;
        }

        // References queues by format name.
        public void SendByFormatName()
        {
            MessageQueue myQueue = new
                MessageQueue("FormatName:Public=5A5F7535-AE9A-41d4" +
                "-935C-845C2AFF7112");
            myQueue.Send("Queue by format name.");

            return;
        }

        // References computer journal queues.
        public void MonitorComputerJournal()
        {
            MessageQueue computerJournal = new
                MessageQueue(".//Journal___FCKpd___7quot");
            while (true)
            {
                Message journalMessage = computerJournal.Receive();
                // Process the journal message.
            }
        }

        // References queue journal queues.
        public void MonitorQueueJournal()
        {
            MessageQueue queueJournal = new
                MessageQueue(".//myQueue//Journal___FCKpd___7quot");
            while (true)
            {
                Message journalMessage = queueJournal.Receive();
                // Process the journal message.
            }
        }

        // References dead-letter queues.
        public void MonitorDeadLetter()
        {
            MessageQueue deadLetter = new
                MessageQueue(".//DeadLetter___FCKpd___7quot");
            while (true)
            {
                Message deadMessage = deadLetter.Receive();
                // Process the dead-letter message.
            }
        }

        // References transactional dead-letter queues.
        public void MonitorTransactionalDeadLetter()
        {
            MessageQueue TxDeadLetter = new
                MessageQueue(".//XactDeadLetter___FCKpd___7quot");
            while (true)
            {
                Message txDeadLetter = TxDeadLetter.Receive();
                // Process the transactional dead-letter message.
            }
        }

    }
}
