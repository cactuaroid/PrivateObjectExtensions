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
            if (obj == null) { throw new ArgumentNullException("obj"); }

            return GetPrivate(obj, name, obj.GetType(), null);
        }

        /// <summary>
        /// Get from private (and any other) field/property with casting the return value.
        /// If the real type of specified object doesn't contain the specified field/property,
        /// it will be automatically search from base type.
        /// </summary>
        /// <typeparam name="T">The type of the field/property to get</typeparam>
        /// <param name="obj">The object to get</param>
        /// <param name="name">The name of the field/property to get</param>
        /// <returns>The object got from the field/property</returns>
        /// <exception cref="ArgumentException">'name' is not a field/property of 'obj'.</exception>
        /// <exception cref="ArgumentNullException">Arguments contain null.</exception>
        /// <exception cref="InvalidCastException">The object got from the field/property cannot be casted to 'T'.</exception>
        public static T GetPrivate<T>(this object obj, string name)
        {
            if (obj == null) { throw new ArgumentNullException("obj"); }

            return (T)GetPrivate(obj, name, obj.GetType(), typeof(T));
        }

        /// <summary>
        /// Get from private (and any other) field/property with assuming the specified object as specified type.
        /// If the specified type doesn't contain the specified field/property,
        /// it will be automatically search from base type.
        /// </summary>
        /// <param name="obj">The object to get</param>
        /// <param name="name">The name of the field/property to get</param>
        /// <param name="objType">The type of 'obj' for seaching member. Real type of 'obj' is ignored.</param>
        /// <returns>The object got from the field/property</returns>
        /// <exception cref="ArgumentException">'name' is not a field/property of 'obj'</exception>
        /// <exception cref="ArgumentNullException">Arguments contain null.</exception>
        public static object GetPrivate(this object obj, string name, Type objType)
        {
            return GetPrivate(obj, name, objType, null);
        }

        /// <summary>
        /// Get from private (and any other) field/property with assuming the specified object as specified type with casting the return value.
        /// If the specified type doesn't contain the specified field/property,
        /// it will be automatically search from base type.
        /// </summary>
        /// <typeparam name="T">The type of the field/property to get</typeparam>
        /// <param name="obj">The object to get</param>
        /// <param name="name">The name of the field/property to get</param>
        /// <param name="objType">The type of 'obj' for seaching member. Real type of 'obj' is ignored.</param>
        /// <returns>The object got from the field/property</returns>
        /// <exception cref="ArgumentException">'name' is not a field/property of 'obj'.</exception>
        /// <exception cref="ArgumentNullException">Arguments contain null.</exception>
        /// <exception cref="InvalidCastException">The object got from the field/property cannot be casted to 'T'.</exception>
        public static T GetPrivate<T>(this object obj, string name, Type objType)
        {
            return (T)GetPrivate(obj, name, objType, typeof(T));
        }

        private static object GetPrivate(object obj, string memberName, Type objType, Type memberType)
        {
            if (obj == null) { throw new ArgumentNullException("obj"); }
            if (memberName == null) { throw new ArgumentNullException("memberName"); }
            if (objType == null) { throw new ArgumentNullException("type"); }

            Type ownerType;
            if (TryFindInstanceFieldOrPropertyOwnerType(objType, memberName, memberType, out ownerType))
            {
                return new PrivateObject(obj, new PrivateType(ownerType)).GetFieldOrProperty(memberName);
            }
            else if (TryFindStaticFieldOrPropertyOwnerType(objType, memberName, memberType, out ownerType))
            {
                return new PrivateType(ownerType).GetStaticFieldOrProperty(memberName);
            }

            throw new ArgumentException(((memberType != null) ? memberType.ToString() + " " : "") + memberName + " is not a member of " + objType);
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
        public static void SetPrivate<T>(this object obj, string name, T value)
        {
            if (obj == null) { throw new ArgumentNullException("obj"); }

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
        /// <param name="objType">The type of 'obj' for seaching member. Real type of 'obj' is ignored.</param>
        /// <exception cref="ArgumentException">'name' is not a field/property of 'obj'</exception>
        /// <exception cref="ArgumentNullException">Arguments contain null.</exception>
        public static void SetPrivate<T>(this object obj, string name, T value, Type objType)
        {
            if (obj == null) { throw new ArgumentNullException("obj"); }
            if (name == null) { throw new ArgumentNullException("memberName"); }
            if (value == null) { throw new ArgumentNullException("value"); }
            if (objType == null) { throw new ArgumentNullException("objType"); }

            Type ownerType;
            if (TryFindInstanceFieldOrPropertyOwnerType(objType, name, typeof(T), out ownerType))
            {
                new PrivateObject(obj, new PrivateType(ownerType)).SetFieldOrProperty(name, value);
                return;
            }
            else if (TryFindStaticFieldOrPropertyOwnerType(objType, name, typeof(T), out ownerType))
            {
                new PrivateType(ownerType).SetStaticFieldOrProperty(name, value);
                return;
            }

            throw new ArgumentException(name + " is not a member of " + objType);
        }

        private static bool TryFindInstanceFieldOrPropertyOwnerType(Type objType, string name, Type memberType, out Type ownerType)
        {
            ownerType = FindFieldOrPropertyOwnerType(objType, name, memberType, BindingFlags.Public | BindingFlags.NonPublic | BindingFlags.DeclaredOnly | BindingFlags.Instance);
            return (ownerType != null);
        }

        private static bool TryFindStaticFieldOrPropertyOwnerType(Type objType, string name, Type memberType, out Type ownerType)
        {
            ownerType = FindFieldOrPropertyOwnerType(objType, name, memberType, BindingFlags.Public | BindingFlags.NonPublic | BindingFlags.DeclaredOnly | BindingFlags.Static);
            return (ownerType != null);
        }

        private static Type FindFieldOrPropertyOwnerType(Type objectType, string name, Type memberType, BindingFlags bindingFlags)
        {
            if (string.IsNullOrWhiteSpace(name)) { throw new ArgumentException("name has to be not null or white space."); }

            var fields = objectType
                .GetFields(bindingFlags)
                .Select((x) => new { Type = x.FieldType, Member = x as MemberInfo });

            var properties = objectType
                .GetProperties(bindingFlags)
                .Select((x) => new { Type = x.PropertyType, Member = x as MemberInfo });

            var members = fields.Concat(properties);

            if (members.Any((x) => ((memberType != null) ? memberType.IsAssignableFrom(x.Type) : true) && x.Member.Name == name)) { return objectType; }
            if (objectType.BaseType == null) { return null; }

            return FindFieldOrPropertyOwnerType(objectType.BaseType, name, memberType, bindingFlags);
        }
    }
}