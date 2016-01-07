// automatically generated, do not modify

namespace SerializationTests
{

using System;
using FlatBuffers;

public sealed class TestTableWithDeprecatedField : Table {
  public static TestTableWithDeprecatedField GetRootAsTestTableWithDeprecatedField(ByteBuffer _bb) { return GetRootAsTestTableWithDeprecatedField(_bb, new TestTableWithDeprecatedField()); }
  public static TestTableWithDeprecatedField GetRootAsTestTableWithDeprecatedField(ByteBuffer _bb, TestTableWithDeprecatedField obj) { return (obj.__init(_bb.GetInt(_bb.Position) + _bb.Position, _bb)); }
  public TestTableWithDeprecatedField __init(int _i, ByteBuffer _bb) { bb_pos = _i; bb = _bb; return this; }

  public int IntProp { get { int o = __offset(4); return o != 0 ? bb.GetInt(o + bb_pos) : (int)0; } }
  public short ShortProp { get { int o = __offset(8); return o != 0 ? bb.GetShort(o + bb_pos) : (short)0; } }

  public static Offset<TestTableWithDeprecatedField> CreateTestTableWithDeprecatedField(FlatBufferBuilder builder,
      int IntProp = 0,
      short ShortProp = 0) {
    builder.StartObject(3);
    TestTableWithDeprecatedField.AddIntProp(builder, IntProp);
    TestTableWithDeprecatedField.AddShortProp(builder, ShortProp);
    return TestTableWithDeprecatedField.EndTestTableWithDeprecatedField(builder);
  }

  public static void StartTestTableWithDeprecatedField(FlatBufferBuilder builder) { builder.StartObject(3); }
  public static void AddIntProp(FlatBufferBuilder builder, int IntProp) { builder.AddInt(0, IntProp, 0); }
  public static void AddShortProp(FlatBufferBuilder builder, short ShortProp) { builder.AddShort(2, ShortProp, 0); }
  public static Offset<TestTableWithDeprecatedField> EndTestTableWithDeprecatedField(FlatBufferBuilder builder) {
    int o = builder.EndObject();
    return new Offset<TestTableWithDeprecatedField>(o);
  }
};


}
