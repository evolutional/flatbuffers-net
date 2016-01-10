using FlatBuffers.Attributes;

namespace FlatBuffers.Tests.TestTypes
{
    [FlatBuffersStruct]
    public class ClassReflectedAsAStruct
    {
        public int A { get; set; }
        public byte B { get; set; }
    }
}