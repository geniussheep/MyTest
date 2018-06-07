using System;
using System.IO;
using System.Net;


namespace UseAsyncCallBackDelegateDonotBlockAppExecute
{
    // This class stores the State of the request.
    public class RequestState
    {
        public int BufferSize = 1024;
        public string savepath = Environment.GetFolderPath(Environment.SpecialFolder.Desktop) + "\\TAP.docx";
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

    // The Application can do other work while waiting 
    // for the results of an asynchronous operation 
    // should not block waiting until the operation completes.
    class Program
    {
        static void Main(string[] args)
        {                 
            //string downurl = "http://download.microsoft.com/download/9/5/A/95A9616B-7A37-4AF6-BC36-D6EA96C8DAAE/dotNetFx40_Full_x86_x64.exe";
            string downUrl = "http://download.microsoft.com/download/5/B/9/5B924336-AA5D-4903-95A0-56C6336E32C9/TAP.docx";
            
            // File Download Asynchronously
            // Use Async method to download file, the Main Thread cannot block before the download operation completes
            // We can see the "Start DownLoad File......." Message shows before download operation completes
            DownloadFileAsync(downUrl);

            // File Download Synchronously
            // Use Sync method to download file, Block the Main Thread until the download operation completes
            // We can see the "Start DownLoad File......." Message shows after download operation completes

            //DownLoadFileSync(downUrl);
            Console.WriteLine("Start DownLoad File.........");

            Console.ReadLine();
        }

        #region Download File Synchrously
        private static void DownLoadFileSync(string url)
        {
            // Create an instance of the RequestState 
            RequestState requestState=new RequestState();
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

                Console.WriteLine("\nThe Length of the File is: {0}", requestState.filestream.Length);
                Console.WriteLine("DownLoad Completely, Download path is: {0}", requestState.savepath);
            }
            catch (Exception e)
            {
                Console.WriteLine("Error Message is:{0}", e.Message);
            }
            finally
            {
                requestState.response.Close();
                requestState.filestream.Close();
            }
        }
        #endregion 

        #region use APM to download file asynchronously

        private static void DownloadFileAsync(string url)
        {
            try
            {
                // Initialize an HttpWebRequest object
                HttpWebRequest myHttpWebRequest = (HttpWebRequest)WebRequest.Create(url);

                // Create an instance of the RequestState and assign HttpWebRequest instance to its request field.
                RequestState requestState = new RequestState();
                requestState.request = myHttpWebRequest;
                myHttpWebRequest.BeginGetResponse(new AsyncCallback(ResponseCallback), requestState);
            }
            catch (Exception e)
            {
                Console.WriteLine("Error Message is:{0}",e.Message);
            }
        }

        // The following method is called when each asynchronous operation completes. 
        private static void ResponseCallback(IAsyncResult callbackresult)
        {
            // Get RequestState object
            RequestState myRequestState = (RequestState)callbackresult.AsyncState;

            HttpWebRequest myHttpRequest = myRequestState.request;

            // End an Asynchronous request to the Internet resource
            myRequestState.response = (HttpWebResponse)myHttpRequest.EndGetResponse(callbackresult);
            
            // Get Response Stream from Server
            Stream responseStream = myRequestState.response.GetResponseStream();
            myRequestState.streamResponse = responseStream;

            IAsyncResult asynchronousRead = responseStream.BeginRead(myRequestState.BufferRead, 0, myRequestState.BufferRead.Length, ReadCallBack, myRequestState);         
        }

        // Write bytes to FileStream
        private static void ReadCallBack(IAsyncResult asyncResult)
        {
            try
            {
                // Get RequestState object
                RequestState myRequestState = (RequestState)asyncResult.AsyncState;

                // Get Response Stream from Server
                Stream responserStream = myRequestState.streamResponse;

                // 
                int readSize = responserStream.EndRead(asyncResult);
                if (readSize > 0)
                {
                    myRequestState.filestream.Write(myRequestState.BufferRead, 0, readSize);
                    responserStream.BeginRead(myRequestState.BufferRead, 0, myRequestState.BufferRead.Length, ReadCallBack, myRequestState);
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
        #endregion
    }
}
