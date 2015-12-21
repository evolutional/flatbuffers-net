using System;
using System.Collections;
using System.Text;

namespace FlatBuffers.Serialization
{
    public class TypeModel
    {
        private readonly string _typeName;
        private readonly TypeModelRegistry _registry;
        private readonly Type _clrType;
        private readonly BaseType _baseType;
        private readonly BaseType _elementType;

        private StructTypeDefinition _structDef;

        public StructTypeDefinition StructDef { get { return _structDef; } set { _structDef = value; } }

        public string Name { get { return _typeName; } }

        public bool IsTable { get { return _baseType == BaseType.Struct && !_structDef.IsFixed; } }
        public bool IsStruct { get { return _baseType == BaseType.Struct && _structDef.IsFixed; } }

        public int InlineSize { get { return IsStruct ? _structDef.ByteSize : _baseType.SizeOf(); }}
        public int InlineAlignment { get { return IsStruct ? _structDef.MinAlign : _baseType.SizeOf(); }}

        public BaseType BaseType { get { return _baseType; } }
        public BaseType ElementType { get { return _elementType; } }

        public Type Type
        {
            get { return _clrType; }
        }

        internal TypeModel(TypeModelRegistry registry, string typeName, Type clrType, BaseType baseType, BaseType elementType = BaseType.None)
        {
            _registry = registry;
            _typeName = typeName;
            _clrType = clrType;
            _baseType = baseType;
            _elementType = elementType;
        }
    }
}
