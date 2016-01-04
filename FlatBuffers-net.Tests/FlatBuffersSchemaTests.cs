using System;
using System.Collections.Generic;
using System.Diagnostics;
using System.IO;
using System.Linq;
using System.Text;
using NUnit.Framework;

namespace FlatBuffers.Tests
{
    [TestFixture]
    public class FlatBuffersSchemaTests
    {
        public enum RootEnum
        {
            Apples,
            Pears,
        }

        public struct RootStruct1
        {
            public int PropA { get; set; }
        }

        public struct Level1StructWithStructDep
        {
            public RootStruct1 StructProp { get; set; }
        }

        public struct Level1StructWithEnumDep
        {
            public RootEnum EnumProp { get; set; }
        }

        public struct Level2StructWithStructDep
        {
            public Level1StructWithStructDep StructProp { get; set; }
        }

        [Test]
        public void AddType_When2LevelStructChain_CollectsStructDependencies()
        {
            var generator = new FlatBuffersSchemaGenerator();
            var schema = generator.Generate<Level2StructWithStructDep>();

            var allTypes = schema.AllTypes.ToList();

            Assert.IsTrue(allTypes.Any(i => i.TypeModel.Name == "Level2StructWithStructDep"));
            Assert.IsTrue(allTypes.Any(i => i.TypeModel.Name == "Level1StructWithStructDep"));
            Assert.IsTrue(allTypes.Any(i => i.TypeModel.Name == "RootStruct1"));
            Assert.AreEqual(3, allTypes.Count);
        }

        [Test]
        public void AddType_When1LevelStructAndEnumChain_CollectsDependenciesIncludingEnum()
        {
            var generator = new FlatBuffersSchemaGenerator();
            var schema = generator.Generate<Level1StructWithEnumDep>();

            var allTypes = schema.AllTypes.ToList();

            Assert.IsTrue(allTypes.Any(i => i.TypeModel.Name == "Level1StructWithEnumDep"));
            Assert.IsTrue(allTypes.Any(i => i.TypeModel.Name == "RootEnum"));
            Assert.AreEqual(2, allTypes.Count);
        }

        [Test]
        public void WriteTo_When1LevelStructChainAndEnumChain_ResolvesDependenciesInCorrectOrder()
        {
            var generator = new FlatBuffersSchemaGenerator();
            var schema = generator.Generate<Level1StructWithEnumDep>();

            var sb = new StringBuilder();
            using (var sw = new StringWriter(sb))
            {
                schema.WriteTo(sw);
            }

            var expected =  "enum RootEnum : int {\n" +
                            "    Apples,\n" +
                            "    Pears\n" +
                            "}\n" +
                            "struct Level1StructWithEnumDep {\n" +
                            "    EnumProp:RootEnum;\n" +
                            "}";

            AssertExtensions.AreEqual(expected, sb.ToString());
        }

        [Test]
        public void WriteTo_When2LevelStructChain_ResolvesDependenciesInCorrectOrder()
        {
            var generator = new FlatBuffersSchemaGenerator();
            var schema = generator.Generate<Level2StructWithStructDep>();

            var sb = new StringBuilder();
            using (var sw = new StringWriter(sb))
            {
                schema.WriteTo(sw);
            }

            var expected = "struct RootStruct1 {\n" +
                            "    PropA:int;\n" +
                            "}\n" +
                            "struct Level1StructWithStructDep {\n" +
                            "    StructProp:RootStruct1;\n" +
                            "}\n" +
                            "struct Level2StructWithStructDep {\n" +
                            "    StructProp:Level1StructWithStructDep;\n" +
                            "}";

            AssertExtensions.AreEqual(expected, sb.ToString());
        }

        [Test]
        public void WriteTo_WhenManyLeafTypes_ResolvesDependenciesInCorrectOrder()
        {
            var generator = new FlatBuffersSchemaGenerator();
            var schema = generator.Create();

            schema.AddType<Level2StructWithStructDep>();
            schema.AddType<Level1StructWithEnumDep>();

            var sb = new StringBuilder();
            using (var sw = new StringWriter(sb))
            {
                schema.WriteTo(sw);
            }

            var expected = "enum RootEnum : int {\n" +
                            "    Apples,\n" +
                            "    Pears\n" +
                            "}\n" + 
                            "struct RootStruct1 {\n" +
                            "    PropA:int;\n" +
                            "}\n" +
                            "struct Level1StructWithEnumDep {\n" +
                            "    EnumProp:RootEnum;\n" +
                            "}\n" +
                            "struct Level1StructWithStructDep {\n" +
                            "    StructProp:RootStruct1;\n" +
                            "}\n" +
                            "struct Level2StructWithStructDep {\n" +
                            "    StructProp:Level1StructWithStructDep;\n" +
                            "}";

            AssertExtensions.AreEqual(expected, sb.ToString());
        }
    }
}