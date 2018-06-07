using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.Data;
using System.Drawing;
using System.IO;
using System.Linq;
using System.Net;
using System.Runtime.Remoting.Messaging;
using System.Text;
using System.Threading;
using System.Windows.Forms;

namespace APUsingDelegateCrossThreadCaller
{
    // 该程序演示使用委托来调用同步方法来实现异步编程的目的
    // 使用委托进行异步编程时，需要解决的问题就是跨线程访问控件错误的问题，
    // 这个程序中没有把CheckForIllegalCrossThreadCalls 设置为false
    // 而是使用同步上下文的方式来解决在异步线程中访问界面控件的
    public partial class MainForm : Form
    {
        // 定义用来实现异步编程的委托
        private delegate string AsyncMethodCaller(string fileurl);

          // 定义显示状态的委托
        //private delegate void ShowStateDelegate(string value);
        //private ShowStateDelegate showStateCallback;

        SynchronizationContext sc;
        public MainForm()
        {
            InitializeComponent();
            txbUrl.Text = "http://api.sgzb2.com/newheroservice/Sg2College/Service.ashx?method=getallitemjson";
            //showStateCallback = new ShowStateDelegate(ShowState);
        }

        private void btnDownLoad_Click(object sender, EventArgs e)
        {
            rtbState.Text = String.Format("Download............{0}",Thread.CurrentThread.ManagedThreadId);
            btnDownLoad.Enabled = false;
            if (txbUrl.Text == string.Empty)
            {
                MessageBox.Show("Please input valid download file url");
                return;
            }

            AsyncMethodCaller methodCaller = new AsyncMethodCaller(DownLoadFileSync);
            methodCaller.BeginInvoke(txbUrl.Text.Trim(), GetResult, null);

            // 捕捉调用线程的同步上下文派生对象
            sc = SynchronizationContext.Current;
        }

        // 同步下载文件的方法
        // 该方法会阻塞主线程，使用户无法对界面进行操作
        // 在文件下载完成之前，用户甚至都不能关闭运行的程序。
        private string DownLoadFileSync(string url)
        {
            // Create an instance of the RequestState 
            RequestState requestState = new RequestState();
            try
            {
                // Initialize an HttpWebRequest object
                HttpWebRequest myHttpWebRequest = (HttpWebRequest)WebRequest.Create(url);

                // assign HttpWebRequest instance to its request field.
                requestState.request = myHttpWebRequest;
                requestState.response = (HttpWebResponse)myHttpWebRequest.GetResponse();
                requestState.streamResponse = requestState.response.GetResponseStream();
                int readSize = requestState.streamResponse.Read(requestState.BufferRead, 0, requestState.BufferRead.Length);

                while (readSize > 0)
                {
                    requestState.filestream.Write(requestState.BufferRead, 0, readSize);
                    readSize = requestState.streamResponse.Read(requestState.BufferRead, 0, requestState.BufferRead.Length);
                }

                // 执行该方法的线程是线程池线程，该线程不是与创建richTextBox控件的线程不是一个线程
                // 如果不把 CheckForIllegalCrossThreadCalls 设置为false，该程序会出现“不能跨线程访问控件”的异常
                return string.Format("The Length of the File is: {0}\nDownLoad Completely, Download path is: {1}\nthread id:{2}", requestState.filestream.Length, requestState.savepath,Thread.CurrentThread.ManagedThreadId);
            }
            catch (Exception e)
            {
                return string.Format("Exception occurs in DownLoadFileSync method, Error Message is:{0}", e.Message);
            }
            finally
            {
                requestState.response.Close();
                requestState.filestream.Close();
            }
        }

        // 异步操作完成时执行的方法
        private void GetResult(IAsyncResult result)
        {
            AsyncMethodCaller caller = (AsyncMethodCaller)((AsyncResult)result).AsyncDelegate;
            // 调用EndInvoke去等待异步调用完成并且获得返回值
            // 如果异步调用尚未完成，则 EndInvoke 会一直阻止调用线程，直到异步调用完成
            string returnstring = caller.EndInvoke(result);

            // 通过获得GUI线程的同步上下文的派生对象，
            // 然后调用Post方法来使更新GUI操作方法由GUI 线程去执行
            sc.Post(ShowState,returnstring);      
        }

        // 显示结果到richTextBox
        // 因为该方法是由GUI线程执行的，所以当然就可以访问窗体控件了
        private void ShowState(object result)
        {

            rtbState.Text = result.ToString();
            btnDownLoad.Enabled = true;
        }
    }
     
    // This class stores the State of the request.
    public class RequestState
    {
        public int BufferSize = 2048;
        public string savepath = Environment.GetFolderPath(Environment.SpecialFolder.Desktop) + "\\item.txt";
        public byte[] BufferRead;
        public HttpWebRequest request;
        public HttpWebResponse response;
        public Stream streamResponse;

        public FileStream filestream;
        public RequestState()
        {
            BufferRead = new byte[BufferSize];
            request = null;
            streamResponse = null;
            if (File.Exists(savepath))
            {
                File.Delete(savepath);
            }

            filestream = new FileStream(savepath, FileMode.OpenOrCreate);
        }
    }
}
