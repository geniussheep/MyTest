using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace NugetPublishExtTool.Utils
{
    public class CmdUtils
    {
        public static string ExecCmd(string cmdStr)
        {

            System.Diagnostics.Process p = new System.Diagnostics.Process
            {
                StartInfo =
                {
                    FileName = "cmd.exe",
                    //是否使用操作系统shell启动
                    UseShellExecute = false,
                    //接受来自调用程序的输入信息
                    RedirectStandardInput = true,
                    //由调用程序获取输出信息
                    RedirectStandardOutput = true,
                    //重定向标准错误输出
                    RedirectStandardError = true,
                    //不显示程序窗口
                    CreateNoWindow = true
                }
            };
            //启动程序
            p.Start();

            //向cmd窗口发送输入信息
            //p.StandardInput.WriteLine("exit");
            //向标准输入写入要执行的命令。这里使用&是批处理命令的符号，表示前面一个命令不管是否执行成功都执行后面(exit)命令，如果不执行exit命令，后面调用ReadToEnd()方法会假死
            //同类的符号还有&&和||前者表示必须前一个命令执行成功才会执行后面的命令，后者表示必须前一个命令执行失败才会执行后面的命令
            p.StandardInput.WriteLine(cmdStr + "&exit");
            p.StandardInput.AutoFlush = true;

            //获取cmd窗口的输出信息
            string output = p.StandardOutput.ReadToEnd();

            p.WaitForExit();//等待程序执行完退出进程
            p.Close();
            return output;
        }
    }
}
