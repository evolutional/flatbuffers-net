using System.Collections.Generic;

namespace FlatBuffers
{
    public class TypeDefinitionMetadataCollection
    {
        private readonly Dictionary<string, TypeDefinitionMetadata> _metadata = new Dictionary<string, TypeDefinitionMetadata>();

        public void Add(string key, bool isUser)
        {
            Add(key, null, isUser);
        }

        public void Remove(string key)
        {
            _metadata.Remove(key);
        }

        public void Add(string key, object value, bool isUser)
        {
            _metadata.Remove(key);
            _metadata.Add(key, new TypeDefinitionMetadata() { Key = key, Value = value, IsUserMetaData = isUser});
        }


        public int Count { get { return _metadata.Count; } }

        public IEnumerable<TypeDefinitionMetadata> Items { get { return _metadata.Values; } }

        public TypeDefinitionMetadata GetByName(string name)
        {
            TypeDefinitionMetadata obj = null;
            _metadata.TryGetValue(name, out obj);
            return obj;
        }
    }
}