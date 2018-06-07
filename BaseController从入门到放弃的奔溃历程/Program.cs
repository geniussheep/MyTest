using System;
using System.Collections.Generic;
using System.Linq;

namespace BaseController从入门到放弃的奔溃历程
{
    class Program
    {
        private static string GetInfoByProvince(string provinceName)
        {

            var recommendation = "";
            try
            {
                if (!string.IsNullOrEmpty(provinceName)) provinceName = provinceName.Replace("市", "").Replace("省", "");
                var dict = new Dictionary<string, string>();
                dict.Add("北京,1", "北京|天津|河北|河南|山东|山西|黑龙江|吉林|辽宁|内蒙古|陕西|宁夏|青海|甘肃|新疆|西藏");
                dict.Add("广东,2", "广东|广西|海南|云南|四川|重庆|贵州|香港|澳门|台湾");
                dict.Add("上海,3", "上海|江苏|浙江|安徽|湖南|湖北|江西|福建");
                foreach (var item in dict)
                    if (item.Value.IndexOf(provinceName) >= 0)
                    {
                        recommendation = item.Key;
                        break;
                    }
                return recommendation;
            }
            catch (Exception e)
            {
                Console.WriteLine("Exception:" + e);
            }
            return recommendation;
        }

        private static int GetWebSiteSysNoByProvince(string provinceName)
        {
            if (!string.IsNullOrEmpty(provinceName)) provinceName = provinceName.Replace("市", "").Replace("省", "");
            var recommendation = 0;
            var dict = new Dictionary<int, string>();
            dict.Add(1, "北京|天津|河北|河南|山东|山西|黑龙江|吉林|辽宁|内蒙古|陕西|宁夏|青海|甘肃|新疆|西藏");
            dict.Add(2, "广东|广西|海南|云南|四川|重庆|贵州|香港|澳门|台湾");
            dict.Add(3, "上海|江苏|浙江|安徽|湖南|湖北|江西|福建");
            foreach (var item in dict)
            {
                if (!item.Value.Contains(provinceName)) continue;
                recommendation = item.Key;
                break;
            }
            return recommendation;
        }

        public static void ExceptTest()
        {
            var listA = new List<int>() { 1, 2, 3, 4 };

            var listB = new List<int>() { 2, 3, 4, 5 };

            var listr = listA.Except(listB);

            Console.WriteLine(string.Join(",", listr));
        }

        static void Main(string[] args)
        {
            //            Console.WriteLine(GetInfoByProvince(""));
            //            Console.WriteLine(GetInfoByProvince(null));
            //
            //            Console.WriteLine(GetWebSiteSysNoByProvince(""));
            //            Console.WriteLine(GetWebSiteSysNoByProvince(null));
            ExceptTest();
            Console.Read();
        }
    }
}
