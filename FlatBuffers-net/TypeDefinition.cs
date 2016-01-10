namespace FlatBuffers
{
    /// <summary>
    /// An abstract class that represents the base of any reflected type or field
    /// </summary>
    public abstract class TypeDefinition
    {
        private readonly TypeDefinitionMetadataCollection _metadata = new TypeDefinitionMetadataCollection();

        /// <summary>
        /// Gets a Boolean to indicate if this type definition has metadata
        /// </summary>
        public bool HasMetadata
        {
            get { return _metadata.Count > 0; }
        }

        /// <summary>
        /// Gets the Metadata collection
        /// </summary>
        public TypeDefinitionMetadataCollection Metadata { get { return _metadata; } } 

        /// <summary>
        /// Gets or sets the name of the type
        /// </summary>
        public string Name { get; set; }
    }
}