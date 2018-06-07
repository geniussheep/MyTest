using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;

namespace ConsoleTest
{
    public enum RedisCompressType
    {
        None = 0,
        GZip = 1
    }
    public class TestEnum
    {
        private static void TestIntToEnumForce()
        {
            RedisCompressType foo0 = 0;
            RedisCompressType foo1 = (RedisCompressType)1;
        }

        private static void TestInttoEnumEnumToObject()
        {
            RedisCompressType foo0 = (RedisCompressType)Enum.ToObject(typeof(RedisCompressType), 0);
            RedisCompressType foo1 = (RedisCompressType)Enum.ToObject(typeof(RedisCompressType), 1);
        }

        private static void TestEnumToIntForce()
        {
            int a = (int)RedisCompressType.None;
            int b = (int)RedisCompressType.GZip;
        }

        private static void TestEnumToIntConvertToInt32()
        {
            int a = Convert.ToInt32(RedisCompressType.None);
            int b = Convert.ToInt32(RedisCompressType.GZip);
        }

        public static void Result()
        {
            TestUtils.ConsoleResult(TestUtils.TestMethodUseTime(TestIntToEnumForce, "TestIntToEnumForce"), "TestIntToEnumForce");

            TestUtils.ConsoleResult(TestUtils.TestMethodUseTime(TestInttoEnumEnumToObject, "TestInttoEnumEnumToObject"), "TestInttoEnumEnumToObject");

            TestUtils.ConsoleResult(TestUtils.TestMethodUseTime(TestEnumToIntForce, "TestEnumToIntForce"), "TestEnumToIntForce");

            TestUtils.ConsoleResult(TestUtils.TestMethodUseTime(TestEnumToIntConvertToInt32, "TestEnumToIntConvertToInt32"), "TestEnumToIntConvertToInt32");
        }
    }

}
