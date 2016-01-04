using System;

namespace FlatBuffers.Attributes
{
    /// <summary>
    /// Overrides the field index ordering. If used on one field in a table or struct, all must have it.
    /// </summary>
    [AttributeUsage(AttributeTargets.Field | AttributeTargets.Property)]
    public class FlatBuffersFieldIndexAttribute : Attribute
    {
        public FlatBuffersFieldIndexAttribute(int index)
        {
            Index = index;
        }
        
        public int Index { get; private set; }
    }
}