using System;
using System.Collections.Generic;
using System.Threading;

namespace ConsoleTaskPool.QueuePool
{
    public class QueueService
    {
        public QueueService()
        {
            this._signal = new ManualResetEvent(false);
            this._queue = new Queue<QueueModel>();
            _thread = new Thread(Process) {IsBackground = true};
            _thread.Start();
        }


        public int CurrentQueueCount => _queue.Count;


        private bool _isFirst = true;


        private readonly Thread _thread;


        // 用于通知是否有新任务需要处理的“信号器”
        private readonly ManualResetEvent _signal;


        //存放任务的队列
        private readonly Queue<QueueModel> _queue;


        private void Process()
        {
            while (true)
            {
                _signal.WaitOne();
                _signal.Reset();
                ExecuteTask();
            }
        }


        private void ExecuteTask()
        {
            if (_queue.Count == 0)
            {
                _signal.Reset();
                return;
            }
            QueueModel m = _queue.Dequeue();
            Console.WriteLine("QueueCount：" + this.CurrentQueueCount + "\t" + "id:" + m.id + "," + "p1:" + m.p1 + "\n");
            Thread.Sleep(2000);
            _signal.Set();
        }


        public void AddToQueue(QueueModel model)
        {
            _queue.Enqueue(model);
            if (_isFirst)
                _isFirst = false;
            _signal.Set();
        }


        public void Wait()
        {
            this._signal.Reset();
        }


        public void Finished()
        {
            this._signal.Set();
        }
    }
}
