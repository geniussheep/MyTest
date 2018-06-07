using System;
using System.Reflection;
using System.Runtime.Remoting;
using System.Threading;

namespace ConsoleApp
{
    class AppdomainAndHost
    {

    }

    public partial class Program
    {
        public static void MainAppdomainAndHost()
        {

            Console.WriteLine(Guid.NewGuid());
            //Clr via 528页
            //获取对Appdomain的一个引用
            AppDomain adCallThreadDomain = Thread.GetDomain();

            String callingDomainName = adCallThreadDomain.FriendlyName;

            Console.WriteLine("Default AppDomain's friendly name={0}", callingDomainName);

            String exeAssembly = Assembly.GetEntryAssembly().FullName;
            Console.WriteLine("Main assembly={0}", callingDomainName);

            AppDomain ad2 = null;
            Console.WriteLine("{0} Demo #1", Environment.NewLine);
            ad2 = AppDomain.CreateDomain("Ad #2", null, null);
            MarshalByRefType mbrt = null;

            mbrt = (MarshalByRefType) ad2.CreateInstanceAndUnwrap(exeAssembly, "MarshalByRefType");

            //CLR在类型上撒谎
            Console.WriteLine("Type={0}", mbrt.GetType());

            //证明得到的是一个代理对象的引用
            Console.WriteLine("Is proxy={0}", RemotingServices.IsTransparentProxy(mbrt));

            mbrt.SomeMethod();

            AppDomain.Unload(ad2);

            try
            {
                mbrt.SomeMethod();
                Console.WriteLine("Successful call.");
            }
            catch (Exception)
            {
                Console.WriteLine("Failed call.");
            }

            Console.WriteLine("{0} Demo #2", Environment.NewLine);

            ad2 = AppDomain.CreateDomain("Ad #2", null, null);

            mbrt = (MarshalByRefType) ad2.CreateInstanceAndUnwrap(exeAssembly, "MarshalByRefType");

            MarshalByValType mbvt = mbrt.MethodWithReturn();

            Console.WriteLine("Is proxy={0}", RemotingServices.IsTransparentProxy(mbvt));
            Console.WriteLine("Returned object created {0}", mbvt);

            AppDomain.Unload(ad2);
            try
            {
                Console.WriteLine("Returned object created {0}", mbvt);
                Console.WriteLine("Successful call.");

            }
            catch (Exception)
            {

                Console.WriteLine("Failed call.");
            }


            Console.WriteLine("{0} Demo #3", Environment.NewLine);

            ad2 = AppDomain.CreateDomain("Ad #2", null, null);
            mbrt = (MarshalByRefType) ad2.CreateInstanceAndUnwrap(exeAssembly, "MarshalByRefType");

            NonMarshalableType nmt = mbrt.MethodArgAndReturn(callingDomainName);

            Console.WriteLine("Continue.......");

            Console.Read();

        }

        //该类的实例可以跨APPDomain的边界 按引用封送
        public sealed class MarshalByRefType:MarshalByRefObject
        {
            public MarshalByRefType()
            {
                Console.WriteLine("{0} ctor running in {1}",this.GetType(),Thread.GetDomain().FriendlyName);
            }

            public void SomeMethod()
            {
                Console.WriteLine("Executing in {0}",Thread.GetDomain().FriendlyName);
            }

            public MarshalByValType MethodWithReturn()
            {
                Console.WriteLine("Executing in {0}", Thread.GetDomain().FriendlyName);
                return new MarshalByValType();
            }

            public NonMarshalableType MethodArgAndReturn(string callingDomainName)
            {
                Console.WriteLine("Calling from  {0} to {1}",
                    callingDomainName, Thread.GetDomain().FriendlyName);
                return new NonMarshalableType();
            }
        }

        //该类的实例可以跨APPDomain的边界 按值封送
        public sealed class MarshalByValType : Object
        {
            private DateTime m_creationTime = DateTime.Now;

            public MarshalByValType()
            {
                Console.WriteLine("{0} ctor running in {1} ,Created on {2}",
                    this.GetType(), Thread.GetDomain().FriendlyName,m_creationTime);
                
            }

            public override string ToString()
            {
                return m_creationTime.ToLongDateString();
            }
        }


        //该类的实例可以跨APPDomain的边界 按值封送
        public sealed class NonMarshalableType : Object
        {

            public NonMarshalableType()
            {
                Console.WriteLine("Executing in {0}",
                     Thread.GetDomain().FriendlyName);

            }
        }
    }
}
