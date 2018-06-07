using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading;
using System.Threading.Tasks;

namespace ConsoleTaskPool
{
    public class MyTaskList
    {
        public List<Action> Tasks = new List<Action>();

        public void Start()
        {
            for (var i = 0; i < 5; i++)
                StartAsync();
        }

        public event Action Completed;

        public void StartAsync()
        {
            lock (Tasks)
            {
                if (Tasks.Count > 0)
                {
                    var t = Tasks[Tasks.Count - 1];
                    Tasks.Remove(t);
                    ThreadPool.QueueUserWorkItem(h =>
                    {
                        t();
                        StartAsync();
                    });
                }
                else if (Completed != null)
                    Completed();
            }
        }
    }
}
