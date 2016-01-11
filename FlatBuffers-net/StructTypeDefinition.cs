using System;
using System.Collections.Generic;
using System.Linq;

namespace FlatBuffers
{
    /// <summary>
    /// A class that represents the reflected information about a FlatBuffers struct or table
    /// </summary>
    public class StructTypeDefinition : TypeDefinition
    {
        private string _identifier;
        private int _forceAlignSize;
        private bool _originalOrdering;

        private readonly List<FieldTypeDefinition> _fields = new List<FieldTypeDefinition>();

        /// <summary>
        /// Gets an enumerable of the fields on the type
        /// </summary>
        public IEnumerable<FieldTypeDefinition> Fields { get { return _fields; } }

        /// <summary>
        /// Gets the count of fields on the type
        /// </summary>
        public int FieldCount { get { return _fields.Count; } }
        
        internal StructTypeDefinition(bool isFixed)
        {
            IsFixed = isFixed;
            MinAlign = 1;
            ByteSize = 0;
        }

        /// <summary>
        /// Gets a Boolean to indicate if this type has custom field ordering
        /// </summary>
        public bool HasCustomOrdering { get; internal set; }

        
        /// <summary>
        /// Gets a Boolean to indicate if this type is of a fixed size. This is true for FlatBuffers structs.
        /// </summary>
        public bool IsFixed { get; private set; }

        /// <summary>
        /// Gets the size in bytes of this type
        /// </summary>
        public int ByteSize { get; private set; }
        /// <summary>
        /// Gets the minimum alignment required when serializing this type
        /// </summary>
        public int MinAlign { get; internal set; }

        /// <summary>
        /// Gets a Boolean to indicate if this type has been forced to align to a the size in <see cref="ForceAlignSize"/>
        /// </summary>
        public bool IsForceAlignSet { get; internal set; }

        /// <summary>
        /// Gets a Boolean to indicate if the fields on this table is ordered using its reflected order
        /// </summary>
        public bool UseOriginalOrdering
        {
            get { return _originalOrdering; }
            internal set
            {
                _originalOrdering = value;
                if (value)
                    Metadata.Add(StructTypeMetadata.OriginalOrder, false);
                else
                    Metadata.Remove(StructTypeMetadata.OriginalOrder);
            }
        }

        /// <summary>
        /// Gets the byte alignment that this struct has been forced to use
        /// </summary>
        public int ForceAlignSize
        {
            get { return _forceAlignSize; }
            internal set { 
                _forceAlignSize = value;
                IsForceAlignSet = true;
                Metadata.Add(StructTypeMetadata.ForceAlign, value, false);
            }
        }

        /// <summary>
        /// Gets whether the struct has a key field present
        /// </summary>
        public bool HasKey { get; private set; }


        /// <summary>
        /// Gets whether the struct has a buffer identifier present
        /// </summary>
        public bool HasIdentifier { get; private set; }

        /// <summary>
        /// Gets the identifier used by the table when used as a root type
        /// </summary>
        public string Identifier
        {
            get { return _identifier; }
            internal set
            {
                if (IsFixed)
                {
                    throw new ArgumentException();
                }

                _identifier = value;
                HasIdentifier = true;
            }
        }

        private static int CalcPadding(int bufSize, int valueSize)
        {
            return ((~bufSize) + 1) & (valueSize - 1);
        }

        internal void PadLastField(int alignment)
        {
            var padding = CalcPadding(ByteSize, alignment);
            ByteSize += padding;
            if (_fields.Count > 0)
            {
                _fields.Last().Padding = padding;
            }
        }

        /// <summary>
        /// Adds a field to the struct, recalculating the valious sizing, alignment and padding values.
        /// </summary>
        /// <param name="field"></param>
        /// <exception cref="FlatBuffersStructFieldReflectionException"></exception>
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

        /// <summary>
        /// Allows lookup of a field by name
        /// </summary>
        /// <param name="name">The name of the field to find</param>
        /// <returns>The field that was found, or null</returns>
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