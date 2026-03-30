using System;
using System.Collections.Generic;
using System.Linq;
using System.Reflection;

namespace SBS.IT.Utilities.Shared.UtilityExtension
{
    public static class PropertyExtension
    {
        private static readonly Dictionary<Type, Dictionary<string, PropertyInfo>> properties = new Dictionary<Type, Dictionary<string, PropertyInfo>>();
        public static T GetPropertyValue<T>(this object obj, string propertyName)
        {
            return (T)GetPropertyValue(obj, propertyName);
        }
        public static object GetPropertyValue(this object obj, string propertyName)
        {
            return obj.GetPropertyInfo(propertyName).GetValue(obj);
        }
        public static void SetPropertyValue<T>(this object obj, string propertyName, T value)
        {
            obj.SetPropertyValue(propertyName, value);
        }
        public static void SetPropertyValue(this object obj, string propertyName, object value)
        {
            obj.GetPropertyInfo(propertyName).SetValue(obj, value);
        }
        public static PropertyInfo GetPropertyInfo(this object obj, string propertyName)
        {
            Type type = obj.GetType();
            PropertyInfo propertyInfo;
            if (!properties.ContainsKey(type))
            {
                propertyInfo = type.GetProperty(propertyName);

                Dictionary<string, PropertyInfo> propertyInfos = new Dictionary<string, PropertyInfo>();
                propertyInfos.Add(propertyName, propertyInfo);
                properties.Add(type, propertyInfos);
            }
            else if (!properties[type].ContainsKey(propertyName))
            {
                propertyInfo = type.GetProperty(propertyName, BindingFlags.IgnoreCase | BindingFlags.Public | BindingFlags.Instance);
                properties[type].Add(propertyName, propertyInfo);
            }
            else
            {
                propertyInfo = properties[type][propertyName];
            }

            return propertyInfo;
        }
        public static bool HasProperty(this object obj, string propertyName)
        {
            return obj.GetPropertyInfo(propertyName) != null;
        }
        public static bool HasProperty(this Type type, string name)
        {
            return type
                .GetProperties(BindingFlags.Public | BindingFlags.Instance | BindingFlags.IgnoreCase).Any(p => p.Name == name);
        }
    }
}
