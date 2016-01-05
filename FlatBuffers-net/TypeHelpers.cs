using System;
using System.Collections.Generic;
using System.Linq;
using System.Reflection;

namespace FlatBuffers
{
    public static class TypeHelpers
    {
        public static Type GetEnumerableElementType(Type enumerable)
        {
            if (enumerable == null)
            {
                throw new ArgumentException();
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

        public static T Attribute<T>(this MemberInfo member)
        {
            return (T)member.GetCustomAttributes(typeof(T), true).FirstOrDefault();
        }
    }
}