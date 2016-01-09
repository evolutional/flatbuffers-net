using FlatBuffers.Attributes;

namespace FlatBuffers.Tests.TestTypes
{
    [FlatBuffersUnion]
    public enum TestUnion
    {
        [FlatBuffersUnionMember(typeof(TestTable1))]
        TestTable1,

        [FlatBuffersUnionMember(typeof(TestTable2))]
        TestTable2
    }

    [FlatBuffersMetadata("somemeta")]
    [FlatBuffersUnion]
    public enum TestUnionWithMetadata
    {
        [FlatBuffersUnionMember(typeof(TestTable1))]
        TestTable1,

        [FlatBuffersUnionMember(typeof(TestTable2))]
        TestTable2
    }
}
