using FlatBuffers.Attributes;

namespace FlatBuffers.Tests.TestTypes
{
    public class TestTableWithIgnoredField
    {
        public int IntProp { get; set; }

        [FlatBuffersIgnore]
        public byte ByteProp { get; set; }
        public short ShortProp { get; set; }
    }
}