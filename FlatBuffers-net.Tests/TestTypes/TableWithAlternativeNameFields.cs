using FlatBuffers.Attributes;

namespace FlatBuffers.Tests.TestTypes
{
    public class TableWithAlternativeNameFields
    {
        [FlatBuffersField(Name = "AltIntProp")]
        public int IntProp { get; set; }

        [FlatBuffersField(Name = "AltStringProp")]
        public string StringProp { get; set; } 
    }
}