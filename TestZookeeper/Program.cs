using log4net;
using Spring.Threading.Locks;
using System;
using System.Text;
using Benlai.Zookeeper;
using Benlai.Zookeeper.Interface;

namespace TestConsole
{
    internal class DataChange : IZkDataListener
    {
        private ReentrantLock dataLock;

        private ICondition dataExistsOrChanged;

        private ZkClient zkClient;

        public DataChange(ReentrantLock dataLock, ICondition dataExistsOrChanged,  ZkClient zkClient)
        {
            Console.WriteLine("init data change...");
            this.dataLock = dataLock;
            this.dataExistsOrChanged = dataExistsOrChanged;
            this.zkClient = zkClient;
        }

        public void HandleDataChange(string dataPath, object data)
        {
            Console.WriteLine("run data change HandleDataChange ,path:dataPath");
            this.dataLock.Lock();

            try
            {
                Console.WriteLine("path:"+dataPath +",data:"+ zkClient.ReadData<string>(dataPath));
                this.dataExistsOrChanged.Signal();
            }
            finally
            {
                this.dataLock.Unlock();
            }

        }

        public void HandleDataDeleted(string dataPath)
        {
            dataLock.Lock();
            try
            {
                dataExistsOrChanged.Signal();
            }
            finally
            {
                dataLock.Unlock();
            }
        }
    }

    public class ZkStringSerializer : IZkSerializer
    {
        public byte[] Serialize(object data)
        {
            return Encoding.UTF8.GetBytes(data.ToString());
        }

        public object Deserialize(byte[] bytes)
        {
            if (bytes == null)
            {
                return null;
            }

            return Encoding.UTF8.GetString(bytes);
        }
    }

    class Program
    {
        public const int ZkConnectionTimeout = 6000;

        public const int ZkSessionTimeout = 6000;

        public static string ZkConnect = "192.168.60.6:2181";

        static void Main(string[] args)
        {
            ZkConnection zkConnection = new ZkConnection("192.168.60.6:2181");
            ZkClient zkClient = new ZkClient(ZkConnect, ZkSessionTimeout, ZkConnectionTimeout, new ZkStringSerializer());
            var dataLock = new ReentrantLock();
            var dataExistsOrChanged = dataLock.NewCondition();
            zkClient.SubscribeDataChanges("/testyy", new DataChange(dataLock, dataExistsOrChanged, zkClient));
            //dataExistsOrChanged.Await(TimeSpan.FromMilliseconds(1000));

            Console.Read();
        }
    }
}
