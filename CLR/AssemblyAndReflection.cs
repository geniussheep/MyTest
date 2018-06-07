using System;
using System.Collections.Generic;
using System.IO;
using System.Linq;
using System.Reflection;
using System.Runtime.InteropServices.WindowsRuntime;
using System.Runtime.Serialization;
using System.Text;
using System.Threading.Tasks;
using Microsoft.CSharp.RuntimeBinder;

namespace ConsoleApp
{
    class AssemblyAndReflection
    {
    }

    public partial class Program
    {
        public static void MainAssemblyAndReflection()
        {
            //var dataAssembly = "System.Data,version=4.0.0.0,culture=neutral,PublicKeyToken=b77a5c561934e089";
            //LoadAssemAndShowPublic(dataAssembly);

            //泛型
            //Reflection_Generic();

            //成员信息
            //Reflection_MemberInfoTest();

            //获取方法定义来源
            //Reflection_TestMyType();
            
            //接口的反射获取
            //Reflection_InterfaceTest();

            //一次绑定多次调用
            //InvokeMemberTest.InvokeMember_RunTest();

            //使用绑定句柄来减少进程的内存消耗
            TypeToRuntimeTypeTest.RunTest_MethodInfoToRuntimeMethodHandle();
            Console.ReadLine();

        }

        private static void LoadAssemAndShowPublic(String assmid)
        {
            Assembly a = Assembly.Load(assmid);
            foreach (var t in a.GetExportedTypes())
            {
                Console.WriteLine(t.FullName);
            }
        }

        /// <summary>
        /// 泛型反射
        /// </summary>
        private static void Reflection_Generic()
        {
            var openType = typeof (Dictionary<,>);

            var closedType = openType.MakeGenericType(typeof (string), typeof (int));

            var o = Activator.CreateInstance(closedType);

            Console.WriteLine(o.GetType().FullName);
        }


        private static void Reflection_MemberInfoTest()
        {
            Assembly[] assemblies = AppDomain.CurrentDomain.GetAssemblies();

            foreach (var assembly in assemblies)
            {
                WriteLine(0 ,"Assembly : {0}",assembly);

                foreach (var type in assembly.GetExportedTypes())
                {
                    WriteLine(1,"Type : {0}",type);
                    const BindingFlags bf =
                        BindingFlags.DeclaredOnly | BindingFlags.NonPublic |
                        BindingFlags.Public | BindingFlags.Instance |
                        BindingFlags.Static;

                    foreach (var mi in type.GetMembers(bf))
                    {
                        string typeName = string.Empty;
                        if (mi is Type) typeName = "Nested Type";
                        else if (mi is FieldInfo) typeName = "FieldInfo";
                        else if (mi is MethodInfo) typeName = "MethodInfo";
                        else if (mi is ConstructorInfo) typeName = "ConstructorInfo";
                        else if (mi is PropertyInfo) typeName = "PropertyInfo";
                        else if (mi is EventInfo) typeName = "EventInfo";

                    WriteLine(2,"{0} : {1}",typeName,mi);

                    }
                }
            }
        }

        private static void WriteLine(int indent, string formart, params object[] args)
        {
            Console.WriteLine(new string(' ', 3*indent) + formart, args);
        }

        private static void Reflection_TestMyType()
        {
            MemberInfo[] memberInfos1 = typeof(MyType1).GetMembers();
            foreach (var mi in memberInfos1)
            {
                Console.WriteLine("MyType1 MemberInfo Name:{0} DeclaringType:{1} ReflectedType:{2}", mi.Name, mi.DeclaringType, mi.ReflectedType);
            } 
            MemberInfo[] memberInfos2 = typeof(MyType2).GetMembers();
            foreach (var mi in memberInfos2)
            {
                Console.WriteLine("MyType2 MemberInfo Name:{0} DeclaringType:{1} ReflectedType:{2}", mi.Name, mi.DeclaringType, mi.ReflectedType);
            }
        }

        public sealed class  MyType1
        {
            public override string ToString()
            {
                return null;
            }
        }

        public sealed class MyType2
        {
        
        }

        private static void Reflection_InterfaceTest()
        {
            var t = typeof (MyRetailer);
            //只获取当前程序集的接口
            Type[] interfaces = t.FindInterfaces(TypeFilter, typeof (Program).Assembly);
            Console.WriteLine("MyRetailer implements the following interfaces (defined in this assembly):");
            foreach (var i in interfaces)
            {
                Console.WriteLine("\n Interface: {0}",i);

                InterfaceMapping map = t.GetInterfaceMap(i);

                for (int j = 0; j < map.InterfaceMethods.Length; j++)
                {
                    Console.WriteLine("{0} is implemented by {1} ",map.InterfaceMethods[j],map.TargetMethods[j]);
                }
            }
        }

        private static bool TypeFilter(Type t, object filterCriteria)
        {
            return t.Assembly == (Assembly) filterCriteria;
        }
    }


    public interface IBookRetailer : IDisposable
    {
        void Purchase();
        void ApplyDiscount();
    }

    public interface IMusicRetailer
    {
        void Purchase();
    }

    public class MyRetailer : IBookRetailer, IMusicRetailer, IDisposable
    {
        void IBookRetailer.Purchase() { }
        public void ApplyDiscount() { }
        void IMusicRetailer.Purchase() { }

        public void Dispose()
        {
        }

        public void Purchase() { }
    }


    public sealed class SomeTypeReflection
    {
        private int m_someField;

        public SomeTypeReflection(ref int x)
        {
            x *= 2;
        }

        public override string ToString()
        {
            return m_someField.ToString();
        }

        public int SomeProp
        {
            get { return m_someField;}
            set {
                if (value <1)
                {
                    throw new ArgumentOutOfRangeException("value");
                }
                m_someField = value;
            }
        }

        public event EventHandler SomeEvent;
        private void NoCompilerWarnings()
        {
            SomeEvent.ToString();
        }
    }

    public class InvokeMemberTest
    {
        private const BindingFlags c_bf =
            BindingFlags.DeclaredOnly | BindingFlags.Public | 
            BindingFlags.NonPublic | BindingFlags.Instance;

        public static void InvokeMember_RunTest()
        {
            Type t = typeof (SomeTypeReflection);
            UseInvokeMemberToBindAndInvokeTheMember(t);
            Console.WriteLine();
            BindToMemberThenInvokeTheMember(t);
            Console.WriteLine();
            BindToMemberCreateDelegateToMemberThenInvokeTheMember(t);
            Console.WriteLine();
            UseDynamicToBindAndInvokeMember(t);
            Console.WriteLine();
        }

        private static void EventCallback(object sender, EventArgs e) { }

        /// <summary>
        /// 利用Type的InvokeMember来绑定并调用一个成员
        /// </summary>
        /// <param name="t"></param>
        private static void UseInvokeMemberToBindAndInvokeTheMember(Type t)
        {
            Console.WriteLine("UseInvokeMemberToBindAndInvokeTheMember");

            //构造Type的一个实例
            object[] args = { 12 };//构造器的实参
            Console.WriteLine("x before constructor called : {0}",args[0]);

            object obj = t.InvokeMember(null, c_bf | BindingFlags.CreateInstance, null, null, args);
            Console.WriteLine("x after constructor returns : {0}", args[0]);

            //读写一个字段
            t.InvokeMember("m_someField", c_bf | BindingFlags.SetField, null, obj, new object[]{5});
            int v = (int)t.InvokeMember("m_someField", c_bf | BindingFlags.GetField, null, obj, null);
            Console.WriteLine("someField:{0}", v);

            //调用一个方法
            string s = (string) t.InvokeMember("ToString", c_bf | BindingFlags.InvokeMethod, null, obj, null);
            Console.WriteLine("ToString:{0}", s);

            //读写一个属性
            try
            {
                t.InvokeMember("SomeProp", c_bf | BindingFlags.SetProperty, null, obj, new object[] {0});

            }
            catch (TargetInvocationException e)
            {
                if (e.InnerException.GetType() != typeof (ArgumentOutOfRangeException)) throw;
            Console.WriteLine("Property set catch.");
            }
            t.InvokeMember("SomeProp", c_bf | BindingFlags.SetProperty, null, obj, new object[] { 2 });
            v = (int)t.InvokeMember("SomeProp", c_bf | BindingFlags.GetProperty, null, obj, null);
            Console.WriteLine("SomeProp:{0}", v);

            //调用事件的add/remove方法，为事件添加和删除一个委托
            EventHandler eh = new EventHandler(EventCallback);
            t.InvokeMember("add_SomeEvent", c_bf | BindingFlags.InvokeMethod, null, obj, new object[] {eh});
            t.InvokeMember("remove_SomeEvent", c_bf | BindingFlags.InvokeMethod, null, obj, new object[] {eh});
        }

        /// <summary>
        /// 如何绑定到一个成员，并在以后调用它，
        /// 打算在不同的对象上多次调用一个成员，这个技术可以产生性能更好的代码
        /// </summary>
        /// <param name="t"></param>
        private static void BindToMemberThenInvokeTheMember(Type t)
        {
            Console.WriteLine("BindToMemberThenInvokeTheMember");
            //构造一个实例
            ConstructorInfo ctor = t.GetConstructor(new Type[] {Type.GetType("System.Int32&")});
            //也可以向下方这样写
            //ConstructorInfo ctor = t.GetConstructor(new Type[] {typeof(int).MakeByRefType()});
            object[] args = {12};//构造器的实参
            Console.WriteLine("x before constructor called : {0}", args[0]);
            object obj = ctor.Invoke(args);
            Console.WriteLine("x after constructor returns : {0}", args[0]);

            //读写一个字段
            FieldInfo fi = obj.GetType().GetField("m_someField", c_bf);
            fi.SetValue(obj,33);
            Console.WriteLine("someField : {0}",fi.GetValue(obj));

            //调用一个方法
            MethodInfo mi = obj.GetType().GetMethod("ToString", c_bf);
            string s = (string)mi.Invoke(obj, null);
            Console.WriteLine("ToString:{0}",s);

            //读写一个属性
            PropertyInfo pi = obj.GetType().GetProperty("SomeProp", typeof (int));
            try
            {
                pi.SetValue(obj,0,null);
            }
            catch (TargetInvocationException e)
            {
                if (e.InnerException.GetType() != typeof(ArgumentOutOfRangeException)) throw;
                Console.WriteLine("Property set catch.");
            }
            pi.SetValue(obj, 2,null);
            Console.WriteLine("SomeProp:{0}", pi.GetValue(obj,null));

            //调用事件的add/remove方法，为事件添加和删除一个委托
            EventInfo ei = obj.GetType().GetEvent("SomeEvent", c_bf);
            EventHandler eh = new EventHandler(EventCallback);
            ei.AddEventHandler(obj,eh);
            ei.RemoveEventHandler(obj,eh);
        }

        /// <summary>
        /// 如何绑定到一个成员或对象，然后创建一个委托来引用该对象或成员，通过委托调用的速度非常快
        /// 在相同的对象上多次调用相同的成员，性能比上面的技术更快
        /// </summary>
        /// <param name="t"></param>
        private static void BindToMemberCreateDelegateToMemberThenInvokeTheMember(Type t)
        {
            Console.WriteLine("BindToMemberCreateDelegateToMemberThenInvokeTheMember");
            //构造一个实例(不能创建对一恶搞构造器的委托)
            object[] args = { 12 };//构造器的实参
            Console.WriteLine("x before constructor called : {0}", args[0]);
            object obj = Activator.CreateInstance(t,args);
            Console.WriteLine("Type : {0}", obj.GetType().ToString());
            Console.WriteLine("x after constructor returns : {0}", args[0]);

            //不能创建一个字段的委托

            //调用一个方法
            MethodInfo mi = obj.GetType().GetMethod("ToString", c_bf);
            var toString = (Func<string>) Delegate.CreateDelegate(typeof (Func<string>), obj, mi);
            string s = toString();
            Console.WriteLine("ToString:{0}", s);

            //读写一个属性
            PropertyInfo pi = obj.GetType().GetProperty("SomeProp", typeof(int));
            var setSomeProp = (Action<int>) Delegate.CreateDelegate(typeof (Action<int>), obj, pi.GetSetMethod());
            try
            {
                //setSomeProp(0);
            }
            catch (Exception)
            {
                Console.WriteLine("Property set catch.");
            }
            setSomeProp(2);
            var getSomeProp = (Func<int>)Delegate.CreateDelegate(typeof(Func<int>), obj, pi.GetGetMethod());
            Console.WriteLine("SomeProp:{0}", getSomeProp());

            //调用事件的add/remove方法，为事件添加和删除一个委托
            EventInfo ei = obj.GetType().GetEvent("SomeEvent", c_bf);
            var addSomeEvent = (Action<EventHandler>)Delegate.CreateDelegate(typeof(Action<EventHandler>), obj, ei.GetAddMethod());
            addSomeEvent(EventCallback);
            var removeSomeEvent = (Action<EventHandler>)Delegate.CreateDelegate(typeof(Action<EventHandler>), obj, ei.GetRemoveMethod());
            removeSomeEvent(EventCallback);
        }

        /// <summary>
        /// 如何使用dynamic基元类型来简化访问成员时使用的语法。
        /// 除此之外，若果打算在相同的类型的不同对象上调用相同的成员，这个技术还能提供不错的性能
        /// 因为针对每个类型，绑定都只会发生一次且可以缓存起来，以后多次调用时速度回非常快。
        /// 还可用这个技术调用不同类型的对象成员
        /// </summary>
        /// <param name="t"></param>
        private static void UseDynamicToBindAndInvokeMember(Type t)
        {
            Console.WriteLine("UseDynamicToBindAndInvokeMember");
            //构造一个实例(不能创建对一恶搞构造器的委托)
            object[] args = { 12 };//构造器的实参
            Console.WriteLine("x before constructor called : {0}", args[0]);
            dynamic obj = Activator.CreateInstance(t, args);
            Console.WriteLine("Type : {0}", obj.GetType().ToString());
            Console.WriteLine("x after constructor returns : {0}", args[0]);

            //读写一个字段
            try
            {
                obj.m_someField = 5;
                int v = (int) obj.m_someField;
                Console.WriteLine("someField:{0}", v);
            }
            catch (RuntimeBinderException e)
            {
                Console.WriteLine("Failed to access field:{0}",e.Message);
            }

            //调用一个方法
            string s = (string) obj.ToString();
            Console.WriteLine("ToString:{0}", s);

            //读写一个属性
            try
            {
                //obj.SomeProp = 0;
            }
            catch (TargetInvocationException e)
            {
                Console.WriteLine("Property set catch.");
            }
            obj.SomeProp = 2;
            int val = (int) obj.SomeProp;
            Console.WriteLine("SomeProp:{0}", val);

            //调用事件的add/remove方法，为事件添加和删除一个委托
            obj.SomeEvent += new EventHandler(EventCallback);
            obj.SomeEvent -= new EventHandler(EventCallback);
        }
    }

    /// <summary>
    /// 使用绑定句柄来减少进程的内存消耗
    /// </summary>
    public class TypeToRuntimeTypeTest
    {
        private const BindingFlags c_bf = BindingFlags.Static | BindingFlags.Public |
                                          BindingFlags.NonPublic | BindingFlags.Instance | 
                                          BindingFlags.FlattenHierarchy;
        /// <summary>
        /// 比较绑定句柄和直接绑定对象的内存差异
        /// </summary>
        public static void RunTest_MethodInfoToRuntimeMethodHandle()
        {
            Show("before doing anything");
            //为Mscorlib。dll内的所有方法构建MEthodInfo对象缓存
            List<MethodBase> methodInfos = new List<MethodBase>();
            foreach (var t in typeof(object).Assembly.GetExportedTypes())
            {
                if (t.IsGenericTypeDefinition) continue;
                MethodBase[] mb = t.GetMethods(c_bf);
                methodInfos.AddRange(mb);
            }
            //显示绑定所有方法后的方法个数和堆大小
            Console.WriteLine("method count = {0:###.###}",methodInfos.Count);
            Show("After building cache of methodinfo objects");

            //为所有Methodinfo对象构建RuntimeMethodHandle缓存
            List<RuntimeMethodHandle> methodHandles = methodInfos.ConvertAll<RuntimeMethodHandle>(mb => mb.MethodHandle);

            Show("Holding methodinfo and runtimemethodhandle cache");
            GC.KeepAlive(methodInfos);//阻止缓存被过早的回收

            methodInfos = null;
            Show("After freeing methodinfo objects");

            methodInfos = methodHandles.ConvertAll<MethodBase>(rmh => MethodBase.GetMethodFromHandle(rmh));
            Show("Size of heap after re-creating methodinfo objects");
            GC.KeepAlive(methodHandles);//阻止缓存被过早的回收
            GC.KeepAlive(methodInfos);//阻止缓存被过早的回收

            methodHandles = null;
            methodInfos = null;
            Show("After freeing methodinfo and reuntimemethodhandles objects");
        }

        private static void Show(string msg)
        {
            Console.WriteLine("Heap size={0,12:##.###.###} - {1}", GC.GetTotalMemory(true), msg);
        }
    }
}
