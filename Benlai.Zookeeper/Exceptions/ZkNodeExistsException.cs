using System;

namespace Benlai.Zookeeper.Exceptions
{
    /// <summary>
    /// ZK节点已存在异常类
    /// </summary>
    public class ZkNodeExistsException : ZkException
    {
        public ZkNodeExistsException()
        {
        }

        public ZkNodeExistsException(string message)
            : base(message)
        {
        }

        public ZkNodeExistsException(string message, Exception innerException)
            : base(message, innerException)
        {
        }
    }
}
