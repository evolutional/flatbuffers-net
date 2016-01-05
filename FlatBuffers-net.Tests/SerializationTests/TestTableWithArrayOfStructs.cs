// automatically generated, do not modify

namespace SerializationTests
{

using System;
using FlatBuffers;

public sealed class TestTableWithArrayOfStructs : Table {
  public static TestTableWithArrayOfStructs GetRootAsTestTableWithArrayOfStructs(ByteBuffer _bb) { return GetRootAsTestTableWithArrayOfStructs(_bb, new TestTableWithArrayOfStructs()); }
  public static TestTableWithArrayOfStructs GetRootAsTestTableWithArrayOfStructs(ByteBuffer _bb, TestTableWithArrayOfStructs obj) { return (obj.__init(_bb.GetInt(_bb.Position) + _bb.Position, _bb)); }
  public TestTableWithArrayOfStructs __init(int _i, ByteBuffer _bb) { bb_pos = _i; bb = _bb; return this; }

  public TestStruct1 GetStructArray(int j) { return GetStructArray(new TestStruct1(), j); }
  public TestStruct1 GetStructArray(TestStruct1 obj, int j) { int o = __offset(4); return o != 0 ? obj.__init(__vector(o) + j * 8, bb) : null; }
  public int StructArrayLength { get { int o = __offset(4); return o != 0 ? __vector_len(o) : 0; } }

  public static Offset<TestTableWithArrayOfStructs> CreateTestTableWithArrayOfStructs(FlatBufferBuilder builder,
      VectorOffset StructArrayOffset = default(VectorOffset)) {
    builder.StartObject(1);
    TestTableWithArrayOfStructs.AddStructArray(builder, StructArrayOffset);
    return TestTableWithArrayOfStructs.EndTestTableWithArrayOfStructs(builder);
  }

  public static void StartTestTableWithArrayOfStructs(FlatBufferBuilder builder) { builder.StartObject(1); }
  public static void AddStructArray(FlatBufferBuilder builder, VectorOffset StructArrayOffset) { builder.AddOffset(0, StructArrayOffset.Value, 0); }
  public static void StartStructArrayVector(FlatBufferBuilder builder, int numElems) { builder.StartVector(8, numElems, 4); }
  public static Offset<TestTableWithArrayOfStructs> EndTestTableWithArrayOfStructs(FlatBufferBuilder builder) {
    int o = builder.EndObject();
    return new Offset<TestTableWithArrayOfStructs>(o);
  }
};


}
