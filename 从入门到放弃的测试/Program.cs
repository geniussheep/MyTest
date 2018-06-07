using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;

namespace 从入门到放弃的测试
{
    class Program
    {
        private static string GetInfoByProvince(string provinceName)
        {
            if (!string.IsNullOrEmpty(provinceName)) provinceName = provinceName.Replace("市", "").Replace("省", "");
            var recommendation = "";
            var dict = new Dictionary<string, string>();
            dict.Add("北京,1", "北京|天津|河北|河南|山东|山西|黑龙江|吉林|辽宁|内蒙古|陕西|宁夏|青海|甘肃|新疆|西藏");
            dict.Add("广东,2", "广东|广西|海南|云南|四川|重庆|贵州|香港|澳门|台湾");
            dict.Add("上海,3", "上海|江苏|浙江|安徽|湖南|湖北|江西|福建");
            foreach (var item in dict)
            {
                if (provinceName != null && item.Value.IndexOf(provinceName, StringComparison.OrdinalIgnoreCase) >= 0)
                {
                    recommendation = item.Key;
                    break;
                }
            }
            return recommendation;
        }

        static void Main(string[] args)
        {
            Console.WriteLine(GetInfoByProvince("广东"));
            Console.Read();
        }
    }
}
