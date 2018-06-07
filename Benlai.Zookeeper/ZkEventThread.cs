using System;
using System.Collections.Concurrent;
using System.Threading;

using Benlai.Zookeeper.Common;
using Benlai.Common;

namespace Benlai.Zookeeper
{
    public class ZkEventThread
    {
        private static readonly LogInfoWriter Logger = LogInfoWriter.GetInstance("ZookeeperLog");

        private BlockingCollection<ZkEvent> _events = new BlockingCollection<ZkEvent>();

        private AtomicInteger _eventId = new AtomicInteger(0);

        public string Name { get; private set; }

        private CancellationTokenSource tokenSource = new CancellationTokenSource();

        public Thread Thread { get; private set; }

        public ZkEventThread(string name)
        {
            this.Name = name;
        }

        public void Start()
        {
            this.Thread = new Thread(this.Run);
            this.Thread.Name = this.Name;
            this.Thread.IsBackground = true;
            this.Thread.Start();
        }

        public void Run()
        {
            System.Diagnostics.Debug.WriteLine("Starting ZkClient event thread.");
            try
            {
                while (!this.tokenSource.IsCancellationRequested)
                {
                    ZkEvent zkEvent = this._events.Take();
                    int eventId = this._eventId.GetAndIncrement();
                    System.Diagnostics.Debug.WriteLine("Delivering event #" + eventId + " " + zkEvent);
                    try
                    {
                        zkEvent.RunAction();
                    }
                    catch (ThreadInterruptedException)
                    {
                        this.tokenSource.Cancel();
                    }
                    catch (Exception e)
                    {
                        Logger.Error("Error handling event " + zkEvent, e);
                    }

                    System.Diagnostics.Debug.WriteLine("Delivering event #" + eventId + " done");
                }
            }
            catch (ThreadInterruptedException)
            {
                System.Diagnostics.Debug.WriteLine("Terminate ZkClient event thread.");
            }
        }

        public void Send(ZkEvent zkEvent)
        {
            if (!this.tokenSource.IsCancellationRequested)
            {
                System.Diagnostics.Debug.WriteLine("New event: " + zkEvent);
                this._events.Add(zkEvent);
            }
        }

        public void Interrupt()
        {
            this.Thread.Interrupt();
        }

        public void Join(int i)
        {
            this.Thread.Join(i);
        }
    }
}
