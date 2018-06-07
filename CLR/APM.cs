using System;
using System.IO;
using System.Net;
using System.Text;
using System.Threading;

namespace ConsoleApp
{
    public class RequestState
    {
        // This class stores the State of the request.
        public int BufferSize = 1024;
        public string savepath = Environment.GetFolderPath(Environment.SpecialFolder.Desktop) + "\\downloaddemo1.txt";
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

    public partial class Program
    {
        private static void MainTestApm(string[] args)
        {

            Console.WriteLine("AsyncIO ...");
            AsyncStreamDemo1();
            AsyncStreamDemo2();
            ApmExceptionHandling();
            Console.ReadLine();
            DownloadFileAsync("http://api.sgzb2.com/newheroservice/Sg2College/Service.ashx?method=getallitemjson");
            Console.ReadKey();
        }


        private static async void AsyncStreamDemo1()
        {
            //byte[] bytesToWrite = Encoding.Unicode.GetBytes("test async write");
            //byte[] bytesToWrite = Encoding.Unicode.GetBytes("0123456789");
            byte[] bytesToWrite = Encoding.Unicode.GetBytes("婷婷");
            using (
                FileStream createdFile =
                    File.Create(String.Format("{0}/asynctestdemo1.txt", Environment.CurrentDirectory), 4096,
                        FileOptions.Asynchronous))
            {

                await createdFile.WriteAsync(bytesToWrite, 0, bytesToWrite.Length);
            }
        }

        private static void AsyncStreamDemo2()
        {
            using (
                FileStream fs = new FileStream(String.Format("{0}/asynctestdemo1.txt", Environment.CurrentDirectory),
                    FileMode.OpenOrCreate, FileAccess.ReadWrite, FileShare.ReadWrite, 100, FileOptions.Asynchronous))
            {
                byte[] bytesToRead1 = new byte[10];
                fs.BeginRead(bytesToRead1, 4, bytesToRead1.Length - 4, (iar) =>
                {
                    int a = 1;
                    Console.WriteLine("AsyncState = {0}, a= {1}, IsCompleted = {2}, CompletedSynchronously = {3}",
                        iar.AsyncState, a++, iar.IsCompleted, iar.CompletedSynchronously);
                    while (!iar.IsCompleted)
                    {
                        Console.WriteLine("{0}{1}", iar.AsyncState, a++);
                    }
                    Console.WriteLine("R1:{0},Length:{1}", Encoding.Unicode.GetString(bytesToRead1), bytesToRead1.Length);
                }, "Test async ...R1");
                fs.Position = 0;
                byte[] bytesToRead0 = new byte[12];
                fs.BeginRead(bytesToRead0, 0, bytesToRead0.Length, (iar) =>
                {
                    int a = 1;
                    Console.WriteLine("AsyncState = {0}, a= {1}, IsCompleted = {2}, CompletedSynchronously = {3}",
                        iar.AsyncState, a++, iar.IsCompleted, iar.CompletedSynchronously);
                    while (!iar.IsCompleted)
                    {
                        Console.WriteLine("{0}{1}", iar.AsyncState, a++);
                    }
                    Console.WriteLine("L10:{0},Length:{1}", Encoding.Unicode.GetString(bytesToRead0),
                        bytesToRead0.Length);
                }, "Test async ...L10");

                fs.Position = 0;
                byte[] bytesToReadf = new byte[fs.Length];
                fs.BeginRead(bytesToReadf, 0, bytesToReadf.Length, (iar) =>
                {
                    int a = 1;
                    Console.WriteLine("AsyncState = {0}, a= {1}, IsCompleted = {2}, CompletedSynchronously = {3}",
                        iar.AsyncState, a++, iar.IsCompleted, iar.CompletedSynchronously);
                    while (!iar.IsCompleted)
                    {
                        Console.WriteLine("{0}{1}", iar.AsyncState, a++);
                    }
                    Console.WriteLine("F:{0},Length:{1}", Encoding.Unicode.GetString(bytesToReadf), bytesToReadf.Length);
                }, "Test async ...F");
            }
        }

        /// <summary>
        /// APM异常处理
        /// </summary>
        private static void ApmExceptionHandling()
        {
            Console.WriteLine("ApmExceptionHandling...");
            WebRequest webRequest = WebRequest.Create("http://0.0.0.0/");
            webRequest.Timeout = 3000;
            webRequest.BeginGetResponse((result) =>
            {
                WebRequest asyncWebRequest = (WebRequest)result.AsyncState;
                WebResponse asyncWebResponse = null;
                try
                {
                    asyncWebResponse = asyncWebRequest.EndGetResponse(result);
                    Console.WriteLine("Content length:" + asyncWebResponse.ContentLength);
                }
                catch (WebException ex)
                {
                    Console.WriteLine(ex.GetType() + ": " + ex.Message);
                }
                finally
                {
                    if (asyncWebResponse != null) asyncWebResponse.Close();
                }
            }, webRequest);
        }

        #region use APM to download file asynchronously

        private static void DownloadFileAsync(string url)
        {
            try
            {
                // Initialize an HttpWebRequest object
                HttpWebRequest myHttpWebRequest = (HttpWebRequest) WebRequest.Create(url);

                // Create an instance of the RequestState and assign HttpWebRequest instance to its request field.
                RequestState requestState = new RequestState();
                requestState.request = myHttpWebRequest;
                myHttpWebRequest.BeginGetResponse(ResponseCallback, requestState);
            }
            catch (Exception e)
            {
                Console.WriteLine("Error Message is:{0}", e.Message);
            }
        }

        // The following method is called when each asynchronous operation completes. 
        private static void ResponseCallback(IAsyncResult callbackresult)
        {
            // Get RequestState object
            RequestState myRequestState = (RequestState) callbackresult.AsyncState;

            HttpWebRequest myHttpRequest = myRequestState.request;

            // End an Asynchronous request to the Internet resource
            myRequestState.response = (HttpWebResponse) myHttpRequest.EndGetResponse(callbackresult);

            // Get Response Stream from Server
            Stream responseStream = myRequestState.response.GetResponseStream();
            myRequestState.streamResponse = responseStream;

            IAsyncResult asynchronousRead = responseStream.BeginRead(myRequestState.BufferRead, 0,
                myRequestState.BufferRead.Length, ReadCallBack, myRequestState);
        }

        // Write bytes to FileStream
        private static void ReadCallBack(IAsyncResult asyncResult)
        {
            try
            {
                // Get RequestState object
                RequestState myRequestState = (RequestState) asyncResult.AsyncState;

                // Get Response Stream from Server
                Stream responserStream = myRequestState.streamResponse;

                // 
                int readSize = responserStream.EndRead(asyncResult);
                if (readSize > 0)
                {
                    myRequestState.filestream.Write(myRequestState.BufferRead, 0, readSize);
                    responserStream.BeginRead(myRequestState.BufferRead, 0, myRequestState.BufferRead.Length,
                        ReadCallBack, myRequestState);
                    Console.WriteLine("thread id:{0},current size:{1}",Thread.CurrentThread.ManagedThreadId,readSize);
                }
                else
                {
                    Console.WriteLine("\nThe Length of the File is: {0}", myRequestState.filestream.Length);
                    Console.WriteLine("DownLoad Completely, Download path is: {0}", myRequestState.savepath);
                    myRequestState.response.Close();
                    myRequestState.filestream.Close();
                }
            }
            catch (Exception e)
            {
                Console.WriteLine("Error Message is:{0}", e.Message);
            }
        }


    }

    #endregion
}
