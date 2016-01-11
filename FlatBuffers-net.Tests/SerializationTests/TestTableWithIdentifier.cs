// automatically generated, do not modify

namespace SerializationTests
{

using System;
using FlatBuffers;

public sealed class TestTableWithIdentifier : Table {
  public static TestTableWithIdentifier GetRootAsTestTableWithIdentifier(ByteBuffer _bb) { return GetRootAsTestTableWithIdentifier(_bb, new TestTableWithIdentifier()); }
  public static TestTableWithIdentifier GetRootAsTestTableWithIdentifier(ByteBuffer _bb, TestTableWithIdentifier obj) { return (obj.__init(_bb.GetInt(_bb.Position) + _bb.Position, _bb)); }
  public static bool TestTableWithIdentifierBufferHasIdentifier(ByteBuffer _bb) { return __has_identifier(_bb, "TEST"); }
  public TestTableWithIdentifier __init(int _i, ByteBuffer _bb) { bb_pos = _i; bb = _bb; return this; }

  public int IntProp { get { int o = __offset(4); return o != 0 ? bb.GetInt(o + bb_pos) : (int)0; } }

  public static Offset<TestTableWithIdentifier> CreateTestTableWithIdentifier(FlatBufferBuilder builder,
      int IntProp = 0) {
    builder.StartObject(1);
    TestTableWithIdentifier.AddIntProp(builder, IntProp);
    return TestTableWithIdentifier.EndTestTableWithIdentifier(builder);
  }

  public static void StartTestTableWithIdentifier(FlatBufferBuilder builder) { builder.StartObject(1); }
  public static void AddIntProp(FlatBufferBuilder builder, int IntProp) { builder.AddInt(0, IntProp, 0); }
  public static Offset<TestTableWithIdentifier> EndTestTableWithIdentifier(FlatBufferBuilder builder) {
    int o = builder.EndObject();
    return new Offset<TestTableWithIdentifier>(o);
  }
  public static void FinishTestTableWithIdentifierBuffer(FlatBufferBuilder builder, Offset<TestTableWithIdentifier> offset) { builder.Finish(offset.Value, "TEST"); }
};


}
