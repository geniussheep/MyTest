using System;
using System.Collections.Generic;
using System.Diagnostics;
using System.Linq;
using System.Reflection;
using System.Runtime.InteropServices;
using System.Text;
using System.Threading.Tasks;

namespace ConsoleApp
{
    public class TestAttribute
    {
    }

    [Serializable]
    [DefaultMember("Main")]
    [DebuggerDisplay("Richter", Name = "jeff", Target = typeof(Test))]
    public sealed partial class Test
    {
        [Conditional("Debug")]
        public static void DoSomethingDebug()
        {
            Console.WriteLine("Debug");
        }

        [Conditional("Release")]
        public static void DoSomethingRelease()
        {
            Console.WriteLine("Release");
            
        }

        public Test() { }

        [CLSCompliant(true)]
        [STAThread]
        public static void MainTestAttribute()
        {
            ShowAttributes(typeof(Test));

            //MemberInfo[] mebers = typeof (Test).FindMembers(
            //    MemberTypes.Constructor | MemberTypes.Method,
            //    BindingFlags.Public | BindingFlags.DeclaredOnly | BindingFlags.Instance | BindingFlags.Static,
            //    Type.FilterAttribute, "ReferenceEquals");

            //foreach (var member in mebers)
            //{
            //    ShowAttributes(member);
            //}
        }

        private static void ShowAttributes(MemberInfo attrbuteTarget)
        {
            Attribute[] attributes=Attribute.GetCustomAttributes(attrbuteTarget);
            Console.WriteLine("attributes applied to {0}:{1}",attrbuteTarget.Name,attributes.Length == 0?"none":String.Empty);
            foreach (var attribute in attributes)
            {
                Console.WriteLine("{0}",attribute.GetType().ToString());
                if (attribute is DefaultMemberAttribute)
                {
                    Console.WriteLine("MemberName = {0}", (attribute as DefaultMemberAttribute).MemberName);
                    
                }

                if (attribute is ConditionalAttribute)
                {
                    Console.WriteLine("ConditionString = {0}", (attribute as ConditionalAttribute).ConditionString);

                }

                if (attribute is CLSCompliantAttribute)
                {
                    Console.WriteLine("IsCompliant = {0}", (attribute as CLSCompliantAttribute).IsCompliant);
                }

                DebuggerDisplayAttribute dda = attribute as DebuggerDisplayAttribute;
                if (dda!=null)
                {
                    Console.WriteLine("Value = {0},Name={1},Target={2}", dda.Value, dda.Name, dda.Target);
                }
            }
            Console.WriteLine();
        }

        [CLSCompliant(true)]
        [STAThread]
        public static void MainTest1()
        {
            ShowAttributes1(typeof(Test));

            //MemberInfo[] mebers = typeof(Test).FindMembers(
            //    MemberTypes.Constructor | MemberTypes.Method,
            //    BindingFlags.Public | BindingFlags.DeclaredOnly | BindingFlags.Instance | BindingFlags.Static,
            //    Type.FilterAttribute, "ReferenceEquals");

            //foreach (var member in mebers)
            //{
            //    ShowAttributes1(member);
            //}
        }

        private static void ShowAttributes1(MemberInfo attrbuteTarget)
        {
            IList<CustomAttributeData> attributes = CustomAttributeData.GetCustomAttributes(attrbuteTarget);
            Console.WriteLine("attributes applied to {0}:{1}", attrbuteTarget.Name, attributes.Count == 0 ? "none" : String.Empty);
            foreach (var attribute in attributes)
            {
                Type t = attribute.Constructor.DeclaringType;

                Console.WriteLine("{0} ",t.ToString());
                Console.WriteLine("Constructor called = {0} ",attribute.Constructor);

                IList<CustomAttributeTypedArgument> posArgs = attribute.ConstructorArguments;
                Console.WriteLine(" positional arguments passed to constructor {0}",
                    posArgs.Count == 0 ? "none" : String.Empty);
                foreach (var pa in posArgs)
                {
                    Console.WriteLine(" Type={0},value={1}", pa.ArgumentType, pa.Value);
                }

                IList<CustomAttributeNamedArgument> namedArgs = attribute.NamedArguments;
                Console.WriteLine(" named arguments set after constructor {0}",
                    namedArgs != null && namedArgs.Count == 0 ? "none" : String.Empty);
                if (namedArgs != null)
                    foreach (var na in namedArgs)
                    {
                        Console.WriteLine(" Name={0},Type={1},value={2}", na.MemberInfo.Name, na.TypedValue.ArgumentType,
                            na.TypedValue.Value);
                    }
                Console.WriteLine();
            }
            Console.WriteLine();
        }
    }

    [Flags]
    internal enum Accounts
    {
        Savings= 0x0001,
        Checking = 0x0002,
        Brokerage = 0x0004,
    }

    [AttributeUsage(AttributeTargets.Class)]
    internal sealed class AccountsAttribute : Attribute
    {
        private Accounts m_accounts;

        public AccountsAttribute(Accounts accounts)
        {
            m_accounts = accounts;
        }

        public override bool Match(object obj)
        {
            if(!base.Match(obj)) return false;

            if (obj == null) return false;

            if (this.GetType() != obj.GetType()) return false;

            AccountsAttribute other = obj as AccountsAttribute;

            if ((other.m_accounts & m_accounts) != m_accounts)
            {
                return false;
            }
            return true;
        }

        public override bool Equals(object obj)
        {
            if (!base.Equals(obj)) return false;

            if (obj == null) return false;

            if (this.GetType() != obj.GetType()) return false;

            AccountsAttribute other = obj as AccountsAttribute;

            if (other.m_accounts  != m_accounts)
            {
                return false;
            }
            return true;
        }

        public override int GetHashCode()
        {
            return Convert.ToInt32(m_accounts);
        }
    }
    [Accounts (Accounts.Savings)]
    internal sealed class ChildAccount { }

    [Accounts(Accounts.Savings|Accounts.Checking|Accounts.Brokerage)]
    internal sealed class AdultAccount { }

    public partial class Program
    {
        public void GetVersion([In, Out] string a)
        {
            
        }

        public static void CanWriteCheck(object obj)
        {
            //构造Attribute类型的一个实例，并初始化 用于显示查找的内容
            Attribute checking = new AccountsAttribute(Accounts.Checking);

            //构造应用于类型的Attribute实例
            Attribute validAccounts = Attribute.GetCustomAttribute(obj.GetType(), typeof (AccountsAttribute), false);

            //如果Attribute应用于类型而且Attribute指定了， “Checking”账户，表明该类型可以开支票
            if (validAccounts!=null && checking.Match(validAccounts))
            {
                Console.WriteLine("{0} types can write checks",obj.GetType());
            }
            else
            {
                Console.WriteLine("{0} types can not write checks", obj.GetType());
            }
        }

        public static void AttributeTestMain()
        {
            CanWriteCheck(new ChildAccount());
            CanWriteCheck(new AdultAccount());

            CanWriteCheck(new Program());

            Test.DoSomethingDebug();
            Test.DoSomethingRelease();

            Test.MainTest1();
            Console.ReadLine();

            Test.MainTest1();
            Console.ReadLine();
        }
    }
}
