using System;
using System.Collections.Generic;

namespace FlatBuffers
{
    public class UnionFieldTypeDefinition : TypeDefinition
    {
        public int Index { get; internal set; }
        public TypeModel MemberType { get; internal set; }
    }

    /// <summary>
    /// Provides additional information about the reflected union
    /// </summary>
    public class UnionTypeDefinition : TypeDefinition
    {
        private readonly List<UnionFieldTypeDefinition> _fields = new List<UnionFieldTypeDefinition>();

        public UnionTypeDefinition()
        {
            AddField(0, "None", null);
        }

        public IEnumerable<UnionFieldTypeDefinition> Fields { get { return _fields; } }

        public UnionFieldTypeDefinition AddField(int index, string name, TypeModel memberType)
        {
            var field = new  UnionFieldTypeDefinition {Name = name, Index = index, MemberType = memberType};
            _fields.Add(field);
            return field;
        }
    }
}