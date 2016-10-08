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
        /// <summary>
        /// Get from private (and any other) member.
        /// If the real type of specified object doesn't contain the specified member,
        /// it will be automatically search from base type.
        /// </summary>
        /// <param name="obj">target object</param>
        /// <param name="name">member name</param>
        /// <returns>Object got from the member</returns>
        /// <exception cref="ArgumentException">name is not a member of obj</exception>
        public static object GetPrivate(this object obj, string name)
        {
            Type type;
            if (TryFindInstanceMemberOwnerType(obj, name, out type))
            {
                return new PrivateObject(obj, new PrivateType(type)).GetFieldOrProperty(name);
            }
            else if (TryFindStaticMemberOwnerType(obj, name, out type))
            {
                return new PrivateType(type).GetStaticFieldOrProperty(name);
            }

            throw new ArgumentException(name + " is not a member of " + obj.GetType());
        }

        /// <summary>
        /// Get from private (and any other) member with casting the return value.
        /// If the real type of specified object doesn't contain the specified member,
        /// it will be automatically search from base type.
        /// </summary>
        /// <typeparam name="T">type for casting the return value</typeparam>
        /// <param name="obj">target object</param>
        /// <param name="name">member name</param>
        /// <returns>Object got from the member</returns>
        /// <exception cref="ArgumentException">name is not a member of obj</exception>
        public static T GetPrivate<T>(this object obj, string name)
        {
            return (T)GetPrivate(obj, name);
        }

        /// <summary>
        /// Set to private (and any other) member.
        /// If the real type of specified object doesn't contain the specified member,
        /// it will be automatically search from base type.
        /// </summary>
        /// <param name="obj">target object</param>
        /// <param name="name">member name</param>
        /// <param name="value">value to set</param>
        /// <exception cref="ArgumentException">name is not a member of obj</exception>
        public static void SetPrivate(this object obj, string name, object value)
        {
            Type type;
            if (TryFindInstanceMemberOwnerType(obj, name, out type))
            {
                new PrivateObject(obj, new PrivateType(type)).SetFieldOrProperty(name, value);
                return;
            }
            else if (TryFindStaticMemberOwnerType(obj, name, out type))
            {
                new PrivateType(type).SetStaticFieldOrProperty(name, value);
                return;
            }

            throw new ArgumentException(name + " is not a member of " + obj.GetType());
        }

        private static bool TryFindInstanceMemberOwnerType(object obj, string memberName, out Type ownerType)
        {
            ownerType = FindMemberOwnerType(obj.GetType(), memberName, BindingFlags.Public | BindingFlags.NonPublic | BindingFlags.DeclaredOnly | BindingFlags.Instance);
            return (ownerType != null);
        }

        private static bool TryFindStaticMemberOwnerType(object obj, string memberName, out Type ownerType)
        {
            ownerType = FindMemberOwnerType(obj.GetType(), memberName, BindingFlags.Public | BindingFlags.NonPublic | BindingFlags.DeclaredOnly | BindingFlags.Static);
            return (ownerType != null);
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