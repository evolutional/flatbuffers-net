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
    [FlatBuffersUnion]
    public enum Any
    {
        [FlatBuffersUnionMember(typeof(Monster))]
        Monster,
        [FlatBuffersUnionMember(typeof(TestSimpleTableWithEnum))]
        TestSimpleTableWithEnum,
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
    public class TestSimpleTableWithEnum
    {
        [FlatBuffersDefaultValue(Color.Green)]
        public Color Color { get; set; }
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

    /// <summary>
    /// Type from the monster_test.fbs file
    /// </summary>
    public class Stat
    {
        public string Id { get; set; }
        public long Val { get; set; }
        public ushort Count { get; set; }
    }

    /// <summary>
    /// Type from the monster_test.fbs file
    /// </summary>
    [FlatBuffersTable(Identifier = "MONS")]
    public class Monster
    {
        [FlatBuffersField(Id = 0)]
        public Vec3 Pos { get; set; }

        [FlatBuffersDefaultValue(100)]
        [FlatBuffersField(Id = 2)]
        public short Hp { get; set; }

        [FlatBuffersDefaultValue(150)]
        [FlatBuffersField(Id = 1)]
        public short Mana { get; set; }

        [FlatBuffersField(Id = 3, Required = true, Key = true)]
        public string Name { get; set; }

        [FlatBuffersDefaultValue(Color.Blue)]
        [FlatBuffersField(Id = 6)]
        public Color Color { get; set; }

        [FlatBuffersField(Id = 5)]
        public sbyte[] Inventory { get; set; }

        [FlatBuffersDefaultValue(false)]
        [FlatBuffersMetadata("priority", 1)]
        [FlatBuffersField(Id = 4, Deprecated = true)]
        public bool Friendly { get; set; }

        [FlatBuffersField(Id = 11)]
        public Monster[] TestArrayOfTables { get; set; }

        [FlatBuffersField(Id = 10)]
        public string[] TestArrayOfString { get; set; }

        [FlatBuffersField(Id = 24)]
        public bool[] TestArrayOfBools { get; set; }

        [FlatBuffersField(Id = 12)]
        public Monster Enemy { get; set; }

        [FlatBuffersField(Id = 8, UnionType = typeof(Any))]
        public object Test { get; set; }

        [FlatBuffersField(Id = 9, UnionType = typeof(Any))]
        public object[] Test4 { get; set; }

        [FlatBuffersField(Id = 13, NestedFlatBufferType = typeof(Monster))]
        public object TestNestedFlatBuffer { get; set; }

        [FlatBuffersField(Id = 14)]
        public Stat TestEmpty { get; set; }

        [FlatBuffersField(Id = 15)]
        public bool TestBool { get; set; }

        [FlatBuffersField(Id = 16, Hash = FlatBuffersHash.Fnv1_32)]
        public int TestHashs32Fnv1 { get; set; }

        [FlatBuffersField(Id = 17, Hash = FlatBuffersHash.Fnv1_32)]
        public uint TestHashu32Fnv1 { get; set; }

        [FlatBuffersField(Id = 18, Hash = FlatBuffersHash.Fnv1_64)]
        public long TestHashs64Fnv1 { get; set; }

        [FlatBuffersField(Id = 19, Hash = FlatBuffersHash.Fnv1_64)]
        public ulong TestHashu64Fnv1 { get; set; }

        [FlatBuffersField(Id = 20, Hash = FlatBuffersHash.Fnv1a_32)]
        public int TestHashs32Fnv1a { get; set; }

        [FlatBuffersField(Id = 21, Hash = FlatBuffersHash.Fnv1a_32)]
        public uint TestHashu32Fnv1a { get; set; }

        [FlatBuffersField(Id = 22, Hash = FlatBuffersHash.Fnv1a_64)]
        public long TestHashs64Fnv1a { get; set; }

        [FlatBuffersField(Id = 23, Hash = FlatBuffersHash.Fnv1a_64)]
        public ulong TestHashu64Fnv1a { get; set; }
    }
}
