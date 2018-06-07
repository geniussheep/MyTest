using System;
using System.Collections.Generic;
using System.IO;
using System.Reflection;
using System.Runtime.Serialization;
using System.Runtime.Serialization.Formatters.Binary;
using System.Security.Permissions;

namespace ConsoleApp
{
    internal class SerializeAndDeserialize
    {
    }


    public partial class Program
    {
        public static void MainSerializeTest()
        {
            //QuickStart();

            //CompleteControlDemo();

            //InheritSerializeDemo();

            SingletonSerializationTest();

            SerializationSurrogateDemo();

            DifferentTypeSerializeDemo();

            Console.ReadLine();
        }

        public static void QuickStart()
        {
            var objectGraph0 = new List<string> { "jeff", "kristin", "aidan", "grant" };
            var objectGraph1= new List<int> { 1,2,5,8 };
            var objectGraph2 = new List<char> { 'e','4','a','5' };
            //序列化,构造一个流容纳序列化对象
            MemoryStream stream =new MemoryStream();
            //构造序列化格式化器，负责所有序列化工作
            BinaryFormatter formatter =new BinaryFormatter();
            //告诉格式化器将对象序列化到流
            formatter.Serialize(stream, objectGraph0);
            formatter.Serialize(stream, objectGraph1);
            formatter.Serialize(stream, objectGraph2);

            //移动指针到序列化内存的首位
            stream.Position = 0;
            //清空原对象
            objectGraph0 = null;
            objectGraph1 = null;
            objectGraph2 = null;
            //反序列化
            objectGraph0 = (List<string>)formatter.Deserialize(stream);
            foreach (var s in objectGraph0)
            {
                Console.WriteLine(s);
            }
            //反序列化
            objectGraph1 = (List<int>)formatter.Deserialize(stream);
            foreach (var s in objectGraph1)
            {
                Console.WriteLine(s);
            }
            //反序列化
            objectGraph2 = (List<char>)formatter.Deserialize(stream);
            foreach (var s in objectGraph2)
            {
                Console.WriteLine(s);
            }
        }

        private static object Deepclone(object original)
        {
            using (var stream = new MemoryStream())
            {
                BinaryFormatter formatter = new BinaryFormatter();

                formatter.Context = new StreamingContext(StreamingContextStates.Clone);

                formatter.Serialize(stream,original);

                stream.Position = 0;
                //将对象图序列化为一个新对象，且想调用者返回对象（深拷贝）的根
                return formatter.Deserialize(stream);
            }
        }

        /// <summary>
        /// 自行控制需要序列化的字段
        /// </summary>
        private static void CompleteControlDemo()
        {
            var ccs = new CompleteControlSerialize { testField1 = 1, testField2 = "1233333" };
            var obj = (CompleteControlSerialize)Deepclone(ccs);
            Console.WriteLine("testField1 : {0}", obj.testField1);
            Console.WriteLine("testField2 : {0}", obj.testField2);
        }

        //继承基类的序列化
        private static void InheritSerializeDemo()
        {
            var ds = new DerivedSerialize(DateTime.Now,"testSerialize",12356);
            var obj = (DerivedSerialize)Deepclone(ds);
            Console.WriteLine("NumBase : {0}", obj.NumBase);
            Console.WriteLine("NameBase : {0}", obj.NameBase);
            Console.WriteLine("NameDerived : {0}", obj.NameDerived);
        }

        private static void SingletonSerializationTest()
        {
            Singleton[] a1 = {Singleton.GetSingleton(), Singleton.GetSingleton()};
            Console.WriteLine("Do both elements refer to the same object?{0}",a1[0] == a1[1]);

            using (var stream = new MemoryStream())
            {
                BinaryFormatter formatter = new BinaryFormatter();
                formatter.Serialize(stream,a1);
                stream.Position = 0;
                Singleton[] a2 = (Singleton[]) formatter.Deserialize(stream);

                Console.WriteLine("Do both elements refer to the same object?{0}", a2[0] == a2[1]);

                Console.WriteLine("Do all elements refer to the same object?{0}", a1[0] == a2[0]);
            }
        }

        private static void SerializationSurrogateDemo()
        {
            using (var stream = new MemoryStream())
            {
                IFormatter formatter = new BinaryFormatter();

                SurrogateSelector ss = new SurrogateSelector();

                //登记代理，告诉选择器为Datetime对象使用代理
                ss.AddSurrogate(typeof(DateTime),formatter.Context,new UniversalToLocalTimeSerializationSurrogate());
                //告诉格式化器使用代理选择器
                formatter.SurrogateSelector = ss;

                DateTime localTimetBeforeSerialize = DateTime.Now;
                formatter.Serialize(stream,localTimetBeforeSerialize);

                stream.Position = 0;
                Console.WriteLine(new StreamReader(stream).ReadToEnd());

                stream.Position = 0;
                DateTime localTimetAfterSerialize = (DateTime) formatter.Deserialize(stream);
                Console.WriteLine("localTimetBeforeSerialize = {0}", localTimetBeforeSerialize);
                Console.WriteLine("localTimetAfterSerialize = {0}", localTimetAfterSerialize);
            }
        }

        static void DifferentTypeSerializeDemo()
        {
            using (var stream = new MemoryStream())
            {
                try
                {
                    // Construct a BinaryFormatter and use it 
                    // to serialize the data to the stream.
                    BinaryFormatter formatter = new BinaryFormatter();

                    // Construct a Version1Type object and serialize it.
                    Version1Type v1obj = new Version1Type();
                    v1obj.x = 123;
                    formatter.Serialize(stream, v1obj);
                    Console.WriteLine("Type of object serialized: " + v1obj.GetType());
                    Console.WriteLine("x = {0}", v1obj.x);

                    // Construct an instance of our the
                    // Version1ToVersion2TypeSerialiationBinder type.
                    // This Binder type can deserialize a Version1Type  
                    // object to a Version2Type object.
                    formatter.Binder = new Version1ToVersion2DeserializationBinder();

                    Version2Type v2obj = (Version2Type)formatter.Deserialize(stream);
                    // To prove that a Version2Type object was deserialized, 
                    // display the object's type and fields to the console.
                    Console.WriteLine("Type of object deserialized: " + v2obj.GetType());
                    Console.WriteLine("x = {0}, name = {1}", v2obj.x, v2obj.name);
                }
                catch (SerializationException e)
                {
                    Console.WriteLine("Failed to serialize. Reason: " + e.Message);
                }
            }
        }
    }

    [Serializable]
    public class Circle
    {

        private double m_radius;

        [NonSerialized] 
        private double m_area;

        public Circle(double radius)
        {
            m_radius = radius;
            m_area = Math.PI*m_radius*m_radius;
        }

        //反序列时系统会调用这个方法，对没有序列化的m_area进行赋值
        [OnDeserialized]//[OnDeserializing]反序列后//[OnSerialized]在序列化后调用的方法[OnDeserializing]反序列前
        private void OnDeserialized(StreamingContext context)
        {
            m_area = Math.PI*m_radius*m_radius;
        }
    }

    /// <summary>
    /// 自行实现完全控制序列化的类
    /// </summary>
    [Serializable]
    public class CompleteControlSerialize : ISerializable, IDeserializationCallback
    {
        public int testField1;

        public string testField2;

        private SerializationInfo mSiInfo;//用于反序列化

        public CompleteControlSerialize()
        { }
        //类是密封的（sealed）则该方法定义为private 否则定义为protected，确保派生类能调用
        //当需要返回敏感数据是可以使用这个属性
        [SecurityPermission(SecurityAction.Demand,SerializationFormatter = true)]
        protected CompleteControlSerialize(SerializationInfo siInfo, StreamingContext context)
        {
            mSiInfo = siInfo;
            mSiInfo.GetEnumerator();
        }

        public void GetObjectData(SerializationInfo info, StreamingContext context)
        {
            info.AddValue("testField1", testField1);
            info.AddValue("testField2", testField2);
        }

        public void OnDeserialization(object sender)
        {
            if (mSiInfo == null)
            {
                return;
            }

            testField1 = mSiInfo.GetInt32("testField1");
            testField2 = mSiInfo.GetString("testField2");
        }

    }

    [Serializable]
    internal class BaseSerialize
    {
        protected string m_name;

        private int m_num;

        public int NumBase { get { return m_num; } }

        public BaseSerialize() { }
        public BaseSerialize(string name,int num) 
        { 
            m_name = name;
            m_num = num;
        }
    }
    [Serializable]
    internal class DerivedSerialize : BaseSerialize, ISerializable
    {
        private DateTime m_name;

        public DerivedSerialize(DateTime dateTime,string name, int num)
            : base(name,num)
        {
            m_name = dateTime;
        }

        public string NameBase { get { return base.m_name; } }
        public DateTime NameDerived { get { return this.m_name; } }

        //如果这个特殊构造器不存在，会抛出SerializationException异常
        [SecurityPermission(SecurityAction.Demand, SerializationFormatter = false)]
        private DerivedSerialize(SerializationInfo info, StreamingContext context)
        {
            if (info == null) return;
            //为本类设置反序列化好的值
            m_name = info.GetDateTime("date");

            //base.m_name = info.GetString(String.Format("{0}.{1}", this.GetType().BaseType.FullName, "m_Name"));

            //查找基类的可序列化字段集合
            Type baseType = this.GetType().BaseType;
            MemberInfo[] members = FormatterServices.GetSerializableMembers(baseType, context);
            foreach (MemberInfo mi in members)
            {
                if ((mi.MemberType & MemberTypes.Field)!= 0)
                {
                    //为基类设置反序列化好的值
                    FieldInfo fi = (FieldInfo)mi;
                    fi.SetValue(this, info.GetValue(baseType.FullName + "+" + mi.Name, fi.FieldType));
                }
            }
        }
        #region ISerializable
        public void GetObjectData(SerializationInfo info, StreamingContext context)
        {
            //为本类序列化值
            info.AddValue("date", m_name);
            //info.AddValue(String.Format("{0}.{1}", this.GetType().BaseType.FullName, "m_Name"), base.m_name);

            //查找基类的可序列化字段集合
            Type baseType = this.GetType().BaseType;
            MemberInfo[] members = FormatterServices.GetSerializableMembers(typeof(DerivedSerialize).BaseType, context);

            foreach (MemberInfo mi in members)
            {
                if ((mi.MemberType & MemberTypes.Field) != 0)
                {
                    //为基类设置序列化值，最好加上基类的前缀名，避免和子类有同名成员时冲突
                    info.AddValue(baseType.FullName + "+" + mi.Name, ((FieldInfo) mi).GetValue(this));
                }
            }

        }
        #endregion
    }

    /// <summary>
    /// 单例模式序列化
    /// </summary>
    [Serializable]
    public class Singleton : ISerializable
    {
        private static readonly Singleton s_theObeObject = new Singleton();

        public string Name = "Jeff";

        public DateTime Date = DateTime.Now;

        private Singleton()
        {
        }

        public static Singleton GetSingleton()
        {
            return s_theObeObject;
        }

        #region ISerializable
        //序列化Singleton时调用的方法，显示调用接口方法的实现
        [SecurityPermission(SecurityAction.Demand, SerializationFormatter = true)]
        void ISerializable.GetObjectData(SerializationInfo info, StreamingContext context)
        {
            info.SetType(typeof(SingletonSerialicationHelper));
        }
        #endregion

        [Serializable]
        private sealed class SingletonSerialicationHelper:IObjectReference
        {
            //这个方法在对象反序列之后调用
            public object GetRealObject(StreamingContext context)
            {
                return Singleton.GetSingleton();
            }
        }
        //特殊的构造器不必要了 ，因为他永远不会被调用
    }


    internal sealed class UniversalToLocalTimeSerializationSurrogate : ISerializationSurrogate
    {
        public void GetObjectData(object obj, SerializationInfo info, StreamingContext context)
        {
            //本地时间转化为UTC时间
            info.AddValue("Date",((DateTime)obj).ToUniversalTime().ToString("u"));
        }

        public object SetObjectData(object obj, SerializationInfo info, StreamingContext context, ISurrogateSelector selector)
        {
            //UTC转化为本地时间
            return DateTime.ParseExact(info.GetString("Date"), "u", null).ToLocalTime();
        }
    }

    /// <summary>
    /// 
    /// </summary>
    [Serializable]
    class Version1Type
    {
        public Int32 x;
    }


    [Serializable]
    class Version2Type : ISerializable
    {
        public Int32 x;
        public String name;

        // The security attribute demands that code that calls
        // this method have permission to perform serialization.
        [SecurityPermissionAttribute(SecurityAction.Demand, SerializationFormatter = true)]
        void ISerializable.GetObjectData(SerializationInfo info, StreamingContext context)
        {
            info.AddValue("x", x);
            info.AddValue("name", name);
        }

        // The security attribute demands that code that calls  
        // this method have permission to perform serialization.
        [SecurityPermissionAttribute(SecurityAction.Demand, SerializationFormatter = true)]
        private Version2Type(SerializationInfo info, StreamingContext context)
        {
            x = info.GetInt32("x");
            try
            {
                name = info.GetString("name");
            }
            catch (SerializationException)
            {
                // The "name" field was not serialized because Version1Type 
                // did not contain this field.
                // Set this field to a reasonable default value.
                name = "Reasonable default value";
            }
        }
    }

    sealed class Version1ToVersion2DeserializationBinder : SerializationBinder
    {
        public override Type BindToType(string assemblyName, string typeName)
        {
            Type typeToDeserialize = null;

            // For each assemblyName/typeName that you want to deserialize to
            // a different type, set typeToDeserialize to the desired type.
            String assemVer1 = Assembly.GetExecutingAssembly().FullName;
            String typeVer1 = "ConsoleApp.Version1Type";

            if (assemblyName == assemVer1 && typeName == typeVer1)
            {
                // To use a type from a different assembly version, 
                // change the version number.
                // To do this, uncomment the following line of code.
                // assemblyName = assemblyName.Replace("1.0.0.0", "2.0.0.0");

                // To use a different type from the same assembly, 
                // change the type name.
                typeName = "ConsoleApp.Version2Type";
            }

            // The following line of code returns the type.
            typeToDeserialize = Type.GetType(String.Format("{0}, {1}",
                typeName, assemblyName));

            return typeToDeserialize;
        }
    }
}




