using FlatBuffers.Attributes;

namespace FlatBuffers.Tests.TestTypes
{
    public class TestTableWithNestedTestTable1
    {
        public int IntProp { get; set; }

        [FlatBuffersField(NestedFlatBufferType = typeof(TestTable1))]
        public object Nested { get; set; }
    }
}