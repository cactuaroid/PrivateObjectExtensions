using System;
using System.Linq;
using System.Reflection;

namespace Microsoft.VisualStudio.TestTools.UnitTesting
{
    /// <summary>
    /// This class provides shortcut extension methods for PrivateObject.
    /// Furthermore, even if the member is owned by the base class, it will be automatically resolved.
    /// </summary>
    public static class PrivateObjectExtensions
    {
        public static object GetPrivateField(this object obj, string name)
        {
            return GetPrivateObject(obj, name).GetField(name);
        }

        public static T GetPrivateField<T>(this object obj, string name)
        {
            return (T)GetPrivateField(obj, name);
        }

        public static object GetPrivateFieldOrProperty(this object obj, string name)
        {
            return GetPrivateObject(obj, name).GetFieldOrProperty(name);
        }

        public static T GetPrivateFieldOrProperty<T>(this object obj, string name)
        {
            return (T)GetPrivateFieldOrProperty(obj, name);
        }

        public static object GetPrivateProperty(this object obj, string name)
        {
            return GetPrivateObject(obj, name).GetProperty(name);
        }

        public static T GetPrivateProperty<T>(this object obj, string name)
        {
            return (T)GetPrivateProperty(obj, name);
        }

        public static object InvokePrivate(this object obj, string name, params object[] args)
        {
            return GetPrivateObject(obj, name).Invoke(name, args);
        }

        public static T InvokePrivate<T>(this object obj, string name, params object[] args)
        {
            return (T)InvokePrivate(obj, name, args);
        }

        public static object InvokePrivate(this object obj, string name, Type[] parameterTypes, object[] args)
        {
            return GetPrivateObject(obj, name).Invoke(name, parameterTypes, args);
        }

        public static T InvokePrivate<T>(this object obj, string name, Type[] parameterTypes, object[] args)
        {
            return (T)InvokePrivate(obj, name, parameterTypes, args);
        }

        public static void SetPrivateField(this object obj, string name, object value)
        {
            GetPrivateObject(obj, name).SetField(name, value);
        }

        public static void SetPrivateFieldOrProperty(this object obj, string name, object value)
        {
            GetPrivateObject(obj, name).SetFieldOrProperty(name, value);
        }

        public static void SetPrivateProperty(this object obj, string name, object value)
        {
            GetPrivateObject(obj, name).SetProperty(name, value);
        }

        private static PrivateObject GetPrivateObject(object obj, string memberName)
        {
            var ownerType = FindMemberOwnerType(obj.GetType(), memberName, BindingFlags.Public | BindingFlags.NonPublic | BindingFlags.Instance);
            if (ownerType == null) { throw new ArgumentException(memberName + " is not a member of " + obj.GetType()); }

            return new PrivateObject(obj, new PrivateType(ownerType));
        }

        private static Type FindMemberOwnerType(Type objectType, string memberName, BindingFlags bindingFlags)
        {
            var members = objectType.GetMembers(bindingFlags);

            if (members.Any((x) => x.Name == memberName)) { return objectType; }
            if (objectType.BaseType == null) { return null; }

            return FindMemberOwnerType(objectType.BaseType, memberName, bindingFlags);
        }
    }
}