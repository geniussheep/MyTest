using System;

namespace Benlai.Zookeeper.Exceptions
{
    /// <summary>
    /// Zk非法版本异常类
    /// </summary>
    public class ZkBadVersionException : ZkException
    {
        public ZkBadVersionException()
        {
        }

        public ZkBadVersionException(string message)
            : base(message)
        {
        }

        public ZkBadVersionException(string message, Exception innerException)
            : base(message, innerException)
        {
        }
    }
}
