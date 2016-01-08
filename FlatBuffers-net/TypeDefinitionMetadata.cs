using System;

namespace FlatBuffers
{
    public class TypeDefinitionMetadata
    {
        private object _value;
        public string Key { get; set; }

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
            set
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

        public bool HasValue { get; private set; }

        /// <summary>
        /// Gets if this is a user-created attribute
        /// </summary>
        public bool IsUserMetaData { get; internal set; }
    }
}