using System;
using FlatBuffers.Utilities;

namespace FlatBuffers
{
    /// <summary>
    /// A class which represents reflected and FlatBuffers-specific information about a given <see cref="Type"/>
    /// </summary>
    public class TypeModel
    {
        private readonly string _typeName;
        private readonly TypeModelRegistry _registry;
        private readonly Type _clrType;
        private readonly BaseType _baseType;
        private readonly BaseType _elementType;
        private readonly bool _isEnum;

        private StructTypeDefinition _structDef;
        private EnumTypeDefinition _enumDef;
        private UnionTypeDefinition _unionDef;

        /// <summary>
        /// Gets the <see cref="StructTypeDefinition"/> if this type is a table/struct
        /// </summary>
        public StructTypeDefinition StructDef { get { return _structDef; } internal set { _structDef = value; } }
        /// <summary>
        /// Gets the <see cref="EnumTypeDefinition"/> if this type is an enum
        /// </summary>
        public EnumTypeDefinition EnumDef { get { return _enumDef; } internal set { _enumDef = value; } }

        /// <summary>
        /// Gets the <see cref="UnionTypeDefinition"/> if this type is a union
        /// </summary>
        public UnionTypeDefinition UnionDef { get { return _unionDef; } internal set { _unionDef = value; } }

        /// <summary>
        /// Gets the name of this type
        /// </summary>
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
        public bool IsEnum { get { return _isEnum; } }

        /// <summary>
        /// Gets if this type is a union
        /// </summary>
        public bool IsUnion { get { return _baseType == BaseType.Union; } }

        /// <summary>
        /// Gets if this type is a vector
        /// </summary>
        public bool IsVector { get { return _baseType == BaseType.Vector; } }

        /// <summary>
        /// Gets if this type is a string
        /// </summary>
        public bool IsString { get { return _baseType == BaseType.String; } }

        /// <summary>
        /// Gets if this type is a reference type. Reference types are not serialized inline
        /// </summary>
        public bool IsReferenceType 
        { 
            get { return IsTable || IsVector || IsString || IsUnion; } 
        }

        /// <summary>
        /// Gets the size of the type when serialized
        /// </summary>
        public int InlineSize { get { return IsStruct ? _structDef.ByteSize : _baseType.SizeOf(); }}
        /// <summary>
        /// Gets the natural alignment size of the type
        /// </summary>
        public int InlineAlignment { get { return IsStruct ? _structDef.MinAlign : _baseType.SizeOf(); }}

        /// <summary>
        /// Gets the FlatBuffers <see cref="BaseType"/> the type resolves to
        /// </summary>
        public BaseType BaseType { get { return _baseType; } }
        /// <summary>
        /// Gets the FlatBuffers <see cref="BaseType"/> of the elements contained by this type
        /// </summary>
        public BaseType ElementType { get { return _elementType; } }
        
        /// <summary>
        /// Gets the CLR <see cref="Type"/>
        /// </summary>
        public Type Type
        {
            get { return _clrType; }
        }

        /// <summary>
        /// Gets the <see cref="TypeModelRegistry"/> that this model is part of
        /// </summary>
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
            _isEnum = typeof(Enum).IsAssignableFrom(_clrType) && _baseType != BaseType.Union;
        }

        /// <summary>
        /// Returns the <see cref="TypeModel"/> for this models ElementType
        /// </summary>
        /// <returns>Returns the ElementType TypeModel</returns>
        public TypeModel GetElementTypeModel()
        {
            var elementType = _clrType.GetElementType();

            if (elementType != null)
            {
                return _registry.GetTypeModel(elementType);
            }
            else if (_clrType.IsGenericType)
            {
                var a = _clrType.GetGenericArguments();

                if (a.Length > 0)
                {
                    return _registry.GetTypeModel(_clrType.GetGenericArguments()[0]);
                }
            }

            return null;
        }
    }
}
