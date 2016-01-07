using FlatBuffers.Attributes;

namespace FlatBuffers.Tests.TestTypes
{
    [FlatBuffersTable(OriginalOrdering = true)]
    public class TestTableWithOriginalOrdering
    {
        public int IntProp { get; set; }

        public byte ByteProp { get; set; }

        public short ShortProp { get; set; }
    }
}