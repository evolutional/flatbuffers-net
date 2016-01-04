using System;

namespace FlatBuffers.Attributes
{
    /// <summary>
    /// Attribute to signify that this class is to be serialized as a struct (inline)
    /// </summary>
    [AttributeUsage(AttributeTargets.Class, AllowMultiple = false, Inherited = true)]
    public class FlatBuffersStructAttribute : Attribute
    {
    }
}