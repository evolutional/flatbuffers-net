using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using FlatBuffers.Attributes;

namespace FlatBuffers.Tests.TestTypes
{
    [FlatBuffersStruct(ForceAlign = 16)]
    public struct TestStructWithForcedAlignment
    {
        public float X { get; set; }
        public float Y { get; set; }
        public float Z { get; set; }
    }
}
