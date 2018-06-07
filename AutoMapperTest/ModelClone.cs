using System;
using System.Reflection;

namespace AutoMapperTest
{
    /// <summary>
    /// 克隆
    /// </summary>
    /// <typeparam name="S"></typeparam>
    /// <typeparam name="T"></typeparam>
    public class ModelClone<S, T>
    {
        /// <summary>
        /// 克隆对象
        /// </summary>
        /// <param name="sourceObject"></param>
        /// <returns></returns>
        public static T Clone(S sourceObject)
        {
//            return sourceObject.AutoMapTo<S, T>();
            if (sourceObject == null)
                throw new Exception("Clone source object is null.");

            Type targetType = typeof(T);
            Type sourceType = typeof(S);

            PropertyInfo[] targetPropertyList = targetType.GetProperties();
            FieldInfo[] targetFieldList = targetType.GetFields();

            PropertyInfo[] sourcePropertyList = sourceType.GetProperties();
            FieldInfo[] sourceFieldList = sourceType.GetFields();

            // 首先建立指定类型的一个实例
            object targetObject = Activator.CreateInstance(targetType);

            // 取得新的类型实例的字段数组。
            foreach (FieldInfo targetField in targetFieldList)
            {
                FieldInfo sourceField = GetFieldInfo(targetField.Name, sourceFieldList);
                if (sourceField != null)
                {
                    object value = sourceField.GetValue(sourceObject);
                    targetField.SetValue(targetObject, value);
                }
            }

            //我们取得新的类型实例的字段数组。
            foreach (PropertyInfo targetProperty in targetPropertyList)
            {
                PropertyInfo sourceProperty = GetPropertyInfo(targetProperty.Name, sourcePropertyList);
                if (sourceProperty != null)
                {
                    object m_val = sourceProperty.GetValue(sourceObject, null);
                    targetProperty.SetValue(targetObject, m_val, null);
                }
            }
            return (T)targetObject;
        }

        private static FieldInfo GetFieldInfo(string name, FieldInfo[] propertyInfoList)
        {
            foreach (FieldInfo propertyInfo in propertyInfoList)
            {
                if (propertyInfo.Name == name)
                {
                    return propertyInfo;
                }
            }
            return null;
        }

        private static PropertyInfo GetPropertyInfo(string name, PropertyInfo[] propertyInfoList)
        {
            foreach (PropertyInfo propertyInfo in propertyInfoList)
            {
                if (propertyInfo.Name == name)
                {
                    return propertyInfo;
                }
            }
            return null;
        }
    }
}
