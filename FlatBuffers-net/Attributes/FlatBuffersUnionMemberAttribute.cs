using System;

namespace FlatBuffers.Attributes
{
    /// <summary>
    /// Attribute to bind a member of an Enum with the FlatBuffersUnionAttribute with a .NET <see cref="Type"/>
    /// </summary>
    [AttributeUsage(AttributeTargets.Field, AllowMultiple = false, Inherited = true)]
    public class FlatBuffersUnionMemberAttribute : Attribute
    {
        /// <summary>
        /// Gets the .NET Type that this member should be serialized as
        /// </summary>
        public Type MemberType { get; private set; }

        /// <summary>
        /// Initializes and instance of the <see cref="FlatBuffersUnionMemberAttribute"/> class
        /// </summary>
        /// <param name="memberType">The CLR <see cref="Type"/> that this union memeber resolves to</param>
        public FlatBuffersUnionMemberAttribute(Type memberType)
        {
            MemberType = memberType;
        }
    }
}