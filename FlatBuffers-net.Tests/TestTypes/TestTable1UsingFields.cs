using FlatBuffers.Attributes;

namespace FlatBuffers.Tests.TestTypes
{
    public class TestTable1UsingFields
    {
        public int IntProp;
        public byte ByteProp;
        public short ShortProp;
    }

    public class TestTable1UsingFieldsAndDefaultFieldValues
    {
        [FlatBuffersDefaultValue(424242)]
        public int IntProp = 424242;

        [FlatBuffersDefaultValue(22)]
        public byte ByteProp = 22;

        [FlatBuffersDefaultValue(1024)]
        public short ShortProp = 1024;
    }
}