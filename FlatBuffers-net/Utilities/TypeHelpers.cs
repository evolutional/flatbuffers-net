using System;
using System.Collections.Generic;
using System.Linq;
using System.Reflection;

namespace FlatBuffers.Utilities
{
    /// <summary>
    /// A helper class for reflection and type manipulation
    /// </summary>
    public static class TypeHelpers
    {
        /// <summary>
        /// Gets the element type of a generic IEnumerable
        /// </summary>
        /// <param name="enumerable">Enumerable type to extract the element from</param>
        /// <returns>The <see cref="Type"/> of the IEnumerable</returns>
        public static Type GetEnumerableElementType(Type enumerable)
        {
            if (enumerable == null)
            {
                throw new ArgumentNullException();
            }

            var genericEnumerableInterface = enumerable
                .GetInterfaces()
                .FirstOrDefault(i => i.IsGenericType && i.GetGenericTypeDefinition() == typeof(IEnumerable<>));

            if (genericEnumerableInterface == null)
            {
                throw new NotSupportedException();
            }
            var elementType = genericEnumerableInterface.GetGenericArguments()[0];
            return elementType.IsGenericType && elementType.GetGenericTypeDefinition() == typeof(Nullable<>)
                ? elementType.GetGenericArguments()[0]
                : elementType;
        }

        /// <summary>
        /// Helper method to extract the first attribute of a type from a <see cref="ICustomAttributeProvider"/>
        /// </summary>
        /// <param name="member">The <see cref="ICustomAttributeProvider"/> with the attribute</param>
        /// <typeparam name="T">The type of attribute to extract</typeparam>
        /// <returns>An attribute of type T, or null if none</returns>
        public static T Attribute<T>(this ICustomAttributeProvider member)
        {
            return (T)member.GetCustomAttributes(typeof(T), true).FirstOrDefault();
        }

        /// <summary>
        /// Helper method to extract all attributes of a type from a <see cref="ICustomAttributeProvider"/>
        /// </summary>
        /// <param name="member">The <see cref="ICustomAttributeProvider"/> with the attribute</param>
        /// <typeparam name="T">The type of attribute to extract</typeparam>
        /// <returns>An attribute of type T, or null if none</returns>
        public static IEnumerable<T> Attributes<T>(this ICustomAttributeProvider member)
        {
            return member.GetCustomAttributes(typeof(T), true).Select(i=>(T)i);
        }

        /// <summary>
        /// Helper method to indicate if an attribute of a specific type s present on a <see cref="ICustomAttributeProvider"/>
        /// </summary>
        /// <param name="member">The <see cref="ICustomAttributeProvider"/> with the attribute</param>
        /// <typeparam name="T">The type of attribute to extract</typeparam>
        /// <returns>Boolean to indicate if an attribute of the specified type was found</returns>
        public static bool Defined<T>(this ICustomAttributeProvider member)
        {
            return member.IsDefined(typeof (T), true);
        }
    }
}