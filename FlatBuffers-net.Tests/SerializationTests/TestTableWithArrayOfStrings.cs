// automatically generated, do not modify

namespace SerializationTests
{

using System;
using FlatBuffers;

public sealed class TestTableWithArrayOfStrings : Table {
  public static TestTableWithArrayOfStrings GetRootAsTestTableWithArrayOfStrings(ByteBuffer _bb) { return GetRootAsTestTableWithArrayOfStrings(_bb, new TestTableWithArrayOfStrings()); }
  public static TestTableWithArrayOfStrings GetRootAsTestTableWithArrayOfStrings(ByteBuffer _bb, TestTableWithArrayOfStrings obj) { return (obj.__init(_bb.GetInt(_bb.Position) + _bb.Position, _bb)); }
  public TestTableWithArrayOfStrings __init(int _i, ByteBuffer _bb) { bb_pos = _i; bb = _bb; return this; }

  public string GetStringArrayProp(int j) { int o = __offset(4); return o != 0 ? __string(__vector(o) + j * 4) : null; }
  public int StringArrayPropLength { get { int o = __offset(4); return o != 0 ? __vector_len(o) : 0; } }
  public string GetStringListProp(int j) { int o = __offset(6); return o != 0 ? __string(__vector(o) + j * 4) : null; }
  public int StringListPropLength { get { int o = __offset(6); return o != 0 ? __vector_len(o) : 0; } }

  public static Offset<TestTableWithArrayOfStrings> CreateTestTableWithArrayOfStrings(FlatBufferBuilder builder,
      VectorOffset StringArrayPropOffset = default(VectorOffset),
      VectorOffset StringListPropOffset = default(VectorOffset)) {
    builder.StartObject(2);
    TestTableWithArrayOfStrings.AddStringListProp(builder, StringListPropOffset);
    TestTableWithArrayOfStrings.AddStringArrayProp(builder, StringArrayPropOffset);
    return TestTableWithArrayOfStrings.EndTestTableWithArrayOfStrings(builder);
  }

  public static void StartTestTableWithArrayOfStrings(FlatBufferBuilder builder) { builder.StartObject(2); }
  public static void AddStringArrayProp(FlatBufferBuilder builder, VectorOffset StringArrayPropOffset) { builder.AddOffset(0, StringArrayPropOffset.Value, 0); }
  public static VectorOffset CreateStringArrayPropVector(FlatBufferBuilder builder, StringOffset[] data) { builder.StartVector(4, data.Length, 4); for (int i = data.Length - 1; i >= 0; i--) builder.AddOffset(data[i].Value); return builder.EndVector(); }
  public static void StartStringArrayPropVector(FlatBufferBuilder builder, int numElems) { builder.StartVector(4, numElems, 4); }
  public static void AddStringListProp(FlatBufferBuilder builder, VectorOffset StringListPropOffset) { builder.AddOffset(1, StringListPropOffset.Value, 0); }
  public static VectorOffset CreateStringListPropVector(FlatBufferBuilder builder, StringOffset[] data) { builder.StartVector(4, data.Length, 4); for (int i = data.Length - 1; i >= 0; i--) builder.AddOffset(data[i].Value); return builder.EndVector(); }
  public static void StartStringListPropVector(FlatBufferBuilder builder, int numElems) { builder.StartVector(4, numElems, 4); }
  public static Offset<TestTableWithArrayOfStrings> EndTestTableWithArrayOfStrings(FlatBufferBuilder builder) {
    int o = builder.EndObject();
    return new Offset<TestTableWithArrayOfStrings>(o);
  }
};


}
