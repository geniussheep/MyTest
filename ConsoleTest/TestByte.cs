using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;

namespace ConsoleTest
{
    public class TestByte
    {

        private static readonly byte[] _sourceBytes = { 1, 2, 3, 4, 5, 6, 7, 8, 9, 10 };

        private static void TestConvertToList()
        {
            IList<byte> bytesSourceList = new List<byte>(_sourceBytes);
            var bytesNew = new byte[_sourceBytes.Length - 2];
            bytesSourceList.CopyTo(bytesNew, 3);
        }

        private static void TestArrayDotCopy()
        {
            var bytesNew = new byte[_sourceBytes.Length - 2];
            Array.Copy(_sourceBytes, 2, bytesNew, 0, bytesNew.Length);
        }

        private static void TestBlockCopy()
        {
            var bytesNew = new byte[_sourceBytes.Length - 2];
            Buffer.BlockCopy(_sourceBytes, 2, bytesNew, 0, bytesNew.Length);
        }

        private static void TestSkipTake()
        {
            var bytesNew = new byte[_sourceBytes.Length - 2];
            bytesNew = _sourceBytes.Skip(2).Take(_sourceBytes.Length - 2).ToArray();
        }

        public static void Result()
        {
            TestUtils.ConsoleResult(TestUtils.TestMethodUseTime(TestConvertToList, "TestConvertToList"), "TestConvertToList");

            TestUtils.ConsoleResult(TestUtils.TestMethodUseTime(TestArrayDotCopy, "TestArrayDotCopy"), "TestArrayDotCopy");

            TestUtils.ConsoleResult(TestUtils.TestMethodUseTime(TestBlockCopy, "TestBlockCopy"), "TestBlockCopy");

            TestUtils.ConsoleResult(TestUtils.TestMethodUseTime(TestSkipTake, "TestSkipTake"), "TestSkipTake");
        }

    }

}
