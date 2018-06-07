namespace Benlai.Zookeeper.Interface
{
    /// <summary>
    /// Zk节点数据更新接口
    /// </summary>
    public interface IDataUpdater<TData>
    {
        /// <summary>
        /// 更新当前Zk节点内数据
        /// </summary>
        /// <param name="currentData">当前Zk内的数据</param>
        /// <returns>更新后的数据</returns>
        TData Update(TData currentData);
    }
}
