// automatically generated, do not modify

namespace SerializationTests
{

using System;
using FlatBuffers;

public sealed class TestTableWithUnionAndMoreFields : Table {
  public static TestTableWithUnionAndMoreFields GetRootAsTestTableWithUnionAndMoreFields(ByteBuffer _bb) { return GetRootAsTestTableWithUnionAndMoreFields(_bb, new TestTableWithUnionAndMoreFields()); }
  public static TestTableWithUnionAndMoreFields GetRootAsTestTableWithUnionAndMoreFields(ByteBuffer _bb, TestTableWithUnionAndMoreFields obj) { return (obj.__init(_bb.GetInt(_bb.Position) + _bb.Position, _bb)); }
  public TestTableWithUnionAndMoreFields __init(int _i, ByteBuffer _bb) { bb_pos = _i; bb = _bb; return this; }

  public int IntProp { get { int o = __offset(4); return o != 0 ? bb.GetInt(o + bb_pos) : (int)0; } }
  public TestUnion UnionPropType { get { int o = __offset(6); return o != 0 ? (TestUnion)bb.Get(o + bb_pos) : TestUnion.NONE; } }
  public TTable GetUnionProp<TTable>(TTable obj) where TTable : Table { int o = __offset(8); return o != 0 ? __union(obj, o) : null; }
  public string StringProp { get { int o = __offset(10); return o != 0 ? __string(o + bb_pos) : null; } }
  public ArraySegment<byte>? GetStringPropBytes() { return __vector_as_arraysegment(10); }
  public float FloatProp { get { int o = __offset(12); return o != 0 ? bb.GetFloat(o + bb_pos) : (float)0; } }
  public double DoubleProp { get { int o = __offset(14); return o != 0 ? bb.GetDouble(o + bb_pos) : (double)0; } }

  public static Offset<TestTableWithUnionAndMoreFields> CreateTestTableWithUnionAndMoreFields(FlatBufferBuilder builder,
      int IntProp = 0,
      TestUnion UnionProp_type = TestUnion.NONE,
      int UnionPropOffset = 0,
      StringOffset StringPropOffset = default(StringOffset),
      float FloatProp = 0,
      double DoubleProp = 0) {
    builder.StartObject(6);
    TestTableWithUnionAndMoreFields.AddDoubleProp(builder, DoubleProp);
    TestTableWithUnionAndMoreFields.AddFloatProp(builder, FloatProp);
    TestTableWithUnionAndMoreFields.AddStringProp(builder, StringPropOffset);
    TestTableWithUnionAndMoreFields.AddUnionProp(builder, UnionPropOffset);
    TestTableWithUnionAndMoreFields.AddIntProp(builder, IntProp);
    TestTableWithUnionAndMoreFields.AddUnionPropType(builder, UnionProp_type);
    return TestTableWithUnionAndMoreFields.EndTestTableWithUnionAndMoreFields(builder);
  }

  public static void StartTestTableWithUnionAndMoreFields(FlatBufferBuilder builder) { builder.StartObject(6); }
  public static void AddIntProp(FlatBufferBuilder builder, int IntProp) { builder.AddInt(0, IntProp, 0); }
  public static void AddUnionPropType(FlatBufferBuilder builder, TestUnion UnionPropType) { builder.AddByte(1, (byte)UnionPropType, 0); }
  public static void AddUnionProp(FlatBufferBuilder builder, int UnionPropOffset) { builder.AddOffset(2, UnionPropOffset, 0); }
  public static void AddStringProp(FlatBufferBuilder builder, StringOffset StringPropOffset) { builder.AddOffset(3, StringPropOffset.Value, 0); }
  public static void AddFloatProp(FlatBufferBuilder builder, float FloatProp) { builder.AddFloat(4, FloatProp, 0); }
  public static void AddDoubleProp(FlatBufferBuilder builder, double DoubleProp) { builder.AddDouble(5, DoubleProp, 0); }
  public static Offset<TestTableWithUnionAndMoreFields> EndTestTableWithUnionAndMoreFields(FlatBufferBuilder builder) {
    int o = builder.EndObject();
    return new Offset<TestTableWithUnionAndMoreFields>(o);
  }
};


}
