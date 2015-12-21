// automatically generated, do not modify

namespace SerializationTests
{

using System;
using FlatBuffers;

public sealed class TestStruct1 : Struct {
  public TestStruct1 __init(int _i, ByteBuffer _bb) { bb_pos = _i; bb = _bb; return this; }

  public int IntProp { get { return bb.GetInt(bb_pos + 0); } }
  public byte ByteProp { get { return bb.Get(bb_pos + 4); } }
  public short ShortProp { get { return bb.GetShort(bb_pos + 6); } }

  public static Offset<TestStruct1> CreateTestStruct1(FlatBufferBuilder builder, int IntProp, byte ByteProp, short ShortProp) {
    builder.Prep(4, 8);
    builder.PutShort(ShortProp);
    builder.Pad(1);
    builder.PutByte(ByteProp);
    builder.PutInt(IntProp);
    return new Offset<TestStruct1>(builder.Offset);
  }
};


}
