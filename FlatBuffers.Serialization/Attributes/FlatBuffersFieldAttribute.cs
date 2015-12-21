using System;

namespace FlatBuffers.Serialization.Attributes
{
    [AttributeUsage(AttributeTargets.Field | AttributeTargets.Property)]
    public class FlatBuffersFieldAttribute : Attribute
    {
        /// <summary>
        /// Override the name
        /// </summary>
        public string Name { get; set; }

        /// <summary>
        /// This field is required in Tables
        /// </summary>
        public bool Required { get; set; }
    }

    public class FlatBuffersDefaultValueAttribute : Attribute
    {
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

        public FlatBuffersDefaultValueAttribute(string value)
        {
            Value = value;
        }

        public object Value { get; private set; }
    }
}
