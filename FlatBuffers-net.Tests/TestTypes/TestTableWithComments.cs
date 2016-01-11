using FlatBuffers.Attributes;

namespace FlatBuffers.Tests.TestTypes
{
    [FlatBuffersComment("This is a comment on a table")]
    public class TestTableWithComments
    {
        [FlatBuffersComment("Comment on an int field")]
        public int Field { get; set; }

        [FlatBuffersComment(0, "First comment of Multiple comments")]
        [FlatBuffersComment(1, "Second comment of Multiple comments")]
        public string StringField { get; set; }

        [FlatBuffersComment("Multiline\nIs supported\ntoo")]
        public int AnotherField { get; set; }
    }
}