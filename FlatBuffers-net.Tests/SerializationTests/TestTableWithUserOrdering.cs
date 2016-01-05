// automatically generated, do not modify

namespace SerializationTests
{

using System;
using FlatBuffers;

public sealed class TestTableWithUserOrdering : Table {
  public static TestTableWithUserOrdering GetRootAsTestTableWithUserOrdering(ByteBuffer _bb) { return GetRootAsTestTableWithUserOrdering(_bb, new TestTableWithUserOrdering()); }
  public static TestTableWithUserOrdering GetRootAsTestTableWithUserOrdering(ByteBuffer _bb, TestTableWithUserOrdering obj) { return (obj.__init(_bb.GetInt(_bb.Position) + _bb.Position, _bb)); }
  public TestTableWithUserOrdering __init(int _i, ByteBuffer _bb) { bb_pos = _i; bb = _bb; return this; }

  public byte ByteProp { get { int o = __offset(4); return o != 0 ? bb.Get(o + bb_pos) : (byte)0; } }
  public short ShortProp { get { int o = __offset(6); return o != 0 ? bb.GetShort(o + bb_pos) : (short)0; } }
  public int IntProp { get { int o = __offset(8); return o != 0 ? bb.GetInt(o + bb_pos) : (int)0; } }

  public static Offset<TestTableWithUserOrdering> CreateTestTableWithUserOrdering(FlatBufferBuilder builder,
      byte ByteProp = 0,
      short ShortProp = 0,
      int IntProp = 0) {
    builder.StartObject(3);
    TestTableWithUserOrdering.AddIntProp(builder, IntProp);
    TestTableWithUserOrdering.AddShortProp(builder, ShortProp);
    TestTableWithUserOrdering.AddByteProp(builder, ByteProp);
    return TestTableWithUserOrdering.EndTestTableWithUserOrdering(builder);
  }

  public static void StartTestTableWithUserOrdering(FlatBufferBuilder builder) { builder.StartObject(3); }
  public static void AddByteProp(FlatBufferBuilder builder, byte ByteProp) { builder.AddByte(0, ByteProp, 0); }
  public static void AddShortProp(FlatBufferBuilder builder, short ShortProp) { builder.AddShort(1, ShortProp, 0); }
  public static void AddIntProp(FlatBufferBuilder builder, int IntProp) { builder.AddInt(2, IntProp, 0); }
  public static Offset<TestTableWithUserOrdering> EndTestTableWithUserOrdering(FlatBufferBuilder builder) {
    int o = builder.EndObject();
    return new Offset<TestTableWithUserOrdering>(o);
  }
};


}
