using System;
using System.Collections.Generic;
using System.Linq;
using System.Reflection;
using System.Text;

namespace ConsoleTest
{
    public interface INamedMemberAccessor
    {
        void SetValue(object instance, object newValue);

        object GetValue(object instance);
    }

    internal class PropertyAccessor<T, TP> : INamedMemberAccessor
    {
        private readonly Action<T, TP> _setValueDelegate;
        private readonly Func<T, TP> _getValueDelegate;

        public PropertyAccessor(PropertyInfo propertyInfo)
        {
            if (propertyInfo != null)
            {
                _setValueDelegate = (Action<T, TP>)Delegate.CreateDelegate(typeof(Action<T, TP>), propertyInfo.GetSetMethod());
                _getValueDelegate = (Func<T, TP>)Delegate.CreateDelegate(typeof(Func<T, TP>), propertyInfo.GetGetMethod());
            }
        }

        public void SetValue(object instance, object newValue)
        {
            _setValueDelegate((T)instance, (TP)newValue);
        }

        public object GetValue(object instance)
        {
            return _getValueDelegate((T)instance);
        }
    }

    public static class DelegatedReflectionMemberAccessor
    {
        public static Dictionary<Tuple<Type, PropertyInfo>, INamedMemberAccessor> CacheAccessors { get; } =
            new Dictionary<Tuple<Type, PropertyInfo>, INamedMemberAccessor>();

        public static INamedMemberAccessor FindAccessor(Type type, PropertyInfo propertyInfo)
        {
            var cacheKey = Tuple.Create(type, propertyInfo);
            if (CacheAccessors.ContainsKey(cacheKey))
            {
                return CacheAccessors[cacheKey];
            }
            var accessor =
                Activator.CreateInstance(typeof(PropertyAccessor<,>).MakeGenericType(type, propertyInfo.PropertyType),
                    propertyInfo) as INamedMemberAccessor;
            CacheAccessors.Add(cacheKey, accessor);
            return accessor;
        }
    }
}
