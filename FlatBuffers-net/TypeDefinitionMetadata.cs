using System;

namespace FlatBuffers
{
    /// <summary>
    /// Represents a key/value pair of a metadata attribute for the <see cref="FlatBuffersSchema"/>
    /// </summary>
    public class TypeDefinitionMetadata
    {
        private object _value;
        /// <summary>
        /// The attribute name
        /// </summary>
        public string Key { get; internal set; }

        /// <summary>
        /// The value of this attribute
        /// </summary>
        /// <exception cref="InvalidOperationException"></exception>
        public object Value
        {
            get
            {
                if (!HasValue)
                {
                    throw new InvalidOperationException();
                }
                return _value;
            }
            internal set
            {
                if (value == null)
                {
                    _value = null;
                    HasValue = false;
                    return;
                }
                _value = value;
                HasValue = true;
            }
        }

        /// <summary>
        /// Gets a Boolean to indicate if this attribute has a value
        /// </summary>
        public bool HasValue { get; private set; }

        /// <summary>
        /// Gets if this is a user-created attribute
        /// </summary>
        public bool IsUserMetaData { get; internal set; }
    }
}