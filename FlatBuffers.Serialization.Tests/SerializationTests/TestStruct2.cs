// automatically generated, do not modify

namespace SerializationTests
{

using System;
using FlatBuffers;

public sealed class TestStruct2 : Struct {
  public TestStruct2 __init(int _i, ByteBuffer _bb) { bb_pos = _i; bb = _bb; return this; }

  public int IntProp { get { return bb.GetInt(bb_pos + 0); } }
  public TestStruct1 TestStruct1Prop { get { return GetTestStruct1Prop(new TestStruct1()); } }
  public TestStruct1 GetTestStruct1Prop(TestStruct1 obj) { return obj.__init(bb_pos + 4, bb); }

  public static Offset<TestStruct2> CreateTestStruct2(FlatBufferBuilder builder, int IntProp, int TestStruct1Prop_IntProp, byte TestStruct1Prop_ByteProp, short TestStruct1Prop_ShortProp) {
    builder.Prep(4, 12);
    builder.Prep(4, 8);
    builder.PutShort(TestStruct1Prop_ShortProp);
    builder.Pad(1);
    builder.PutByte(TestStruct1Prop_ByteProp);
    builder.PutInt(TestStruct1Prop_IntProp);
    builder.PutInt(IntProp);
    return new Offset<TestStruct2>(builder.Offset);
  }
};


}
