using System;
using System.Collections.Generic;
using System.Data;
using System.Linq;
using System.Text;
using System.Threading;
using System.Threading.Tasks;

namespace ConsoleApp
{
    internal class NewMailEventArgs : EventArgs
    {
        private readonly String m_form, m_to, m_subject;

        public NewMailEventArgs(String form, String to, String subject)
        {
            m_form = form;
            m_to = to;
            m_subject = subject;
        }

        public String From
        {
            get { return m_form; }
        }

        public String To
        {
            get { return m_to; }
        }

        public String Subject
        {
            get { return m_subject; }
        }
    }


    internal class MailManager
    {
        public event EventHandler<NewMailEventArgs> m_NewMail;

        protected virtual void OnNewMail(NewMailEventArgs e)
        {
            e.Raise(this, ref m_NewMail);
        }

        public void SimulateNewMail(String from, String to, String subject)
        {
            Console.WriteLine("Get New Mail ...");
            NewMailEventArgs e = new NewMailEventArgs(from, to, subject);

            OnNewMail(e);
        }
    }

    public static class EventArgExtensions
    {
        public static void Raise<TEventArgs>(this TEventArgs e, Object sender,
            ref EventHandler<TEventArgs> eventDelegate) where TEventArgs : EventArgs
        {
            Console.WriteLine("Check EventHandler ...");

            EventHandler<TEventArgs> tempEventHandler = Interlocked.CompareExchange(ref eventDelegate, null, null);
            if (tempEventHandler != null) tempEventHandler(sender, e);
        }
    }

    internal sealed class Fax
    {
        public Fax(MailManager mm)
        {
            Console.WriteLine("Register FaxMsg");
            mm.m_NewMail += FaxMsg;
        }

        private void FaxMsg(Object sender, NewMailEventArgs e)
        {
            Console.WriteLine("Faxing mail message:");
            Console.WriteLine(" From={0},To={1},Subject={2}", e.From, e.To, e.Subject);
            Console.ReadLine();
        }

        public void Unregister(MailManager mm)
        {
            mm.m_NewMail -= FaxMsg;
        }
    }

    public sealed class EventKey : Object
    {
    }

    public sealed class EventSet
    {
        private readonly Dictionary<EventKey, Delegate> m_events = new Dictionary<EventKey, Delegate>();

        public void Add(EventKey eventKey, Delegate handle)
        {
            Console.WriteLine("Add Event in Hash...EventKey:{0}",eventKey);
            Monitor.Enter(m_events);
            Delegate d;
            m_events.TryGetValue(eventKey, out d);
            m_events[eventKey] = Delegate.Combine(d, handle);
            Monitor.Exit(m_events);
        }

        public void Remove(EventKey eventKey, Delegate handle)
        {
            Console.WriteLine("Remove Event in Hash...EventKey:{0}", eventKey);
            Monitor.Enter(m_events);
            Delegate d;
            m_events.TryGetValue(eventKey, out d);
            m_events[eventKey] = Delegate.Combine(d, handle);
            Monitor.Exit(m_events);
        }

        public void Raise(EventKey eventKey, Object sender, EventArgs e)
        {
            Console.WriteLine("Check Event ...EventKey:{0}", eventKey);
            Delegate d;
            Monitor.Enter(m_events);
            m_events.TryGetValue(eventKey, out d);
            Monitor.Exit(m_events);
            if (d!=null)
            {
                d.DynamicInvoke(sender, e);
            }
        }
    }

    public class FooEventArgs : EventArgs
    {
        String m_flag = String.Empty;

        public FooEventArgs(String flag)
        {
            m_flag = flag;
        }

        public String Flag { get { return m_flag;} }
    }

    public class TypeWithLotsOfEvents
    {
        private readonly EventSet m_eventSet = new EventSet();

        protected EventSet EventSet
        {
            get { return m_eventSet; }
        }

        protected static  readonly EventKey s_fooEventKey = new EventKey();

        public event EventHandler<FooEventArgs> Foo
        {
            add { m_eventSet.Add(s_fooEventKey, value); }
            remove { m_eventSet.Remove(s_fooEventKey, value); }
        }

        protected virtual void OnFoo(FooEventArgs e)
        {
            Console.WriteLine("Run OnFoo...");
            m_eventSet.Raise(s_fooEventKey, this, e);
        }

        public void SimulateFoo(String flag)
        {
            OnFoo(new FooEventArgs (flag));
        }
    }

    public partial class Program
    {
        static void MainEventTest(string[] args)
        {
            MailManager mm = new MailManager();
            Fax fax = new Fax(mm);
            mm.SimulateNewMail("yy1", "yy2", "yys");

            TypeWithLotsOfEvents twie = new TypeWithLotsOfEvents();

            twie.Foo += HandleFooEvent;
            twie.Foo += HandleFooEvent1;
            twie.Foo += HandleFooEvent2;

            twie.SimulateFoo("yytest");

            Console.ReadLine();

        }

        private static void HandleFooEvent(Object sender, FooEventArgs e)
        {
            Console.WriteLine("Handling Foo Event here...Flag:{0}", e.Flag);
        }


        private static void HandleFooEvent1(Object sender, FooEventArgs e)
        {
            Console.WriteLine("Handling1 Foo Event here...Flag:{0}", e.Flag);
        }


        private static void HandleFooEvent2(Object sender, FooEventArgs e)
        {
            Console.WriteLine("Handling2 Foo Event here...Flag:{0}", e.Flag);
        }
    }
}
