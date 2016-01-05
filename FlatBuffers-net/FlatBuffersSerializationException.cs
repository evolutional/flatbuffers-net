namespace FlatBuffers
{
    public class FlatBuffersSerializationException : FlatBuffersBaseException
    {
        public FlatBuffersSerializationException()
        {
        }

        public FlatBuffersSerializationException(string format, params object[] args)
            : base(string.Format(format, args))
        {
        }
    }
}