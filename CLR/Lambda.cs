using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace ConsoleApp
{
    public class Lambda
    {
        public void ExceptTest()
        {
            var listA = new List<int>() {1,2,3,4};

            var listB = new List<int>() {2,3,4,5};

            var listr = listA.Except(listB);

            Console.WriteLine(string.Join(",",listr));
        }
    }
}
