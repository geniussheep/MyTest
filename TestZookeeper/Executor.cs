using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using ZooKeeperNet;

namespace TestZookeeper
{
    class Executor : IWatcher, DataMonitor.DataMonitorListener
    {
        String znode;

        DataMonitor dm;

        ZooKeeper zk;

        String filename;

        String[] exec;

        //Process child;

        public Executor(String hostPort, String znode, String filename,
                String[] exec) {
            this.filename = filename;
            this.exec = exec;
            zk = new ZooKeeper(hostPort, new TimeSpan(0, 0, 3), this);
            dm = new DataMonitor(zk, znode, null, this);
        }

        /**
         * @param args
         */
        public static void Main(String[] args)
        {
            String hostPort = "192.168.60.6:2181";
            String znode = "/testyy";
            String filename = "testyy.txt";
            String[] exec = new String[1];
            try
            {
                new Executor(hostPort, znode, filename, exec).Run();
            }
            catch (Exception e)
            {
                Console.WriteLine(e.ToString());
            }
        }

        /***************************************************************************
         * We do process any events ourselves, we just need to forward them on.
         *
         * @see org.apache.zookeeper.Watcher#process(org.apache.zookeeper.proto.WatcherEvent)
         */
        public void Process(WatchedEvent watchedEvent)
        {
            dm.Process(watchedEvent);
        }

        public void Run()
        {
            try
            {
                System.Threading.Monitor.Enter(this);
                while (!dm.dead)
                {
                    Console.WriteLine("----监控----");
                    System.Threading.Monitor.Wait(this);
                }
            }
            catch
            {
                System.Threading.Monitor.Exit(this);
            }
        }

        public void Closing(int rc)
        {
            System.Threading.Monitor.Enter(this);
            System.Threading.Monitor.PulseAll(this);
        }

        public void Exists(byte[] data)
        {
            if (data != null)
            {
                //if (child != null)
                //{
                //    System.out.println("Stopping child");
                //    child.destroy();
                //    try
                //    {
                //        child.waitFor();
                //    }
                //    catch (InterruptedException e)
                //    {
                //        e.printStackTrace();
                //    }
                //}
                //    //try
                //{
                //    FileOutputStream fos = new FileOutputStream(filename);
                //    fos.write(data);
                //    fos.close();
                //}
                //catch (Exception e)
                //{
                //    e.printStackTrace();
                //}
                Console.WriteLine(System.Text.Encoding.Default.GetString(data));
                //try
                //{
                //    System.out.println("Starting child");
                //    child = Runtime.getRuntime().exec(exec);
                //    new StreamWriter(child.getInputStream(), System.out);
                //    new StreamWriter(child.getErrorStream(), System.err);
                //}
                //catch (IOException e)
                //{
                //    e.printStackTrace();
                //}
            }
        }
    }
}
