using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;

namespace ConsoleTest
{
    public class TestString
    {
        public static void TestStringBuilderAppend(string str1, string str2, string str3)
        {
            var exist = new StringBuilder();
            exist.Append("string");
            exist.Append(str2);
            exist.Append(str1);
            exist.Append(str3);
        }

        public static void StringAdd(string str1, string str2, string str3)
        {
            string str = "string" + str1 + str2 + str3;
        }

        private static void TestStringConcat(string str1, string str2, string str3)
        {
            string s = String.Concat("string", str2, str1, str3);
        }


        private static void StringFormatNew(string str1, string str2, string str3)
        {
            string s = $"string{str2}{str1}{str3}";
        }

        private static void StringFormat(string str1, string str2, string str3)
        {
            string s = String.Format("string{0}{1}{2}", str2, str1, str3);
        }

        public static void ResultStringAddSomeMethods()
        {
            var str1 = "afadsaadfk";
            var str2 = "发大水发生大";
            var str3 = "afdas";
            TestUtils.ConsoleResult(TestUtils.TestMethodUseTime(TestStringBuilderAppend, str1, str2, str3, "TestStringBuilderAppend"), "TestStringBuilderAppend");
            TestUtils.ConsoleResult(TestUtils.TestMethodUseTime(StringAdd, str1, str2, str3, "StringAdd"), "StringAdd");
            TestUtils.ConsoleResult(TestUtils.TestMethodUseTime(TestStringConcat, str1, str2, str3, "TestStringConcat"), "TestStringConcat");
            TestUtils.ConsoleResult(TestUtils.TestMethodUseTime(StringFormatNew, str1, str2, str3, "StringFormatNew"), "StringFormatNew");
            TestUtils.ConsoleResult(TestUtils.TestMethodUseTime(StringFormat, str1, str2, str3, "StringFormat"), "StringFormat");
        }

        public static string TestStringBuilderAppendFor(List<string> strList)
        {
            var exist = new StringBuilder();
            exist.Append("string");
            for (int i = 0; i < strList.Count; i++)
            {
                exist.Append(strList[i]);
            }
            return exist.ToString();
        }

        public static string StringAddFor(List<string> strList)
        {
            string exist = "string";
            for (int i = 0; i < strList.Count; i++)
            {
                exist += strList[i];
            }
            return exist;
        }

        private static string TestStringConcatFor(List<string> strList)
        {
            string exist = "string";
            for (int i = 0; i < strList.Count; i++)
            {
                exist =String.Concat(exist,strList[i]);
            }
            return exist;
        }


        private static string StringFormatNewFor(List<string> strList)
        {
            string exist = "string";
            for (int i = 0; i < strList.Count; i++)
            {
                exist = $"{exist}{strList[i]}";
            }
            return exist;
        }

        private static string StringFormatFor(List<string> strList)
        {
            string exist = "string";
            for (int i = 0; i < strList.Count; i++)
            {
                exist = String.Format("{0}{1}",exist,strList[i]);
            }
            return exist;
        }

        public static void ResultStringAddSomeMethodsFor()
        {
            var strList = new List<string>()
            {
                "afadsaadfk",
                "sfaafdsdfsadfsdf",
                "发大水发生大",
                "afdas",
                "大大发大水发的说法是电风扇",
                "大发大法师打发沙士大夫",
                "sdfadasff",
                "王文强2312334帮你4",
                "发的撒阿萨德发的"
            };
            TestUtils.ConsoleResult(TestUtils.TestMethodUseTime(TestStringBuilderAppendFor, strList, "TestStringBuilderAppendFor"), "TestStringBuilderAppendFor");
            TestUtils.ConsoleResult(TestUtils.TestMethodUseTime(StringAddFor, strList, "StringAddFor"), "StringAddFor");
            TestUtils.ConsoleResult(TestUtils.TestMethodUseTime(TestStringConcatFor, strList, "TestStringConcatFor"), "TestStringConcatFor");
            TestUtils.ConsoleResult(TestUtils.TestMethodUseTime(StringFormatNewFor, strList, "StringFormatNewFor"), "StringFormatNewFor");
            TestUtils.ConsoleResult(TestUtils.TestMethodUseTime(StringFormatFor, strList, "StringFormatFor"), "StringFormatFor");
        }

        private static void TestIndexOf(string oStr, string iStr)
        {
            var isIndexOf = oStr.IndexOf(iStr) >= 0;
        }

        private static void TestIndexOfWithCurrentCultureIgnoreCase(string oStr, string iStr)
        {
            var isIndexOf = oStr.IndexOf(iStr, StringComparison.CurrentCultureIgnoreCase) >= 0;
        }

        private static void TestIndexOfWithOrdinalIgnoreCase(string oStr, string iStr)
        {
            var isIndexOf = oStr.IndexOf(iStr, StringComparison.OrdinalIgnoreCase) >= 0;
        }

        private static void TestIndexOfWithToLower(string oStr, string iStr)
        {
            var isIndexOf = oStr.ToLower().IndexOf(iStr.ToLower()) >= 0;
        }

        private static void TestContain(string oStr, string iStr)
        {
            var isIndexOf = oStr.Contains(iStr);
        }

        private static void TestContainWithToLower(string oStr, string iStr)
        {
            var isIndexOf = oStr.ToLower().Contains(iStr.ToLower());
        }

        public static void ResultSomeIndexOfMethodAndContain()
        {
            string ostr = "AbsddffDDFSSASFSDsdfadsfasdfsdsdfDDD$%$$2322342FDSdfasfdfsaAFDASdsfadsdffsda";
            string istr = "asdfsdsdfDDD$%$$2322342FDSdfasfdfsaA";
            TestUtils.ConsoleResult(TestUtils.TestMethodUseTime(TestIndexOf, ostr, istr, "TestIndexOf"), "TestIndexOf");
            TestUtils.ConsoleResult(TestUtils.TestMethodUseTime(TestIndexOfWithCurrentCultureIgnoreCase, ostr, istr, "TestIndexOfWithCurrentCultureIgnoreCase"), "TestIndexOfWithCurrentCultureIgnoreCase");
            TestUtils.ConsoleResult(TestUtils.TestMethodUseTime(TestIndexOfWithOrdinalIgnoreCase, ostr, istr, "TestIndexOfWithOrdinalIgnoreCase"), "TestIndexOfWithOrdinalIgnoreCase");
            TestUtils.ConsoleResult(TestUtils.TestMethodUseTime(TestIndexOfWithToLower, ostr, istr, "TestIndexOfWithToLower"), "TestIndexOfWithToLower");
            TestUtils.ConsoleResult(TestUtils.TestMethodUseTime(TestContain, ostr, istr, "TestContain"), "TestContain");
        }


        private static void TestEqual(string oStr, string eStr)
        {
            var isEquals = oStr.Equals(eStr);
        }

        private static void TestEqualCurrentCultureIgnoreCase(string oStr, string eStr)
        {
            var isEquals = oStr.Equals(eStr, StringComparison.CurrentCultureIgnoreCase);
        }

        private static void TestEqualOrdinalIgnoreCase(string oStr, string eStr)
        {
            var isEquals = oStr.Equals(eStr, StringComparison.OrdinalIgnoreCase);
        }

        private static void TestEqualToLower(string oStr, string eStr)
        {
            var isEquals = oStr.ToLower().Equals(eStr.ToLower());
        }


        public static void ResultStringEqual()
        {
            string ostr = "AbsddffDDFSSASFSDsdfadsfasdfsdsdfDDD$%$$2322342FDSdfasfdfsaAFDASdsfadsdffsda";
            string eStr = "ABSDDFFDDFSSASFSDSDFADSFASDFSDSDFDDD$%$$2322342FDSDFASFDFSAAFDASDSFADSDFFSDA";
            TestUtils.ConsoleResult(TestUtils.TestMethodUseTime(TestEqual, ostr, eStr, "TestEqual"), "TestEqual");
            TestUtils.ConsoleResult(TestUtils.TestMethodUseTime(TestEqual, ostr, eStr, "TestEqual"), "TestEqual");
            TestUtils.ConsoleResult(TestUtils.TestMethodUseTime(TestEqualCurrentCultureIgnoreCase, ostr, eStr, "TestEqualCurrentCultureIgnoreCase"), "TestEqualCurrentCultureIgnoreCase");
            TestUtils.ConsoleResult(TestUtils.TestMethodUseTime(TestEqualOrdinalIgnoreCase, ostr, eStr, "TestEqualOrdinalIgnoreCase"), "TestEqualOrdinalIgnoreCase");
            TestUtils.ConsoleResult(TestUtils.TestMethodUseTime(TestEqualToLower, ostr, eStr, "TestEqualToLower"), "TestEqualToLower");
        }

        private static void TestSplitEveryTime(string str)
        {
            var result = string.Format("{0}|{1}|{2}", str.Split(',')[2], str.Split(',')[0], str.Split(',')[1]);
        }

        private static void TestSplitOneTime(string str)
        {
            var array = str.Split(',');
            var result = string.Format("{0}|{1}|{2}", array[2], array[0], array[1]);
        }

        private static void TestSplitOneTimewithStringConcat(string str)
        {
            var array = str.Split(',');
            var result = string.Concat(array[2], "|", array[0], "|", array[1]);
            //            var result = array[2]+"|"+ array[0]+ "|"+ array[1];
        }

        public static void ResultSplitSomeMethod()
        {
            string ostr = "AbsddffDDFSSASFSDsdf,adsfasdfsdsdfDDD$%$$2322342FD,SdfasfdfsaAFDASdsfadsdffsda";
            TestUtils.ConsoleResult(TestUtils.TestMethodUseTime(TestSplitEveryTime, ostr, "TestSplitEveryTime"), "TestSplitEveryTime");
            TestUtils.ConsoleResult(TestUtils.TestMethodUseTime(TestSplitOneTime, ostr, "TestSplitOneTime"), "TestSplitOneTime");
            TestUtils.ConsoleResult(TestUtils.TestMethodUseTime(TestSplitOneTimewithStringConcat, ostr, "TestSplitOneTimewithStringConcat"), "TestSplitOneTimewithStringConcat");
        }

        private static void TestTuple(string str1, string str2, string str3)
        {
            var result = Tuple.Create(str1, str2, str3);
            var resultstr = result.ToString();
            var realresult = result.Item3;
        }

        private static void TestSplit(string str1, string str2, string str3)
        {
            var result = str1 + "|" + str2 + "|" + str3;

            var resultArr = result.Split('|');

            var realresult = resultArr[2];
        }

        public static void ResultSplitAndTuple()
        {
            var test1 = "afadsaadfk";
            var test2 = "发大水发生大";

            string str = "afdas";
            TestUtils.ConsoleResult(TestUtils.TestMethodUseTime(TestSplit, test1, test2, str, "TestSplit"), "TestSplit");
            TestUtils.ConsoleResult(TestUtils.TestMethodUseTime(TestTuple, test1, test2, str, "TestTuple"), "TestTuple");
        }
    }
}
