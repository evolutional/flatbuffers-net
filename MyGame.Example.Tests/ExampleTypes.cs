using FlatBuffers.Attributes;

namespace MyGame.Example.Tests
{
    /// <summary>
    /// Type from the monster_test.fbs file
    /// </summary>
    public enum Color : byte
    {
        Red = 1,
        Green,
        Blue = 3
    }

    /// <summary>
    /// Type from the monster_test.fbs file
    /// </summary>
    public struct Test
    {
        public short A { get; set; }
        public sbyte B { get; set; }
    }

    /// <summary>
    /// Type from the monster_test.fbs file
    /// </summary>
    [FlatBuffersStruct(ForceAlign = 16)]
    public struct Vec3
    {
        public float X { get; set; }
        public float Y { get; set; }
        public float Z { get; set; }
        public double Test1 { get; set; }
        public Color Test2 { get; set; }
        public Test Test3 { get; set; }
    }
}
