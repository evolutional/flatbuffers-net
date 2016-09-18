using System.Collections.Generic;

namespace FlatBuffers.Tests.TestTypes
{
    public class TestTableWithArrayOfTables
    {
        public TestTable1[] TableArrayProp { get; set; }
        public List<TestTable1> TableListProp { get; set; }
    }
}