using System;

namespace Benlai.Zookeeper.Exceptions
{
    /// <summary>
    /// Zk连接超时异常类
    /// </summary>
    public class ZkTimeoutException : Exception
    {
        public ZkTimeoutException()
        {
        }

        public ZkTimeoutException(string message)
            : base(message)
        {
        }

        public ZkTimeoutException(string message, Exception innerException)
            : base(message, innerException)
        {
        }
    }
}
