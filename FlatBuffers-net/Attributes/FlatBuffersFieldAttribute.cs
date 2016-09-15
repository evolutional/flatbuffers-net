using System;
using FlatBuffers.Utilities;

namespace FlatBuffers.Attributes
{
    /// <summary>
    /// An optional attribute to override FlatBuffers-specific metadata on a field or property
    /// </summary>
    [AttributeUsage(AttributeTargets.Field | AttributeTargets.Property)]
    public class FlatBuffersFieldAttribute : Attribute
    {
        private bool _hasIdSet;
        private int _id = -1;
        private Type _unionType;

        /// <summary>
        /// Gets and sets an alternative field name to use in fbs schema.
        /// This attribute is only for fbs schema compatibility and isn't used during serialization.
        /// </summary>
        public string Name { get; set; }

        /// <summary>
        /// Gets and sets if this field is a key on the table. Can only be applied to scalars and strings.
        /// This attribute is only for fbs schema compatibility and isn't used during serialization.
        /// </summary>
        public bool Key { get; set; }

        /// <summary>
        /// Gets and sets the hashing algorithm for this field. Can only be applied fields of type int/uint/long/ulong.
        /// This attribute is only for fbs schema compatibility and isn't used during serialization.
        /// </summary>
        public FlatBuffersHash Hash { get; set; }

        /// <summary>
        /// Gets and sets the order of serialization and deserialization of the member
        /// </summary>
        public int Id
        {
            get { return _id; }
            set { _hasIdSet = true;
                _id = value;
            }
        }

        /// <summary>
        /// Gets if the Id value has been set explicitly
        /// </summary>
        public bool IsIdSetExplicitly { get { return _hasIdSet; } }

        /// <summary>
        /// Gets and sets if the field is required to be set during serialization
        /// </summary>
        public bool Required { get; set; }

        /// <summary>
        /// Gets and sets if this field is deprecated. It will still exist in the schema but will
        /// be skipped by serialization
        /// </summary>
        public bool Deprecated { get; set; }

        /// <summary>
        /// Gets and sets the Union type this field will serialize as.  Provided type must have a FlatBuffersUnionAttribute set.
        /// The field/property type must be object.
        /// </summary>
        public Type UnionType
        {
            get { return _unionType; }
            set
            {
                if (!value.Defined<FlatBuffersUnionAttribute>())
                {
                    throw new ArgumentException();
                }
                _unionType = value;
            }
        }

        /// <summary>
        /// Gets if this field holds a union type.
        /// </summary>
        public bool IsUnionField { get { return _unionType != null; } }

        /// <summary>
        /// Gets and sets the Type of the nested flatbuffer. Must be applied to a member of type <see cref="object"/>
        /// </summary>
        public Type NestedFlatBufferType { get; set; }

        /// <summary>
        /// Gets a Boolean to indicate whether this field has a nested flatbuffer
        /// </summary>
        public bool HasNestedFlatBufferType { get { return NestedFlatBufferType != null; }}
    }
}
