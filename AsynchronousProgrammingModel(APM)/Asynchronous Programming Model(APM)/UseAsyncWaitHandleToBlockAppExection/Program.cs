using System;
using System.IO;
using System.Net;

namespace UseAsyncWaitHandleToBlockAppExection
{
   
    class Program
    {
        static void Main(string[] args)
        {
            string downUrl = "http://download.microsoft.com/download/5/B/9/5B924336-AA5D-4903-95A0-56C6336E32C9/TAP.docx";

            // File Download Asynchronously
            // Use Async method to download file, the Main Thread will block before the download operation completes
            // We can see the "Start DownLoad File......." Message shows after download operation completes
            DownloadFileAsync(downUrl);

            Console.WriteLine("Start DownLoad File.........");
            Console.ReadLine();
        }

        private static void DownloadFileAsync(string url)
        {
            string savepath = Environment.GetFolderPath(Environment.SpecialFolder.Desktop) + "\\TAP.docx";

            if (File.Exists(savepath))
            {
                File.Delete(savepath);
            }

            FileStream savestream = new FileStream(savepath, FileMode.OpenOrCreate);

            // Initialize an HttpWebRequest object
            HttpWebRequest myHttpWebRequest = (HttpWebRequest)WebRequest.Create(url);
            IAsyncResult result = myHttpWebRequest.BeginGetResponse(null, null);

            // Block the current thread until the operation completes
            result.AsyncWaitHandle.WaitOne();
            try
            {
                // get results
                // EndGetResponse blocks until the process completes
                HttpWebResponse myHttpWebResponse = (HttpWebResponse)myHttpWebRequest.EndGetResponse(result);
                Stream stream = myHttpWebResponse.GetResponseStream();
                
                byte[] bytes = new byte[1024];
                int readSize;
                IAsyncResult readresult = stream.BeginRead(bytes, 0, bytes.Length, null, null);
                readresult.AsyncWaitHandle.WaitOne();
                readSize = stream.EndRead(readresult);
                while (readSize > 0)
                {
                    savestream.Write(bytes, 0, readSize);
                    readresult = stream.BeginRead(bytes, 0, bytes.Length, null, null);
                    readresult.AsyncWaitHandle.WaitOne();
                    readSize = stream.EndRead(readresult);
                }

                Console.WriteLine("\nThe Length of the File is: {0}", savestream.Length);
                Console.WriteLine("DownLoad Completely, Download path is: {0}", savepath);
            }
            catch (Exception e)
            {
                Console.WriteLine("Error Message is:{0}", e.Message);
            }
            finally
            {
                savestream.Close();
            }
        }
    }
}
