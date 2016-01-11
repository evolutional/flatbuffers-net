using System;

namespace FlatBuffers
{
    /// <summary>
    /// Interface that provides a value for a given object member
    /// </summary>
    public interface IValueProvider
    {
        /// <summary>
        /// Gets the member value from the object instance
        /// </summary>
        /// <param name="obj">The object instance that contains the member</param>
        /// <returns>The value of the member</returns>
        object GetValue(object obj);

        /// <summary>
        /// Sets the member value on the object instance
        /// </summary>
        /// <param name="obj">The object instance that contains the member</param>
        /// <param name="value">Value to set</param>
        void SetValue(object obj, object value);
        /// <summary>
        /// The memebr value <see cref="Type"/>
        /// </summary>
        Type ValueType { get; }
    }
}