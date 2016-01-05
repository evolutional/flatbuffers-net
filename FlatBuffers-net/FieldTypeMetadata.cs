using System;

namespace FlatBuffers
{
    public struct FieldTypeMetadata
    {
        private string _value;
        public string Key { get; set; }

        public string Value
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
                if (string.IsNullOrEmpty(value))
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