// automatically generated, do not modify

namespace SerializationTests
{

using System;
using FlatBuffers;

public sealed class TestTableWithStruct : Table {
  public static TestTableWithStruct GetRootAsTestTableWithStruct(ByteBuffer _bb) { return GetRootAsTestTableWithStruct(_bb, new TestTableWithStruct()); }
  public static TestTableWithStruct GetRootAsTestTableWithStruct(ByteBuffer _bb, TestTableWithStruct obj) { return (obj.__init(_bb.GetInt(_bb.Position) + _bb.Position, _bb)); }
  public TestTableWithStruct __init(int _i, ByteBuffer _bb) { bb_pos = _i; bb = _bb; return this; }

  public TestStruct1 StructProp { get { return GetStructProp(new TestStruct1()); } }
  public TestStruct1 GetStructProp(TestStruct1 obj) { int o = __offset(4); return o != 0 ? obj.__init(o + bb_pos, bb) : null; }
  public int IntProp { get { int o = __offset(6); return o != 0 ? bb.GetInt(o + bb_pos) : (int)0; } }

  public static void StartTestTableWithStruct(FlatBufferBuilder builder) { builder.StartObject(2); }
  public static void AddStructProp(FlatBufferBuilder builder, Offset<TestStruct1> StructPropOffset) { builder.AddStruct(0, StructPropOffset.Value, 0); }
  public static void AddIntProp(FlatBufferBuilder builder, int IntProp) { builder.AddInt(1, IntProp, 0); }
  public static Offset<TestTableWithStruct> EndTestTableWithStruct(FlatBufferBuilder builder) {
    int o = builder.EndObject();
    return new Offset<TestTableWithStruct>(o);
  }
};


}
