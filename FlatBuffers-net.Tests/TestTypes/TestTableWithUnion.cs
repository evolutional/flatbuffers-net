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
        [FlatBuffersField(Order = 1)]
        public int IntProp { get; set; }

        [FlatBuffersField(UnionType = typeof(TestUnion), Order = 0)]
        public object UnionProp { get; set; }
    }
}