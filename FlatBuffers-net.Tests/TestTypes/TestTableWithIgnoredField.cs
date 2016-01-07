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

    public class TestTableWithDeprecatedField
    {
        public const byte DefaultBytePropValue = 127;

        public TestTableWithDeprecatedField()
        {
            ByteProp = DefaultBytePropValue;
        }

        public int IntProp { get; set; }

        [FlatBuffersField(Deprecated = true)]
        public byte ByteProp { get; set; }
        public short ShortProp { get; set; }
    }
}