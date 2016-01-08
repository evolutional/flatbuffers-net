using System;

namespace FlatBuffers
{
    /// <summary>
    /// Provides additional information about the reflected enum
    /// </summary>
    public class EnumTypeDefinition : TypeDefinition
    {
        /// <summary>
        /// Gets if the enum type has been auto sized to fit the smallest type it can do
        /// </summary>
        public bool IsAutoSized { get; internal set; }

        /// <summary>
        /// Gets the underlying type of the num
        /// </summary>
        public Type UnderlyingType { get; internal set; }
    }
}