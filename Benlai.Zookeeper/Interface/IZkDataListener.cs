namespace Benlai.Zookeeper.Interface
{
    /// <summary>
    /// Zk节点数据监听器，用于注册对某个节点内数据进行监听的接口
    /// </summary>
    public interface IZkDataListener
    {
        void HandleDataChange(string dataPath, object data);

        void HandleDataDeleted(string dataPath);
    }
}
