using System;
using System.Collections.Generic;
using System.Configuration;
using System.Linq;
using System.Text;
using System.Threading;
using System.Threading.Tasks;
using Benlai.Common;

namespace DemoService
{
    public class DemoMain
    {
        public static void Start()
        {
            new Thread(() =>
            {
                Benlai.Common.HttpClientUtils.Get("http://111.com", Encoding.UTF8, int.MaxValue);
                //while (true)
                //{
                //    Thread.Sleep(int.MaxValue);
                    LogInfoWriter.GetInstance("DemoService").InfoFormat("服务：{0}", DateTime.Now);
                //}
            }).Start();
        }

        public static void Stop()
        {
            new Thread(() =>
            {
                int waitTime;
                int.TryParse(ConfigurationManager.AppSettings["WaitTime"].ToLower(), out waitTime);
                LogInfoWriter.GetInstance("DemoService").InfoFormat("服务：{0}\t{1}", DateTime.Now, waitTime);
                Thread.Sleep(waitTime*1000);
            }).Start();

            //Thread.Sleep(waitTime * 1000);
        }
    }
}
