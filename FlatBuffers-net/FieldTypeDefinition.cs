using System;
using FlatBuffers.Attributes;

namespace FlatBuffers
{
    /// <summary>
    /// Class that represents reflected information about a FlatBuffers struct/table field
    /// </summary>
    public sealed class FieldTypeDefinition : TypeDefinition
    {
        private int _index;
        private bool _required;
        private bool _deprecated;
        private bool _isKey;
        private FlatBuffersHash _hash;
        private TypeModel _nestedFlatBufferType;

        /// <summary>
        /// Initializes an instance of the FieldTypeDefinition class
        /// </summary>
        /// <param name="valueProvider">The value provider to use</param>
        /// <param name="defaultValueProvider">The devault value provider to use</param>
        public FieldTypeDefinition(IValueProvider valueProvider, IDefaultValueProvider defaultValueProvider)
        {
            ValueProvider = valueProvider;
            DefaultValueProvider = defaultValueProvider;
        }

        

        /// <summary>
        /// Gets and sets the index (order) of this field. If not set explicitly, the
        /// field will use its default reflected order
        /// </summary>
        public int Index
        {
            get
            {
                if (!IsIndexSetExplicitly)
                {
                    return OriginalIndex;
                }
                return UserIndex;
            }
        }

        /// <summary>
        /// Gets a Boolean to indicate if the index has been explictly set by the user
        /// </summary>
        public bool IsIndexSetExplicitly { get; private set; }

        /// <summary>
        /// Gets and sets the user-specified field ordering index
        /// </summary>
        public int UserIndex
        {
            get
            {
                return _index;
            }
            set
            {
                _index = value;
                IsIndexSetExplicitly = true;
                Metadata.Add(FieldTypeMetadata.Index, _index, false);
            }
        }

        /// <summary>
        /// Gets the original (reflected) index of this field
        /// </summary>
        public int OriginalIndex { get; internal set; }

        /// <summary>
        /// Gets and sets a Boolean to indicate if this field is a 'key' field. Only used by the schema writer to emit the attribute.
        /// </summary>
        public bool Key
        {
            get
            {
                return _isKey;
            }
            set
            {
                _isKey = value;
                if (_isKey)
                {
                    Metadata.Add(FieldTypeMetadata.Key, false);
                }
                else
                {
                    Metadata.Remove(FieldTypeMetadata.Key);
                }
            }
        }

        /// <summary>
        /// Gets and sets the hasing algorithm that was used to determine the value for this field. Only used by the schema writer to emit the attribute.
        /// </summary>
        public FlatBuffersHash Hash
        {
            get
            {
                return _hash;
            }
            set
            {
                _hash = value;
                if (_hash != FlatBuffersHash.None)
                {
                    Metadata.Add(FieldTypeMetadata.Hash, _hash.HashName(), false);
                }
                else
                {
                    Metadata.Remove(FieldTypeMetadata.Hash);
                }
            }
        }

        /// <summary>
        /// Gets the amount of padding (in bytes) that will preceed this field when serialized
        /// </summary>
        public int Padding { get; internal set; }

        /// <summary>
        /// Gets the TypeModel for the value type of this field
        /// </summary>
        public TypeModel TypeModel { get; internal set; }

        /// <summary>
        /// Gets the offset into the flatbuffer this field is written to
        /// </summary>
        public int Offset { get; internal set; }

        /// <summary>
        /// Gets the interface that allows access to the value for this field during serialization
        /// </summary>
        public IValueProvider ValueProvider { get; private set; }
        
        /// <summary>
        /// Gets the interface that allows access to the default value for this field during serialization
        /// </summary>
        public IDefaultValueProvider DefaultValueProvider { get; private set; }

        /// <summary>
        /// Gets the paired field that holds the 'type' info when this field is a union value.
        /// </summary>
        public FieldTypeDefinition UnionTypeField { get; internal set; }

        /// <summary>
        /// Gets and sets whether this field is required to be set during serialization
        /// </summary>
        public bool Required
        {
            get { return _required; }
            set
            {
                _required = value;
                if (_required)
                {
                   Metadata.Add(FieldTypeMetadata.Required, false);
                }
                else
                {
                    Metadata.Remove(FieldTypeMetadata.Required);
                }
            }
            
        }

        /// <summary>
        /// Gets and sets whether this field is deprecated and will be skipped by the serialization process
        /// </summary>
        public bool Deprecated
        {
            get { return _deprecated; }
            set
            {
                _deprecated = value;
                if (_deprecated)
                {
                    Metadata.Add(FieldTypeMetadata.Deprecated, false);
                }
                else
                {
                    Metadata.Remove(FieldTypeMetadata.Deprecated);
                }
            }
        }

        /// <summary>
        /// Gets the <see cref="TypeModel"/> for the nested FlatBuffer
        /// </summary>
        public TypeModel NestedFlatBufferType
        {
            get { return _nestedFlatBufferType; }
            internal set
            {
                if (ValueProvider.ValueType != typeof (object))
                {
                    throw new FlatBuffersStructFieldReflectionException("Cannot apply NestedFlatBufferType to a field that isn't 'object' type");
                }
                _nestedFlatBufferType = value;
                if (_nestedFlatBufferType != null)
                {
                    Metadata.Add(FieldTypeMetadata.NestedFlatBuffer, _nestedFlatBufferType.Name, false);
                }
                else
                {
                    Metadata.Remove(FieldTypeMetadata.NestedFlatBuffer);
                }
            }
        }

        /// <summary>
        /// Gets a Boolean to indicate if this field has a nested flatbuffer
        /// </summary>
        public bool HasNestedFlatBufferType { get { return _nestedFlatBufferType != null; } }
    }
}