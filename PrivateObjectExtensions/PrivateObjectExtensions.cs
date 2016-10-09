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
        /// <param name="obj">The object to get</param>
        /// <param name="name">The name of the field or property to get</param>
        /// <returns>The object got from the field or property</returns>
        /// <exception cref="ArgumentException">'name' is not a member of 'obj'</exception>
        public static object GetPrivate(this object obj, string name)
        {
            Type type;
            if (TryFindInstanceFieldOrPropertyOwnerType(obj, name, out type))
            {
                return new PrivateObject(obj, new PrivateType(type)).GetFieldOrProperty(name);
            }
            else if (TryFindStaticFieldOrPropertyOwnerType(obj, name, out type))
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
        /// <typeparam name="T">The type for casting the return value</typeparam>
        /// <param name="obj">The object to get</param>
        /// <param name="name">The name of the field or property to get</param>
        /// <returns>The object got from the field or property</returns>
        /// <exception cref="ArgumentException">'name' is not a member of 'obj'</exception>
        public static T GetPrivate<T>(this object obj, string name)
        {
            return (T)GetPrivate(obj, name);
        }

        /// <summary>
        /// Set to private (and any other) member.
        /// If the real type of specified object doesn't contain the specified member,
        /// it will be automatically search from base type.
        /// </summary>
        /// <param name="obj">The object to set</param>
        /// <param name="name">The name of the field or property to set</param>
        /// <param name="value">The value to set for 'name'</param>
        /// <exception cref="ArgumentException">'name' is not a member of 'obj'</exception>
        public static void SetPrivate(this object obj, string name, object value)
        {
            Type type;
            if (TryFindInstanceFieldOrPropertyOwnerType(obj, name, out type))
            {
                new PrivateObject(obj, new PrivateType(type)).SetFieldOrProperty(name, value);
                return;
            }
            else if (TryFindStaticFieldOrPropertyOwnerType(obj, name, out type))
            {
                new PrivateType(type).SetStaticFieldOrProperty(name, value);
                return;
            }

            throw new ArgumentException(name + " is not a member of " + obj.GetType());
        }

        private static bool TryFindInstanceFieldOrPropertyOwnerType(object obj, string name, out Type ownerType)
        {
            ownerType = FindFieldOrPropertyOwnerType(obj.GetType(), name, BindingFlags.Public | BindingFlags.NonPublic | BindingFlags.DeclaredOnly | BindingFlags.Instance);
            return (ownerType != null);
        }

        private static bool TryFindStaticFieldOrPropertyOwnerType(object obj, string name, out Type ownerType)
        {
            ownerType = FindFieldOrPropertyOwnerType(obj.GetType(), name, BindingFlags.Public | BindingFlags.NonPublic | BindingFlags.DeclaredOnly | BindingFlags.Static);
            return (ownerType != null);
        }

        private static Type FindFieldOrPropertyOwnerType(Type objectType, string name, BindingFlags bindingFlags)
        {
            var fields = objectType.GetFields(bindingFlags);
            var properties = objectType.GetProperties(bindingFlags);
            var members = fields.Concat<MemberInfo>(properties);

            if (members.Any((x) => x.Name == name)) { return objectType; }
            if (objectType.BaseType == null) { return null; }

            return FindFieldOrPropertyOwnerType(objectType.BaseType, name, bindingFlags);
        }
    }
}