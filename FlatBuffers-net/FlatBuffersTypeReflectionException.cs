using System;

namespace FlatBuffers
{
    public class FlatBuffersTypeReflectionException : FlatBuffersBaseException
    {
        public FlatBuffersTypeReflectionException()
        {
        }

        public FlatBuffersTypeReflectionException(string format, params object[] args)
            : base(string.Format(format, args))
        {
        }

        public Type ClrType { get; set; }
    }
}