using System;

namespace FlatBuffers.Tests
{
    public enum TestEnum : byte
    {
        Apple,
        Orange,
        Pear,
        Banana,
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
}