using System;

namespace FlatBuffers.Utilities
{
    /// <summary>
    /// A Helper class for dealing with <see cref="BaseType"/>
    /// </summary>
    public static class BaseTypeExtensions
    {
        private static readonly int[] _baseTypeSizes;
        private static readonly Type[] _clrTypes;
        private static readonly string[] _flatBufferTypeNames;

        private const int OffsetSize = sizeof (short);

        static BaseTypeExtensions()
        {
            _baseTypeSizes = new[]
            {
                1, 1, sizeof (bool), sizeof (sbyte), sizeof (byte), sizeof (short), sizeof (ushort), sizeof (int),
                sizeof (uint), sizeof (long), sizeof (ulong),
                sizeof (float), sizeof (double), 0, 0, OffsetSize, 0
            };

            _clrTypes = new[]
            {
                null, typeof(UnionFieldType),
                typeof(bool), typeof (sbyte), typeof (byte), typeof (short), typeof (ushort), typeof (int),
                typeof (uint), typeof (long), typeof (ulong),
                typeof (float), typeof (double), typeof(string), null, null, null
            };

            _flatBufferTypeNames = new[]
            {
                null,
                null, "bool", "byte", "ubyte", "short", "ushort", "int", "uint", "long", "ulong", 
                "float", "double",
                "string", null, null, null,
            };
        }

        /// <summary>
        /// Indicates if a given <see cref="BaseType"/> represents a scalar value
        /// </summary>
        /// <param name="t">The <see cref="BaseType"/> to test</param>
        /// <returns>A Boolean to indicate if the <see cref="BaseType"/> represents a scalar value</returns>
        public static bool IsScalar(this BaseType t)
        {
            return t >= BaseType.UType && t <= BaseType.Double;
        }

        /// <summary>
        /// Indicates if a given <see cref="BaseType"/> represents an integer value
        /// </summary>
        /// <param name="t">The <see cref="BaseType"/> to test</param>
        /// <returns>A Boolean to indicate if the <see cref="BaseType"/> represents an integer value</returns>
        public static bool IsInteger(this BaseType t)
        {
            return t >= BaseType.UType && t <= BaseType.ULong;
        }

        /// <summary>
        /// Indicates if a given <see cref="BaseType"/> represents a floating point value
        /// </summary>
        /// <param name="t">The <see cref="BaseType"/> to test</param>
        /// <returns>A Boolean to indicate if the <see cref="BaseType"/> represents a floating point value</returns>
        public static bool IsFloat(this BaseType t)
        {
            return t == BaseType.Float || t == BaseType.Double;
        }

        /// <summary>
        /// Indicates if a given <see cref="BaseType"/> represents a fixed size value
        /// </summary>
        /// <param name="t">The <see cref="BaseType"/> to test</param>
        /// <returns>A Boolean to indicate if the <see cref="BaseType"/> represents a fixed size</returns>
        public static bool IsFixed(this BaseType t)
        {
            return IsScalar(t) || t == BaseType.Struct;
        }

        /// <summary>
        /// Gets the inline size of a <see cref="BaseType"/>
        /// </summary>
        /// <param name="t">The <see cref="BaseType"/> to get the size of</param>
        /// <returns>The inline size of a <see cref="BaseType"/></returns>
        public static int SizeOf(this BaseType t)
        {
            return _baseTypeSizes[(int) t];
        }

        /// <summary>
        /// Gets the CLR <see cref="Type"/> that a <see cref="BaseType"/> represents
        /// </summary>
        /// <param name="t">The <see cref="BaseType"/> to get the type of</param>
        /// <returns>The CLR <see cref="Type"/> that a <see cref="BaseType"/> represents</returns>
        public static Type TypeOf(this BaseType t)
        {
            return _clrTypes[(int)t];
        }

        /// <summary>
        /// Gets the name of the <see cref="BaseType"/> as used in the FlatBuffersSchema
        /// </summary>
        /// <param name="t">The <see cref="BaseType"/> to get the name of</param>
        /// <returns>The name of the <see cref="BaseType"/> as used in the FlatBuffersSchema</returns>
        public static string FlatBufferTypeName(this BaseType t)
        {
            return _flatBufferTypeNames[(int)t];
        }
    }
}