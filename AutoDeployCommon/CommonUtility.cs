using System;
using System.Threading;

namespace AutoDeployCommon
{
    public class CommonUtility
    {

        public static void ThreadWait(int waitInterval)
        {
            using (AutoResetEvent done = new AutoResetEvent(false))
            {
                done.WaitOne(waitInterval);
            }
        }
    }
}
