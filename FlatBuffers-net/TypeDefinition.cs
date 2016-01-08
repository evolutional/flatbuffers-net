namespace FlatBuffers
{
    public class TypeDefinition
    {
        // base stuff, like namespace, type name, etc
        private readonly TypeDefinitionMetadataCollection _metadata = new TypeDefinitionMetadataCollection();

        public bool HasMetadata
        {
            get { return _metadata.Count > 0; }
        }

        public TypeDefinitionMetadataCollection Metadata { get { return _metadata; } } 

        public string Name { get; set; }
    }
}