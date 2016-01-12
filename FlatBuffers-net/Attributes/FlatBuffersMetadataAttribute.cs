using System;

namespace FlatBuffers.Attributes
{
    /// <summary>
    /// Attribute to annotate Flatbuffers objects with custom metadata.
    /// This metadata is declared in the schema.
    /// </summary>
    [AttributeUsage(AttributeTargets.Class | AttributeTargets.Struct | AttributeTargets.Enum | AttributeTargets.Field | AttributeTargets.Property, AllowMultiple = true, Inherited = true)]
    public class FlatBuffersMetadataAttribute : Attribute
    {
        /// <summary>
        /// Gets the metadata atrribute name
        /// </summary>
        public string Name { get; private set; }

        /// <summary>
        /// Gets the metadata value.
        /// </summary>
        public object Value { get; set; }

        /// <summary>
        /// Gets a Boolean to indicate if the metadata attribute has a value
        /// </summary>
        public bool HasValue { get; private set; }

        /// <summary>
        /// Initializes an instance of the <see cref="FlatBuffersMetadataAttribute"/> class
        /// </summary>
        /// <param name="name">The name of the FlatBuffers schema attribute</param>
        public FlatBuffersMetadataAttribute(string name)
        {
            Name = name;
        }

        /// <summary>
        /// Initializes an instance of the <see cref="FlatBuffersMetadataAttribute"/> class
        /// </summary>
        /// <param name="name">The name of the FlatBuffers schema attribute</param>
        /// <param name="value">The value of the FlatBuffers schema attribute</param>
        public FlatBuffersMetadataAttribute(string name, int value)
        {
            Name = name;
            Value = value;
            HasValue = true;
        }

        /// <summary>
        /// Initializes an instance of the <see cref="FlatBuffersMetadataAttribute"/> class
        /// </summary>
        /// <param name="name">The name of the FlatBuffers schema attribute</param>
        /// <param name="value">The value of the FlatBuffers schema attribute</param>
        public FlatBuffersMetadataAttribute(string name, bool value)
        {
            Name = name;
            Value = value;
            HasValue = true;
        }

        /// <summary>
        /// Initializes an instance of the <see cref="FlatBuffersMetadataAttribute"/> class
        /// </summary>
        /// <param name="name">The name of the FlatBuffers schema attribute</param>
        /// <param name="value">The value of the FlatBuffers schema attribute</param>
        public FlatBuffersMetadataAttribute(string name, string value)
        {
            Name = name;
            Value = value;
            HasValue = true;
        }
    }
}
