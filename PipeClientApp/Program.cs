using System;
using System.Collections.Generic;
using System.IO.Pipes;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace PipeClientApp
{
    public class PipeClient
    {
        private readonly NamedPipeClientStream m_pipe;

        public PipeClient(string serverName,string message)
        {
            m_pipe = new NamedPipeClientStream(serverName, "Echo", PipeDirection.InOut,
                PipeOptions.Asynchronous | PipeOptions.WriteThrough);
            m_pipe.Connect();
            m_pipe.ReadMode = PipeTransmissionMode.Message;

            byte[] output = Encoding.UTF8.GetBytes(message);
            m_pipe.BeginWrite(output, 0, output.Length,_writeDone,null);
        }

        private void _writeDone(IAsyncResult result)
        {
            //响应已发给了客户端，关闭我们的这段连接
            m_pipe.EndWrite(result);
            byte[] data =new byte[1000];
            m_pipe.BeginRead(data, 0, data.Length, _gotResponse, data);
        }

        private void _gotResponse(IAsyncResult result)
        {
            //客户端向我们发送一个请求处理它
            int bytesRead = m_pipe.EndRead(result);
            byte[] data = (byte[])result.AsyncState;
            //将字符转换为大写
            data = Encoding.UTF8.GetBytes(Encoding.UTF8.GetString(data, 0, bytesRead).ToUpper().ToCharArray());
            //将响应的异步地址发送给客户端
            m_pipe.BeginWrite(data, 0, data.Length, _writeDone, null);
        }
    }
    class Program
    {
        static void Main(string[] args)
        {
            for (int n = 0; n < 100; n++)
            {
                new PipeClient("localhost", "request #" + n);
            }
            Console.ReadLine();
        }
    }
}
