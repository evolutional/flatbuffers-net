using System;

namespace FlatBuffers.Serialization.Attributes
{
    /// <summary>
    /// Instructs the reflection systems to ignore this type
    /// </summary>
    [AttributeUsage(AttributeTargets.Field | AttributeTargets.Property | AttributeTargets.Class | AttributeTargets.Struct)]
    public class FlatBuffersIgnoreAttribute : Attribute
    {
    }
}