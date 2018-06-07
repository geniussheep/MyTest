using System;
using System.Collections.Generic;
using System.Diagnostics.Contracts;
using System.IO;
using System.Runtime.CompilerServices;
using System.Runtime.ConstrainedExecution;
using System.Runtime.Serialization;
using System.Security.Permissions;
using System.Windows.Forms.VisualStyles;

namespace ConsoleApp
{
    [Serializable]
    public sealed class Exception<TExceptionArgs>:Exception,ISerializable
        where TExceptionArgs : ExceptionArgs
    {
        private const string c_args = "args";//用于（反）序列化

        private readonly TExceptionArgs _mArgs;

        public TExceptionArgs Args {
            get { return _mArgs; }
        }

        public Exception(TExceptionArgs args, string message = null, Exception innerException = null)
            : base(message, innerException)
        {
            _mArgs = args;
        }

        public Exception(string message = null, Exception innerException = null) 
            : this(null, message, innerException)
        {
        }

        //用于构造器反序列化
        [SecurityPermission(SecurityAction.LinkDemand,
            Flags = SecurityPermissionFlag.SerializationFormatter)]
        private Exception(SerializationInfo info, StreamingContext context) : base(info, context)
        {
            _mArgs = (TExceptionArgs) info.GetValue(c_args, typeof (TExceptionArgs));
        }

        //这个方法用于序列化：由于ISerializable接口 ，所以它是公共的
        [SecurityPermission(SecurityAction.LinkDemand, 
            Flags = SecurityPermissionFlag.SerializationFormatter)]
        public override void GetObjectData(SerializationInfo info, StreamingContext context)
        {
            info.AddValue(c_args,_mArgs);
            base.GetObjectData(info,context);
        }

        public override string Message
        {
            get
            {
                string baseMsg = base.Message;
                return _mArgs == null ? baseMsg : String.Format("{0}({1})", baseMsg, _mArgs.Message);
            }
        }

        public override bool Equals(object obj)
        {
            if (obj == null ) return false;
            if (!(obj is Exception<TExceptionArgs>)) return false;
            Exception<TExceptionArgs> other = (Exception<TExceptionArgs>) obj;
            return Object.Equals(_mArgs,other._mArgs) && base.Equals(obj);
        }
    }

    [Serializable]
    public abstract class ExceptionArgs
    {
        public virtual string Message { get { return String.Empty; } }
    }

    [Serializable]
    public sealed class DiskFullExceptionArgs : ExceptionArgs
    {
        private readonly string m_diskpath;

        public DiskFullExceptionArgs(string diskpath)
        {
            m_diskpath = diskpath;
        }

        public string DiskPath { get { return m_diskpath;} }

        public override string Message
        {
            get { return m_diskpath == null ? base.Message : "DiskPath:" + m_diskpath; }
        }
    }

    public partial class Program
    {
        public static void MainException()
        {
            //try
            //{
            //    throw new Exception<DiskFullExceptionArgs>(new DiskFullExceptionArgs(@"c:\"),"the disk is full");
            //}
            //catch (Exception<DiskFullExc eptionArgs> e)
            //{
            //    Console.WriteLine(e.Message);
            //}

            Demo1();
            Demo2();

            var shoppingCart = new ShoppingCart (null,1.00M);
            shoppingCart.AddItem(new Type1());
            Console.Read();
        }

        public static void Demo1()
        {
            //强迫finally快中的代码提取准备好
            RuntimeHelpers.PrepareConstrainedRegions();
            try
            {
                Console.WriteLine("in try");
            }
            finally 
            {
                Type1.M();
            }
        }

        public static void Demo2()
        {
            try
            {
                Console.WriteLine("in try");
            }
            finally
            {
                Type2.M();
            }
        }
    }

    public class Type1
    {
        static Type1()
        {
            Console.WriteLine("type1 static ctor called");
        }

        [ReliabilityContract(Consistency.WillNotCorruptState,Cer.Success)]
        public static void M()
        { }
    }

    public class Type2
    {
        static Type2()
        {
            Console.WriteLine("type2 static ctor called");
        }

        public static void M()
        { }
    }

    public sealed class ShoppingCart
    {
        private List<object> m_cart = new List<object>();

        private decimal m_totalCost = 0;

        public ShoppingCart(List<object> cart,decimal totalCost )
        {
            m_cart = cart;
            m_totalCost = totalCost;
        }

        public void AddItem(object item)
        {
            AddItemHelper(m_cart,item,ref m_totalCost);
        }

        private static void AddItemHelper(List<object> m_cart, object newItem, ref decimal totalCost)
        {
            //前条件
            Contract.Requires(newItem !=null);
            Contract.Requires(Contract.ForAll(m_cart, s => s != newItem));

            //后条件
            Contract.Ensures(Contract.Exists(m_cart, s => s == newItem));
            Contract.Ensures(totalCost>=Contract.OldValue(totalCost));
            Contract.EnsuresOnThrow<IOException>(totalCost==Contract.OldValue(totalCost));

            //做一些事情 （可能抛出异常IOException）
            m_cart.Add(newItem);
            totalCost += 1.00M;
        }

        [ContractInvariantMethod]
        private void ObjectInvariant()
        {
            Contract.Invariant(m_totalCost>=0);
        }
    }
}
