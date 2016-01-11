using System;

namespace FlatBuffers
{
    /// <summary>
    /// A class that is used as the base for exceptions raised by FlatBuffers.net
    /// </summary>
    public abstract class FlatBuffersBaseException : Exception
    {
        /// <summary>
        /// Initializes an instance of the FlatBuffersBaseException class
        /// </summary>
        protected FlatBuffersBaseException()
        { }

        /// <summary>
        /// Initializes an instance of the FlatBuffersBaseException class
        /// </summary>
        protected FlatBuffersBaseException(string format, params object[] args)
            : base(string.Format(format, args))
        { }

        /// <summary>
        /// Initializes an instance of the FlatBuffersBaseException class
        /// </summary>
        protected FlatBuffersBaseException(Exception innerException, string format, params object[] args)
            : base(string.Format(format, args), innerException)
        { }
    }
}