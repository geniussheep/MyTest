 using System;
 using System.IO;
 using System.Linq;
 using Ionic.Zip;

namespace FileSplitDemo
{
    class Program
    {
        static void Main(string[] args)
        {
            var zipFileName = $@"D:\Temp\TestFile_{DateTime.Now:yyyyMMddHHmmss}.zip";

            CreateSplitZip(@"D:\Temp\Wms_46634_20170727183329", zipFileName, true);

            ZipUnpack(@"D:\Temp\Wms_46634_20170727183329.zip", @"d:\Temp\Wms\");

            ZipUnpack(zipFileName, @"d:\Temp\Wms_Mulite\");
        }

        private static Tuple<bool,string> CreateSplitZip(string sourceDirPath, string zipFilePath, bool isSplitZip = false)
        {

            using (ZipFile zip = new ZipFile())
            {
                zip.AddDirectory(sourceDirPath);
                if (isSplitZip)
                {
                    zip.MaxOutputSegmentSize = 1024 * 1024 * 10;
                }
                zip.Save(zipFilePath);
                var zipFileName = Path.GetFileName(zipFilePath);
                if (string.IsNullOrWhiteSpace(zipFileName))
                {
                    throw new Exception($"get zip file name from zip file path:{zipFilePath} error");
                }
                var zipFileNameWithoutExtend = zipFileName.Replace(".zip","");
                if (zip.NumberOfSegmentsForMostRecentSave > 1)
                {
                    var zipParentDir = Path.GetDirectoryName(zipFilePath) ??
                                       zipFilePath.Replace(zipFileName, "");
                    var zipParentDirInfo = new DirectoryInfo(zipParentDir);
                    var allZipFileNames = zipParentDirInfo.GetFiles()
                        .Where(f => 
                        f.Name.StartsWith(zipFileNameWithoutExtend)).Select(f => f.Name)
                        .OrderByDescending(s => s).Aggregate((r, s) => r + "," + s);
                    return Tuple.Create(true, allZipFileNames);
                }
                return Tuple.Create(false, zipFileName);
            }
        }

        private static void ZipUnpack(string zipFilePath, string targetDirPath)
        {
            if(!File.Exists(zipFilePath)) throw new Exception($"extract file from zip file failed,the zip file：{zipFilePath} is not exist");
            using (ZipFile zip = ZipFile.Read(zipFilePath))
            {
                if (!Directory.Exists(targetDirPath))
                {
                    Directory.CreateDirectory(targetDirPath);
                }
                zip.ExtractAll(targetDirPath,ExtractExistingFileAction.OverwriteSilently);
            }
        }
    }
}
