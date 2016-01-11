using System;

namespace FlatBuffers.Attributes
{
    /// <summary>
    /// Attribute to signify that this type is to be serialized as a table
    /// </summary>
    public class FlatBuffersTableAttribute : Attribute
    {
        private string _identifier;

        /// <summary>
        /// Gets and sets whether the table will be serialized in the original order of
        /// field declaration. If false, the serializer will sort them by size (largest->smallest)
        /// </summary>
        public bool OriginalOrdering { get; set; }

        /// <summary>
        /// Gets and sets the 4 character identifier for this table when used as a root type
        /// </summary>
        public string Identifier
        {
            get { return _identifier; }
            set
            {
                if (string.IsNullOrEmpty(value))
                {
                    throw new ArgumentNullException();
                }
                if (value.Length != 4)
                {
                    throw new ArgumentOutOfRangeException("value", value.Length, "Identifier must be exactly 4 characters");
                }
                _identifier = value;
                HasIdentitifer = true;
            }
        }

        /// <summary>
        /// Gets a Boolean to indicicate if this table has a buffer identifier
        /// </summary>
        public bool HasIdentitifer { get; private set; }
    }
}