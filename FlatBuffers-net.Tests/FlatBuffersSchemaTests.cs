using System;
using System.Collections.Generic;
using System.Diagnostics;
using System.IO;
using System.Linq;
using System.Text;
using FlatBuffers.Tests.TestTypes;
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

        public class Table1
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
        public struct Level1StructWithStructArrayDep
        {
            public RootStruct1[] StructArrayProp { get; set; }
            public List<RootStruct1> StructListProp { get; set; }
        }
        public struct Level1StructWithTableArrayDep
        {
            public Table1[] TableArrayProp { get; set; }
            public List<Table1> TableListProp { get; set; }
        }

        public struct Level1StructWithByteArrayDep
        {
            public byte[] ByteArrayProp { get; set; }
            public List<byte> ByteListProp { get; set; }
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
        public void AddType_When1Level1StructWihStructArrayDep_CollectsDependencies()
        {
            var generator = new FlatBuffersSchemaGenerator();
            var schema = generator.Generate<Level1StructWithStructArrayDep>();

            var allTypes = schema.AllTypes.ToList();

            Assert.IsTrue(allTypes.Any(i => i.TypeModel.Name == "Level1StructWithStructArrayDep"));
            Assert.IsTrue(allTypes.Any(i => i.TypeModel.Name == "RootStruct1"));
            Assert.AreEqual(2, allTypes.Count);
        }

        [Test]
        public void AddType_WhenLevel1StructWithTableArrayDep_CollectsDependencies()
        {
            var generator = new FlatBuffersSchemaGenerator();
            var schema = generator.Generate<Level1StructWithTableArrayDep>();

            var allTypes = schema.AllTypes.ToList();

            Assert.IsTrue(allTypes.Any(i => i.TypeModel.Name == "Level1StructWithTableArrayDep"));
            Assert.IsTrue(allTypes.Any(i => i.TypeModel.Name == "Table1"));
            Assert.AreEqual(2, allTypes.Count);
        }

        [Test]
        public void AddType_WhenLevel1StructWithByteArrayDep_CollectsDependencies()
        {
            var generator = new FlatBuffersSchemaGenerator();
            var schema = generator.Generate<Level1StructWithByteArrayDep>();

            var allTypes = schema.AllTypes.ToList();

            Assert.IsTrue(allTypes.Any(i => i.TypeModel.Name == "Level1StructWithByteArrayDep"));
            Assert.AreEqual(1, allTypes.Count);
        }

        [Test]
        public void WriteTo_Level1StructWithStructArrayDep_ResolvesDependenciesInCorrectOrder()
        {
            var generator = new FlatBuffersSchemaGenerator();
            var schema = generator.Generate<Level1StructWithStructArrayDep>();

            var sb = new StringBuilder();
            using (var sw = new StringWriter(sb))
            {
                schema.WriteTo(sw);
            }

            var expected = "struct RootStruct1 {\n" +
                           "    PropA:int;\n" +
                           "}\n" +
                           "struct Level1StructWithStructArrayDep {\n" +
                           "    StructArrayProp:[RootStruct1];\n" +
                           "    StructListProp:[RootStruct1];\n" +
                           "}";

            AssertExtensions.AreEquivalent(expected, sb.ToString());
        }

        [Test]
        public void WriteTo_Level1StructWithTableArrayDep_ResolvesDependenciesInCorrectOrder()
        {
            var generator = new FlatBuffersSchemaGenerator();
            var schema = generator.Generate<Level1StructWithTableArrayDep>();

            var sb = new StringBuilder();
            using (var sw = new StringWriter(sb))
            {
                schema.WriteTo(sw);
            }

            var expected = "table Table1 {\n" +
                           "    PropA:int;\n" +
                           "}\n" +
                           "struct Level1StructWithTableArrayDep {\n" +
                           "    TableArrayProp:[Table1];\n" +
                           "    TableListProp:[Table1];\n" +
                           "}";

            AssertExtensions.AreEquivalent(expected, sb.ToString());
        }

        [Test]
        public void WriteTo_Level1StructWithByteArrayDep_WritesExpectedSchema()
        {
            var generator = new FlatBuffersSchemaGenerator();
            var schema = generator.Generate<Level1StructWithByteArrayDep>();

            var sb = new StringBuilder();
            using (var sw = new StringWriter(sb))
            {
                schema.WriteTo(sw);
            }

            var expected = "struct Level1StructWithByteArrayDep {\n" +
                           "    ByteArrayProp:[ubyte];\n" +
                           "    ByteListProp:[ubyte];\n" +
                           "}";

            AssertExtensions.AreEquivalent(expected, sb.ToString());
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

            AssertExtensions.AreEquivalent(expected, sb.ToString());
        }

        [Test]
        public void WriteTo_WhenNestedFlatBuffer_ResolvesDependenciesInCorrectOrder()
        {
            var generator = new FlatBuffersSchemaGenerator();
            var schema = generator.Generate<TestTableWithNestedTestTable1>();

            var sb = new StringBuilder();
            using (var sw = new StringWriter(sb))
            {
                schema.WriteTo(sw);
            }

            var expected = "table TestTable1 {\n" +
                            "    IntProp:int;\n" +
                            "    ByteProp:ubyte;\n" +
                            "    ShortProp:short;\n" +
                            "}\n" +
                            "table TestTableWithNestedTestTable1 {\n" +
                            "    IntProp:int;\n" +
                            "    Nested:[ubyte] (nested_flatbuffer: \"TestTable1\");\n" +
                            "}";

            AssertExtensions.AreEquivalent(expected, sb.ToString());
        }

        [Test]
        public void WriteTo_WhenRootTypeSet_RootTypeIsWritten()
        {
            var generator = new FlatBuffersSchemaGenerator();
            var schema = generator.Generate<Level1StructWithEnumDep>(true);

            var sb = new StringBuilder();
            using (var sw = new StringWriter(sb))
            {
                schema.WriteTo(sw);
            }

            var expected = "enum RootEnum : int {\n" +
                            "    Apples,\n" +
                            "    Pears\n" +
                            "}\n" +
                            "struct Level1StructWithEnumDep {\n" +
                            "    EnumProp:RootEnum;\n" +
                            "}\n" +
                            "root_type Level1StructWithEnumDep;";

            AssertExtensions.AreEquivalent(expected, sb.ToString());
        }

        [Test]
        public void WriteTo_WhenRootTypeSetAndTypeHasAnIdentifier_RootTypeAndIdentifierIsWritten()
        {
            var generator = new FlatBuffersSchemaGenerator();
            var schema = generator.Generate<TestTableWithIdentifier>(true);

            var sb = new StringBuilder();
            using (var sw = new StringWriter(sb))
            {
                schema.WriteTo(sw);
            }

            var expected = "table TestTableWithIdentifier {\n" +
                            "    IntProp:int;\n" +
                            "}\n" +
                            "root_type TestTableWithIdentifier;\n" +
                            "file_identifier \"TEST\";";

            AssertExtensions.AreEquivalent(expected, sb.ToString());
        }

        [ExpectedException(typeof(FlatBuffersSchemaException), ExpectedMessage = "Type must be a Table or Struct type to be used as a root type")]
        [Test]
        public void WriteTo_WhenRootTypeSetAndIsNotStruct_ExceptionIsThrown()
        {
            var generator = new FlatBuffersSchemaGenerator();
            generator.Generate<RootEnum>(true);
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

            AssertExtensions.AreEquivalent(expected, sb.ToString());
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

            AssertExtensions.AreEquivalent(expected, sb.ToString());
        }

        [Test]
        public void WriteTo_WhenUserMetadataPresent_WritesAttributeDeclaration()
        {
            var generator = new FlatBuffersSchemaGenerator();
            var schema = generator.Generate<TableWithUserMetadata>();

            var sb = new StringBuilder();
            using (var sw = new StringWriter(sb))
            {
                schema.WriteTo(sw);
            }

            var expected =  "attribute \"category\";\n" +
                            "attribute \"priority\";\n" +
                            "attribute \"toggle\";\n" +
                            "attribute \"types\";\n" +
                            "\n" +
                            "table TableWithUserMetadata (types) {\n" +
                               "    PropA:int (priority: 1);\n" +
                               "    PropB:bool (toggle: true);\n" +
                               "    PropC:int (category: \"tests\");\n" +
                               "}";

            AssertExtensions.AreEquivalent(expected, sb.ToString());
        }

        [Test]
        public void WriteTo_WhenEnumUserMetadataPresent_WritesAttributeDeclaration()
        {
            var generator = new FlatBuffersSchemaGenerator();
            var schema = generator.Generate<EnumWithUserMetadata>();

            var sb = new StringBuilder();
            using (var sw = new StringWriter(sb))
            {
                schema.WriteTo(sw);
            }

            var expected = "attribute \"magicEnum\";\n" +
                            "\n" +
                            "enum EnumWithUserMetadata : int (magicEnum) {\n" +
                               "    Cat,\n" +
                               "    Dog,\n" +
                               "    Fish\n" +
                               "}";

            AssertExtensions.AreEquivalent(expected, sb.ToString());
        }

        [Test]
        public void WriteTo_WhenUnion_WritesSchemaWithUnion()
        {
            var generator = new FlatBuffersSchemaGenerator();
            var schema = generator.Generate<TestTableWithUnion>();

            var sb = new StringBuilder();
            using (var sw = new StringWriter(sb))
            {
                schema.WriteTo(sw);
            }

            var expected =  "union TestUnion {\n" +
                            "    TestTable1,\n" +
                            "    TestTable2\n" +
                            "}\n" +
                            "table TestTable1 {\n" +
                            "    IntProp:int;\n" +
                            "    ByteProp:ubyte;\n" +
                            "    ShortProp:short;\n" +
                            "}\n" +
                            "table TestTable2 {\n" +
                            "    StringProp:string;\n" +
                            "}\n"+
                            "table TestTableWithUnion {\n" +
                            "    IntProp:int;\n" +
                            "    UnionProp:TestUnion;\n" +
                            "}";

            AssertExtensions.AreEquivalent(expected, sb.ToString());
        }
    }
}