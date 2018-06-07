using System;
using System.Collections.Generic;
using System.Diagnostics;
using System.Threading;
using System.Threading.Tasks;
using AutoMapper;

namespace AutoMapperTest
{

    public class Bb
    {
        public int Bd { get; set; }
        public string BS { get; set; }
    }


    public class Aa
    {


        public string cc { get; set; }


        public int bb { get; set; }

    }
    class Program
    {
        private const int TestTimes = 1000;

        private static double TestMethodUseTime(Action<int> testMethod, string methodName)
        {
            var stopWatch = new Stopwatch();
            stopWatch.Start();
            for (int i = 0; i < TestTimes; i++)
            {
                testMethod(i);
            }
            testMethod(TestTimes);
            stopWatch.Stop();
//            Console.WriteLine("{0}: 执行{3}次耗时 {1} s, {2} ms.", methodName, stopWatch.Elapsed.Seconds,
//                stopWatch.Elapsed.Milliseconds, TestTimes);
            return stopWatch.Elapsed.TotalMilliseconds;
        }

        public static List<Aa> GetAAs(int start, int end)
        {
            var result = new List<Aa>();
            for (int i = start; i <= end; i++)
            {
                result.Add(new Aa() { bb = i, cc = i.ToString() });
            }
            return result;
        }

        static void Main(string[] args)
        {
//            double sum = 0;
//
//            int max = 500;
//
//            sum = 0;
//            for (int i = 0; i < max; i++)
//            {
//                sum += TestMethodUseTime(TestMethodAutoMapper, "TestMethodAutoMapper");
//            }
//            Console.WriteLine($"TestMethodAutoMapper 平均耗时 :{sum / max} MS");
//
//
//            sum = 0;
//            for (int i = 0; i < max; i++)
//            {
//                sum += TestMethodUseTime(TestMethodModelMapper, "TestMethodModelMapper");
//            }
//            Console.WriteLine($"TestMethodModelMapper 平均耗时 :{sum / max} MS");
//
//            sum = 0;
//            for (int i = 0; i < max; i++)
//            {
//                sum += TestMethodUseTime(TestMethodModelClone, "TestMethodModelClone");
//            }
//            Console.WriteLine($"TestMethodModelClone 平均耗时 :{sum / max} MS");


            //            TestMethodAutoMapper(2);
            //            TestMethodModelClone(5);
            //            TestMethodModelMapper(7);
            var tList = new List<Task>();
            var listaa = GetAAs(1, 5);
            foreach (var aa in listaa)
            {
                var t = Task.Run(() =>
                {
                    Console.WriteLine("before thread {0} is done!, {1}:{2}", Thread.CurrentThread.ManagedThreadId, aa.cc,aa.bb);
                    aa.bb = aa.bb + 1;
                    Console.WriteLine("after thread {0} is done!, {1}:{2}", Thread.CurrentThread.ManagedThreadId, aa.cc, aa.bb);
                });
                tList.Add(t);
            }
            Task.WaitAll(tList.ToArray());
            foreach (var aa in listaa)
            {
                    Console.WriteLine("main thread {0} is done!, {1}:{2}", Thread.CurrentThread.ManagedThreadId, aa.cc, aa.bb);
            }
            Console.Read();
        }

        private static ProductBasicModifyModel result1;
        private static ProductBasicModifyModel result2;
        private static ProductBasicModifyModel result5;
        private static ProductBasicModifyModelCopy result3;
        private static ProductBasicModifyModelCopy result4;
        private static ProductBasicModifyModelCopy result6;

        public static ORM_ProductBasicModifyInfo GetORMProductBasicModifyInfo(int i)
        {
            return new ORM_ProductBasicModifyInfo
            {
                SysNo = 1 + i,
                ApplicantSysNo = 2 + i,
                ApplicantTime = DateTime.Now,
                ApplyType = 3 + i,
                AuditPrivilege = 4 + i,
                AuditStatus = 5 + i,
                AuditTime = DateTime.Now,
                AuditUserSysNo = 7 + i,
                AuxMeasurementUnit = 8 + i,
                BarCode = "9" + i,
                MainAuxRatio = 10 + i,
                OriginArea = "11" + i,
                OriginAreaSysNo = 12 + i,
                ProducingArea = "13" + i,
                ProductAreaSysNo = 14 + i,
                ProductAttr = 15 + i,
                BoxSpecifications = 16 + i,
                InternalBarCode = "17" + i,
                C1SysNo = 18 + i,
                C2SysNo = 19 + i,
                C3SysNo = 20 + i,
                CloneProductSysNo = 21 + i,
                CreateTime = DateTime.Now,
                CreateUserSysNo = 22 + i,
                IsCanPurchase = 23 + i,
                IsGiftCard = 24 + i,
                ReviewCount = 25 + i,
                TaxC3SysNo = 26 + i,
                TaxCode = "TaxCode" + i,
                ProductID = "ProductID" + i,
                ProductName = "ProductName" + i,
                ProductMode = "ProductMode" + i,
                ProductType = 27 + i,
                Weight = 28 + i,
                ManufacturerSysNo = 29 + i,
                DefaultVendorSysNo = 30 + i,
                DefaultPurchasePrice = 31 + i,
                SaleUnit = "33" + i,
                StorageDay = 34 + i,
                Volume = 35 + i,
                Long = 36 + i,
                Width = 37 + i,
                HIGH = 38 + i,
                StorageMethods = 40 + i,
                ShelfLife = 42 + i,
                IsEasyHome = 43 + i,
                EasyHomeDeliveryTimes = 44 + i,
                IsNeedProcessing = 45 + i,
                ProcessiType = 46 + i,
                PMUserSysNo = 47 + i,
                Status = 48 + i,
                LastUpdateUserSysNo = 49 + i,
                LastUpdateTime = DateTime.Now,
                PackageList = "50" + i,
                IsValuable = 51 + i,
                Note = "52" + i,
                IsDelete = false,
                IsOutOfSpecification = false,
                MerchantSysNo = 53 + i,
                IsProductOrigin = 54 + i,
                SaleTaxRate = 55 + i,
                TaxRate = 56 + i,
                PackQuantity = 57 + i,
                ProductStorageDay = "58" + i,
                DeliveryParttern = 59 + i,
                IsWeigh = false,
                ProductSysNo = 60 + i,
                WarmHintWeight = 61 + i,
                WarmHint = "62" + i,
                WarningDay = 63 + i,
                IsDirectPurchase = true
            };
        }


        public static void TestMethodModelClone(int i)
        {
            var ormProductBasicModifyInfo = GetORMProductBasicModifyInfo(i);
            if (i % 2 == 0)
            {
                result4 =
                    ModelClone<ORM_ProductBasicModifyInfo, ProductBasicModifyModelCopy>
                        .Clone(ormProductBasicModifyInfo);
            }
            else
            {
                result1 =
                    ModelClone<ORM_ProductBasicModifyInfo, ProductBasicModifyModel>.Clone(ormProductBasicModifyInfo);
            }
        }

        public static void TestMethodAutoMapper(int i)
        {
            var ormProductBasicModifyInfo = GetORMProductBasicModifyInfo(i);
            if (i%2 == 0)
            {
                result3 = ormProductBasicModifyInfo.AutoMapTo<ORM_ProductBasicModifyInfo, ProductBasicModifyModelCopy>();
            }
            else
            {
                result2 = ormProductBasicModifyInfo.AutoMapTo<ORM_ProductBasicModifyInfo, ProductBasicModifyModel>();
            }
        }

        public static void TestMethodModelMapper(int i)
        {
            var ormProductBasicModifyInfo = GetORMProductBasicModifyInfo(i);
            if (i % 2 == 0)
            {
                result6 = ormProductBasicModifyInfo.MapTo<ORM_ProductBasicModifyInfo, ProductBasicModifyModelCopy>();


            }
            else
            {
                result5 = ormProductBasicModifyInfo.MapTo<ORM_ProductBasicModifyInfo, ProductBasicModifyModel>();
            }
        }
    }
}