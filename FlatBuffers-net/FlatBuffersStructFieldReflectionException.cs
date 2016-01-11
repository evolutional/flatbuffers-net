using System.Reflection;

namespace FlatBuffers
{
    /// <summary>
    /// Exception class for errors raised during reflection of struct/table type fields
    /// </summary>
    public class FlatBuffersStructFieldReflectionException : FlatBuffersTypeReflectionException
    {
        /// <summary>
        /// The <see cref="MemberInfo"/> that has the error
        /// </summary>
        public MemberInfo Member { get; internal set; }

        /// <summary>
        /// Initializes an instance of the <see cref="FlatBuffersStructFieldReflectionException"/> class
        /// </summary>
        public FlatBuffersStructFieldReflectionException()
        {
        }

        /// <summary>
        /// Initializes an instance of the <see cref="FlatBuffersStructFieldReflectionException"/> class
        /// </summary>
        public FlatBuffersStructFieldReflectionException(string format, params object[] args) : 
            base(format, args)
        {
        }
    }
}