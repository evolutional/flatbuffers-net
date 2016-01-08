using System;

namespace FlatBuffers
{
    public class TypeModel
    {
        private readonly string _typeName;
        private readonly TypeModelRegistry _registry;
        private readonly Type _clrType;
        private readonly BaseType _baseType;
        private readonly BaseType _elementType;

        private StructTypeDefinition _structDef;
        private EnumTypeDefinition _enumDef;

        public StructTypeDefinition StructDef { get { return _structDef; } set { _structDef = value; } }
        public EnumTypeDefinition EnumDef { get { return _enumDef; } set { _enumDef = value; } }

        public string Name { get { return _typeName; } }

        /// <summary>
        /// Is this object a table or struct type?
        /// </summary>
        public bool IsObject { get { return IsTable || IsStruct; } }

        /// <summary>
        /// Is this object a table type?
        /// </summary>
        public bool IsTable { get { return _baseType == BaseType.Struct && !_structDef.IsFixed; } }

        /// <summary>
        /// Gets if this object a (fixed) struct type
        /// </summary>
        public bool IsStruct { get { return _baseType == BaseType.Struct && _structDef.IsFixed; } }

        /// <summary>
        /// Gets if this type is an enum
        /// </summary>
        public bool IsEnum { get { return typeof (Enum).IsAssignableFrom(_clrType); } }

        /// <summary>
        /// Gets if this type is a vector
        /// </summary>
        public bool IsVector { get { return _baseType == BaseType.Vector; } }

        /// <summary>
        /// Gets if this type is a string
        /// </summary>
        public bool IsString { get { return _baseType == BaseType.String; } }

        /// <summary>
        /// Gets if this type is a reference type
        /// </summary>
        public bool IsReferenceType 
        { 
            get { return IsTable || IsVector || IsString; } 
        }

        public int InlineSize { get { return IsStruct ? _structDef.ByteSize : _baseType.SizeOf(); }}
        public int InlineAlignment { get { return IsStruct ? _structDef.MinAlign : _baseType.SizeOf(); }}

        public BaseType BaseType { get { return _baseType; } }
        public BaseType ElementType { get { return _elementType; } }

        public Type Type
        {
            get { return _clrType; }
        }

        public TypeModelRegistry Registry
        {
            get { return _registry; }
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
