using System;
using System.Collections.Generic;
using Org.Apache.Zookeeper.Data;
using ZooKeeperNet;

namespace Benlai.Zookeeper.Interface
{
    /// <summary>
    /// Zk连接器接口
    /// </summary>
    public interface IZkConnection : IDisposable
    {
        void Connect(IWatcher watcher);

        string Create(string path, byte[] data, CreateMode mode);

        void Delete(string path);

        bool Exists(string path, bool watch);

        IEnumerable<string> GetChildren(string path, bool watch);

        byte[] ReadData(string path, Stat stat, bool watch);

        void WriteData(string path, byte[] data, int expectedVersion);

        Stat WriteDataReturnStat(string path, byte[] data, int expectedVersion);

        ZooKeeper.States ZookeeperState { get; }

        long GetCreateTime(string path);

        string Servers { get; }
    }
}
