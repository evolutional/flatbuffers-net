// automatically generated, do not modify

namespace SerializationTests
{

using System;
using FlatBuffers;

public sealed class TestTableWithDefaults : Table {
  public static TestTableWithDefaults GetRootAsTestTableWithDefaults(ByteBuffer _bb) { return GetRootAsTestTableWithDefaults(_bb, new TestTableWithDefaults()); }
  public static TestTableWithDefaults GetRootAsTestTableWithDefaults(ByteBuffer _bb, TestTableWithDefaults obj) { return (obj.__init(_bb.GetInt(_bb.Position) + _bb.Position, _bb)); }
  public TestTableWithDefaults __init(int _i, ByteBuffer _bb) { bb_pos = _i; bb = _bb; return this; }

  public int IntProp { get { int o = __offset(4); return o != 0 ? bb.GetInt(o + bb_pos) : (int)123456; } }
  public byte ByteProp { get { int o = __offset(6); return o != 0 ? bb.Get(o + bb_pos) : (byte)42; } }
  public short ShortProp { get { int o = __offset(8); return o != 0 ? bb.GetShort(o + bb_pos) : (short)1024; } }

  public static Offset<TestTableWithDefaults> CreateTestTableWithDefaults(FlatBufferBuilder builder,
      int IntProp = 123456,
      byte ByteProp = 42,
      short ShortProp = 1024) {
    builder.StartObject(3);
    TestTableWithDefaults.AddIntProp(builder, IntProp);
    TestTableWithDefaults.AddShortProp(builder, ShortProp);
    TestTableWithDefaults.AddByteProp(builder, ByteProp);
    return TestTableWithDefaults.EndTestTableWithDefaults(builder);
  }

  public static void StartTestTableWithDefaults(FlatBufferBuilder builder) { builder.StartObject(3); }
  public static void AddIntProp(FlatBufferBuilder builder, int IntProp) { builder.AddInt(0, IntProp, 123456); }
  public static void AddByteProp(FlatBufferBuilder builder, byte ByteProp) { builder.AddByte(1, ByteProp, 42); }
  public static void AddShortProp(FlatBufferBuilder builder, short ShortProp) { builder.AddShort(2, ShortProp, 1024); }
  public static Offset<TestTableWithDefaults> EndTestTableWithDefaults(FlatBufferBuilder builder) {
    int o = builder.EndObject();
    return new Offset<TestTableWithDefaults>(o);
  }
};


}
