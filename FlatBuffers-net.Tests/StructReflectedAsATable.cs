using FlatBuffers.Attributes;

namespace FlatBuffers.Tests
{
    [FlatBuffersTable]
    public struct StructReflectedAsATable
    {
        public int A { get; set; }
        public byte B { get; set; }
    }
}