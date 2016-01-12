using System;

namespace FlatBuffers
{
    /// <summary>
    /// Provides additional information about the reflected enum
    /// </summary>
    public class EnumTypeDefinition : TypeDefinition
    {
        private bool _flags;
        internal EnumTypeDefinition()
        { }

        /// <summary>
        /// Gets if the enum type has been auto sized to fit the smallest type it can do
        /// </summary>
        public bool IsAutoSized { get; internal set; }

        /// <summary>
        /// Gets the underlying type of the num
        /// </summary>
        public Type UnderlyingType { get; internal set; }

        /// <summary>
        /// Gets a Boolean to indicate that the enum represents bit flags
        /// </summary>
        public bool BitFlags
        {
            get { return _flags; }
            set 
            { 
                _flags = value;
                if (_flags)
                {
                    Metadata.Add(EnumTypeMetadata.BitFlags, false);
                }
                else
                {
                    Metadata.Remove(EnumTypeMetadata.BitFlags);
                }
            }
        }
    }
}