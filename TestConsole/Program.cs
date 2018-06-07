using Benlai.Common;
using Benlai.Common.Redis;
using System;
using JSU = Benlai.Common.JsonSerializerUnits;
using System.Collections.Concurrent;
using System.Collections.Generic;
using System.Diagnostics;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Configuration;

namespace TestConsole
{

    [Serializable]
    public class M1 {
        private string _aa = "";
        public string AA {
            get { return "ok"; }           
        }
    }

    public class M2
    {
        private string _aa = "";
        public string AA
        {
            get { return "ok"; }
        }
    }

    class Program
    {
       
        

        static void Main(string[] args)
        {
            var m = new M1();
            TestType.GetTypeTest.GetType(m.GetType().FullName);

            //var ssss = JSU.SerializeObject(m);

            //Console.WriteLine(ssss);

            //var m2 = new M1();
            //var fffff = Benlai.Common.Redis.ByteHexHelper.ObjectToBytes(m2);



            //var cc = System.Text.Encoding.Default.GetString(fffff);
            //Console.WriteLine(cc);

            //Console.ReadLine();
            //return;
            //var oldKey = Convert.ToString(ConfigurationManager.AppSettings["oldKey"]);
            //var newKey = AppConfigRedis.NewRedisKeyPrefix + oldKey;
            //if (String.IsNullOrEmpty(oldKey))
            //{
            //    return;
            //}
            //var hosts = AppConfigRedis.RedisConnectionSearchKeys.Split(',');
            //foreach (string host in hosts)
            //{
            //    try
            //    {
            //        var redisHostAndPW = host.ToHostAndPassword();
            //        using (var client = new ServiceStack.Redis.RedisNativeClient(redisHostAndPW.Item2, redisHostAndPW.Item3, String.IsNullOrWhiteSpace(redisHostAndPW.Item1) ? null : redisHostAndPW.Item1))
            //        {
            //            var ser = client.Get(oldKey);
            //            if (ser != null)
            //            {
            //                LogInfoWriter.GetInstance("test").Info(System.Text.Encoding.UTF8.GetString(ser));
            //            }
            //        }
            //    }
            //    catch (Exception ex)
            //    {
            //        LogInfoWriter.GetInstance("test").Error(ex);
            //    }
            //}
            Console.Read();
        }
    }
}
