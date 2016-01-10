using FlatBuffers.Attributes;

namespace FlatBuffers.Tests.TestTypes
{
    public class TestTableWithHash
    {
        [FlatBuffersField(Hash = FlatBuffersHash.Fnv1_32)]
        public int IntProp { get; set; }
        public int OtherProp { get; set; }
    }

    /// <summary>
    /// Example of a bad 'hash' attribute
    /// </summary>
    public class TestTableWithHashOnNonIntType
    {
        public int IntProp { get; set; }

        [FlatBuffersField(Hash = FlatBuffersHash.Fnv1_32)]
        public string OtherProp { get; set; }
    }
}