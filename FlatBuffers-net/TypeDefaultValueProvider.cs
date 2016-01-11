using System;
using System.Collections.Generic;

namespace FlatBuffers
{
    internal sealed class TypeDefaultValueProvider : IDefaultValueProvider
    {
        private readonly static TypeDefaultValueProvider _instance = new TypeDefaultValueProvider();


        private readonly static Dictionary<Type, object> _defaultValues; 

        static TypeDefaultValueProvider()
        {
            _defaultValues = new Dictionary<Type, object>
            {
                { typeof(sbyte), (sbyte)0 },
                { typeof(byte), (byte)0 },
                { typeof(short), (short)0 },
                { typeof(ushort), (ushort)0 },
                { typeof(int), (int)0 },
                { typeof(uint), (uint)0 },
                { typeof(long), (long)0 },
                { typeof(ulong), (ulong)0 },
                { typeof(float), (float)0 },
                { typeof(double), (double)0 },
                { typeof(bool), false },
            };

        }

        public static TypeDefaultValueProvider Instance { get { return _instance; } }

        public object GetDefaultValue(Type valueType)
        {
            object defaultValue = null;
            if (_defaultValues.TryGetValue(valueType, out defaultValue))
            {
                return defaultValue;
            }
            return null;
        }

        public bool IsDefaultValue(object value)
        {
            if (value == null)
            {
                return true;
            }

            object defaultValue = null;
            if (_defaultValues.TryGetValue(value.GetType(), out defaultValue))
            {
                return defaultValue.Equals(value);
            }
            return value == null;
        }

        public bool IsDefaultValueSetExplicity { get { return false; } }
    }
}