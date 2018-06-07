using System;
using System.Collections.Generic;
using System.IO.Pipes;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace PipServerApp
{
    public class PipeServer
    {
        //每个服务器对象在这个管道上执行异步操作
        private readonly NamedPipeServerStream m_pipe = new NamedPipeServerStream("Echo", PipeDirection.InOut, -1,
            PipeTransmissionMode.Message, PipeOptions.Asynchronous | PipeOptions.WriteThrough);

        public PipeServer()
        {
            //异步的接受一个客户端连接
            m_pipe.BeginWaitForConnection(_clientConnected, null);
        }

        private void _clientConnected(IAsyncResult result)
        {
            //一个客户端建立了连接，让我们接受另一个客户端
            new PipeServer();

            //接受客户端连接
            m_pipe.EndWaitForConnection(result);

            //异步第从客户端读取一个请求
            byte[] data = new byte[1000];
            m_pipe.BeginRead(data, 0, data.Length, _gotRequest, data);
        }

        private void _gotRequest(IAsyncResult result)
        {
            //客户端向我们发送一个请求处理它
            int bytesRead = m_pipe.EndRead(result);
            byte[] data = (byte[])result.AsyncState;
            //将字符转换为大写
            data = Encoding.UTF8.GetBytes(Encoding.UTF8.GetString(data, 0, bytesRead).ToUpper().ToCharArray());
            //将响应的异步地址发送给客户端
            m_pipe.BeginWrite(data, 0, data.Length, _writeDone, null);
        }

        private void _writeDone(IAsyncResult result)
        {
            //响应已发给了客户端，关闭我们的这段连接
            m_pipe.EndWrite(result);
            m_pipe.Close();
        }
    }
    class Program
    {
        static void Main(string[] args)
        {
            for (int n = 0; n < Environment.ProcessorCount; n++)
            {
                new PipeServer();
            }
            Console.WriteLine("press <Enter> to tierminate this server app");
            Console.ReadLine();
        }
    }
}
            //long ms = (new DateTime(2015,7,31,0,0,0).Ticks - DateTime.Now.Ticks) / 10000;
            //if (ms < 0)
            //    ms = 0;
            //var timer = new Timer((a) =>
            //{
            //    var systemOutbox = new Sg2Community_MailOutBox();
            //    using (var db = new MongoDBHelper(ConnectionString.Sg2Community_Mongodb))
            //    {
            //        const int threadCount = 10;
            //        try
            //        {
            //            systemOutbox =
            //                db.GetByCondition<Sg2Community_MailOutBox>(mj => mj.UserId == SystemUser.UserId)
            //                    .FirstOrDefault();
            //        }
            //        catch
            //        {
            //            systemOutbox = new Sg2Community_MailOutBox();
            //        }
            //        var systemMail = systemOutbox.MailList.FirstOrDefault(m => m.Title.Contains("官职体验领奖") && !m.IsSend);
            //        if (systemMail !=null)
            //        {
            //            systemMail.IsSend = true;
            //        }
            //        Mail.SendSystemMail(systemMail);
            //        Console.WriteLine("send mail success!");
            //    }
            //}, null, ms, Timeout.Infinite);