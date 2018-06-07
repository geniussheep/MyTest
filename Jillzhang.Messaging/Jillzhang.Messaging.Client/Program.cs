using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using Jillzhang.Messaging.Contract;

namespace Jillzhang.Messaging.Client
{
    class Program
    {
        static void Main(string[] args)
        {
            Program p = new Program();
            p.OneWayCall();
            p.NormalCall();
            p.DuplexCall();
            Console.Read();
        }

        void DuplexCall()
        {
            try
            {
                MyCallback callback = new MyCallback();
                IJob ws = new JobClient(new System.ServiceModel.InstanceContext(callback));
                Console.WriteLine("--------------------Duplex Calls ---------------------------");
                Console.WriteLine("开始调用服务");
                string result = ws.Do("duplex job");
                Console.WriteLine("收到返回信息：" + result);
                Console.WriteLine("-------------------------------------------------------------");
                Console.WriteLine("\r\n\r\n\r\n");
            }
            catch (Exception ex)
            {
                Console.WriteLine(ex.Message);
            }
        }

        void OneWayCall()
        {
            try
            {
                Console.WriteLine("-----------------------One-Way Calls-----------------------");
                IOneWayJob ws = new OneWayJobClient();
                ws.Do("one-way job");
                Console.WriteLine("请求完成！");
                Console.WriteLine("-------------------------------------------------------------");
                Console.WriteLine("\r\n\r\n\r\n");
            }
            catch (Exception ex)
            {
                Console.WriteLine(ex.Message);
            }
        }

        void NormalCall()
        {
            try
            {
                Console.WriteLine("-----------------------Request/Reply Calls-----------------------");
                INormalJob ws = new NormalJobClient();
                string result = ws.Do("request/reply job");
                Console.WriteLine("请求完成,返回结果："+result);
                Console.WriteLine("-------------------------------------------------------------");
                Console.WriteLine("\r\n\r\n\r\n");
            }
            catch (Exception ex)
            {
                Console.WriteLine(ex.Message);
            }
        }
    }
}
