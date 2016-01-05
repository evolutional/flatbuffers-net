// automatically generated, do not modify

namespace SerializationTests
{

using System;
using FlatBuffers;

public sealed class TestTableWithTable : Table {
  public static TestTableWithTable GetRootAsTestTableWithTable(ByteBuffer _bb) { return GetRootAsTestTableWithTable(_bb, new TestTableWithTable()); }
  public static TestTableWithTable GetRootAsTestTableWithTable(ByteBuffer _bb, TestTableWithTable obj) { return (obj.__init(_bb.GetInt(_bb.Position) + _bb.Position, _bb)); }
  public TestTableWithTable __init(int _i, ByteBuffer _bb) { bb_pos = _i; bb = _bb; return this; }

  public TestTable1 TableProp { get { return GetTableProp(new TestTable1()); } }
  public TestTable1 GetTableProp(TestTable1 obj) { int o = __offset(4); return o != 0 ? obj.__init(__indirect(o + bb_pos), bb) : null; }
  public int IntProp { get { int o = __offset(6); return o != 0 ? bb.GetInt(o + bb_pos) : (int)0; } }

  public static Offset<TestTableWithTable> CreateTestTableWithTable(FlatBufferBuilder builder,
      Offset<TestTable1> TablePropOffset = default(Offset<TestTable1>),
      int IntProp = 0) {
    builder.StartObject(2);
    TestTableWithTable.AddIntProp(builder, IntProp);
    TestTableWithTable.AddTableProp(builder, TablePropOffset);
    return TestTableWithTable.EndTestTableWithTable(builder);
  }

  public static void StartTestTableWithTable(FlatBufferBuilder builder) { builder.StartObject(2); }
  public static void AddTableProp(FlatBufferBuilder builder, Offset<TestTable1> TablePropOffset) { builder.AddOffset(0, TablePropOffset.Value, 0); }
  public static void AddIntProp(FlatBufferBuilder builder, int IntProp) { builder.AddInt(1, IntProp, 0); }
  public static Offset<TestTableWithTable> EndTestTableWithTable(FlatBufferBuilder builder) {
    int o = builder.EndObject();
    return new Offset<TestTableWithTable>(o);
  }
};


}
