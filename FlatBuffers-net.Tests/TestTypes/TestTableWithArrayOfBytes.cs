using System.Collections.Generic;

namespace FlatBuffers.Tests.TestTypes
{
    public class TestTableWithArrayOfBytes
    {
        public byte[] ByteArrayProp { get; set; }
        public List<byte> ByteListProp { get; set; }
    }
}