using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;

namespace ConsoleTest
{
    public class TestListAndHashAndArray
    {

        private static String[] TestStrArray = { "1", "2", "3" };
        private static HashSet<string> TestHashSet = new HashSet<string> { "1", "2", "3" };
        private static List<string> TestListStr = new List<string> { "1", "2", "3" };

        private static void TestHashSetCountMethod()
        {
            var exist = TestHashSet.Count() > 0;
            //            var exist = TestStrArray.Count() > 0;
        }

        private static void TestHashSetAnyMethod()
        {
            var exist = TestHashSet.Any();
            //            var exist = TestStrArray.Any();
        }

        private static void TestArrayLengthMethod()
        {
            var exist = TestStrArray.Length > 0;
        }

        private static void TestArrayAnyMethod()
        {
            var exist = TestStrArray.Any();
        }

        private static void TestListLengthMethod()
        {
            var exist = TestListStr.Count > 0;
        }

        private static void TestListAnyMethod()
        {
            var exist = TestListStr.Any();
        }

        public static void ResultAnyCount()
        {
            TestUtils.ConsoleResult(TestUtils.TestMethodUseTime(TestHashSetCountMethod, "TestHashSetCountMethod"), "TestHashSetCountMethod");

            TestUtils.ConsoleResult(TestUtils.TestMethodUseTime(TestHashSetAnyMethod, "TestHashSetAnyMethod"), "TestHashSetAnyMethod");

            TestUtils.ConsoleResult(TestUtils.TestMethodUseTime(TestArrayLengthMethod, "TestArrayLengthMethod"), "TestArrayLengthMethod");

            TestUtils.ConsoleResult(TestUtils.TestMethodUseTime(TestArrayAnyMethod, "TestArrayAnyMethod"), "TestArrayAnyMethod");

            TestUtils.ConsoleResult(TestUtils.TestMethodUseTime(TestListLengthMethod, "TestListLengthMethod"), "TestListLengthMethod");

            TestUtils.ConsoleResult(TestUtils.TestMethodUseTime(TestListAnyMethod, "TestListAnyMethod"), "TestListAnyMethod");
        }

        private static void ListForAndRemoveAt()
        {
            var testList = new List<int>() { 1, 2, 3, 4, 5, 6, 7, 8, 9 };
            for (int i = 0; i < testList.Count; i++)
            {
                if (testList[i] % 3 == 0)
                {
                    testList.RemoveAt(i);
                    i--;
                }
            }
        }

        private static void ListRemoveAll()
        {
            var testList = new List<int>() { 1, 2, 3, 4, 5, 6, 7, 8, 9 };
            testList.RemoveAll(s =>
            {
                if (s % 3 == 0)
                {
                    return true;
                }
                return false;
            });
        }

        public static void ResultRemove()
        {
            TestUtils.ConsoleResult(TestUtils.TestMethodUseTime(ListForAndRemoveAt, "ListForAndRemoveAt"), "ListForAndRemoveAt");

            TestUtils.ConsoleResult(TestUtils.TestMethodUseTime(ListRemoveAll, "ListRemoveAll"), "ListRemoveAll");
        }
    }
}
