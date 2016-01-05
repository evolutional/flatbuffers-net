using System.Collections.Generic;
using FlatBuffers.Attributes;

namespace FlatBuffers.Tests.TestTypes
{
    public class TableWithRequiredFields
    {
        [FlatBuffersField(Required = true)]
        public string StringProp { get; set; }

        [FlatBuffersField(Required = true)]
        public TestTable1 TableProp { get; set; }

        [FlatBuffersField(Required = true)]
        public List<int> VectorProp { get; set; }
    }
}