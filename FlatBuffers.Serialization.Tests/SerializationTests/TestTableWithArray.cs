// automatically generated, do not modify

namespace SerializationTests
{

using System;
using FlatBuffers;

public sealed class TestTableWithArray : Table {
  public static TestTableWithArray GetRootAsTestTableWithArray(ByteBuffer _bb) { return GetRootAsTestTableWithArray(_bb, new TestTableWithArray()); }
  public static TestTableWithArray GetRootAsTestTableWithArray(ByteBuffer _bb, TestTableWithArray obj) { return (obj.__init(_bb.GetInt(_bb.Position) + _bb.Position, _bb)); }
  public TestTableWithArray __init(int _i, ByteBuffer _bb) { bb_pos = _i; bb = _bb; return this; }

  public int GetIntArray(int j) { int o = __offset(4); return o != 0 ? bb.GetInt(__vector(o) + j * 4) : (int)0; }
  public int IntArrayLength { get { int o = __offset(4); return o != 0 ? __vector_len(o) : 0; } }
  public ArraySegment<byte>? GetIntArrayBytes() { return __vector_as_arraysegment(4); }
  public int GetIntList(int j) { int o = __offset(6); return o != 0 ? bb.GetInt(__vector(o) + j * 4) : (int)0; }
  public int IntListLength { get { int o = __offset(6); return o != 0 ? __vector_len(o) : 0; } }
  public ArraySegment<byte>? GetIntListBytes() { return __vector_as_arraysegment(6); }

  public static Offset<TestTableWithArray> CreateTestTableWithArray(FlatBufferBuilder builder,
      VectorOffset IntArrayOffset = default(VectorOffset),
      VectorOffset IntListOffset = default(VectorOffset)) {
    builder.StartObject(2);
    TestTableWithArray.AddIntList(builder, IntListOffset);
    TestTableWithArray.AddIntArray(builder, IntArrayOffset);
    return TestTableWithArray.EndTestTableWithArray(builder);
  }

  public static void StartTestTableWithArray(FlatBufferBuilder builder) { builder.StartObject(2); }
  public static void AddIntArray(FlatBufferBuilder builder, VectorOffset IntArrayOffset) { builder.AddOffset(0, IntArrayOffset.Value, 0); }
  public static VectorOffset CreateIntArrayVector(FlatBufferBuilder builder, int[] data) { builder.StartVector(4, data.Length, 4); for (int i = data.Length - 1; i >= 0; i--) builder.AddInt(data[i]); return builder.EndVector(); }
  public static void StartIntArrayVector(FlatBufferBuilder builder, int numElems) { builder.StartVector(4, numElems, 4); }
  public static void AddIntList(FlatBufferBuilder builder, VectorOffset IntListOffset) { builder.AddOffset(1, IntListOffset.Value, 0); }
  public static VectorOffset CreateIntListVector(FlatBufferBuilder builder, int[] data) { builder.StartVector(4, data.Length, 4); for (int i = data.Length - 1; i >= 0; i--) builder.AddInt(data[i]); return builder.EndVector(); }
  public static void StartIntListVector(FlatBufferBuilder builder, int numElems) { builder.StartVector(4, numElems, 4); }
  public static Offset<TestTableWithArray> EndTestTableWithArray(FlatBufferBuilder builder) {
    int o = builder.EndObject();
    return new Offset<TestTableWithArray>(o);
  }
};


}
