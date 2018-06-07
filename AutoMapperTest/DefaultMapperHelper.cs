using System;
using System.Collections.Generic;
using System.Linq;
using System.Reflection;
using Benlai.Common.Extensions;

namespace AutoMapperTest
{
    public class ModelTypeInfo
    {
        public Type ModelType { get; set; }

        public Dictionary<string,PropertyInfo> ModelPropertyInfoDic { get; set; }

        public Dictionary<string, FieldInfo> ModeFieldInfoDic { get; set; }

        public Dictionary<string, INamedMemberAccessor> ModelPropertyAccessorDic { get; set; }
    }

    public interface INamedMemberAccessor
    {
        void SetValue(object instance, object newValue);

        object GetValue(object instance);
    }

    public class PropertyAccessor<T, TP> : INamedMemberAccessor
    {
        private readonly Action<T, TP> _setValueDelegate;
        private readonly Func<T, TP> _getValueDelegate;

        public PropertyAccessor(PropertyInfo propertyInfo)
        {
            if (propertyInfo != null)
            {
                _setValueDelegate = (Action<T, TP>) Delegate.CreateDelegate(typeof(Action<T, TP>), propertyInfo.GetSetMethod());
                _getValueDelegate = (Func<T, TP>) Delegate.CreateDelegate(typeof(Func<T, TP>),propertyInfo.GetGetMethod());
            }
        }

        public void SetValue(object instance, object newValue)
        {
            _setValueDelegate((T)instance, (TP)newValue);
        }

        public object GetValue(object instance)
        {
            return _getValueDelegate((T) instance);
        }
    }

//    internal class FieldAccessor<T, TP> : INamedMemberAccessor
//    {
//        private readonly Action<T, TP> _setValueDelegate;
//        private readonly Func<T, object> _getValueDelegate;
//
//        public FieldAccessor(FieldInfo fieldInfo)
//        {
//            if (fieldInfo != null)
//            {
//                _setValueDelegate = (Action<T, TP>)Delegate.CreateDelegate(typeof(Action<T, TP>), fieldInfo.GetSetMethod());
//                _getValueDelegate = (Func<T, object>)Delegate.CreateDelegate(typeof(Func<T, object>), fieldInfo.GetGetMethod());
//            }
//        }
//
//        public void SetValue(object instance, object newValue)
//        {
//            _setValueDelegate((T)instance, (TP)newValue);
//        }
//
//        public object GetValue(object instance)
//        {
//            return _getValueDelegate((T)instance);
//        }
//    }

    public static class DelegatedReflectionMemberAccessor
    {
        public static INamedMemberAccessor FindAccessor(Type type, PropertyInfo propertyInfo)
        {
            var accessor =
                Activator.CreateInstance(typeof(PropertyAccessor<,>).MakeGenericType(type, propertyInfo.PropertyType),
                    propertyInfo) as INamedMemberAccessor;

            return accessor;
        }
    }

    public static class DefaultMapperHelper
    {

        private static readonly Dictionary<Type,ModelTypeInfo> SourceCacheDictionary = new Dictionary<Type, ModelTypeInfo>();

        private static readonly Dictionary<Type,ModelTypeInfo> DestinationCacheDictionary = new Dictionary<Type, ModelTypeInfo>();

        private static readonly object LockObj = new object();

        /// <summary>
        /// 克隆对象
        /// </summary>
        /// <param name="sourceObject"></param>
        /// <returns></returns>
        public static TD MapTo<TS,TD>(this TS sourceObject)
        {
            if (sourceObject == null)
                throw new Exception("Mapper source object is null.");

            Type destinationType = typeof(TD);
            Type sourceType = typeof(TS);
            ModelTypeInfo destinationModelTypeInfo;
            ModelTypeInfo sourceModelTypeInfo;
            lock (LockObj)
            {
                sourceModelTypeInfo = SetGetModelTypeInfoCache(SourceCacheDictionary, sourceType);
                destinationModelTypeInfo = SetGetModelTypeInfoCache(DestinationCacheDictionary, destinationType);
            }


            // 首先建立指定类型的一个实例
            object destinationObject = Activator.CreateInstance(destinationType);

            // 取得新的类型实例的字段数组。
            foreach (string destinationFieldName in destinationModelTypeInfo.ModeFieldInfoDic.Keys)
            {
                var destinationField = destinationModelTypeInfo.ModeFieldInfoDic[destinationFieldName];
                if (!sourceModelTypeInfo.ModeFieldInfoDic.ContainsKey(destinationFieldName))
                {
                    //此处可以记录下在源Model内不存在的字段名
                    continue;
                }
                var sourceField = sourceModelTypeInfo.ModeFieldInfoDic[destinationFieldName];
                if (sourceField != null)
                {
                    object value = sourceField.GetValue(sourceObject);
                    destinationField.SetValue(destinationObject, value);
                }
            }

            //我们取得新的类型实例的字段数组。
            foreach (string destinationPropertyName in destinationModelTypeInfo.ModelPropertyInfoDic.Keys)
            {
                var destinationProperty = destinationModelTypeInfo.ModelPropertyInfoDic[destinationPropertyName];
                var destinationMemberAccessor = destinationModelTypeInfo.ModelPropertyAccessorDic[destinationPropertyName];
                if (!sourceModelTypeInfo.ModelPropertyInfoDic.ContainsKey(destinationPropertyName))
                {
                    //此处可以记录下在源Model内不存在的属性名
                    continue;
                }
                var sourceProperty = sourceModelTypeInfo.ModelPropertyInfoDic[destinationPropertyName];
                var sourceMemberAccessor = sourceModelTypeInfo.ModelPropertyAccessorDic[destinationPropertyName];
                if (sourceProperty != null && destinationProperty!=null)
                {
                    object sourcePropertyValue = sourceMemberAccessor.GetValue(sourceObject);
                    destinationMemberAccessor.SetValue(destinationObject, sourcePropertyValue);
                }
            }
            return (TD)destinationObject;
        }

        private static ModelTypeInfo SetGetModelTypeInfoCache(Dictionary<Type, ModelTypeInfo> cacheDictionary, Type type)
        {
            ModelTypeInfo modelTypeInfo;
            if (cacheDictionary.ContainsKey(type))
            {
                modelTypeInfo = cacheDictionary[type];
            }
            else
            {
                var propertyArray = type.GetProperties();
                var fieldArray = type.GetFields();
                modelTypeInfo = new ModelTypeInfo
                {
                    ModelPropertyInfoDic = propertyArray.ToDictionary(s => s.Name),
                    ModeFieldInfoDic = fieldArray.ToDictionary(s => s.Name),
                    ModelPropertyAccessorDic = new Dictionary<string, INamedMemberAccessor>(),
                };
                propertyArray.ForEach(s =>
                {
                    var propertyAccessor = DelegatedReflectionMemberAccessor.FindAccessor(type, s);
                    modelTypeInfo.ModelPropertyAccessorDic.Add(s.Name, propertyAccessor);
                });
                cacheDictionary.Add(type, modelTypeInfo);
            }
            return modelTypeInfo;
        }
    }
}
    