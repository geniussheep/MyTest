using System;
using System.Collections.Generic;
using System.Linq;
using System.Reflection;
using System.Text;
using System.Threading.Tasks;

namespace ConsoleTaskPool.QueuePool
{
    public class QueueModel
    {
        public int id { get; set; }


        public string p1 { get; set; }


        /// <summary>
        /// 动态设置属性 
        /// </summary>
        /// <param name="propertyName"></param>
        /// <returns></returns>
        public object this[string propertyName]
        {
            get
            {
                PropertyInfo p = this.GetType().GetProperty(propertyName);
                if (p == null)
                    return null;
                return p.GetValue(this);
            }
            set
            {
                PropertyInfo p = this.GetType().GetProperty(propertyName);
                if (p == null)
                    return;
                p.SetValue(this, value);
            }
        }
    }
}
