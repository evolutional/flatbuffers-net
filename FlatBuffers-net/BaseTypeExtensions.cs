using System;

namespace FlatBuffers
{
    public static class BaseTypeExtensions
    {
        private static readonly int[] _baseTypeSizes;
        private static readonly Type[] _clrTypes;
        private static readonly string[] _flatBufferTypeNames;

        static BaseTypeExtensions()
        {
            _baseTypeSizes = new[]
            {
                1, 1, sizeof (bool), sizeof (sbyte), sizeof (byte), sizeof (short), sizeof (ushort), sizeof (int),
                sizeof (uint), sizeof (float), sizeof (double), 0, 0, 0, 0
            };

            _clrTypes = new Type[]
            {
                null, null,
                typeof(bool), typeof (sbyte), typeof (byte), typeof (short), typeof (ushort), typeof (int),
                typeof (uint), typeof (float), typeof (double), typeof(string), null, null, null
            };

            _flatBufferTypeNames = new[]
            {
                null,
                null, "bool", "byte", "ubyte", "short", "ushort", "int", "uint", "long", "ulong", "float", "double",
                "string", null, null, null,
            };
        }

        public static bool IsScalar(this BaseType t)
        {
            return t >= BaseType.UType && t <= BaseType.Double;
        }

        public static bool IsInteger(this BaseType t)
        {
            return t >= BaseType.UType && t <= BaseType.ULong;
        }

        public static bool IsFloat(this BaseType t)
        {
            return t == BaseType.Float || t == BaseType.Double;
        }

        public static bool IsFixed(this BaseType t)
        {
            return IsScalar(t) || t == BaseType.Struct;
        }

        public static int SizeOf(this BaseType t)
        {
            return _baseTypeSizes[(int) t];
        }

        public static Type TypeOf(this BaseType t)
        {
            return _clrTypes[(int)t];
        }

        public static string FlatBufferTypeName(this BaseType t)
        {
            return _flatBufferTypeNames[(int)t];
        }
    }
}