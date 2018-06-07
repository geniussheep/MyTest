using System;
using System.Collections.Generic;
using System.Dynamic;
using System.Text;

namespace ConsoleApp
{
    //<summary>
    //Description:    动态Model类，用于组合页面的各个信息
    //Created Author: Sheep.Yang
    //Created Date:   2014-10-23
    //</summary>
    public class DynamicModel : DynamicObject
    {
        /// <summary>
        /// 动态属性宿主
        /// </summary>
        Dictionary<string, object> _dictionary = new Dictionary<string, object>();

        public Dictionary<string, object> Dictionary { get { return _dictionary; } }
        /// <summary>
        /// 动态Model的属性个数
        /// </summary>
        public int Count
        {
            get
            {
                return _dictionary.Count;
            }
        }

        /// <summary>
        /// Description:    索引  
        /// Created Author: Sheep.Yang
        /// Created Date:   2014-10-24
        /// </summary>
        /// <param name="key"></param>
        /// <returns></returns>
        public object this[string key]
        {
            get { return _dictionary[key.ToLower()]; }
            set { _dictionary[key.ToLower()] = value; }
        }

        /// <summary>
        /// Description:    判断是否存在Key
        /// Created Author: Sheep.Yang
        /// Created Date:   2014-10-24
        /// </summary>
        /// <param name="key">待判定的Key</param>
        /// <returns></returns>
        public bool IsHaveKey(string key)
        {
            return _dictionary.ContainsKey(key.ToLower());
        }

        /// <summary>
        /// Description:    反序列化构造函数
        /// Created Author: Sheep.Yang
        /// Created Date:   2014-10-24
        /// </summary>
        /// <param name="jsonStr">反序列化字符串</param>
        public DynamicModel(string jsonStr)
        {
            JsonDeserialize(jsonStr);
        }

        public DynamicModel(Dictionary<string, object> dictionary)
        {
            _dictionary = dictionary;
        }

        public DynamicModel()
        {

        }

        /// <summary>
        /// Description:    获取动态Model内某个属性
        /// Created Author: Sheep.Yang
        /// Created Date:   2014-10-23
        /// </summary>
        /// <param name="binder">属性名</param>
        /// <param name="result">属性值</param>
        /// <returns>是否获取成功</returns>
        public override bool TryGetMember(GetMemberBinder binder, out object result)
        {
            var name = binder.Name.ToLower();
            return _dictionary.TryGetValue(name, out result);
        }

        /// <summary>
        /// Description:    新增属性或修改属性值
        /// Created Author: Sheep.Yang
        /// Created Date:   2014-10-23
        /// </summary>
        /// <param name="binder">属性名</param>
        /// <param name="value">属性值</param>
        /// <returns>是否获取成功</returns>
        public override bool TrySetMember(SetMemberBinder binder, object value)
        {
            _dictionary[binder.Name.ToLower()] = value;
            return true;
        }

        /// <summary>
        /// Description:    该函数用于将WebModel序列化为Json字符串
        /// Created Author: Sheep.Yang
        /// Created Date:   2014-10-24
        /// </summary>
        /// <returns>json字符串</returns>
        public string JsonSerializer()
        {
            StringBuilder sbuilder = new StringBuilder();
            System.Web.Script.Serialization.JavaScriptSerializer js = new System.Web.Script.Serialization.JavaScriptSerializer();
            js.Serialize(_dictionary, sbuilder);
            return sbuilder.ToString();
        }

        /// <summary>
        /// Description:    根据json字符串反序列化为DynamicWebModel
        /// Created Author: Sheep.Yang
        /// Created Date:   2014-10-24
        /// </summary>
        /// <param name="jsonStr">json字符串</param>
        /// <returns>DynamicModel</returns>
        public DynamicModel JsonDeserialize(string jsonStr)
        {
            System.Web.Script.Serialization.JavaScriptSerializer js = new System.Web.Script.Serialization.JavaScriptSerializer();
            _dictionary = (Dictionary<string, object>)js.DeserializeObject(jsonStr);
            return this;
        }

        public Dictionary<string, object> Clone()
        {
            Dictionary<string, object> result = new Dictionary<string, object>();
            foreach (var key in _dictionary.Keys)
            {
                result[key] = _dictionary[key];
            }
            return result;
        }
    }

    public  partial class Program
    {
        static void MainDynamicModelTest()
        {
            // Creating a dynamic dictionary.
            dynamic person = new DynamicModel();

            // Adding new dynamic properties. 
            // The TrySetMember method is called.
            person.FirstName = "Ellen";
            person.LastName = "Adams";

            // Getting values of the dynamic properties.
            // The TryGetMember method is called.
            // Note that property names are case-insensitive.
            Console.WriteLine(person.firstname + " " + person.lastname);

            // Getting the value of the Count property.
            // The TryGetMember is not called, 
            // because the property is defined in the class.
            Console.WriteLine("Number of dynamic properties:" + person.Count);


            Console.ReadLine();
            // The following statement throws an exception at run time.
            // There is no "address" property,
            // so the TryGetMember method returns false and this causes a
            // RuntimeBinderException.
            Console.WriteLine(person.address);
        }
    }
}
