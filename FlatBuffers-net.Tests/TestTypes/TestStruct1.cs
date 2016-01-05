using System;

namespace FlatBuffers.Tests.TestTypes
{
    public struct TestStruct1 : IEquatable<TestStruct1>
    {
        public int IntProp { get; set; }
        public byte ByteProp { get; set; }
        public short ShortProp { get; set; }

        public bool Equals(TestStruct1 other)
        {
            return IntProp == other.IntProp && ByteProp == other.ByteProp && ShortProp == other.ShortProp;
        }
    }
}