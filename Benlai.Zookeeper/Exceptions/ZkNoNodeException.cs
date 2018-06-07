using System;

namespace Benlai.Zookeeper.Exceptions
{
    /// <summary>
    /// Zk节点不存在异常类
    /// </summary>
    public class ZkNoNodeException : ZkException
    {
        public ZkNoNodeException()
        {
        }

        public ZkNoNodeException(string message)
            : base(message)
        {
        }

        public ZkNoNodeException(string message, Exception innerException)
            : base(message, innerException)
        {
        }
    }
}
