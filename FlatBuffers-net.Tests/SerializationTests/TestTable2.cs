// automatically generated, do not modify

namespace SerializationTests
{

using System;
using FlatBuffers;

public sealed class TestTable2 : Table {
  public static TestTable2 GetRootAsTestTable2(ByteBuffer _bb) { return GetRootAsTestTable2(_bb, new TestTable2()); }
  public static TestTable2 GetRootAsTestTable2(ByteBuffer _bb, TestTable2 obj) { return (obj.__init(_bb.GetInt(_bb.Position) + _bb.Position, _bb)); }
  public TestTable2 __init(int _i, ByteBuffer _bb) { bb_pos = _i; bb = _bb; return this; }

  public string StringProp { get { int o = __offset(4); return o != 0 ? __string(o + bb_pos) : null; } }
  public ArraySegment<byte>? GetStringPropBytes() { return __vector_as_arraysegment(4); }

  public static Offset<TestTable2> CreateTestTable2(FlatBufferBuilder builder,
      StringOffset StringPropOffset = default(StringOffset)) {
    builder.StartObject(1);
    TestTable2.AddStringProp(builder, StringPropOffset);
    return TestTable2.EndTestTable2(builder);
  }

  public static void StartTestTable2(FlatBufferBuilder builder) { builder.StartObject(1); }
  public static void AddStringProp(FlatBufferBuilder builder, StringOffset StringPropOffset) { builder.AddOffset(0, StringPropOffset.Value, 0); }
  public static Offset<TestTable2> EndTestTable2(FlatBufferBuilder builder) {
    int o = builder.EndObject();
    return new Offset<TestTable2>(o);
  }
};


}
