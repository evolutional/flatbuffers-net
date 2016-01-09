using System;

namespace FlatBuffers.Attributes
{
    /// <summary>
    /// Attribute to bind a member of an Enum with the FlatBuffersUnionAttribute with a .NET type
    /// </summary>
    [AttributeUsage(AttributeTargets.Field, AllowMultiple = false, Inherited = true)]
    public class FlatBuffersUnionMemberAttribute : Attribute
    {
        public Type MemberType { get; private set; }

        public FlatBuffersUnionMemberAttribute(Type memberType)
        {
            MemberType = memberType;
        }
    }
}