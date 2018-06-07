using Benlai.Application.AutoPublish.AppStart;

namespace Benlai.Application.AutoPublish
{
    internal class Program
    {
        /// <summary>
        ///     入口函数
        /// </summary>
        /// <param name="args">-d 启动Console程序（调试用），-i 安装windows服务，-u卸载windows服务</param>
        private static void Main(string[] args)
        {
            new SOAServiceHost().Run(args);
        }
    }
}