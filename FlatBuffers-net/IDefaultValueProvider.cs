using System;

namespace FlatBuffers
{
    /// <summary>
    /// Provides a default value for a given field
    /// </summary>
    public interface IDefaultValueProvider
    {
        /// <summary>
        /// Gets the default value for the field
        /// </summary>
        /// <param name="valueType">The expected type of the value</param>
        /// <returns>The default value</returns>
        object GetDefaultValue(Type valueType);

        /// <summary>
        /// Tests if the specified value is equal to the default value
        /// </summary>
        /// <param name="value">Value to test</param>
        /// <returns>Boolean to indicate if the specified value is equal to the default value</returns>
        bool IsDefaultValue(object value);

        /// <summary>
        /// Gets if the default value has been set explicitly by the user
        /// </summary>
        bool IsDefaultValueSetExplicity { get; }
    }
}