using System.Collections.Generic;

namespace Benlai.Zookeeper.Interface
{

    /// <summary>
    /// Zk节点下的子节点变化监听器接口
    /// </summary>
    public interface IZkChildListener
    {
        /// <summary>
        /// 监听到指定节点下的子节点变化时指定具体处理逻辑
        /// </summary>
        /// <param name="parentPath">父节点路径</param>
        /// <param name="currentChilds">当前父节点结点下的子节点集合</param>
        void HandleChildChange(string parentPath, IEnumerable<string> currentChilds);
    }
}
