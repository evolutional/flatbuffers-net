using System;
using System.Collections.Generic;

namespace FlatBuffers
{
    /// <summary>
    /// Information about a field in a given <see cref="UnionTypeDefinition"/>
    /// </summary>
    public class UnionFieldTypeDefinition : TypeDefinition
    {
        /// <summary>
        /// The index this field is at
        /// </summary>
        public int Index { get; internal set; }
        /// <summary>
        /// The <see cref="TypeModel"/> that this field should resolve to
        /// </summary>
        public TypeModel MemberType { get; internal set; }
    }

    /// <summary>
    /// Provides additional information about the reflected union
    /// </summary>
    public class UnionTypeDefinition : TypeDefinition
    {
        private readonly List<UnionFieldTypeDefinition> _fields = new List<UnionFieldTypeDefinition>();

        internal UnionTypeDefinition()
        {
            AddField(0, "Original", null);
        }

        /// <summary>
        /// Gets an enumerable of the fields on this Union
        /// </summary>
        public IEnumerable<UnionFieldTypeDefinition> Fields { get { return _fields; } }

        internal UnionFieldTypeDefinition AddField(int index, string name, TypeModel memberType)
        {
            var field = new  UnionFieldTypeDefinition {Name = name, Index = index, MemberType = memberType};
            _fields.Add(field);
            return field;
        }
    }
}