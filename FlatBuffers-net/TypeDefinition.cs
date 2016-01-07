namespace FlatBuffers
{
    public class TypeDefinition
    {
        // base stuff, like namespace, type name, etc
        private readonly TypeDefinitionMetaDataCollection _metadata = new TypeDefinitionMetaDataCollection();

        public bool HasMetaData
        {
            get { return _metadata.Count > 0; }
        }

        public TypeDefinitionMetaDataCollection MetaData { get { return _metadata; } } 

        public string Name { get; set; }
    }
}