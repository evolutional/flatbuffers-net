using FlatBuffers.Attributes;

namespace FlatBuffers.Tests.TestTypes
{
    [FlatBuffersMetadata("types")]
    public class TableWithUserMetadata
    {
        [FlatBuffersMetadata("priority", 1)]
        public int PropA { get; set; }

        [FlatBuffersMetadata("toggle", true)]
        public bool PropB { get; set; }

        [FlatBuffersMetadata("category", "tests")]
        public int PropC { get; set; }
    }
}