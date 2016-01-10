using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using FlatBuffers.Attributes;

namespace FlatBuffers.Tests.TestTypes
{
    public class TestTableWithKey
    {
        [FlatBuffersField(Key = true)]
        public int IntProp { get; set; }
        public int OtherProp { get; set; }
    }

    /// <summary>
    /// Example of a class that will throw an error on reflection - it has 2 key fields
    /// </summary>
    public class TestTableWith2Keys
    {
        [FlatBuffersField(Key = true)]
        public int IntProp { get; set; }

        [FlatBuffersField(Key = true)]
        public int OtherProp { get; set; }
    }

    public class TestTableWithKeyOnBadType
    {
        [FlatBuffersField(Key = true)]
        public TestTable1 OtherProp { get; set; }
    }
}
