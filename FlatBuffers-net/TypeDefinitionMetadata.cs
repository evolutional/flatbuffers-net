using System;

namespace FlatBuffers
{
    public struct TypeDefinitionMetadata
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
    }
}