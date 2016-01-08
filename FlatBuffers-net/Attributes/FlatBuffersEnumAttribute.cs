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
    }
}