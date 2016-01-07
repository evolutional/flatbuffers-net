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
                MetaData.Add(StructTypeMetaData.OriginalOrder);
            }
        }

        public int ForceAlignSize
        {
            get { return _forceAlignSize; }
            set { 
                _forceAlignSize = value;
                IsForceAlignSet = true;
                MetaData.Add(StructTypeMetaData.ForceAlign, value);
            }
        }

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

            _fields.Add(field);
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