using System;
using System.Collections.Generic;
using System.IO;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using ICSharpCode.SharpZipLib.Core;
using ICSharpCode.SharpZipLib.Zip;

namespace ConsoleFileCompare
{

    public enum FileCompareResult
    {
        Add,
        Update,
        Delete,
        None
    }

    class Program
    {
        private int _uptoFileCount;
        private int _totalFileCount;

        class FileCompare : IEqualityComparer<System.IO.FileInfo>
        {
            public bool Equals(FileInfo f1, FileInfo f2)
            {
                return f1.Name == f2.Name;
            }

            public int GetHashCode(FileInfo fi)
            {
                string s = fi.Name;
                return s.GetHashCode();
            }
        }

        public static List<Tuple<FileCompareResult, string>> CompareDir(string sourceDirPath, string destinationDirPath,
            IReadOnlyCollection<string> skipFileList)
        {
            var compareResult = new List<Tuple<FileCompareResult, string>>();
            var sourceDirInfo = new DirectoryInfo(sourceDirPath);
            var destinationDirInfo = new DirectoryInfo(destinationDirPath);

            var filterFiles = skipFileList.Select(m => Path.GetFullPath($@"{sourceDirPath}\{m}"));

            //获取比较源的 所有刨除源的根文件夹的路径后的文件路径
            var sourceFileList = sourceDirInfo.GetFiles("*.*", SearchOption.AllDirectories)
                .Where(f => !filterFiles.Any(skf => skf.Equals(f.FullName, StringComparison.OrdinalIgnoreCase)))
                .Select(s => s.FullName.Replace(sourceDirPath, "")).ToArray();

            //获取被比较所有刨除被比较源的根文件夹的路径后的文件路径
            filterFiles = skipFileList.Select(m => Path.GetFullPath($@"{destinationDirPath}\{m}"));
            var destinationFileList =
                destinationDirInfo.GetFiles("*.*", SearchOption.AllDirectories)
                    .Where(f => !filterFiles.Any(skf => skf.Equals(f.FullName, StringComparison.OrdinalIgnoreCase)))
                    .Select(s => s.FullName.Replace(destinationDirPath, "")).ToArray();

            //获取更新的文件路径列表
            foreach (var commFile in sourceFileList.Intersect(destinationFileList))
            {
                var tempCompareResult = CompareFile(Path.GetFullPath($@"{sourceDirPath}\{commFile}"),
                    Path.GetFullPath($@"{destinationDirPath}\{commFile}"));
                if (tempCompareResult == FileCompareResult.Update)
                {
                    compareResult.Add(Tuple.Create(FileCompareResult.Update, commFile));
                }
            }

            //获取添加的文件路径
            var addFileList = sourceFileList.Except(destinationFileList).ToArray();
            if (addFileList.Any())
            {
                compareResult.AddRange(addFileList.Select(addFile => Tuple.Create(FileCompareResult.Add, addFile)));
            }

            //获取删除的文件路径
            var deleteFileList = destinationFileList.Except(sourceFileList).ToArray();
            if (deleteFileList.Any())
            {
                compareResult.AddRange(
                    deleteFileList.Select(deleteFile => Tuple.Create(FileCompareResult.Delete, deleteFile)));
            }

            return compareResult;
        }

        static void Main(string[] args)
        {
            // Create two identical or different temporary folders   
            // on a local drive and change these file paths.  
//            var sourceDirPath = @"D:\AdamPublishTest\10120\trunk\jenkins";
//            var destinationDirPath = @"D:\AdamPublishTest\10120\trunk\publish";
            if (!Directory.Exists(@"d:\AdamPublishTest\2\packagelist\"))
            {
                Console.WriteLine($"该应用存没有可发布的线上更新包，故无法继续新建任务");
            }
            Console.WriteLine($"......");
            //
            //            foreach (var compareResult in CompareDir(sourceDirPath, destinationDirPath, new List<string> { "web.config","1.json" , "abc\\1.json" }))
            //            {
            //                Console.WriteLine($"opt:{compareResult.Item1} \t file:{compareResult.Item2}");   
            //            }
            //            Console.ReadKey();
            //            CompareFile(@"D:\AdamPublishTest\10120\trunk\jenkins\rollbackup.txt",
            //                @"D:\AdamPublishTest\10120\trunk\publish\rollbackup.txt");
            //
            //            var a = new string[] {"1","2","3"};
            //            var b = new string[] {"3","2","1"};
            //
            //            Console.WriteLine(a.SequenceEqual(b));

            //            FastZip fastZip = new FastZip();
            //
            //            fastZip.CreateEmptyDirectories = true;
            //
            //            string zipFileName = Directory.GetParent(sourceDirPath).FullName + "\\ZipTest.zip";
            //
            //            fastZip.CreateZip(zipFileName, sourceDirPath, true, "");


            //            DirectoryInfo dir1 = new DirectoryInfo(sourceDirPath);
            //            DirectoryInfo dir2 = new DirectoryInfo(destinationDirPath);
            //
            //            // Take a snapshot of the file system.  
            //            IEnumerable<System.IO.FileInfo> list1 = dir1.GetFiles("*.*", System.IO.SearchOption.AllDirectories);
            //            IEnumerable<System.IO.FileInfo> list2 = dir2.GetFiles("*.*", System.IO.SearchOption.AllDirectories);
            //
            //            //A custom file comparer defined below  
            //            FileCompare myFileCompare = new FileCompare();
            //
            //            // This query determines whether the two folders contain  
            //            // identical file lists, based on the custom file comparer  
            //            // that is defined in the FileCompare class.  
            //            // The query executes immediately because it returns a bool.  
            //            bool areIdentical = list1.SequenceEqual(list2, myFileCompare);
            //
            //            if (areIdentical == true)
            //            {
            //                Console.WriteLine("the two folders are the same");
            //            }
            //            else
            //            {
            //                Console.WriteLine("The two folders are not the same");
            //            }
            //
            //            // Find the common files. It produces a sequence and doesn't   
            //            // execute until the foreach statement.  
            //            var queryCommonFiles = list1.Intersect(list2, myFileCompare);
            //
            //            if (queryCommonFiles.Count() > 0)
            //            {
            //                Console.WriteLine("The following files are in both folders:");
            //                foreach (var v in queryCommonFiles)
            //                {
            //                    Console.WriteLine(v.FullName); //shows which items end up in result list  
            //                }
            //            }
            //            else
            //            {
            //                Console.WriteLine("There are no common files in the two folders.");
            //            }
            //
            //            Console.ReadKey();
            //            // Find the set difference between the two folders.  
            //            // For this example we only check one way.  
            //            var queryList1Only = (from file in list1
            //                                  select file).Except(list2, myFileCompare);
            //
            //            Console.WriteLine("The following files are in list1 but not list2:");
            //            foreach (var v in queryList1Only)
            //            {
            //                Console.WriteLine(v.FullName);
            //            }
            //            Console.ReadKey();
            //
            //            // Find the set difference between the two folders.  
            //            // For this example we only check one way.  
            //            var queryList2Only = (from file in list2
            //                                  select file).Except(list1, myFileCompare);
            //
            //            Console.WriteLine("The following files are in list2 but not list1:");
            //            foreach (var v in queryList2Only)
            //            {
            //                Console.WriteLine(v.FullName);
            //            }
            //
            //            // Keep the console window open in debug mode.  
            //            Console.WriteLine("Press any key to exit.");
            Console.ReadKey();
        }

        public void TestFastZipCreate(string backupFolderPath)
        {

            _totalFileCount = FolderContentsCount(backupFolderPath);

            FastZipEvents events = new FastZipEvents();
            events.ProcessFile = ProcessFileMethod;
            FastZip fastZip = new FastZip(events);

            fastZip.CreateEmptyDirectories = true;

            string zipFileName = Directory.GetParent(backupFolderPath).FullName + "\\ZipTest.zip";

            fastZip.CreateZip(zipFileName, backupFolderPath, true, "");
        }

        private void ProcessFileMethod(object sender, ScanEventArgs args)
        {
            _uptoFileCount++;
            int percentCompleted = _uptoFileCount*100/_totalFileCount;
            // do something here with a progress bar
            // file counts are easier as sizes take more work to calculate, and compression levels vary by file type

            string fileName = args.Name;
            // To terminate the process, set args.ContinueRunning = false
            if (fileName == "stop on this file")
                args.ContinueRunning = false;
        }

        // Returns the number of files in this and all subdirectories
        private int FolderContentsCount(string path)
        {
            int result = Directory.GetFiles(path).Length;
            string[] subFolders = Directory.GetDirectories(path);
            foreach (string subFolder in subFolders)
            {
                result += FolderContentsCount(subFolder);
            }
            return result;
        }

        private static FileCompareResult CompareFile(string sourceFilePath, string destinationFilePath)
        {
//            var sourceFileInfo = new FileInfo(sourceFilePath);
//            var destinationFileInfo = new FileInfo(destinationFilePath);
//            if (sourceFileInfo.Length != destinationFileInfo.Length)
//            {
//                return FileCompareResult.Update;
//            }
            var sbSource = new StringBuilder();
            var sbDestination = new StringBuilder();

            byte[] md5Source, md5Destination;
            //都存在，则判断Md5是否相等，不等的则表示Update
            using (FileStream fsSource = new FileStream(sourceFilePath, FileMode.Open),
                fsDestination = new FileStream(destinationFilePath, FileMode.Open))
            {
                System.Security.Cryptography.MD5 md5 = new System.Security.Cryptography.MD5CryptoServiceProvider();
                md5Source = md5.ComputeHash(fsSource);
                foreach (var source in md5Source)
                {
                    sbSource.Append(source.ToString("X2"));
                }

                md5Destination = md5.ComputeHash(fsDestination);
                foreach (var destination in md5Destination)
                {
                    sbDestination.Append(destination.ToString("X2"));
                }

            }
            Console.WriteLine($"{sourceFilePath}\t{sbSource}\r\n{destinationFilePath}\t{sbDestination}");
            return md5Source.SequenceEqual(md5Destination) ? FileCompareResult.None: FileCompareResult.Update;
        }
    }
}
