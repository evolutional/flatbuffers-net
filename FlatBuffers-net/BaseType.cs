namespace FlatBuffers
{
    /// <summary>
    /// Represents the base type of a serialized object
    /// </summary>
    public enum BaseType
    {
        /// <summary>
        /// No type
        /// </summary>
        None,
        /// <summary>
        /// Field represents a Union Type identifier
        /// </summary>
        UType,
        /// <summary>
        /// Field represents a Boolean
        /// </summary>
        Bool,
        /// <summary>
        /// Field represents an 8-bit signed integer
        /// </summary>
        Char,
        /// <summary>
        /// Field represents an 8-bit unsigned integer
        /// </summary>
        UChar,
        /// <summary>
        /// Field represents a 16-bit signed integer
        /// </summary>
        Short,
        /// <summary>
        /// Field represents a 16-bit unsigned integer
        /// </summary>
        UShort,
        /// <summary>
        /// Field represents a 32-bit signed integer
        /// </summary>
        Int,
        /// <summary>
        /// Field represents a 32-bit unsigned integer
        /// </summary>
        UInt,
        /// <summary>
        /// Field represents a 64-bit signed integer
        /// </summary>
        Long,
        /// <summary>
        /// Field represents a 64-bit unsigned integer
        /// </summary>
        ULong,
        /// <summary>
        /// Field represents a 32-bit floating point value
        /// </summary>
        Float,
        /// <summary>
        /// Field represents a 64-bit floating point value
        /// </summary>
        Double,
        /// <summary>
        /// Field represents a string
        /// </summary>
        String,
        /// <summary>
        /// Field represents a vector
        /// </summary>
        Vector,
        /// <summary>
        /// Field represents a structure (struct or table)
        /// </summary>
        Struct,
        /// <summary>
        /// Field represents a union value
        /// </summary>
        Union,
    }
}