using System;
using System.Collections.Generic;
using System.Linq;

namespace FlatBuffers
{
    public class StructTypeDefinition : TypeDefinition
    {
        private int _forceAlignSize;
        private bool _originalOrdering;

        private readonly List<FieldTypeDefinition> _fields = new List<FieldTypeDefinition>();

        public IEnumerable<FieldTypeDefinition> Fields { get { return _fields; } }

        public int FieldCount { get { return _fields.Count; } }

        public StructTypeDefinition(bool isFixed)
        {
            IsFixed = isFixed;
            MinAlign = 1;
            ByteSize = 0;
        }

        public bool HasCustomOrdering { get; internal set; }

        // True if it's a struct; false for Table
        public bool IsFixed { get; private set; }
        public int ByteSize { get; private set; }
        public int MinAlign { get; set; }

        public bool IsForceAlignSet { get; internal set; }

        public bool UseOriginalOrdering
        {
            get { return _originalOrdering; }
            set
            {
                _originalOrdering = value;
                Metadata.Add(StructTypeMetadata.OriginalOrder, false);
            }
        }

        public int ForceAlignSize
        {
            get { return _forceAlignSize; }
            set { 
                _forceAlignSize = value;
                IsForceAlignSet = true;
                Metadata.Add(StructTypeMetadata.ForceAlign, value, false);
            }
        }

        /// <summary>
        /// Gets whether the struct has a key field present
        /// </summary>
        public bool HasKey { get; private set; }

        private static int CalcPadding(int bufSize, int valueSize)
        {
            return ((~bufSize) + 1) & (valueSize - 1);
        }

        public void PadLastField(int alignment)
        {
            var padding = CalcPadding(ByteSize, alignment);
            ByteSize += padding;
            if (_fields.Count > 0)
            {
                _fields.Last().Padding = padding;
            }
        }

        public void AddField(FieldTypeDefinition field)
        {
            field.OriginalIndex = _fields.Count;
            var typeModel = field.TypeModel;
            if (IsFixed)
            {
                var size = typeModel.InlineSize;
                var alignment = typeModel.InlineAlignment;

                if (typeModel.IsStruct)
                {
                    // We're adding an inline struct
                    var structDef = typeModel.StructDef;
                    size = structDef.ByteSize;
                }

                // align
                MinAlign = Math.Max(MinAlign, alignment);
                PadLastField(alignment);
                field.Offset = ByteSize;
                ByteSize += size;
            }
            else
            {
                field.Offset = FieldIndexToOffset(field.Index);
            }

            if (field.Key && HasKey)
            {
                throw new FlatBuffersStructFieldReflectionException("Cannot add '{0}' as a key field, key already exists", field.Name);
            }
            if (field.Key)
            {
                HasKey = true;
            }

            _fields.Add(field);
        }

        internal void FinalizeFieldDefinition()
        {
            // Recalc offsets if using custom ordering
            if (HasCustomOrdering)
            {
                _fields.Sort(new IndexBasedFieldTypeDefinitionComparaer());

                for (var i = 0; i < _fields.Count; ++i)
                {
                    if (!_fields[i].IsIndexSetExplicitly)
                    {
                        throw new FlatBuffersStructFieldReflectionException("Id must be set on all fields");
                    }

                    if (i != _fields[i].Index)
                    {
                        if (_fields[i].Index != i)
                        {
                            throw new FlatBuffersStructFieldReflectionException("Id range must be contiguous sequence from 0..N");
                        }
                    }

                    _fields[i].Offset = FieldIndexToOffset(i);
                }
            }
        }

        public FieldTypeDefinition GetFieldByName(string name)
        {
            return Fields.FirstOrDefault(i => i.Name == name);
        }

        private int FieldIndexToOffset(int fieldIndex)
        {
            const int staticFieldCount = 2; // vtable size + obj size
            return (fieldIndex + staticFieldCount)*sizeof (short);
        }
    }
}