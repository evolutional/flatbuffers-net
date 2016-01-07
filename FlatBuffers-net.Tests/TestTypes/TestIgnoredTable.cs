using FlatBuffers.Attributes;

namespace FlatBuffers.Tests.TestTypes
{
    [FlatBuffersIgnore]
    public class TestIgnoredTable
    {
        public int IntProp { get; set; }
        
        public byte ByteProp { get; set; }
        public short ShortProp { get; set; }
    }
}