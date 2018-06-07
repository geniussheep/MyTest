using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading;
using System.Threading.Tasks;
using Benlai.RiskControl.Logging.Client;

namespace ConsoleJenkins
{
    public class ThreadExtensions
    {
        private readonly CancellationTokenSource _cts;
        private Task _task;

        public ThreadExtensions(CancellationTokenSource cts)
        {
            this._cts = cts;
        }

        public void WhileTrue(Action callable, int retryCount,long timeoutMs, long loopSleepMs = 0)
        {
            int currentCount = 0;
            _task = Task.Factory.StartNew(() =>
            {
                var hiPerfTimer = new HiPerfTimer();
                while (!this._cts.IsCancellationRequested)
                {
                    hiPerfTimer.Start();
                    callable();
                    currentCount++;
                    Thread.Sleep((int)loopSleepMs);
                    hiPerfTimer.Stop();
                    double remain = timeoutMs - hiPerfTimer.DurationDouble;
                    if (remain <= 0 || currentCount >= retryCount)
                    {
                        _cts.Cancel();
                    }
                }
            }, TaskCreationOptions.LongRunning);
        }

        public void WhileTrue(Action callable, long timeoutMs)
        {
            _task = Task.Factory.StartNew(() =>
            {
                var hiPerfTimer = new HiPerfTimer();
                while (!this._cts.IsCancellationRequested)
                {
                    hiPerfTimer.Start();
                    callable();
                    hiPerfTimer.Stop();
                    double remain = timeoutMs - hiPerfTimer.DurationDouble;
                    if (remain > 0)
                    {
                        Thread.Sleep((int)(remain));
                    }
                }
            }, TaskCreationOptions.LongRunning);
        }

        public void Cancel()
        {
            try
            {
                this._cts.Cancel();
                this._task?.Dispose();
            }
            catch
            {
                // ignored
            }
        }
    }

    public class ThreadExtensions<TResult> where TResult : class 
    {
        private readonly CancellationTokenSource _cts;
        private Task<TResult> _task;

        public ThreadExtensions(CancellationTokenSource cts)
        {
            this._cts = cts;
        }

        public void WhileTrue(Func<CancellationTokenSource, TResult> callable, long timeoutMs)
        {
            _task = Task<TResult>.Factory.StartNew(() =>
            {
                var result = default(TResult);
                var hiPerfTimer = new HiPerfTimer();
                while (!this._cts.IsCancellationRequested)
                {
                    hiPerfTimer.Start();
                    result = callable(_cts);
                    hiPerfTimer.Stop();
                    double remain = timeoutMs - hiPerfTimer.DurationDouble;
                    if (remain > 0)
                    {
                        Thread.Sleep((int)(remain));
                    }
                }
                return result;
            }, TaskCreationOptions.LongRunning);
        }

        public void Cancel()
        {
            try
            {
                this._cts.Cancel();
                this._task?.Dispose();
            }
            catch
            {
                // ignored
            }
        }
    }
}
