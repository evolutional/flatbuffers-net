using System;
using FlatBuffers.Attributes;

namespace FlatBuffers.Tests.TestTypes
{
    public enum TestEnum : byte
    {
        Apple,
        Orange,
        Pear,
        Banana,
    };

    [Flags]
    public enum TestFlagsEnum : byte
    {
        None = 0,
        Apple = 1<<0,
        Orange = 1<<1,
        Pear = 1<<2,
        Banana = 1<<3,
    };

    public enum TestEnumWithNoDeclaredBaseType
    {
        Apple,
        Orange,
        Pear,
        Banana,
    };

    public enum TestIntBasedEnum : int
    {
        Apple,
        Orange,
        Pear,
        Banana = Int32.MaxValue,
    };

    [FlatBuffersEnum(AutoSizeEnum = true)]
    public enum TestEnumAutoSizedToByte
    {
        Apple,
        Orange,
        Pear,
        Banana = byte.MaxValue,
    };

    [FlatBuffersEnum(AutoSizeEnum = true)]
    public enum TestEnumAutoSizedToSByte
    {
        Apple,
        Orange,
        Pear,
        Banana = sbyte.MaxValue,
    };

    [FlatBuffersEnum(AutoSizeEnum = true)]
    public enum TestEnumAutoSizedToShort
    {
        Apple,
        Orange,
        Pear,
        Banana = short.MaxValue,
    };

    [FlatBuffersEnum(AutoSizeEnum = true)]
    public enum TestEnumAutoSizedToUShort
    {
        Apple,
        Orange,
        Pear,
        Banana = ushort.MaxValue,
    };

    [FlatBuffersEnum(AutoSizeEnum = true)]
    public enum TestEnumWithExplictSizeNotAutoSized : short
    {
        Apple,
        Orange,
        Pear,
        Banana = byte.MaxValue,
    };
}