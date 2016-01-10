using System;

namespace FlatBuffers.Attributes
{
    /// <summary>
    /// Instructs the reflection systems to ignore this type. If applied to a field, it will be skipped.
    /// </summary>
    [AttributeUsage(AttributeTargets.Field | AttributeTargets.Property | AttributeTargets.Class | AttributeTargets.Struct | AttributeTargets.Enum)]
    public class FlatBuffersIgnoreAttribute : Attribute
    {
    }
}