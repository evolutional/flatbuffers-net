namespace FlatBuffers
{
    /// <summary>
    /// A collection of built-in field meta data attribute names
    /// </summary>
    public static class FieldTypeMetadata
    {
        /// <summary>
        /// The 'id' attribute from the fbs schema
        /// </summary>
        public const string Index = "id";
        /// <summary>
        /// The 'required' attribute from the fbs schema
        /// </summary>
        public const string Required = "required";
        /// <summary>
        /// The 'deprecated' attribute from the fbs schema
        /// </summary>
        public const string Deprecated = "deprecated";
        /// <summary>
        /// The 'key' attribute from the fbs schema
        /// </summary>
        public const string Key = "key";
        /// <summary>
        /// The 'hash' attribute from the fbs schema
        /// </summary>
        public const string Hash = "hash";
        /// <summary>
        /// The 'nested_flatbuffer' attribute from the fbs schema
        /// </summary>
        public const string NestedFlatBuffer = "nested_flatbuffer";
    }
}