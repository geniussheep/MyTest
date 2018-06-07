using System;
using System.Collections.Generic;
using System.Linq;
using System.Reflection;
using System.Text;

namespace ConsoleTest
{

    public class TestReflection
    {

        /// <summary>
        /// 获取对象属性值
        /// </summary>
        /// <param name="obj"></param>
        /// <param name="attributeName"></param>
        /// <returns></returns>
        public static object GetObjectFieldValue(object objEntity, PropertyInfo field, Type typeEntity)
        {
            // 获取实体已赋值字段
            object colValue = null;
            if (field != null)
            {
                colValue = field.GetValue(objEntity, null);
            }
            else
            {
                throw new Exception(typeEntity.Name + "不存在属性");
            }
            return colValue;
        }
        /// <summary>
        /// 设置对象属性值
        /// </summary>
        /// <param name="obj"></param>
        /// <param name="attributeName"></param>
        /// <returns></returns>
        public static object SetObjectFieldValue(object objEntity, PropertyInfo field, Type typeEntity, object objValue)
        {
            // 设置实体已赋值字段
            if (field != null && objValue != null)
            {
                field.SetValue(objEntity, objValue, null);
            }
            else
            {
                throw new Exception(typeEntity.Name + "不存在属性" + field.Name);
            }
            return objEntity;
        }



        public static void TestGetReflection()
        {

            TestModel testModel = new TestModel() { Name = "testget", Value = 1 };

            Type type = testModel.GetType();

            PropertyInfo namePerproty = type.GetProperties().FirstOrDefault(p => p.Name.Equals("Name", StringComparison.Ordinal));

            var result = GetObjectFieldValue(testModel, namePerproty, type);
#if DEBUG
            Console.WriteLine(result);
#endif
        }


        public static void TestGetDelegate()
        {

            TestModel testModel = new TestModel() { Name = "testget", Value = 1 };

            Type type = testModel.GetType();

            PropertyInfo namePerproty = type.GetProperties().FirstOrDefault(p => p.Name.Equals("Name", StringComparison.Ordinal));
            var result = DelegatedReflectionMemberAccessor.FindAccessor(type, namePerproty).GetValue(testModel);
#if DEBUG
            Console.WriteLine(result);
#endif
        }

        public static void TestSetReflection()
        {

            TestModel testModel = new TestModel() { Name = "testget", Value = 1 };

            Type type = testModel.GetType();

            PropertyInfo namePerproty = type.GetProperties().FirstOrDefault(p => p.Name.Equals("Name", StringComparison.Ordinal));
            var resultModel = new TestModel();

            SetObjectFieldValue(resultModel, namePerproty, type, "testset");
#if DEBUG
            Console.WriteLine("TestSetReflection"+resultModel.Name);
#endif
        }


        public static void TestSetDelegate()
        {

            TestModel testModel = new TestModel() { Name = "testget", Value = 1 };

            Type type = testModel.GetType();

            PropertyInfo namePerproty = type.GetProperties().FirstOrDefault(p => p.Name.Equals("Name", StringComparison.Ordinal));
            var resultModel = new TestModel();
            DelegatedReflectionMemberAccessor.FindAccessor(type, namePerproty).SetValue(resultModel, "testset");
#if DEBUG
            Console.WriteLine("TestSetDelegate"+resultModel.Name);
#endif
        }

        public static void TestReflectionAndDelegate()
        {

#if DEBUG
            TestGetReflection();

            TestGetDelegate();

            TestSetReflection();

            TestSetDelegate();
#else
            double sum = 0;
            int max = 5;
            for (int i = 0; i < max; i++)
            {
                sum += TestUtils.TestMethodUseTime(TestGetReflection, "TestGetReflection");
            }
            Console.WriteLine($"TestGetReflection平均耗时 :{sum / max} ms");

            sum = 0;
            for (int i = 0; i < max; i++)
            {
                sum += TestUtils.TestMethodUseTime(TestGetDelegate, "TestGetDelegate");
                ;
            }
            Console.WriteLine($"TestGetDelegate平均耗时 :{sum / max} ms");

            sum = 0;
            for (int i = 0; i < max; i++)
            {
                sum += TestUtils.TestMethodUseTime(TestSetReflection, "TestSetReflection");
                ;
            }
            Console.WriteLine($"TestSetReflection平均耗时 :{sum / max} ms");

            sum = 0;
            for (int i = 0; i < max; i++)
            {
                sum += TestUtils.TestMethodUseTime(TestSetDelegate, "TestSetDelegate");
                ;
            }
            Console.WriteLine($"TestSetDelegate平均耗时 :{sum / max} ms");
#endif

        }

    }
}
