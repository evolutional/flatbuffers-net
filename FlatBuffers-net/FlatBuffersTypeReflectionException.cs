using System;

namespace FlatBuffers
{
    /// <summary>
    /// Exception class for errors raised during reflection of struct/table types
    /// </summary>
    public class FlatBuffersTypeReflectionException : FlatBuffersBaseException
    {
        /// <summary>
        /// Initializes and instance of the <see cref="FlatBuffersTypeReflectionException"/> class
        /// </summary>
        public FlatBuffersTypeReflectionException()
        {
        }

        /// <summary>
        /// Initializes and instance of the <see cref="FlatBuffersTypeReflectionException"/> class
        /// </summary>
        public FlatBuffersTypeReflectionException(string format, params object[] args)
            : base(string.Format(format, args))
        {
        }

        /// <summary>
        /// The <see cref="Type"/> that caused the exception
        /// </summary>
        public Type ClrType { get; internal set; }
    }
}