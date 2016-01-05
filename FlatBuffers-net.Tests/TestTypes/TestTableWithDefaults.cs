using FlatBuffers.Attributes;

namespace FlatBuffers.Tests.TestTypes
{
    public class TestTableWithDefaults
    {
        [FlatBuffersDefaultValue(123456)]
        public int IntProp { get; set; }

        [FlatBuffersDefaultValue(42)]
        public byte ByteProp { get; set; }

        [FlatBuffersDefaultValue(1024)]
        public short ShortProp { get; set; }
    }
}