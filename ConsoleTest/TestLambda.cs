using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;

namespace ConsoleTest
{
    public class TestLambda
    {

        private static readonly List<TestModel> TestModelList = new List<TestModel>()
        {
            new TestModel() {Name = "test1", Value = 1},
            new TestModel() {Name = "test2", Value = 2},
            new TestModel() {Name = "test3", Value = 3},
            new TestModel() {Name = "test4", Value = 4},
            new TestModel() {Name = "test5", Value = 5},
            new TestModel() {Name = "test6", Value = 6},
            new TestModel() {Name = "test7", Value = 7},
            new TestModel() {Name = "test8", Value = 8},
            new TestModel() {Name = "test9", Value = 9},
        };

        public static void TestForeachAddString()
        {
            List<string> nameList = new List<string>();
            foreach (var testModel in TestModelList)
            {
                nameList.Add(testModel.Name);
            }
            string result = string.Join(",", nameList);
        }

        public static void TestAggregate()
        {
            string result = TestModelList.Select(s => s.Name).Aggregate((r, s) => r + "," + s);
        }

        public static void Result()
        {
            TestUtils.ConsoleResult(TestUtils.TestMethodUseTime(TestForeachAddString, "TestForeachAddString"), "TestForeachAddString");

            TestUtils.ConsoleResult(TestUtils.TestMethodUseTime(TestAggregate, "TestAggregate"), "TestAggregate");
        }
    }
}
