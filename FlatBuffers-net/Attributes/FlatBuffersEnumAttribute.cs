using System;

namespace FlatBuffers.Attributes
{
    /// <summary>
    /// Attribute to provide options for the serialization of enum types
    /// </summary>
    [AttributeUsage(AttributeTargets.Enum, AllowMultiple = false, Inherited = true)]
    public class FlatBuffersEnumAttribute : Attribute
    {
        /// <summary>
        /// Gets and sets whether the enum will be automatically fit into the smallest type it can
        /// based on the values it contains. This flag is *only* used if the enum has no explicit sizing.
        /// </summary>
        public bool AutoSizeEnum { get; set; }

        /// <summary>
        /// Gets and sets a Boolean to indicate if this enum represents a series of bitflags from 1->N (eg: 1, 2, 4, 8, 16, etc)
        /// </summary>
        public bool BitFlags { get; set; }
    }
}