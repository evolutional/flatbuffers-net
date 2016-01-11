using System;
using FlatBuffers.Attributes;

namespace FlatBuffers
{
    /// <summary>
    /// Provides a default value for a field by extracting it from the <see cref="FlatBuffersDefaultValueAttribute"/> on the field
    /// </summary>
    internal class AttributeDefaultValueProvider : IDefaultValueProvider
    {
        private readonly FlatBuffersDefaultValueAttribute _attr;

        /// <summary>
        /// Initializes and instance of the <see cref="AttributeDefaultValueProvider"/> class
        /// </summary>
        /// <param name="attr"><see cref="FlatBuffersDefaultValueAttribute"/>  on the field</param>
        public AttributeDefaultValueProvider(FlatBuffersDefaultValueAttribute attr)
        {
            _attr = attr;
        }

        /// <summary>
        /// Gets the default value for the field
        /// </summary>
        /// <param name="valueType">The expected type of the value</param>
        /// <returns>The default value</returns>
        public object GetDefaultValue(Type valueType)
        {
            return Convert.ChangeType(_attr.Value, valueType);
        }

        /// <summary>
        /// Tests if the specified value is equal to the default value
        /// </summary>
        /// <param name="value">Value to test</param>
        /// <returns>Boolean to indicate if the specified value is equal to the default value</returns>
        public bool IsDefaultValue(object value)
        {
            var convertedValue = Convert.ChangeType(_attr.Value, value.GetType());
            return value.Equals(convertedValue);
        }

        /// <summary>
        /// Gets if the default value has been set explicitly by the user
        /// </summary>
        public bool IsDefaultValueSetExplicity { get { return true; }}
    }
}