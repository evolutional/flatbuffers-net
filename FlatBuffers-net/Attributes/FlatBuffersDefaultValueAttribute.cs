using System;

namespace FlatBuffers.Attributes
{
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

        public FlatBuffersDefaultValueAttribute(Type type, string value)
        {
            Value = Convert.ChangeType(value, type);
        }

        public object Value { get; private set; }
    }
}