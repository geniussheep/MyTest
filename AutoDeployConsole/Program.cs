using System;
using System.Collections.Generic;
using System.Diagnostics;
using System.Linq;
using System.Management;
using System.Text.RegularExpressions;
using AutoDeployCommon;
using Benlai.Common;
using Microsoft.Web.Administration;

namespace AutoDeploy
{

    public class ModelTest
    {
        public int A { get; set; }
        public string B { get; set; }
    }

    class Program
    {



        static void Main(string[] args)
        {
            try
            {
                #region IIS

//                using (ServerManager mgr = new ServerManager(@"C:\Windows\System32\inetsrv\config\applicationHost.config"))
//                {
//                    Site site = mgr.Sites["test.plf.benlai.com"];
//                    
//                }
                var poolName = "test.plf.benlai.com";

                IisUtility.RecycleAppPool(poolName, "");
                Console.WriteLine($"RecycleAppPool");
                IisUtility.StopAppPool(poolName,"");
                Console.WriteLine("StopAppPool");
                IisUtility.RecycleAppPool(poolName, "");
                Console.WriteLine($"RecycleAppPool");
                IisUtility.StopAppPool(poolName, "");
                Console.WriteLine("StopAppPool");
                IisUtility.StopAppPool(poolName, "");
                Console.WriteLine("StopAppPool");
                IisUtility.StartAppPool(poolName, "");
                Console.WriteLine("StartAppPool");
                IisUtility.StopAppPool(poolName, "");
                Console.WriteLine("StopAppPool");
                IisUtility.StartAppPool(poolName, "");
                Console.WriteLine("StartAppPool");
                IisUtility.StartAppPool(poolName, "");
                Console.WriteLine("StartAppPool");
                Console.ReadKey();


                //Console.WriteLine("Please wait...");

                //Process pro = new Process();

                //ProcessStartInfo psi = new ProcessStartInfo()
                //{
                //    UseShellExecute = true,
                //    RedirectStandardOutput = false,
                //    RedirectStandardError = false
                //};
                //psi.Arguments = @"/Online /Enable-Feature /FeatureName:IIS-ApplicationDevelopment /FeatureName:IIS-ASP " +
                //    "/FeatureName:IIS-ASPNET /FeatureName:IIS-BasicAuthentication /FeatureName:IIS-CGI " +
                //    "/FeatureName:IIS-ClientCertificateMappingAuthentication /FeatureName:IIS-CommonHttpFeatures " +
                //    "/FeatureName:IIS-CustomLogging /FeatureName:IIS-DefaultDocument /FeatureName:IIS-DigestAuthentication " +
                //    "/FeatureName:IIS-DirectoryBrowsing /FeatureName:IIS-FTPExtensibility /FeatureName:IIS-FTPServer " +
                //    "/FeatureName:IIS-FTPSvc /FeatureName:IIS-HealthAndDiagnostics /FeatureName:IIS-HostableWebCore " +
                //    "/FeatureName:IIS-HttpCompressionDynamic /FeatureName:IIS-HttpCompressionStatic /FeatureName:IIS-HttpErrors " +
                //    "/FeatureName:IIS-HttpLogging /FeatureName:IIS-HttpRedirect /FeatureName:IIS-HttpTracing " +
                //    "/FeatureName:IIS-IIS6ManagementCompatibility /FeatureName:IIS-IISCertificateMappingAuthentication " +
                //    "/FeatureName:IIS-IPSecurity /FeatureName:IIS-ISAPIExtensions /FeatureName:IIS-ISAPIFilter " +
                //    "/FeatureName:IIS-LegacyScripts /FeatureName:IIS-LegacySnapIn /FeatureName:IIS-LoggingLibraries /" +
                //    "FeatureName:IIS-ManagementConsole  /FeatureName:IIS-ManagementScriptingTools /FeatureName:IIS-ManagementService " +
                //    "/FeatureName:IIS-Metabase /FeatureName:IIS-NetFxExtensibility /FeatureName:IIS-ODBCLogging " +
                //    "/FeatureName:IIS-Performance /FeatureName:IIS-RequestFiltering /FeatureName:IIS-RequestMonitor /FeatureName:IIS-Security " +
                //    "/FeatureName:IIS-ServerSideIncludes /FeatureName:IIS-StaticContent /FeatureName:IIS-URLAuthorization " +
                //    "/FeatureName:IIS-WebDAV /FeatureName:IIS-WebServer /FeatureName:IIS-WebServerManagementTools " +
                //    "/FeatureName:IIS-WebServerRole /FeatureName:IIS-WindowsAuthentication /FeatureName:IIS-WMICompatibility " +
                //    "/FeatureName:WAS-ConfigurationAPI /FeatureName:WAS-NetFxEnvironment /FeatureName:WAS-ProcessModel " +
                //    "/FeatureName:WAS-WindowsActivationService\" > out.txt ";
                //psi.WindowStyle = ProcessWindowStyle.Normal;
                //psi.ErrorDialog = true;

                //psi.FileName = "cmd.exe";

                //if (!File.Exists(@"C:\Windows\SysWOW64\dism.exe"))
                //{
                //    psi.Arguments = @"/C ""C:\Windows\SysWOW64\dism.exe " + psi.Arguments;
                //    psi.WorkingDirectory = @"C:\Windows\SysWOW64\";
                //}
                //else if (File.Exists(@"C:\Windows\System32\dism.exe"))
                //{
                //    psi.Arguments = @"/C ""C:\Windows\System32\dism.exe " + psi.Arguments;
                //    psi.WorkingDirectory = @"C:\Windows\System32\";
                //}

                //pro.StartInfo = psi;
                //pro.Start();
                //pro.WaitForExit();

                //Console.WriteLine("IIS is installed");
                //Console.WriteLine("PLEASE restart the computer once");
                //Thread.Sleep(5000);

                //try
                //{
                //    AutoDeploy.Program.SetupIIS();
                //    Console.WriteLine("Done. Press any key to close.");
                //}
                //catch (Exception ex)
                //{
                //    Console.WriteLine("Exception occurred:" + ex.Message);
                //}
                //Console.ReadLine();

                //                IisUtility.ApplicationHostConfigurationPath = @"C:\Windows\System32\inetsrv\config\applicationHost.config";
                //                Console.WriteLine(IisUtility.ApplicationHostConfigurationPath);
                //                if (IisUtility.VerifyWebSiteIsExist("TestWeb"))
                //                {
                //                    IisUtility.DeleteSite("TestWeb");
                //                Console.WriteLine("delete web site success");
                //                }
                //                if (!IisUtility.VerifyWebSiteIsExist("TestWeb"))
                //                {
                //                    IisUtility.CreateSite("TestWeb", "*:8001:localhost", @ConfigurationManager.AppSettings["WebPhysicalPath"], 650, 20, NetVersion.V4, 25, @"D:\Temp", LogFormat.W3c, LogExtFileFlags.BytesRecv | LogExtFileFlags.ClientIP | LogExtFileFlags.Cookie, LoggingRolloverPeriod.MaxSize, 50,);
                //                }
                //                Console.WriteLine("create web site success");

                #endregion

                //#region Window Service

                //                var serviceName = "TestWinService";
                //
                //                WindowServiceInstallUtility.InstallWinService(serviceName, @ConfigurationManager.AppSettings["WindowServiceProgramFullPath"],2);
                //
                //                WindowsServiceUtility.StartWinService(serviceName, 2);
                //
                //                WindowsServiceUtility.StopWinService(serviceName, 2);
                //
                //                WindowServiceUtility.ChangeServiceStartType(WindowServiceStartType.Automatic, serviceName);
                //
                //                WindowServiceUtility.StartService(serviceName, 2);

                //                WindowServiceUtility.UninstallService(serviceName, @ConfigurationManager.AppSettings["WindowServiceProgramFullPath"]);

                //#endregion

                //    var test = new List<ModelTest>
                //{
                //        new ModelTest {A = 1, B = "1"},
                //        new ModelTest {A = 2, B = "2"},
                //        new ModelTest {A = 3, B = "3"},
                //        new ModelTest {A = 4, B = "4"}
                //    };
                //    Console.WriteLine(new Version(1,0));
                //    test.Select(s => s.A = s.A + 1).ToList().ForEach(Console.WriteLine);
                Console.ReadKey();
            }
            catch (Exception ex)
            {
                LogInfoWriter.GetInstance("DeploymentTest").Error(ex);
            }
            Console.WriteLine("end");
            Console.ReadKey();
        }

        private static string SetupIIS()
        {
            // In command prompt run this command to see all the features names which are equivalent to UI features.
            // c:\>dism /online /get-features /format:table 
            var featureNames = new List<string>
            {
                "IIS-ApplicationDevelopment",
                "IIS-ISAPIExtensions",
                "IIS-ISAPIFilter",
                "IIS-CommonHttpFeatures",
                "IIS-DefaultDocument",
                "IIS-HttpErrors",
                "IIS-StaticContent",
                "IIS-HealthAndDiagnostics",
                "IIS-HttpLogging",
                "IIS-HttpTracing",
                "IIS-WebServer",
                "IIS-WebServerRole",
                "IIS-ManagementConsole",
            };

            Console.WriteLine("Checking the Operating System...\n");

            ManagementObjectSearcher obj = new ManagementObjectSearcher("select * from Win32_OperatingSystem");
            try
            {
                foreach (ManagementObject wmi in obj.Get())
                {
                    string Name = wmi.GetPropertyValue("Caption").ToString();

                    // Remove all non-alphanumeric characters so that only letters, numbers, and spaces are left.
                    // Imp. for 32 bit window server 2008
                    Name = Regex.Replace(Name.ToString(), "[^A-Za-z0-9 ]", "");

                    if (Name.Contains("Server 2012 R2") || Name.Contains("Windows 81"))
                    {
                        featureNames.Add("IIS-ASPNET45");
                        featureNames.Add("IIS-NetFxExtensibility45");
                    }
                    else if (Name.Contains("Server 2008 R2") || Name.Contains("Windows 7") || Name.Contains("Windows 10"))
                    {
                        featureNames.Add("IIS-ASPNET");
                        featureNames.Add("IIS-NetFxExtensibility");
                    }
                    else
                    {
                        featureNames.Clear();
                    }

                    string Version = (string)wmi["Version"];
                    string Architecture = (string)wmi["OSArchitecture"];

                    Console.WriteLine("Operating System details:");
                    Console.WriteLine("OS Name: " + Name);
                    Console.WriteLine("Version: " + Version);
                    Console.WriteLine("Architecture: " + Architecture + "\n");
                }
            }
            catch (Exception ex)
            {
                Console.WriteLine("Exception occurred:" + ex.Message);
            }

            return Run(
                "dism",
                String.Format(
                    "/NoRestart /Online /Enable-Feature {0}",
                    String.Join(
                        " ",
                        featureNames.Select(name => String.Format("/FeatureName:{0}", name)))));
        }

        private static string Run(string fileName, string arguments)
        {
            Console.WriteLine("Enabling IIS features...");
            Console.WriteLine(arguments);

            using (var process = Process.Start(new ProcessStartInfo
            {
                FileName = fileName,
                Arguments = arguments,
                CreateNoWindow = true,
                WindowStyle = ProcessWindowStyle.Hidden,
                RedirectStandardOutput = true,
                UseShellExecute = false,
            }))
            {
                if (process != null)
                {
                    process.WaitForExit();
                    return process.StandardOutput.ReadToEnd();
                }
                return "";
            }
        }
    }
}
