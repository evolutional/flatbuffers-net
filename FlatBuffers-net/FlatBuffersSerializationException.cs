namespace FlatBuffers
{
    /// <summary>
    /// Exception thrown during the Serialization or Deserialization process
    /// </summary>
    public class FlatBuffersSerializationException : FlatBuffersBaseException
    {
        /// <summary>
        /// Initializes an instance of the <see cref="FlatBuffersSerializationException"/> class
        /// </summary>
        public FlatBuffersSerializationException()
        {
        }

        /// <summary>
        /// Initializes an instance of the <see cref="FlatBuffersSerializationException"/> class
        /// </summary>
        public FlatBuffersSerializationException(string format, params object[] args)
            : base(string.Format(format, args))
        {
        }
    }
}