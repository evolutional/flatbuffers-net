// automatically generated, do not modify

namespace SerializationTests
{

using System;
using FlatBuffers;

public sealed class TestTable3 : Table {
  public static TestTable3 GetRootAsTestTable3(ByteBuffer _bb) { return GetRootAsTestTable3(_bb, new TestTable3()); }
  public static TestTable3 GetRootAsTestTable3(ByteBuffer _bb, TestTable3 obj) { return (obj.__init(_bb.GetInt(_bb.Position) + _bb.Position, _bb)); }
  public TestTable3 __init(int _i, ByteBuffer _bb) { bb_pos = _i; bb = _bb; return this; }

  public bool BoolProp { get { int o = __offset(4); return o != 0 ? 0!=bb.Get(o + bb_pos) : (bool)false; } }
  public long LongProp { get { int o = __offset(6); return o != 0 ? bb.GetLong(o + bb_pos) : (long)0; } }
  public sbyte SByteProp { get { int o = __offset(8); return o != 0 ? bb.GetSbyte(o + bb_pos) : (sbyte)0; } }
  public ushort UShortProp { get { int o = __offset(10); return o != 0 ? bb.GetUshort(o + bb_pos) : (ushort)0; } }
  public ulong ULongProp { get { int o = __offset(12); return o != 0 ? bb.GetUlong(o + bb_pos) : (ulong)0; } }
  public TestEnum EnumProp { get { int o = __offset(14); return o != 0 ? (TestEnum)bb.Get(o + bb_pos) : TestEnum.Apple; } }
  public float FloatProp { get { int o = __offset(16); return o != 0 ? bb.GetFloat(o + bb_pos) : (float)0; } }
  public double DoubleProp { get { int o = __offset(18); return o != 0 ? bb.GetDouble(o + bb_pos) : (double)0; } }

  public static Offset<TestTable3> CreateTestTable3(FlatBufferBuilder builder,
      bool BoolProp = false,
      long LongProp = 0,
      sbyte SByteProp = 0,
      ushort UShortProp = 0,
      ulong ULongProp = 0,
      TestEnum EnumProp = TestEnum.Apple,
      float FloatProp = 0,
      double DoubleProp = 0) {
    builder.StartObject(8);
    TestTable3.AddDoubleProp(builder, DoubleProp);
    TestTable3.AddULongProp(builder, ULongProp);
    TestTable3.AddLongProp(builder, LongProp);
    TestTable3.AddFloatProp(builder, FloatProp);
    TestTable3.AddUShortProp(builder, UShortProp);
    TestTable3.AddEnumProp(builder, EnumProp);
    TestTable3.AddSByteProp(builder, SByteProp);
    TestTable3.AddBoolProp(builder, BoolProp);
    return TestTable3.EndTestTable3(builder);
  }

  public static void StartTestTable3(FlatBufferBuilder builder) { builder.StartObject(8); }
  public static void AddBoolProp(FlatBufferBuilder builder, bool BoolProp) { builder.AddBool(0, BoolProp, false); }
  public static void AddLongProp(FlatBufferBuilder builder, long LongProp) { builder.AddLong(1, LongProp, 0); }
  public static void AddSByteProp(FlatBufferBuilder builder, sbyte SByteProp) { builder.AddSbyte(2, SByteProp, 0); }
  public static void AddUShortProp(FlatBufferBuilder builder, ushort UShortProp) { builder.AddUshort(3, UShortProp, 0); }
  public static void AddULongProp(FlatBufferBuilder builder, ulong ULongProp) { builder.AddUlong(4, ULongProp, 0); }
  public static void AddEnumProp(FlatBufferBuilder builder, TestEnum EnumProp) { builder.AddByte(5, (byte)EnumProp, 0); }
  public static void AddFloatProp(FlatBufferBuilder builder, float FloatProp) { builder.AddFloat(6, FloatProp, 0); }
  public static void AddDoubleProp(FlatBufferBuilder builder, double DoubleProp) { builder.AddDouble(7, DoubleProp, 0); }
  public static Offset<TestTable3> EndTestTable3(FlatBufferBuilder builder) {
    int o = builder.EndObject();
    return new Offset<TestTable3>(o);
  }
};


}
