using System;
using System.Collections.Generic;
using System.Data;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace ConsoleDns
{
    class Program
    {
        static void Main(string[] args)
        {
            MyDnsTest();
        }

        /// <summary> 
        /// MyDnsTEST功能测试 
        /// C#利用WMI操作DNS服务器(可远程操作,需要相应权限) 
        /// Author:yaosansi  
        /// Create Date:2005-09-07 
        /// Modify Date:2006-10-25 
        /// Site:http://www.yaosansi.com/ 
        /// E-mail:yaosansi at 126 dot com 
        /// http://www.yaosansi.com/blog/article.asp?id=935 
        /// http://yaosansi.cnblogs.com/archive/2006/11/04/DNSServiceInCsharpWithWMI.html 
        /// 注意:此版本为WINDOWS2003 DNS服务器专用.不适合其它版本操作系统. 
        /// </summary> 
        static void MyDnsTest()
        {
            var dns = new DnsManager();
            //=========================================== 
            //不对以下三个属性赋值默认DNS服务器为本机. 
            dns.ServerName = "192.168.60.56";
            dns.UserName = "depops";
            dns.PassWord = "123.com";
            //=========================================== 
            dns.CreateZone("yytest.com"); 
            //dns.DelZone("yaosansi.com"); 
            dns.CreateAType("yytest.com", "wode", "10.10.110.143", "3600"); 
            //dns.ModifyAType("yaosansi.com","www","127.21.0.1","800"); 
            //dns.DelAType("yaosansi.com", "mail"); 
            //dns.CreateMXType("mail", "yaosansi.com", "5.5.5.5", "20", "3600"); 
            //dns.ModifyMXType("mail", "yaosansi.com", "36000", "218.1.1.1", "26"); 
            //dns.DelMXType("mail", "yaosansi.com"); 
            //dns.CreateCNAMEType("mpq2", "yaosansi.com", "www.yaosansi.com", "3900"); 
            //dns.ModifyCNAMEType("mpq2", "abc.com", "30520", "www.yaosansi.com."); 
            //dns.DelCNAMEType("mpq", "yaosansi.com"); 

            //DataTable table = dns.ListExistsMXType("yaosansi.com"); 
            DataTable table = dns.ListExistsAType("yytest.com");
            //DataTable table = dns.ListExistsCNAMEType("yaosansi.com"); 
            //Yaosansi.Data.DataHelp.PrintTable(table);

            if (!string.IsNullOrEmpty(dns.ErrMessage))
            {
                Console.WriteLine("--------------------------------------------------");
                Console.WriteLine("返回信息:" + dns.ErrMessage);
                Console.WriteLine("--------------------------------------------------");
            }
            Console.WriteLine("");
            Console.WriteLine("===End===");
            Console.ReadLine();
        }
    }
}
