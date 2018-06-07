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

namespace AsyncProgrammingUsingDelegate
{
   // 该程序演示使用委托来调用同步方法来实现异步编程的目的
    public partial class Mainform : Form
    {
        // 定义用来实现异步编程的委托
        private delegate string AsyncMethodCaller(string fileurl);

        public Mainform()
        {
            InitializeComponent();
            txbUrl.Text = "http://download.microsoft.com/download/7/0/3/703455ee-a747-4cc8-bd3e-98a615c3aedb/dotNetFx35setup.exe";
            
            // 允许跨线程调用
            // 实际开发中不建议这样做的，违背了.NET 安全规范
            CheckForIllegalCrossThreadCalls = false;
        }

        private void btnDownLoad_Click(object sender, EventArgs e)
        {
            rtbState.Text = "Download............";
            if (txbUrl.Text == string.Empty)
            {
                MessageBox.Show("Please input valid download file url");
                return;
            }

            AsyncMethodCaller methodCaller = new AsyncMethodCaller(DownLoadFileSync);
            methodCaller.BeginInvoke(txbUrl.Text.Trim(), GetResult, null);
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
                return string.Format("The Length of the File is: {0}", requestState.filestream.Length) + string.Format("\nDownLoad Completely, Download path is: {0}", requestState.savepath);
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
            string returnstring= caller.EndInvoke(result);
            //sc.Post(ShowState,resultvalue);
            rtbState.Text = returnstring;        
        }
    }

    // This class stores the State of the request.
    public class RequestState
    {
        public int BufferSize = 2048;
        public string savepath = Environment.GetFolderPath(Environment.SpecialFolder.Desktop) + "\\dotNetFx35setup.exe";
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
