using System;
using System.Data;
using System.Linq;
using System.Management;

namespace ConsoleDns
{
    /// <summary>
    ///     C#利用WMI操作DNS服务器(可远程操作,需要相应权限)
    ///     Author:yaosansi
    ///     Create Date:2005-09-07
    ///     Modify Date:2006-10-25
    ///     Site:http://www.yaosansi.com/
    ///     E-mail:yaosansi at 126 dot com
    ///     http://www.yaosansi.com/blog/article.asp?id=935
    ///     http://yaosansi.cnblogs.com/archive/2006/11/04/DNSServiceInCsharpWithWMI.html
    ///     注意:此版本为WINDOWS2003 DNS服务器专用.不适合其它版本操作系统.
    /// </summary>
    public class DnsManager
    {
        /// <summary>
        ///     表示管理操作的范围.这里是用来执行DNS的命名空间
        /// </summary>
        private ManagementScope _dns;

        /// <summary>
        /// </summary>
        private ManagementClass _dnsClass;

        /// <summary>
        ///     服务器名称或IP地址
        /// </summary>
        private string _dnsName;

        /// <summary>
        ///     返回的操作信息.
        /// </summary>
        private string _errMessage;

        /// <summary>
        /// </summary>
        private ManagementBaseObject _mgrBaseObj;

        /// <summary>
        ///     用于返回检索的ManagementObject集合
        /// </summary>
        private ManagementObjectCollection _mgrObjCollection;

        /// <summary>
        ///     密码
        /// </summary>
        private string _password;

        /// <summary>
        ///     要连接的DNS服务器
        /// </summary>
        private string _sServerPath;

        /// <summary>
        ///     用户名
        /// </summary>
        private string _username;

        /// <summary>
        ///     构造函数
        /// </summary>
        public DnsManager()
        {
            _sServerPath = @"\\localhost\root\MicrosoftDNS";
            _dnsName = "localhost";
        }

        /// <summary>
        ///     获取错误信息.
        /// </summary>
        public string ErrMessage => _errMessage;

        /// <summary>
        ///     设置DNS服务器名称或IP地址
        /// </summary>
        public string ServerName
        {
            set
            {
                _sServerPath = $@"\\{value}\root\MicrosoftDNS";
                _dnsName = value;
            }
        }

        /// <summary>
        ///     设置连接服务器的用户名(指定服务器IP,用户和密码后将自动连接远程服务器,本机不需要指定)
        /// </summary>
        public string UserName
        {
            set { _username = value; }
        }

        /// <summary>
        ///     设置连接服务器的密码(指定服务器IP,用户和密码后将自动连接远程服务器,本机不需要指定)
        /// </summary>
        public string PassWord
        {
            set { _password = value; }
        }


        /// <summary>
        ///     建立对象.连接
        /// </summary>
        /// <param name="dnsType">DNS类型 MicrosoftDNS_Zone等</param>
        private void Create(string dnsType)
        {
            if (!string.IsNullOrEmpty(_username) && !string.IsNullOrEmpty(_password))
            {
                var conn = new ConnectionOptions
                {
                    Username = _username,
                    Password = _password
                };
                //用户名 
                //口令 
                _dns = new ManagementScope(_sServerPath, conn);
            }
            else
            {
                _dns = new ManagementScope(_sServerPath);
            }
            if (!_dns.IsConnected)
                _dns.Connect();
            var path = new ManagementPath(dnsType);
            _dnsClass = new ManagementClass(_dns, path, null);
        }

        /// <summary>
        ///     查询DNS并建立对象
        /// </summary>
        /// <param name="query">WQL查询语句</param>
        /// <param name="dnsType">DNS类型 MicrosoftDNS_Zone等</param>
        /// <returns></returns>
        public ManagementObjectCollection QueryDns(string query, string dnsType)
        {
            Create(dnsType);
            var qs = new ManagementObjectSearcher(_dns, new ObjectQuery(query)) {Scope = _dns};
            return qs.Get();
        }

        /// <summary>
        ///     仅查询DNS
        /// </summary>
        /// <param name="query">WQL查询语句</param>
        /// <returns></returns>
        public ManagementObjectCollection QueryDns(string query)
        {
            if (!string.IsNullOrEmpty(_username) && !string.IsNullOrEmpty(_password))
            {
                var conn = new ConnectionOptions
                {
                    Username = _username,
                    Password = _password
                };
                //用户名 
                //口令 
                _dns = new ManagementScope(_sServerPath, conn);
            }
            else
            {
                _dns = new ManagementScope(_sServerPath);
            }
            if (!_dns.IsConnected)
                _dns.Connect();
            var qs = new ManagementObjectSearcher(_dns, new ObjectQuery(query)) {Scope = _dns};
            return qs.Get();
        }


        /// <summary>
        ///     判断区域是否存在
        /// </summary>
        /// <param name="domain">区域名称</param>
        /// <returns></returns>
        public bool IsExistsZone(string domain)
        {
            try
            {
                _mgrObjCollection = QueryDns("Select * From MicrosoftDNS_ZONE where ContainerName='" + domain + "'");
                return _mgrObjCollection.Cast<ManagementObject>().Any();
            }
            catch (Exception e)
            {
                _errMessage = e.Message;
                Console.WriteLine(e.ToString());
                return false;
            }
        }


        /// <summary>
        ///     创建一个新的区域,仅区域名称
        /// </summary>
        /// <param name="domain">区域的名称</param>
        public bool CreateZone(string domain)
        {
            try
            {
                Create("MicrosoftDNS_Zone");
                //如果区域已经存在 
                if (IsExistsZone(domain))
                {
                    _errMessage = "域:" + domain + "已经存在.";
                    return false;
                }
                //建立新的区域 
                _mgrBaseObj = _dnsClass.GetMethodParameters("CreateZone");
                _mgrBaseObj["ZoneName"] = domain;
                _mgrBaseObj["ZoneType"] = 0;

                var outParams = _dnsClass.InvokeMethod("CreateZone", _mgrBaseObj, null);
                _errMessage = "域:" + domain + "创建成功.";
                return true;
            }
            catch (Exception e)
            {
                _errMessage = e.Message;
                Console.WriteLine(e.ToString());
                return false;
            }
        }


        /// <summary>
        ///     创建一个区域,包括其它参数
        /// </summary>
        /// <param name="domain">要创建的区域名称</param>
        /// <param name="zoneType">
        ///     Type of zone. Valid values are the following:0 Primary zone. 1 Secondary zone.  2 Stub zone. 3
        ///     Zone forwarder.
        /// </param>
        /// <param name="dataFileName">[in, optional] Name of the data file associated with the zone</param>
        /// <param name="ipAddr">[in, optional] IP address of the mater DNS Server for the zone. </param>
        /// <param name="adminEmailName">[in, optional] Email address of the administrator responsible for the zone</param>
        /// <returns></returns>
        public bool CreateZone(string domain, uint zoneType, string dataFileName, string[] ipAddr, string adminEmailName)
        {
            try
            {
                Create("MicrosoftDNS_Zone");
                //如果区域已经存在 
                if (IsExistsZone(domain))
                {
                    _errMessage = "域:" + domain + "已经存在.";
                    return false;
                }
                //建立新的区域 
                _mgrBaseObj = _dnsClass.GetMethodParameters("CreateZone");
                _mgrBaseObj["ZoneName"] = domain;
                _mgrBaseObj["ZoneType"] = zoneType;
                _mgrBaseObj["DataFileName"] = dataFileName;
                _mgrBaseObj["IpAddr"] = ipAddr;
                _mgrBaseObj["AdminEmailName"] = adminEmailName;
                var outParams = _dnsClass.InvokeMethod("CreateZone", _mgrBaseObj, null);
                _errMessage = "域:" + domain + "创建成功.";
                return true;
            }
            catch (Exception e)
            {
                _errMessage = e.Message;
                Console.WriteLine(e.ToString());
                return false;
            }
        }


        /// <summary>
        ///     修改区域
        /// </summary>
        /// <param name="domain">要修改的区域名称</param>
        /// <param name="zoneType">
        ///     Type of zone. Valid values are the following:0 Primary zone. 1 Secondary zone.  2 Stub zone. 3
        ///     Zone forwarder.
        /// </param>
        /// <param name="dataFileName">[in, optional] Name of the data file associated with the zone</param>
        /// <param name="ipAddr">[in, optional] IP address of the mater DNS Server for the zone. </param>
        /// <param name="adminEmailName">[in, optional] Email address of the administrator responsible for the zone</param>
        /// <returns></returns>
        public bool ChangeZoneType(string domain, uint zoneType, string dataFileName, string[] ipAddr, string adminEmailName)
        {
            try
            {
                _mgrObjCollection = QueryDns("Select * From MicrosoftDNS_ZONE where ContainerName='" + domain + "'",
                    "MicrosoftDNS_Zone");

                foreach (var oManObject in _mgrObjCollection.Cast<ManagementObject>())
                {
                    _mgrBaseObj = oManObject.GetMethodParameters("ChangeZoneType");
                    _mgrBaseObj["ZoneType"] = zoneType;
                    _mgrBaseObj["DataFileName"] = dataFileName;
                    _mgrBaseObj["IpAddr"] = ipAddr;
                    _mgrBaseObj["AdminEmailName"] = adminEmailName;
                    oManObject.InvokeMethod("ChangeZoneType", _mgrBaseObj, null);
                    _errMessage = "域:" + domain + "修改成功.";
                    return true;
                }
                _errMessage = "未找到域:" + domain;
                return false;
            }
            catch (Exception e)
            {
                _errMessage = e.Message;
                Console.WriteLine(e.ToString());
                return false;
            }
        }

        /// <summary>
        ///     删除区域
        /// </summary>
        /// <param name="domain">要册除的区域的名称</param>
        /// <returns></returns>
        public bool DelZone(string domain)
        {
            try
            {
                _mgrObjCollection = QueryDns("Select * From MicrosoftDNS_ZONE where ContainerName='" + domain + "'",
                    "MicrosoftDNS_Zone");
                foreach (var oManObject in _mgrObjCollection.Cast<ManagementObject>())
                {
                    oManObject.Delete();
                    _errMessage = "域:" + domain + "删除成功.";
                    return true;
                }
                _errMessage = "未找到域:" + domain;
                return false;
            }
            catch (Exception e)
            {
                _errMessage = e.Message;
                Console.WriteLine(e.ToString());
                return false;
            }
        }

        /// <summary>
        ///     判断在某MicrosoftDNS_AType是否在指定的域中存在
        /// </summary>
        /// <param name="domain"></param>
        /// <param name="ownerName"></param>
        /// <returns></returns>
        public bool IsExistsAType(string domain, string ownerName)
        {
            try
            {
                _mgrObjCollection =
                    QueryDns("Select * From MicrosoftDNS_AType where OwnerName='" + ownerName + "' and ContainerName='" +
                             domain + "'");
                return _mgrObjCollection.Cast<ManagementObject>().Any();
            }
            catch (Exception e)
            {
                _errMessage = e.Message;
                Console.WriteLine(e.ToString());
                return false;
            }
        }


        /// <summary>
        ///     创建MicrosoftDNS_AType
        /// </summary>
        /// <param name="containerName">Name of the Container for the Zone, Cache, or RootHints instance which contains this RR</param>
        /// <param name="hostName">主机名 [如果为空或NULL,主机名将与域名保持一致.]</param>
        /// <param name="ttl">Time, in seconds, that the RR can be cached by a DNS resolver</param>
        /// <param name="ipAddress">String representing the IPv4 address of the host</param>
        /// <returns></returns>
        public bool CreateAType(string containerName, string hostName, string ipAddress, string ttl)
        {
            try
            {
                string ownerName;
                if (string.IsNullOrEmpty(hostName))
                    ownerName = containerName;
                else
                    ownerName = hostName + "." + containerName;
                Create("MicrosoftDNS_AType");
                //如果区域不存在 
                if (!IsExistsZone(containerName))
                {
                    Console.WriteLine("区域:{0}不存在,创建失败", containerName);
                    _errMessage = $"区域:{containerName}不存在,创建失败";
                    return false;
                }
                if (IsExistsAType(containerName, ownerName))
                {
                    Console.WriteLine("{0}中已存在{1},创建失败", containerName, ownerName);
                    _errMessage = $"{containerName}中已存在{ownerName},创建失败";
                    return false;
                }
                _mgrBaseObj = _dnsClass.GetMethodParameters("CreateInstanceFromPropertyData");
                _mgrBaseObj["DnsServerName"] = "localhost";
                _mgrBaseObj["ContainerName"] = containerName;
                _mgrBaseObj["OwnerName"] = ownerName;
                _mgrBaseObj["IPAddress"] = ipAddress;
                if (String.IsNullOrEmpty(ttl))
                    ttl = "3600";
                _mgrBaseObj["TTL"] = ttl;
                _dnsClass.InvokeMethod("CreateInstanceFromPropertyData", _mgrBaseObj, null);
                _errMessage = "A记录:" + ownerName + "创建成功.";
                return true;
            }
            catch (Exception e)
            {
                _errMessage = e.Message;
                Console.WriteLine(e.ToString());
                return false;
            }
        }


        /// <summary>
        ///     创建MicrosoftDNS_AType
        /// </summary>
        /// <param name="dnsServerName">FQDN or IP address of the DNS Server that contains this RR</param>
        /// <param name="containerName">Name of the Container for the Zone, Cache, or RootHints instance which contains this RR</param>
        /// <param name="hostName">主机名 [如果为空或NULL,主机名将与域名保持一致.]</param>
        /// <param name="recordClass">
        ///     Class of the RR. Default value is 1. The following values are valid.1 IN (Internet) 2 CS
        ///     (CSNET)  3 CH (CHAOS) 4 HS (Hesiod)
        /// </param>
        /// <param name="ttl">Time, in seconds, that the RR can be cached by a DNS resolver</param>
        /// <param name="ipAddress">String representing the IPv4 address of the host</param>
        /// <returns></returns>
        public bool CreateAType(string dnsServerName, string containerName, string hostName, uint recordClass, uint ttl, string ipAddress)
        {
            try
            {
                string ownerName;
                if (string.IsNullOrEmpty(hostName))
                    ownerName = containerName;
                else
                    ownerName = hostName + "." + containerName;
                Create("MicrosoftDNS_AType");
                //如果区域不存在 
                if (!IsExistsZone(containerName))
                {
                    Console.WriteLine("区域:{0}不存在,创建失败", containerName);
                    _errMessage = $"区域:{containerName}不存在,创建失败";
                    return false;
                }
                if (IsExistsAType(containerName, ownerName))
                {
                    Console.WriteLine("{0}中已存在{1},创建失败", containerName, ownerName);
                    _errMessage = $"{containerName}中已存在{ownerName},创建失败";
                    return false;
                }
                _mgrBaseObj = _dnsClass.GetMethodParameters("CreateInstanceFromPropertyData");
                _mgrBaseObj["DnsServerName"] = dnsServerName;
                _mgrBaseObj["ContainerName"] = containerName;
                _mgrBaseObj["OwnerName"] = ownerName;
                _mgrBaseObj["RecordClass"] = recordClass;
                _mgrBaseObj["TTL"] = ttl;
                _mgrBaseObj["IPAddress"] = ipAddress;
                _dnsClass.InvokeMethod("CreateInstanceFromPropertyData", _mgrBaseObj, null);
                _errMessage = "A记录:" + ownerName + "创建成功.";
                return true;
            }
            catch (Exception e)
            {
                _errMessage = e.Message;
                Console.WriteLine(e.ToString());
                return false;
            }
        }


        /// <summary>
        ///     修改MicrosoftDNS_AType
        /// </summary>
        /// <param name="containerName">Name of the Container for the Zone, Cache, or RootHints instance which contains this RR</param>
        /// <param name="hostName">主机名 [如果为空或NULL,主机名将与域名保持一致.]</param>
        /// <param name="ttl">Time, in seconds, that the RR can be cached by a DNS resolver</param>
        /// <param name="ipAddress"></param>
        /// <returns></returns>
        public bool ModifyAType(string containerName, string hostName, string ipAddress, string ttl)
        {
            try
            {
                string ownerName;
                if (string.IsNullOrEmpty(hostName))
                    ownerName = containerName;
                else
                    ownerName = hostName + "." + containerName;

                //如果区域不存在 
                if (!IsExistsZone(containerName))
                {
                    Console.WriteLine("区域:{0}不存在,修改失败", containerName);
                    _errMessage = $"区域:{containerName}不存在,修改失败";
                    return false;
                }
                if (!IsExistsAType(containerName, ownerName))
                {
                    Console.WriteLine("{0}中不存在{1},修改失败", containerName, ownerName);
                    _errMessage = $"{containerName}中不存在{ownerName},修改失败";
                    return false;
                }


                _mgrObjCollection =
                    QueryDns(
                        "Select * From MicrosoftDNS_AType where ContainerName='" + containerName + "' and OwnerName='" +
                        ownerName + "'", "MicrosoftDNS_AType");

                foreach (var oManObject in _mgrObjCollection.Cast<ManagementObject>())
                {
                    //foreach (PropertyData p in oManObject.Properties) 
                    //{ 
                    //    try 
                    //    { Console.WriteLine(p.Name+"="+oManObject[p.Name]); } 
                    //    catch 
                    //    { } 
                    //} 
                    if (oManObject["IPAddress"].ToString() == ipAddress)
                    {
                        _errMessage = "A记录:" + ownerName + "修改失败,必须修改IP地址.";
                        return false;
                    }

                    _mgrBaseObj = oManObject.GetMethodParameters("Modify");
                    _mgrBaseObj["IPAddress"] = ipAddress;
                    _mgrBaseObj["TTL"] = ttl;
                    oManObject.InvokeMethod("Modify", _mgrBaseObj, null);
                    _errMessage = "A记录:" + ownerName + "修改成功.";
                    return true;
                }
                _errMessage = $"{containerName}中不存在{ownerName},修改失败";
                return false;
            }
            catch (Exception e)
            {
                _errMessage = e.Message;
                Console.WriteLine(e.ToString());
                return false;
            }
        }


        /// <summary>
        ///     删除MicrosoftDNS_AType
        /// </summary>
        /// <param name="containerName">Name of the Container for the Zone, Cache, or RootHints instance which contains this RR</param>
        /// <param name="hostName">主机名 [如果为空或NULL,主机名将与域名保持一致.]</param>
        /// <returns></returns>
        public bool DelAType(string containerName, string hostName)
        {
            try
            {
                string ownerName;
                if (string.IsNullOrEmpty(hostName))
                    ownerName = containerName;
                else
                    ownerName = hostName + "." + containerName;

                //如果区域不存在 
                if (!IsExistsZone(containerName))
                {
                    Console.WriteLine("区域:{0}不存在,删除失败", containerName);
                    _errMessage = $"区域:{containerName}不存在,删除失败";
                    return false;
                }
                if (!IsExistsAType(containerName, ownerName))
                {
                    Console.WriteLine("{0}中不存在{1},删除失败", containerName, ownerName);
                    _errMessage = $"{containerName}中不存在{ownerName},删除失败";
                    return false;
                }

                _mgrObjCollection = QueryDns($"Select * From MicrosoftDNS_AType where ContainerName='{containerName}' and OwnerName='{ownerName}'", "MicrosoftDNS_AType");

                foreach (var oManObject in _mgrObjCollection.Cast<ManagementObject>())
                {
                    oManObject.Delete();
                    _errMessage = "A记录:" + ownerName + "删除成功.";
                    return true;
                }
                _errMessage = $"{containerName}中不存在{ownerName},删除失败";
                return false;
            }
            catch (Exception e)
            {
                _errMessage = e.Message;
                Console.WriteLine(e.ToString());
                return false;
            }
        }


        /// <summary>
        ///     列出某域名下的所有A记录.
        /// </summary>
        /// <param name="containerName">Name of the Container for the Zone, Cache, or RootHints instance which contains this RR</param>
        /// <returns></returns>
        public DataTable ListExistsAType(string containerName)
        {
            var table = new DataTable("MicrosoftDNS_AType" + containerName);
            table.Columns.Add("主机名 (A)");
            table.Columns.Add("IP 地址");
            table.Columns.Add("TTL");
            try
            {
                _mgrObjCollection = QueryDns($"Select * From MicrosoftDNS_AType where ContainerName='{containerName}'");

                foreach (var oManObject in _mgrObjCollection.Cast<ManagementObject>())
                    try
                    {
                        var row = table.NewRow();
                        row["主机名 (A)"] = oManObject["OwnerName"];
                        row["IP 地址"] = oManObject["IPAddress"];
                        row["TTL"] = oManObject["TTL"];
                        table.Rows.Add(row);
                    }
                    catch (Exception e)
                    {
                        _errMessage = e.Message;
                        Console.WriteLine(e.ToString());
                    }
            }
            catch (Exception e)
            {
                _errMessage = e.Message;
                Console.WriteLine(e.ToString());
            }
            return table;
        }


        /// <summary>
        ///     列出某域名下的所有MX记录.
        /// </summary>
        /// <param name="containerName">Name of the Container for the Zone, Cache, or RootHints instance which contains this RR</param>
        /// <returns></returns>
        public DataTable ListExistsMxType(string containerName)
        {
            var table = new DataTable("MicrosoftDNS_MXType" + containerName);
            table.Columns.Add("邮件交换记录 (MX)");
            table.Columns.Add("目标主机");
            table.Columns.Add("优先级");
            table.Columns.Add("TTL");
            try
            {
                _mgrObjCollection = QueryDns($"Select * From MicrosoftDNS_MXType where ContainerName='{containerName}'");

                foreach (var oManObject in _mgrObjCollection.Cast<ManagementObject>())
                {
                    try
                    {
                        var row = table.NewRow();
                        row["目标主机"] = oManObject["MailExchange"];
                        row["邮件交换记录 (MX)"] = oManObject["OwnerName"];
                        row["优先级"] = oManObject["Preference"];
                        row["TTL"] = oManObject["TTL"];
                        table.Rows.Add(row);
                    }
                    catch
                    {
                        // ignored
                    }
                }
            }
            catch (Exception e)
            {
                _errMessage = e.Message;
                Console.WriteLine(e.ToString());
            }
            return table;
        }


        /// <summary>
        ///     列出某域名下的所有别名.
        /// </summary>
        /// <param name="containerName">Name of the Container for the Zone, Cache, or RootHints instance which contains this RR</param>
        /// <returns></returns>
        public DataTable ListExistsCnameType(string containerName)
        {
            var table = new DataTable("MicrosoftDNS_CNAMEType" + containerName);
            table.Columns.Add("别名 (CNAME)");
            table.Columns.Add("别名主机");
            table.Columns.Add("TTL");
            try
            {
                _mgrObjCollection = QueryDns($"Select * From MicrosoftDNS_CNAMEType where ContainerName='{containerName}'");
                foreach (var oManObject in _mgrObjCollection.Cast<ManagementObject>())
                {
                    try
                    {
                        var row = table.NewRow();
                        row["别名 (CNAME)"] = oManObject["OwnerName"];
                        row["别名主机"] = oManObject["PrimaryName"];
                        row["TTL"] = oManObject["TTL"];
                        table.Rows.Add(row);
                    }
                    catch
                    {
                        // ignored
                    }
                }
            }
            catch (Exception e)
            {
                _errMessage = e.Message;
                Console.WriteLine(e.ToString());
            }
            return table;
        }


        /// <summary>
        ///     判断在某MicrosoftDNS_MXType是否在指定的域中存在
        /// </summary>
        /// <param name="containerName">
        ///     主域名 主域 Name of the Container for the Zone, Cache, or RootHints instance that contains this
        ///     RR.
        /// </param>
        /// <param name="ownerName">Owner name for the RR. </param>
        /// <returns></returns>
        public bool IsExistsMxType(string containerName, string ownerName)
        {
            try
            {
                _mgrObjCollection = QueryDns($"Select * From MicrosoftDNS_MXType Where ContainerName='{containerName}' and OwnerName='{ownerName}'");

                return _mgrObjCollection.Cast<ManagementObject>().Any();
            }
            catch (Exception e)
            {
                _errMessage = e.Message;
                Console.WriteLine(e.ToString());
                return false;
            }
        }

        /// <summary>
        ///     创建MicrosoftDNS_MXType记录(邮件交换记录)
        /// </summary>
        /// <param name="hostName">主机名 [如果为空或NULL,主机名将与域名保持一致.]</param>
        /// <param name="containerName">主域 Name of the Container for the Zone, Cache, or RootHints instance that contains this RR.</param>
        /// <param name="mailExchange">目标主机 FQDN specifying a host willing to act as a mail exchange for the owner name</param>
        /// <param name="preference">优先级 Preference given to this RR among others at the same owner. Lower values are preferred</param>
        /// <param name="ttl">Time, in seconds, that the RR can be cached by a DNS resolver</param>
        /// <returns></returns>
        public bool CreateMxType(string hostName, string containerName, string mailExchange, string preference, string ttl)
        {
            try
            {
                string ownerName;
                if (string.IsNullOrEmpty(hostName))
                    ownerName = containerName;
                else
                    ownerName = hostName + "." + containerName;

                Create("MicrosoftDNS_MXType");
                //如果区域不存在 
                if (!IsExistsZone(containerName))
                {
                    Console.WriteLine("区域:{0}不存在,创建失败", containerName);
                    _errMessage = $"区域:{containerName}不存在,创建失败";
                    return false;
                }
                if (IsExistsMxType(containerName, ownerName))
                {
                    Console.WriteLine("{0}中已存在{1},创建失败", containerName, ownerName);
                    _errMessage = $"{containerName}中已存在{ownerName},创建失败";
                    return false;
                }

                _mgrBaseObj = _dnsClass.GetMethodParameters("CreateInstanceFromPropertyData");

                _mgrBaseObj["DnsServerName"] = "localhost";
                _mgrBaseObj["ContainerName"] = containerName;
                // MI["RecordClass"] = 1;  //Default value is 1.  //1 IN (Internet)  //2 CS (CSNET)   //3 CH (CHAOS)   //4 HS (Hesiod)  
                _mgrBaseObj["MailExchange"] = mailExchange;
                _mgrBaseObj["OwnerName"] = ownerName;
                _mgrBaseObj["Preference"] = preference;

                if (string.IsNullOrEmpty(ttl))
                    ttl = "3600";
                _mgrBaseObj["TTL"] = ttl;
                _dnsClass.InvokeMethod("CreateInstanceFromPropertyData", _mgrBaseObj, null);
                _errMessage = "MX记录:" + ownerName + "创建成功.";
                return true;
            }
            catch (Exception e)
            {
                _errMessage = e.Message;
                Console.WriteLine(e.ToString());
                return false;
            }
        }

        /// <summary>
        ///     修改MicrosoftDNS_MXType记录(修改邮件交换记录)
        /// </summary>
        /// <param name="hostName">主机名 [如果为空或NULL,主机名将与域名保持一致.]</param>
        /// <param name="containerName">
        ///     主域名 主域 Name of the Container for the Zone, Cache, or RootHints instance that contains this
        ///     RR.
        /// </param>
        /// <param name="ttl">[in, optional] Time, in seconds, that the RR can be cached by a DNS resolver. </param>
        /// <param name="mailExchange">[in, optional] FQDN specifying a host willing to act as a mail exchange for the owner name. </param>
        /// <param name="preference">邮件优先级 [in, optional] Preference given to this RR among others at the same owner. Lower values are preferred. </param>
        /// <returns></returns>
        public bool ModifyMxType(string hostName, string containerName, string ttl, string mailExchange, string preference)
        {
            try
            {
                string ownerName;
                if (string.IsNullOrEmpty(hostName))
                    ownerName = containerName;
                else
                    ownerName = hostName + "." + containerName;

                Create("MicrosoftDNS_MXType");
                //如果区域不存在 
                if (!IsExistsZone(containerName))
                {
                    Console.WriteLine("区域:{0}不存在,修改失败", containerName);
                    _errMessage = $"区域:{containerName}不存在,修改失败";
                    return false;
                }
                if (!IsExistsMxType(containerName, ownerName))
                {
                    Console.WriteLine("{0}中不存在{1},修改失败", containerName, ownerName);
                    _errMessage = $"{containerName}中不存在{ownerName},修改失败";
                    return false;
                }
                _mgrObjCollection =
                    QueryDns("Select * From MicrosoftDNS_MXType Where ContainerName='" + containerName +
                             "' and OwnerName='" + ownerName + "'");
                foreach (var oManObject in _mgrObjCollection.Cast<ManagementObject>())
                {
                    _mgrBaseObj = oManObject.GetMethodParameters("Modify");
                    if (string.IsNullOrEmpty(ttl))
                        ttl = "3600";

                    if ((ClearEndDot(oManObject["MailExchange"].ToString()) == ClearEndDot(mailExchange)) &&
                        (oManObject["Preference"].ToString() == preference))
                    {
                        _errMessage = "MX记录:" + ownerName + "修改失败,必须修改目标主机或邮件优先级.";
                        return false;
                    }

                    _mgrBaseObj["TTL"] = ttl;
                    _mgrBaseObj["MailExchange"] = mailExchange;
                    _mgrBaseObj["Preference"] = preference;
                    oManObject.InvokeMethod("Modify", _mgrBaseObj, null);
                    _errMessage = "MX记录:" + ownerName + "修改成功.";
                    return true;
                }
                return false;
            }
            catch (Exception e)
            {
                _errMessage = e.Message;
                Console.WriteLine(e.ToString());
                return false;
            }
        }


        /// <summary>
        ///     删除MicrosoftDNS_MXType
        /// </summary>
        /// <param name="hostName">主机名 [如果为空或NULL,主机名将与域名保持一致.]</param>
        /// <param name="containerName">
        ///     主域名 主域 Name of the Container for the Zone, Cache, or RootHints instance that contains this RR.
        /// </param>
        /// <returns></returns>
        public bool DelMxType(string hostName, string containerName)
        {
            try
            {
                string ownerName;
                if (string.IsNullOrEmpty(hostName))
                    ownerName = containerName;
                else
                    ownerName = hostName + "." + containerName;

                Create("MicrosoftDNS_MXType");
                //如果区域不存在 
                if (!IsExistsZone(containerName))
                {
                    Console.WriteLine("区域:{0}不存在,删除失败", containerName);
                    _errMessage = $"区域:{containerName}不存在,删除失败";
                    return false;
                }
                if (!IsExistsMxType(containerName, ownerName))
                {
                    Console.WriteLine("{0}中不存在{1},删除失败", containerName, ownerName);
                    _errMessage = $"{containerName}中不存在{ownerName},删除失败";
                    return false;
                }
                _mgrObjCollection =
                    QueryDns("Select * From MicrosoftDNS_MXType Where ContainerName='" + containerName +
                             "' and OwnerName='" + ownerName + "'");

                foreach (var oManObject in _mgrObjCollection.Cast<ManagementObject>())
                {
                    oManObject.Delete();
                    _errMessage = "MX记录:" + ownerName + "删除成功.";
                    return true;
                }
                return false;
            }
            catch (Exception e)
            {
                _errMessage = e.Message;
                Console.WriteLine(e.ToString());
                return false;
            }
        }


        /// <summary>
        ///     判断在某MicrosoftDNS_CNAMEType是否在指定的域中存在
        /// </summary>
        /// <param name="containerName">
        ///     主域名 主域 Name of the Container for the Zone, Cache, or RootHints instance that contains this
        ///     RR.
        /// </param>
        /// <param name="ownerName">Owner name for the RR. </param>
        /// <returns></returns>
        public bool IsExistsCnameType(string containerName, string ownerName)
        {
            try
            {
                _mgrObjCollection =
                    QueryDns("Select * From MicrosoftDNS_CNAMEType Where ContainerName='" + containerName +
                             "' and OwnerName='" + ownerName + "'");

                return _mgrObjCollection.Cast<ManagementObject>().Any();
            }
            catch (Exception e)
            {
                _errMessage = e.Message;
                Console.WriteLine(e.ToString());
                return false;
            }
        }

        /// <summary>
        ///     创建MicrosoftDNS_CNAMEType记录(别名)
        /// </summary>
        /// <param name="hostName">主机名 [如果为空或NULL,主机名将与域名保持一致.]</param>
        /// <param name="containerName">主域 Name of the Container for the Zone, Cache, or RootHints instance that contains this RR.</param>
        /// <param name="primaryName">in] Primary name of the CNAME RR</param>
        /// <param name="ttl">Time, in seconds, that the RR can be cached by a DNS resolver</param>
        /// <returns></returns>
        public bool CreateCnameType(string hostName, string containerName, string primaryName, string ttl)
        {
            try
            {
                string ownerName;
                if (string.IsNullOrEmpty(hostName))
                    ownerName = containerName;
                else
                    ownerName = hostName + "." + containerName;

                Create("MicrosoftDNS_CNAMEType");
                //如果区域不存在 
                if (!IsExistsZone(containerName))
                {
                    Console.WriteLine("区域:{0}不存在,创建失败", containerName);
                    _errMessage = $"区域:{containerName}不存在,创建失败";
                    return false;
                }
                if (IsExistsCnameType(containerName, ownerName))
                {
                    Console.WriteLine("{0}中已存在{1},创建失败", containerName, ownerName);
                    _errMessage = $"{containerName}中已存在{ownerName},创建失败";
                    return false;
                }

                _mgrBaseObj = _dnsClass.GetMethodParameters("CreateInstanceFromPropertyData");

                _mgrBaseObj["DnsServerName"] = "localhost";
                _mgrBaseObj["ContainerName"] = containerName;
                // MI["RecordClass"] = 1;  //Default value is 1.  //1 IN (Internet)  //2 CS (CSNET)   //3 CH (CHAOS)   //4 HS (Hesiod)  
                _mgrBaseObj["PrimaryName"] = primaryName;
                _mgrBaseObj["OwnerName"] = ownerName;

                if (string.IsNullOrEmpty(ttl))
                    ttl = "3600";
                _mgrBaseObj["TTL"] = ttl;
                _dnsClass.InvokeMethod("CreateInstanceFromPropertyData", _mgrBaseObj, null);
                _errMessage = "CNAME:" + ownerName + "创建成功.";
                return true;
            }
            catch (Exception e)
            {
                _errMessage = e.Message;
                Console.WriteLine(e.ToString());
                return false;
            }
        }

        /// <summary>
        ///     修改MicrosoftDNS_CNAMEType记录(别名)
        /// </summary>
        /// <param name="hostName">主机名 [如果为空或NULL,主机名将与域名保持一致.]</param>
        /// <param name="containerName">
        ///     主域名 主域 Name of the Container for the Zone, Cache, or RootHints instance that contains this
        ///     RR.
        /// </param>
        /// <param name="ttl">[in, optional] Time, in seconds, that the RR can be cached by a DNS resolver. </param>
        /// <param name="primaryName">in] Primary name of the CNAME RR</param>
        /// <returns></returns>
        public bool ModifyCnameType(string hostName, string containerName, string ttl, string primaryName)
        {
            try
            {
                string ownerName;
                if (string.IsNullOrEmpty(hostName))
                    ownerName = containerName;
                else
                    ownerName = hostName + "." + containerName;

                Create("MicrosoftDNS_CNAMEType");
                //如果区域不存在 
                if (!IsExistsZone(containerName))
                {
                    Console.WriteLine("区域:{0}不存在,修改失败", containerName);
                    _errMessage = $"区域:{containerName}不存在,修改失败";
                    return false;
                }
                if (!IsExistsCnameType(containerName, ownerName))
                {
                    Console.WriteLine("{0}中不存在{1},修改失败", containerName, ownerName);
                    _errMessage = $"{containerName}中不存在{ownerName},修改失败";
                    return false;
                }
                _mgrObjCollection =
                    QueryDns("Select * From MicrosoftDNS_CNAMEType Where ContainerName='" + containerName +
                             "' and OwnerName='" + ownerName + "'");
                foreach (var oManObject in _mgrObjCollection.Cast<ManagementObject>())
                {
                    if (ClearEndDot(oManObject["PrimaryName"].ToString()) == ClearEndDot(primaryName))
                    {
                        _errMessage = "CName记录:" + ownerName + "修改失败,必须修改别名主机.";
                        return false;
                    }


                    _mgrBaseObj = oManObject.GetMethodParameters("Modify");
                    if (string.IsNullOrEmpty(ttl))
                        ttl = "3600";
                    _mgrBaseObj["TTL"] = ttl;
                    _mgrBaseObj["PrimaryName"] = primaryName;
                    oManObject.InvokeMethod("Modify", _mgrBaseObj, null);
                    _errMessage = "CNAME:" + ownerName + "修改成功.";
                    return true;
                }
                return false;
            }
            catch (Exception e)
            {
                _errMessage = e.Message;
                Console.WriteLine(e.ToString());
                return false;
            }
        }


        /// <summary>
        ///     删除MicrosoftDNS_CNAMEType
        /// </summary>
        /// <param name="hostName">主机名 [如果为空或NULL,主机名将与域名保持一致.]</param>
        /// <param name="containerName">
        ///     主域名 主域 Name of the Container for the Zone, Cache, or RootHints instance that contains this
        ///     RR.
        /// </param>
        /// <returns></returns>
        public bool DelCnameType(string hostName, string containerName)
        {
            try
            {
                string ownerName;
                if (string.IsNullOrEmpty(hostName))
                    ownerName = containerName;
                else
                    ownerName = hostName + "." + containerName;

                Create("MicrosoftDNS_CNAMEType");
                //如果区域不存在 
                if (!IsExistsZone(containerName))
                {
                    Console.WriteLine("区域:{0}不存在,删除失败", containerName);
                    _errMessage = $"区域:{containerName}不存在,删除失败";
                    return false;
                }
                if (!IsExistsCnameType(containerName, ownerName))
                {
                    Console.WriteLine("{0}中不存在{1},删除失败", containerName, ownerName);
                    _errMessage = $"{containerName}中不存在{ownerName},删除失败";
                    return false;
                }
                _mgrObjCollection =
                    QueryDns("Select * From MicrosoftDNS_CNAMEType Where ContainerName='" + containerName +
                             "' and OwnerName='" + ownerName + "'");

                foreach (var oManObject in _mgrObjCollection.Cast<ManagementObject>())
                {
                    oManObject.Delete();
                    _errMessage = "CNAME:" + ownerName + "删除成功.";
                    return true;
                }
                return false;
            }
            catch (Exception e)
            {
                _errMessage = e.Message;
                Console.WriteLine(e.ToString());
                return false;
            }
        }


        /// <summary>
        ///     去除以.结尾的字符串的.
        /// </summary>
        /// <param name="str"></param>
        /// <returns></returns>
        private string ClearEndDot(string str)
        {
            var returnStr = str;
            if (!string.IsNullOrEmpty(str))
            {
                var l = str.LastIndexOf(".", StringComparison.OrdinalIgnoreCase);
                if ((l != -1) && (l == str.Length - 1))
                    returnStr = str.Substring(0, str.Length - 1);
            }
            return returnStr;
        }
    }
}