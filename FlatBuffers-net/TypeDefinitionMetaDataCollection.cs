using System.Collections.Generic;

namespace FlatBuffers
{
    public class TypeDefinitionMetaDataCollection
    {
        private readonly Dictionary<string, TypeDefinitionMetadata> _metadata = new Dictionary<string, TypeDefinitionMetadata>();

        public void Add(string key)
        {
            Add(key, null);
        }

        public void Remove(string key)
        {
            _metadata.Remove(key);
        }

        public void Add(string key, object value)
        {
            _metadata.Remove(key);
            _metadata.Add(key, new TypeDefinitionMetadata() { Key = key, Value = value });
        }

        public int Count { get { return _metadata.Count; } }

        public IEnumerable<TypeDefinitionMetadata> Items { get { return _metadata.Values; } } 
    }
}