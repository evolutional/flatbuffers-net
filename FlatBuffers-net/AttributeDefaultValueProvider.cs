using System;
using FlatBuffers.Attributes;

namespace FlatBuffers
{
    public sealed class AttributeDefaultValueProvider : IDefaultValueProvider
    {
        private readonly FlatBuffersDefaultValueAttribute _attr;

        public AttributeDefaultValueProvider(FlatBuffersDefaultValueAttribute attr)
        {
            _attr = attr;
        }

        public bool HasDefaultValue { get { return true; } }

        public object GetDefaultValue(Type valueType)
        {
            return Convert.ChangeType(_attr.Value, valueType);
        }

        public bool IsDefaultValue(object value)
        {
            return value.Equals(_attr.Value);
        }

        public bool IsDefaultValueSetExplicity { get { return true; }}
    }
}