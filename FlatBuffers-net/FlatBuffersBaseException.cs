using System;

namespace FlatBuffers
{
    public abstract class FlatBuffersBaseException : Exception
    {
        protected FlatBuffersBaseException()
        { }

        protected FlatBuffersBaseException(string format, params object[] args)
            : base(string.Format(format, args))
        { }

        protected FlatBuffersBaseException(Exception innerException, string format, params object[] args)
            : base(string.Format(format, args), innerException)
        { }
    }
}