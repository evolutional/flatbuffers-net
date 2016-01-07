using System;

namespace FlatBuffers.Attributes
{
    /// <summary>
    /// Attribute to signify that this type is to be serialized as a table
    /// </summary>
    [AttributeUsage(AttributeTargets.Struct|AttributeTargets.Class, AllowMultiple = false, Inherited = true)]
    public class FlatBuffersTableAttribute : Attribute
    {
        /// <summary>
        /// Gets and sets whether the table will be serialized in the original order of
        /// field declaration. If false, the serializer will sort them by size (largest->smallest)
        /// </summary>
        public bool OriginalOrdering { get; set; }
    }
}