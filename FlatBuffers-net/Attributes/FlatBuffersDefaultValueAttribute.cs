using System;

namespace FlatBuffers.Attributes
{
    /// <summary>
    /// Attribute to provide a default value on a struct or table field
    /// </summary>
    [AttributeUsage(AttributeTargets.Property|AttributeTargets.Field, AllowMultiple = false, Inherited = true)]
    public sealed class FlatBuffersDefaultValueAttribute : Attribute
    {
        public FlatBuffersDefaultValueAttribute(bool value)
        {
            Value = value;
        }

        public FlatBuffersDefaultValueAttribute(byte value)
        {
            Value = value;
        }

        public FlatBuffersDefaultValueAttribute(sbyte value)
        {
            Value = value;
        }

        public FlatBuffersDefaultValueAttribute(short value)
        {
            Value = value;
        }

        public FlatBuffersDefaultValueAttribute(ushort value)
        {
            Value = value;
        }

        public FlatBuffersDefaultValueAttribute(int value)
        {
            Value = value;
        }

        public FlatBuffersDefaultValueAttribute(uint value)
        {
            Value = value;
        }

        public FlatBuffersDefaultValueAttribute(long value)
        {
            Value = value;
        }

        public FlatBuffersDefaultValueAttribute(ulong value)
        {
            Value = value;
        }

        public FlatBuffersDefaultValueAttribute(float value)
        {
            Value = value;
        }

        public FlatBuffersDefaultValueAttribute(double value)
        {
            Value = value;
        }

        public FlatBuffersDefaultValueAttribute(Type type, string value)
        {
            Value = Convert.ChangeType(value, type);
        }

        public object Value { get; private set; }
    }
}