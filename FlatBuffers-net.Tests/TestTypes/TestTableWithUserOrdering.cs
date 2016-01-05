using FlatBuffers.Attributes;

namespace FlatBuffers.Tests.TestTypes
{
    public class TestTableWithUserOrdering
    {
        [FlatBuffersField(Order = 2)]
        public int IntProp { get; set; }

        [FlatBuffersField(Order = 0)]
        public byte ByteProp { get; set; }

        [FlatBuffersField(Order = 1)]
        public short ShortProp { get; set; }
    }
}