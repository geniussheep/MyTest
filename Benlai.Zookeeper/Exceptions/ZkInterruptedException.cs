using System;

namespace Benlai.Zookeeper.Exceptions
{
    /// <summary>
    /// Zk连接中断异常
    /// </summary>
    public class ZkInterruptedException : Exception
    {
        public ZkInterruptedException()
        {
        }

        public ZkInterruptedException(string message)
            : base(message)
        {
        }

        public ZkInterruptedException(string message, Exception innerException)
            : base(message, innerException)
        {
        }

        public ZkInterruptedException(Exception innerException)
            : base(string.Empty, innerException)
        {
        }
    }
}
