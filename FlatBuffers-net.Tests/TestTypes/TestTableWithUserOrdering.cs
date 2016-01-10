using FlatBuffers.Attributes;

namespace FlatBuffers.Tests.TestTypes
{
    public class TestTableWithUserOrdering
    {
        [FlatBuffersField(Id = 2)]
        public int IntProp { get; set; }

        [FlatBuffersField(Id = 0)]
        public byte ByteProp { get; set; }

        [FlatBuffersField(Id = 1)]
        public short ShortProp { get; set; }
    }
}