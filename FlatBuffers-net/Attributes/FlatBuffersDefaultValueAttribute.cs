using System;

namespace FlatBuffers.Attributes
{
    /// <summary>
    /// Attribute to provide a default value on a struct or table field
    /// </summary>
    [AttributeUsage(AttributeTargets.Property|AttributeTargets.Field, AllowMultiple = false, Inherited = true)]
    public sealed class FlatBuffersDefaultValueAttribute : Attribute
    {
        /// <summary>
        /// Initlaizes an instance of the <see cref="FlatBuffersDefaultValueAttribute"/> class
        /// </summary>
        /// <param name="value">The default value</param>
        public FlatBuffersDefaultValueAttribute(bool value)
        {
            Value = value;
        }

        /// <summary>
        /// Initlaizes an instance of the <see cref="FlatBuffersDefaultValueAttribute"/> class
        /// </summary>
        /// <param name="value">The default value</param>
        public FlatBuffersDefaultValueAttribute(byte value)
        {
            Value = value;
        }

        /// <summary>
        /// Initlaizes an instance of the <see cref="FlatBuffersDefaultValueAttribute"/> class
        /// </summary>
        /// <param name="value">The default value</param>
        public FlatBuffersDefaultValueAttribute(sbyte value)
        {
            Value = value;
        }

        /// <summary>
        /// Initlaizes an instance of the <see cref="FlatBuffersDefaultValueAttribute"/> class
        /// </summary>
        /// <param name="value">The default value</param>
        public FlatBuffersDefaultValueAttribute(short value)
        {
            Value = value;
        }

        /// <summary>
        /// Initlaizes an instance of the <see cref="FlatBuffersDefaultValueAttribute"/> class
        /// </summary>
        /// <param name="value">The default value</param>
        public FlatBuffersDefaultValueAttribute(ushort value)
        {
            Value = value;
        }

        /// <summary>
        /// Initlaizes an instance of the <see cref="FlatBuffersDefaultValueAttribute"/> class
        /// </summary>
        /// <param name="value">The default value</param>
        public FlatBuffersDefaultValueAttribute(int value)
        {
            Value = value;
        }

        /// <summary>
        /// Initlaizes an instance of the <see cref="FlatBuffersDefaultValueAttribute"/> class
        /// </summary>
        /// <param name="value">The default value</param>
        public FlatBuffersDefaultValueAttribute(uint value)
        {
            Value = value;
        }

        /// <summary>
        /// Initlaizes an instance of the <see cref="FlatBuffersDefaultValueAttribute"/> class
        /// </summary>
        /// <param name="value">The default value</param>
        public FlatBuffersDefaultValueAttribute(long value)
        {
            Value = value;
        }

        /// <summary>
        /// Initlaizes an instance of the <see cref="FlatBuffersDefaultValueAttribute"/> class
        /// </summary>
        /// <param name="value">The default value</param>
        public FlatBuffersDefaultValueAttribute(ulong value)
        {
            Value = value;
        }

        /// <summary>
        /// Initlaizes an instance of the <see cref="FlatBuffersDefaultValueAttribute"/> class
        /// </summary>
        /// <param name="value">The default value</param>
        public FlatBuffersDefaultValueAttribute(float value)
        {
            Value = value;
        }

        /// <summary>
        /// Initlaizes an instance of the <see cref="FlatBuffersDefaultValueAttribute"/> class
        /// </summary>
        /// <param name="value">The default value</param>
        public FlatBuffersDefaultValueAttribute(double value)
        {
            Value = value;
        }

        /// <summary>
        /// Initlaizes an instance of the <see cref="FlatBuffersDefaultValueAttribute"/> class
        /// </summary>
        /// <param name="value">The default value</param>
        public FlatBuffersDefaultValueAttribute(object value)
        {
            Value = value;
        }

        /// <summary>
        /// Initlaizes an instance of the <see cref="FlatBuffersDefaultValueAttribute"/> class
        /// </summary>
        /// <param name="type">The <see cref="Type"/> of the default value</param>
        /// <param name="value">The default value</param>
        public FlatBuffersDefaultValueAttribute(Type type, string value)
        {
            if (type.IsEnum)
            {
                Value = Enum.Parse(type, value);
            }
            else
            {
                Value = Convert.ChangeType(value, type);
            }
        }

        /// <summary>
        /// Gets the value to be used as the default value of a field
        /// </summary>
        public object Value { get; private set; }
    }
}