using System.Collections.Generic;

namespace FlatBuffers
{
    /// <summary>
    /// An abstract class that represents the base of any reflected type or field
    /// </summary>
    public abstract class TypeDefinition
    {
        private readonly TypeDefinitionMetadataCollection _metadata = new TypeDefinitionMetadataCollection();
        private readonly List<string> _comments = new List<string>(); 

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

        /// <summary>
        /// Gets and IEnumerable of strings containing the comments for the type
        /// </summary>
        public IEnumerable<string> Comments {  get { return _comments; }}

        /// <summary>
        /// Gets a Boolean to indicate if this type has comments
        /// </summary>
        public bool HasComments { get { return _comments.Count > 0; } }

        internal void AddComment(string comment)
        {
            _comments.Add(comment);
        }
    }
}