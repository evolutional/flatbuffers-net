using System;

namespace FlatBuffers.Attributes
{
    /// <summary>
    /// Attribute to declare an enum as a Union. Members must all have the FlatBuffersUnionMember attribute.
    /// </summary>
    [AttributeUsage(AttributeTargets.Enum, AllowMultiple = false, Inherited = true)]
    public class FlatBuffersUnionAttribute : Attribute
    {
    }
}