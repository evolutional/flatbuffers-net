// automatically generated, do not modify

namespace SerializationTests
{

using System;
using FlatBuffers;

public sealed class TestTableWithArrayOfTables : Table {
  public static TestTableWithArrayOfTables GetRootAsTestTableWithArrayOfTables(ByteBuffer _bb) { return GetRootAsTestTableWithArrayOfTables(_bb, new TestTableWithArrayOfTables()); }
  public static TestTableWithArrayOfTables GetRootAsTestTableWithArrayOfTables(ByteBuffer _bb, TestTableWithArrayOfTables obj) { return (obj.__init(_bb.GetInt(_bb.Position) + _bb.Position, _bb)); }
  public TestTableWithArrayOfTables __init(int _i, ByteBuffer _bb) { bb_pos = _i; bb = _bb; return this; }

  public TestTable1 GetTableArrayProp(int j) { return GetTableArrayProp(new TestTable1(), j); }
  public TestTable1 GetTableArrayProp(TestTable1 obj, int j) { int o = __offset(4); return o != 0 ? obj.__init(__indirect(__vector(o) + j * 4), bb) : null; }
  public int TableArrayPropLength { get { int o = __offset(4); return o != 0 ? __vector_len(o) : 0; } }
  public TestTable1 GetTableListProp(int j) { return GetTableListProp(new TestTable1(), j); }
  public TestTable1 GetTableListProp(TestTable1 obj, int j) { int o = __offset(6); return o != 0 ? obj.__init(__indirect(__vector(o) + j * 4), bb) : null; }
  public int TableListPropLength { get { int o = __offset(6); return o != 0 ? __vector_len(o) : 0; } }

  public static Offset<TestTableWithArrayOfTables> CreateTestTableWithArrayOfTables(FlatBufferBuilder builder,
      VectorOffset TableArrayPropOffset = default(VectorOffset),
      VectorOffset TableListPropOffset = default(VectorOffset)) {
    builder.StartObject(2);
    TestTableWithArrayOfTables.AddTableListProp(builder, TableListPropOffset);
    TestTableWithArrayOfTables.AddTableArrayProp(builder, TableArrayPropOffset);
    return TestTableWithArrayOfTables.EndTestTableWithArrayOfTables(builder);
  }

  public static void StartTestTableWithArrayOfTables(FlatBufferBuilder builder) { builder.StartObject(2); }
  public static void AddTableArrayProp(FlatBufferBuilder builder, VectorOffset TableArrayPropOffset) { builder.AddOffset(0, TableArrayPropOffset.Value, 0); }
  public static VectorOffset CreateTableArrayPropVector(FlatBufferBuilder builder, Offset<TestTable1>[] data) { builder.StartVector(4, data.Length, 4); for (int i = data.Length - 1; i >= 0; i--) builder.AddOffset(data[i].Value); return builder.EndVector(); }
  public static void StartTableArrayPropVector(FlatBufferBuilder builder, int numElems) { builder.StartVector(4, numElems, 4); }
  public static void AddTableListProp(FlatBufferBuilder builder, VectorOffset TableListPropOffset) { builder.AddOffset(1, TableListPropOffset.Value, 0); }
  public static VectorOffset CreateTableListPropVector(FlatBufferBuilder builder, Offset<TestTable1>[] data) { builder.StartVector(4, data.Length, 4); for (int i = data.Length - 1; i >= 0; i--) builder.AddOffset(data[i].Value); return builder.EndVector(); }
  public static void StartTableListPropVector(FlatBufferBuilder builder, int numElems) { builder.StartVector(4, numElems, 4); }
  public static Offset<TestTableWithArrayOfTables> EndTestTableWithArrayOfTables(FlatBufferBuilder builder) {
    int o = builder.EndObject();
    return new Offset<TestTableWithArrayOfTables>(o);
  }
};


}
