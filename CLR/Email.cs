using System;
using System.Collections.Generic;
using System.IO;
using System.Linq;
using System.Net;
using System.Text;
using System.Threading.Tasks;
using System.Web.Script.Serialization;

namespace ConsoleApp
{

    public class Email
    {
        public static bool Send(string email, string type, string userName, string cdKey)
        {
            SortedDictionary<string, string> list = new SortedDictionary<string, string>();
            list.Add("remote_time", EmailFunction.GetTime());
            list.Add("remote_user_id", "6030"); //用户编号
            list.Add("return_ahead", "true");
            list.Add("track_url", "false");
            list.Add("to_addresses", email);
            list.Add("template_id", type);
            list.Add("tag_name_1", "UserName");
            list.Add("tag_body_1", userName);
            list.Add("tag_name_2", "GiftCDKey");
            list.Add("tag_body_2", cdKey);
            list.Add("salt", "i84eds3@Ws"); //盐值
            string md5 = EmailFunction._MD5(EmailFunction.Sort(list, true));
            Post p = new Post();
            return _rtnEmailMessage(p.PagePost(Post.PostString(list, md5)));
        }

        private static bool _rtnEmailMessage(string json)
        {
            try
            {
                JavaScriptSerializer jserializer = new JavaScriptSerializer();
                EmailMsg sysChildSystemModel = jserializer.Deserialize<EmailMsg>(json);
                return sysChildSystemModel.SUCCESS;
            }
            catch
            {
                return false;
            }
        }
    }

    public class EmailMsg
    {
        public string MESSAGE { get; set; }
        public bool SUCCESS { get; set; }
        //public string RESULT { get; set; }
        public class RESULT
        {
            public string result_type { get; set; }
            public string unique_id { get; set; }

            public string error_type_id { get; set; }
            public string delay_time { get; set; }
            public string begin_time { get; set; }
            public string email { get; set; }
            public string finish_time { get; set; }
            public string error_info { get; set; }

        }
    }

    public class Post
    {
        //private static string _sendUrl = string.Format("{0}", System.Configuration.ConfigurationSettings.AppSettings[""]);
        //private static string _clientId = string.Format("{0}", System.Configuration.ConfigurationSettings.AppSettings[""]);
        /// <summary>
        /// post方法
        /// </summary>
        /// <param name="sb"></param>
        /// <returns></returns>
        public string PagePost(StringBuilder sb)
        {
            try
            {
                HttpWebRequest request = (HttpWebRequest) WebRequest.Create(sb.ToString());
                request.Method = "POST";
                request.UserAgent = "Mozilla/5.0 (compatible; MSIE 9.0; Windows NT 6.1; Trident/5.0)";
                HttpWebResponse response = (HttpWebResponse) request.GetResponse();
                StreamReader stream = new StreamReader(response.GetResponseStream(), System.Text.Encoding.UTF8);
                string responseBody = stream.ReadToEnd();
                stream.Close();
                response.Close();
                return responseBody;
            }
            catch
            {
                return "{\"MESSAGE\":\"system error\",\"SUCCESS\":false,\"RESULT\":\"\"}";
            }
        }

        /// <summary>
        /// 生成post的url
        /// </summary>
        /// <param name="list"></param>
        /// <param name="md5"></param>
        /// <returns></returns>
        public static StringBuilder PostString(SortedDictionary<string, string> list, string md5)
        {
            StringBuilder sb = new StringBuilder("http://t263.info-msg.com:9508/send_email.json");

            sb.AppendFormat("?remote_time={0}", list["remote_time"]);
            sb.AppendFormat("&remote_user_id={0}", list["remote_user_id"]);
            sb.AppendFormat("&return_ahead={0}", list["return_ahead"]);
            sb.AppendFormat("&track_url={0}", list["track_url"]);
            sb.AppendFormat("&to_addresses={0}", list["to_addresses"]);
            sb.AppendFormat("&template_id={0}", list["template_id"]);
            sb.AppendFormat("&tag_name_1={0}", list["tag_name_1"]);
            sb.AppendFormat("&tag_body_1={0}", list["tag_body_1"]);
            sb.AppendFormat("&tag_name_2={0}", list["tag_name_2"]);
            sb.AppendFormat("&tag_body_2={0}", list["tag_body_2"]);
            sb.AppendFormat("&remote_verify_code={0}", md5);
            return sb;
        }

        public static string Sort(SortedDictionary<string, string> list)
        {
            string sortStr = null;
            foreach (KeyValuePair<string, string> item in list)
            {
                sortStr += item.Value;
            }
            return sortStr;
        }
    }

    public class EmailFunction
    {
        /// <summary>
        /// 获取格林威治时间
        /// </summary>
        /// <returns></returns>
        public static string GetTime()
        {
            TimeSpan ts = new TimeSpan(System.DateTime.UtcNow.Ticks - new DateTime(1970, 1, 1, 0, 0, 0).Ticks);
            long sec = (long) ts.TotalMilliseconds;
            return sec.ToString();
        }

        /// <summary>
        /// 配置邮件内容
        /// </summary>
        /// <param name="type"></param>
        /// <param name="dr"></param>
        /// <returns></returns>
        public static string GetMailId(int type)
        {
            switch (type)
            {
                case 1:
                    return "{CdKey}"; //注册模板编号
                case 2:
                    return ""; //取回密码模板编号
                case 3:
                    return ""; //密保模板编号
                default:
                    return "";
            }
        }

        /// <summary>
        /// url字段按字典序正序排序
        /// </summary>
        /// <param name="list"></param>
        /// <param name="sort">true:正序号 false:倒序号</param>
        /// <returns></returns>
        public static string Sort(SortedDictionary<string, string> list, bool sort)
        {
            string sortStr = null;
            if (sort)
            {
                foreach (KeyValuePair<string, string> item in list)
                {
                    sortStr += item.Value;
                }
            }
            else
            {
                foreach (KeyValuePair<string, string> item in list.Reverse())
                {
                    sortStr += item.Value;
                }
            }
            return sortStr;
        }

        /// <summary>
        /// MD5加密
        /// </summary>
        /// <param name="message"></param>
        /// <returns></returns>
        public static string _MD5(string message)
        {
            string m_ErrorMessage = "";
            try
            {
                m_ErrorMessage = "";
                System.Security.Cryptography.MD5 md5 = System.Security.Cryptography.MD5.Create();
                string encoded =
                    BitConverter.ToString(md5.ComputeHash(System.Text.Encoding.UTF8.GetBytes(message)))
                        .Replace("-", "");
                return encoded.ToLower();
            }
            catch (Exception e)
            {
                m_ErrorMessage = e.Message;
                return "";
            }
        }
    }

    public partial class Program
    {
        public static void MainEmailTest()
        {

            Email.Send("geniussheep@hotmail.com", "8277457", "羊两头", "123456");
            Email.Send("115674440@qq.com", "8277457", "羊两头", "123456");
            Console.Read();
        }

        public static T Get<T>()
        {
            return default(T);
        }
    }
}