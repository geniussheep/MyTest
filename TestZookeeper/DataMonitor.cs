using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using ZooKeeperNet;

namespace TestZookeeper
{
    public class DataMonitor : IWatcher
    {

        ZooKeeper zk;

        String znode;

        IWatcher chainedWatcher;

        public bool dead;

        DataMonitorListener listener;

        byte[] prevData;

        public DataMonitor(ZooKeeper zk, String znode, IWatcher chainedWatcher,
                DataMonitorListener listener)
        {
            this.zk = zk;
            this.znode = znode;
            this.chainedWatcher = chainedWatcher;
            this.listener = listener;
            // Get things started by checking if the node exists. We are going
            // to be completely event driven
            zk.Exists(znode, true);
        }

        /**
         * Other classes use the DataMonitor by implementing this method
         */
        public interface DataMonitorListener
        {
            /**
             * The existence status of the node has changed.
             */
            void Exists(byte[] data);

            /**
             * The ZooKeeper session is no longer valid.
             *
             * @param rc
             *                the ZooKeeper reason code
             */
            void Closing(int rc);
        }

        public void Process(WatchedEvent watchedEvent)
        {
            String path = watchedEvent.Path;
            if (watchedEvent.Type == EventType.None)
            {
                // We are are being told that the state of the
                // connection has changed
                switch (watchedEvent.State)
                {
                    case KeeperState.SyncConnected:
                        // In this particular example we don't need to do anything
                        // here - watches are automatically re-registered with 
                        // server and any watches triggered while the client was 
                        // disconnected will be delivered (in order of course)
                        break;
                    case KeeperState.Expired:
                        // It's all over
                        dead = true;
                        listener.Closing((int)KeeperException.Code.SESSIONEXPIRED);
                        break;
                }
            }
            else
            {
                if (path != null && path == znode)
                {
                    // Something has changed on the node, let's find out
                    zk.Exists(znode, true);
                }
            }
            if (chainedWatcher != null)
            {
                chainedWatcher.Process(watchedEvent);
            }
        }

        public void ProcessResult(int rc, String path, Object ctx, KeeperState stat)
        {
            bool exists;
            switch ((KeeperException.Code)rc)
            {
                case KeeperException.Code.OK:
                    exists = true;
                    break;
                case KeeperException.Code.NONODE:
                    exists = false;
                    break;
                case KeeperException.Code.SESSIONEXPIRED:
                case KeeperException.Code.NOAUTH:
                    dead = true;
                    listener.Closing(rc);
                    return;
                default:
                    // Retry errors
                    zk.Exists(znode, true);
                    return;
            }

            byte[] b = null;
            if (exists)
            {
                try
                {
                    b = zk.GetData(znode, false, null);
                }
                catch (KeeperException e)
                {
                    // We don't need to worry about recovering now. The watch
                    // callbacks will kick off any exception handling
                    Console.WriteLine(e.ToString());
                }
            }
            if ((b == null && b != prevData)
                    || (b != null && !Array.Equals(prevData, b)))
            {
                listener.Exists(b);
                prevData = b;

            }
        }
    }
}
