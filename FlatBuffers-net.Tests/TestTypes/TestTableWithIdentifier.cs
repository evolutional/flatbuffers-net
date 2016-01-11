using FlatBuffers.Attributes;

namespace FlatBuffers.Tests.TestTypes
{
    [FlatBuffersTable(Identifier = "TEST")]
    public class TestTableWithIdentifier
    {
        public int IntProp { get; set; }
    }
}