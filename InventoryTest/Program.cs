using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using Benlai.Common.WcfInterface;
using Benlai.Inventory.Model;

namespace InventoryTest
{
    class Program
    {
        static void Main(string[] args)
        {
            var result = InventoryWcfManager.GetInstance().GetInventoryList(new List<InventoryQuery>() {new InventoryQuery { ProductSysNo= 12271,MainStockSysNo = 70} });


        }
    }
}
