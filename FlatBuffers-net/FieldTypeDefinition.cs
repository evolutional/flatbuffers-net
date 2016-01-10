using System.Collections.Generic;
using FlatBuffers.Attributes;

namespace FlatBuffers
{
    internal class IndexBasedFieldTypeDefinitionComparaer : IComparer<FieldTypeDefinition>
    {
        public int Compare(FieldTypeDefinition x, FieldTypeDefinition y)
        {
            if (x.Index == y.Index)
                return 0;
            if (x.Index < y.Index)
                return -1;
            return 1;
        }
    }

    public sealed class FieldTypeDefinition : TypeDefinition
    {
        private int _index;
        private bool _required;
        private bool _deprecated;
        private bool _isKey;
        private FlatBuffersHash _hash;

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

        public bool IsIndexSetExplicitly { get; private set; }

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

        public int Padding { get; set; }

        public TypeModel TypeModel { get; set; }

        public int Offset { get; set; }

        public IValueProvider ValueProvider { get; private set; }
        public IDefaultValueProvider DefaultValueProvider { get; private set; }

        public FieldTypeDefinition UnionTypeField { get; set; }

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
    }
}