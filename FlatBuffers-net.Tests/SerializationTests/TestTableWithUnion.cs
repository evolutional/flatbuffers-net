// automatically generated, do not modify

namespace SerializationTests
{

using System;
using FlatBuffers;

public sealed class TestTableWithUnion : Table {
  public static TestTableWithUnion GetRootAsTestTableWithUnion(ByteBuffer _bb) { return GetRootAsTestTableWithUnion(_bb, new TestTableWithUnion()); }
  public static TestTableWithUnion GetRootAsTestTableWithUnion(ByteBuffer _bb, TestTableWithUnion obj) { return (obj.__init(_bb.GetInt(_bb.Position) + _bb.Position, _bb)); }
  public TestTableWithUnion __init(int _i, ByteBuffer _bb) { bb_pos = _i; bb = _bb; return this; }

  public int IntProp { get { int o = __offset(4); return o != 0 ? bb.GetInt(o + bb_pos) : (int)0; } }
  public TestUnion UnionPropType { get { int o = __offset(6); return o != 0 ? (TestUnion)bb.Get(o + bb_pos) : TestUnion.NONE; } }
  public TTable GetUnionProp<TTable>(TTable obj) where TTable : Table { int o = __offset(8); return o != 0 ? __union(obj, o) : null; }

  public static Offset<TestTableWithUnion> CreateTestTableWithUnion(FlatBufferBuilder builder,
      int IntProp = 0,
      TestUnion UnionProp_type = TestUnion.NONE,
      int UnionPropOffset = 0) {
    builder.StartObject(3);
    TestTableWithUnion.AddUnionProp(builder, UnionPropOffset);
    TestTableWithUnion.AddIntProp(builder, IntProp);
    TestTableWithUnion.AddUnionPropType(builder, UnionProp_type);
    return TestTableWithUnion.EndTestTableWithUnion(builder);
  }

  public static void StartTestTableWithUnion(FlatBufferBuilder builder) { builder.StartObject(3); }
  public static void AddIntProp(FlatBufferBuilder builder, int IntProp) { builder.AddInt(0, IntProp, 0); }
  public static void AddUnionPropType(FlatBufferBuilder builder, TestUnion UnionPropType) { builder.AddByte(1, (byte)UnionPropType, 0); }
  public static void AddUnionProp(FlatBufferBuilder builder, int UnionPropOffset) { builder.AddOffset(2, UnionPropOffset, 0); }
  public static Offset<TestTableWithUnion> EndTestTableWithUnion(FlatBufferBuilder builder) {
    int o = builder.EndObject();
    return new Offset<TestTableWithUnion>(o);
  }
};


}
