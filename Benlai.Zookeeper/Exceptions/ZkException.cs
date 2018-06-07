using System;
using ZooKeeperNet;

namespace Benlai.Zookeeper.Exceptions
{
    /// <summary>
    /// Zk通用异常类
    /// </summary>
    public class ZkException : Exception
    {
        public ZkException()
        {
        }

        public ZkException(string message)
            : base(message)
        {
        }

        public ZkException(string message, Exception innerException)
            : base(message, innerException)
        {
        }

        public static ZkException Create(KeeperException e)
        {
            switch (e.ErrorCode)
            {
                //节点不存在异常
                case KeeperException.Code.NONODE:
                    return new ZkNoNodeException(e.Message, e);
                //非法版本异常
                case KeeperException.Code.BADVERSION:
                    return new ZkBadVersionException(e.Message, e);
                //节点已存在异常
                case KeeperException.Code.NODEEXISTS:
                    return new ZkNodeExistsException(e.Message, e);
                default:
                    return new ZkException(e.Message, e);
            }
        }
    }
}
