// automatically generated, do not modify

namespace SerializationTests
{

using System;
using FlatBuffers;

public sealed class TestTable1 : Table {
  public static TestTable1 GetRootAsTestTable1(ByteBuffer _bb) { return GetRootAsTestTable1(_bb, new TestTable1()); }
  public static TestTable1 GetRootAsTestTable1(ByteBuffer _bb, TestTable1 obj) { return (obj.__init(_bb.GetInt(_bb.Position) + _bb.Position, _bb)); }
  public TestTable1 __init(int _i, ByteBuffer _bb) { bb_pos = _i; bb = _bb; return this; }

  public int IntProp { get { int o = __offset(4); return o != 0 ? bb.GetInt(o + bb_pos) : (int)0; } }
  public byte ByteProp { get { int o = __offset(6); return o != 0 ? bb.Get(o + bb_pos) : (byte)0; } }
  public short ShortProp { get { int o = __offset(8); return o != 0 ? bb.GetShort(o + bb_pos) : (short)0; } }

  public static Offset<TestTable1> CreateTestTable1(FlatBufferBuilder builder,
      int IntProp = 0,
      byte ByteProp = 0,
      short ShortProp = 0) {
    builder.StartObject(3);
    TestTable1.AddIntProp(builder, IntProp);
    TestTable1.AddShortProp(builder, ShortProp);
    TestTable1.AddByteProp(builder, ByteProp);
    return TestTable1.EndTestTable1(builder);
  }

  public static void StartTestTable1(FlatBufferBuilder builder) { builder.StartObject(3); }
  public static void AddIntProp(FlatBufferBuilder builder, int IntProp) { builder.AddInt(0, IntProp, 0); }
  public static void AddByteProp(FlatBufferBuilder builder, byte ByteProp) { builder.AddByte(1, ByteProp, 0); }
  public static void AddShortProp(FlatBufferBuilder builder, short ShortProp) { builder.AddShort(2, ShortProp, 0); }
  public static Offset<TestTable1> EndTestTable1(FlatBufferBuilder builder) {
    int o = builder.EndObject();
    return new Offset<TestTable1>(o);
  }
};


}
