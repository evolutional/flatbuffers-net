namespace FlatBuffers
{
    /// <summary>
    /// Exception thrown during the schema creation or writing process
    /// </summary>
    public class FlatBuffersSchemaException : FlatBuffersBaseException
    {
        /// <summary>
        /// Initializes an instance of the <see cref="FlatBuffersSchemaException"/> class
        /// </summary>
        public FlatBuffersSchemaException()
        {
        }

        /// <summary>
        /// Initializes an instance of the <see cref="FlatBuffersSchemaException"/> class
        /// </summary>
        public FlatBuffersSchemaException(string format, params object[] args)
            : base(string.Format(format, args))
        {
        }
    }
}