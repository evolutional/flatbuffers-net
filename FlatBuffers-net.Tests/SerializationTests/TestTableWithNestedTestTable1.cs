// automatically generated, do not modify

namespace SerializationTests
{

using System;
using FlatBuffers;

public sealed class TestTableWithNestedTestTable1 : Table {
  public static TestTableWithNestedTestTable1 GetRootAsTestTableWithNestedTestTable1(ByteBuffer _bb) { return GetRootAsTestTableWithNestedTestTable1(_bb, new TestTableWithNestedTestTable1()); }
  public static TestTableWithNestedTestTable1 GetRootAsTestTableWithNestedTestTable1(ByteBuffer _bb, TestTableWithNestedTestTable1 obj) { return (obj.__init(_bb.GetInt(_bb.Position) + _bb.Position, _bb)); }
  public TestTableWithNestedTestTable1 __init(int _i, ByteBuffer _bb) { bb_pos = _i; bb = _bb; return this; }

  public int IntProp { get { int o = __offset(4); return o != 0 ? bb.GetInt(o + bb_pos) : (int)0; } }
  public byte GetNested(int j) { int o = __offset(6); return o != 0 ? bb.Get(__vector(o) + j * 1) : (byte)0; }
  public int NestedLength { get { int o = __offset(6); return o != 0 ? __vector_len(o) : 0; } }
  public ArraySegment<byte>? GetNestedBytes() { return __vector_as_arraysegment(6); }

  public static Offset<TestTableWithNestedTestTable1> CreateTestTableWithNestedTestTable1(FlatBufferBuilder builder,
      int IntProp = 0,
      VectorOffset NestedOffset = default(VectorOffset)) {
    builder.StartObject(2);
    TestTableWithNestedTestTable1.AddNested(builder, NestedOffset);
    TestTableWithNestedTestTable1.AddIntProp(builder, IntProp);
    return TestTableWithNestedTestTable1.EndTestTableWithNestedTestTable1(builder);
  }

  public static void StartTestTableWithNestedTestTable1(FlatBufferBuilder builder) { builder.StartObject(2); }
  public static void AddIntProp(FlatBufferBuilder builder, int IntProp) { builder.AddInt(0, IntProp, 0); }
  public static void AddNested(FlatBufferBuilder builder, VectorOffset NestedOffset) { builder.AddOffset(1, NestedOffset.Value, 0); }
  public static VectorOffset CreateNestedVector(FlatBufferBuilder builder, byte[] data) { builder.StartVector(1, data.Length, 1); for (int i = data.Length - 1; i >= 0; i--) builder.AddByte(data[i]); return builder.EndVector(); }
  public static void StartNestedVector(FlatBufferBuilder builder, int numElems) { builder.StartVector(1, numElems, 1); }
  public static Offset<TestTableWithNestedTestTable1> EndTestTableWithNestedTestTable1(FlatBufferBuilder builder) {
    int o = builder.EndObject();
    return new Offset<TestTableWithNestedTestTable1>(o);
  }
};


}
