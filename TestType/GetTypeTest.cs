using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace TestType
{
    public class GetTypeTest
    {
        public static Type GetType(string type)
        {
            Type t = Type.GetType(type);
            return t;
        }
    }
}
