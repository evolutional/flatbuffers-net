using FlatBuffers.Attributes;

namespace FlatBuffers.Tests.TestTypes
{
    public class TestTableWithUnion
    {
        public int IntProp { get; set; }

        [FlatBuffersField(UnionType = typeof(TestUnion))]
        public object UnionProp { get; set; }
    }

    public class TestTableWithUnionAndMoreFields
    {
        public int IntProp { get; set; }

        [FlatBuffersField(UnionType = typeof(TestUnion))]
        public object UnionProp { get; set; }

        public string StringProp { get; set; }

        public float FloatProp { get; set; }

        public double DoubleProp { get; set; }
    }

    public class TestTableWithUnionAndCustomOrdering
    {
        [FlatBuffersField(Id = 1)]
        public int IntProp { get; set; }

        [FlatBuffersField(UnionType = typeof(TestUnion), Id = 0)]
        public object UnionProp { get; set; }
    }
}