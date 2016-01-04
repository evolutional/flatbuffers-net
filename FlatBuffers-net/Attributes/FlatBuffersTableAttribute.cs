using System;

namespace FlatBuffers.Attributes
{
    /// <summary>
    /// Attribute to signify that this struct is to be serialized as a table
    /// </summary>
    [AttributeUsage(AttributeTargets.Struct, AllowMultiple = false, Inherited = true)]
    public class FlatBuffersTableAttribute : Attribute
    {
    }
}