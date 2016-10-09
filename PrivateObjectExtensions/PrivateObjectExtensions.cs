using System;
using System.Linq;
using System.Reflection;

namespace Microsoft.VisualStudio.TestTools.UnitTesting
{
    /// <summary>
    /// This class provides extension methods for PrivateObject.
    /// </summary>
    public static class PrivateObjectExtensions
    {
        /// <summary>
        /// Get from private (and any other) field/property.
        /// If the real type of specified object doesn't contain the specified field/property,
        /// it will be automatically search from base type.
        /// </summary>
        /// <param name="obj">The object to get</param>
        /// <param name="name">The name of the field/property to get</param>
        /// <returns>The object got from the field/property</returns>
        /// <exception cref="ArgumentException">'name' is not a field/property of 'obj'</exception>
        /// <exception cref="ArgumentNullException">Arguments contain null.</exception>
        public static object GetPrivate(this object obj, string name)
        {
            return GetPrivate(obj, name, obj.GetType());
        }

        /// <summary>
        /// Get from private (and any other) field/property with casting the return value.
        /// If the real type of specified object doesn't contain the specified field/property,
        /// it will be automatically search from base type.
        /// </summary>
        /// <typeparam name="T">The type for casting the return value</typeparam>
        /// <param name="obj">The object to get</param>
        /// <param name="name">The name of the field/property to get</param>
        /// <returns>The object got from the field/property</returns>
        /// <exception cref="ArgumentException">'name' is not a field/property of 'obj'.</exception>
        /// <exception cref="ArgumentNullException">Arguments contain null.</exception>
        /// <exception cref="InvalidCastException">The object got from the field/property cannot be casted to 'T'.</exception>
        public static T GetPrivate<T>(this object obj, string name)
        {
            return (T)GetPrivate(obj, name, obj.GetType());
        }

        /// <summary>
        /// Get from private (and any other) field/property with assuming the specified object as specified type.
        /// If the specified type doesn't contain the specified field/property,
        /// it will be automatically search from base type.
        /// </summary>
        /// <param name="obj">The object to get</param>
        /// <param name="name">The name of the field/property to get</param>
        /// <param name="type">The type of 'obj' for seaching member. Real type of 'obj' is ignored.</param>
        /// <returns>The object got from the field/property</returns>
        /// <exception cref="ArgumentException">'name' is not a field/property of 'obj'</exception>
        /// <exception cref="ArgumentNullException">Arguments contain null.</exception>
        public static object GetPrivate(this object obj, string name, Type type)
        {
            if (obj == null) { throw new ArgumentNullException("obj"); }
            if (name == null) { throw new ArgumentNullException("name"); }
            if (type == null) { throw new ArgumentNullException("type"); }

            Type ownerType;
            if (TryFindInstanceFieldOrPropertyOwnerType(type, name, out ownerType))
            {
                return new PrivateObject(obj, new PrivateType(ownerType)).GetFieldOrProperty(name);
            }
            else if (TryFindStaticFieldOrPropertyOwnerType(type, name, out ownerType))
            {
                return new PrivateType(ownerType).GetStaticFieldOrProperty(name);
            }

            throw new ArgumentException(name + " is not a member of " + type);
        }

        /// <summary>
        /// Get from private (and any other) field/property with assuming the specified object as specified type with casting the return value.
        /// If the specified type doesn't contain the specified field/property,
        /// it will be automatically search from base type.
        /// </summary>
        /// <typeparam name="T">The type for casting the return value</typeparam>
        /// <param name="obj">The object to get</param>
        /// <param name="name">The name of the field/property to get</param>
        /// <param name="type">The type of 'obj' for seaching member. Real type of 'obj' is ignored.</param>
        /// <returns>The object got from the field/property</returns>
        /// <exception cref="ArgumentException">'name' is not a field/property of 'obj'.</exception>
        /// <exception cref="ArgumentNullException">Arguments contain null.</exception>
        /// <exception cref="InvalidCastException">The object got from the field/property cannot be casted to 'T'.</exception>
        public static T GetPrivate<T>(this object obj, string name, Type type)
        {
            return (T)GetPrivate(obj, name, type);
        }

        /// <summary>
        /// Set to private (and any other) field/property.
        /// If the real type of specified object doesn't contain the specified field/property,
        /// it will be automatically search from base type.
        /// </summary>
        /// <param name="obj">The object to set</param>
        /// <param name="name">The name of the field/property to set</param>
        /// <param name="value">The value to set for 'name'</param>
        /// <exception cref="ArgumentException">'name' is not a field/property of 'obj'</exception>
        /// <exception cref="ArgumentNullException">Arguments contain null.</exception>
        public static void SetPrivate(this object obj, string name, object value)
        {
            SetPrivate(obj, name, value, obj.GetType());
        }

        /// <summary>
        /// Set to private (and any other) field/property with assuming the specified object as specified type.
        /// If the specified type doesn't contain the specified field/property,
        /// it will be automatically search from base type.
        /// </summary>
        /// <param name="obj">The object to set</param>
        /// <param name="name">The name of the field/property to set</param>
        /// <param name="value">The value to set for 'name'</param>
        /// <param name="type">The type of 'obj' for seaching member. Real type of 'obj' is ignored.</param>
        /// <exception cref="ArgumentException">'name' is not a field/property of 'obj'</exception>
        /// <exception cref="ArgumentNullException">Arguments contain null.</exception>
        public static void SetPrivate(this object obj, string name, object value, Type type)
        {
            if (obj == null) { throw new ArgumentNullException("obj"); }
            if (name == null) { throw new ArgumentNullException("name"); }
            if (value == null) { throw new ArgumentNullException("value"); }
            if (type == null) { throw new ArgumentNullException("type"); }

            Type ownerType;
            if (TryFindInstanceFieldOrPropertyOwnerType(type, name, out ownerType))
            {
                new PrivateObject(obj, new PrivateType(ownerType)).SetFieldOrProperty(name, value);
                return;
            }
            else if (TryFindStaticFieldOrPropertyOwnerType(type, name, out ownerType))
            {
                new PrivateType(ownerType).SetStaticFieldOrProperty(name, value);
                return;
            }

            throw new ArgumentException(name + " is not a member of " + type);
        }

        private static bool TryFindInstanceFieldOrPropertyOwnerType(Type objType, string name, out Type ownerType)
        {
            ownerType = FindFieldOrPropertyOwnerType(objType, name, BindingFlags.Public | BindingFlags.NonPublic | BindingFlags.DeclaredOnly | BindingFlags.Instance);
            return (ownerType != null);
        }

        private static bool TryFindStaticFieldOrPropertyOwnerType(Type objType, string name, out Type ownerType)
        {
            ownerType = FindFieldOrPropertyOwnerType(objType, name, BindingFlags.Public | BindingFlags.NonPublic | BindingFlags.DeclaredOnly | BindingFlags.Static);
            return (ownerType != null);
        }

        private static Type FindFieldOrPropertyOwnerType(Type objectType, string name, BindingFlags bindingFlags)
        {
            if (string.IsNullOrWhiteSpace(name)) { throw new ArgumentException("name has to be not null or white space."); }

            var fields = objectType.GetFields(bindingFlags);
            var properties = objectType.GetProperties(bindingFlags);
            var members = fields.Concat<MemberInfo>(properties);

            if (members.Any((x) => x.Name == name)) { return objectType; }
            if (objectType.BaseType == null) { return null; }

            return FindFieldOrPropertyOwnerType(objectType.BaseType, name, bindingFlags);
        }
    }
}