using System.Collections.Generic;

namespace FlatBuffers
{
    /// <summary>
    /// A simple collection of <see cref="TypeDefinitionMetadata"/> that applies to an instance of <see cref="TypeDefinition"/>
    /// </summary>
    public class TypeDefinitionMetadataCollection
    {
        private readonly Dictionary<string, TypeDefinitionMetadata> _metadata = new Dictionary<string, TypeDefinitionMetadata>();

        /// <summary>
        /// Adds a given attribute to the collection
        /// </summary>
        /// <param name="key">Name of the attribute to add</param>
        /// <param name="isUser">A Boolean to indicate if this is a user-created attribute, or one from the system</param>
        public void Add(string key, bool isUser)
        {
            Add(key, null, isUser);
        }

        /// <summary>
        /// Removes an attribute from the collection
        /// </summary>
        /// <param name="key">Name of the attribute to remove</param>
        public void Remove(string key)
        {
            _metadata.Remove(key);
        }

        /// <summary>
        /// Adds a given attribute to the collection
        /// </summary>
        /// <param name="key">Name of the attribute to add</param>
        /// <param name="value">Value of the attribute to add</param>
        /// <param name="isUser">A Boolean to indicate if this is a user-created attribute, or one from the system</param>
        public void Add(string key, object value, bool isUser)
        {
            _metadata.Remove(key);
            _metadata.Add(key, new TypeDefinitionMetadata() { Key = key, Value = value, IsUserMetaData = isUser});
        }


        /// <summary>
        /// Gets the count of attributes in this collection
        /// </summary>
        public int Count { get { return _metadata.Count; } }

        /// <summary>
        /// Gets an enumerable of the attributes in this collection
        /// </summary>
        public IEnumerable<TypeDefinitionMetadata> Items { get { return _metadata.Values; } }

        /// <summary>
        /// Allows the lookup of an attribute by name
        /// </summary>
        /// <param name="name">Name of the attribute to find</param>
        /// <returns>The <see cref="TypeDefinitionMetadata"/> found</returns>
        public TypeDefinitionMetadata GetByName(string name)
        {
            TypeDefinitionMetadata obj = null;
            _metadata.TryGetValue(name, out obj);
            return obj;
        }
    }
}