using System.Collections.Generic;
using System.IO;
using System.Text;
using FlatBuffers.Attributes;
using FlatBuffers.Tests.TestTypes;
using NUnit.Framework;

namespace FlatBuffers.Tests
{
    [TestFixture]
    public class FlatBuffersSchemaTypeWriterTests
    {

        [Test]
        public void Write_WithTestStruct1_EmitsCorrectSchemaFragment()
        {
            var sb = new StringBuilder();
            using (var sw = new StringWriter(sb))
            {
                var schemaWriter = new FlatBuffersSchemaTypeWriter(sw);
                schemaWriter.Write<TestStruct1>();
                var expected = "struct TestStruct1 {\n" +
                               "    IntProp:int;\n" +
                               "    ByteProp:ubyte;\n" +
                               "    ShortProp:short;\n" +
                               "}";

                AssertExtensions.AreEquivalent(expected, sb.ToString());
            }
        }

        [Test]
        public void Write_WithTestStruct1_And2SpacesForIndentation_EmitsCorrectSchemaFragment()
        {
            var sb = new StringBuilder();
            using (var sw = new StringWriter(sb))
            {
                var options = new FlatBuffersSchemaTypeWriterOptions {IndentCount = 2};
                var schemaWriter = new FlatBuffersSchemaTypeWriter(sw, options);
                schemaWriter.Write<TestStruct1>();
                var expected = "struct TestStruct1 {\r\n" +
                               "  IntProp:int;\r\n" +
                               "  ByteProp:ubyte;\r\n" +
                               "  ShortProp:short;\r\n" +
                               "}\r\n";

                Assert.AreEqual(expected, sb.ToString());
            }
        }

        [Test]
        public void Write_WithTestStruct1_AndLfLineTerminator_EmitsCorrectSchemaFragment()
        {
            var sb = new StringBuilder();
            using (var sw = new StringWriter(sb))
            {
                var options = new FlatBuffersSchemaTypeWriterOptions { LineTerminator = FlatBuffersSchemaWriterLineTerminatorType.Lf};
                var schemaWriter = new FlatBuffersSchemaTypeWriter(sw, options);
                schemaWriter.Write<TestStruct1>();
                var expected = "struct TestStruct1 {\n" +
                               "    IntProp:int;\n" +
                               "    ByteProp:ubyte;\n" +
                               "    ShortProp:short;\n" +
                               "}\n";

                Assert.AreEqual(expected, sb.ToString());
            }
        }

        [Test]
        public void Write_WithTestStruct1_And1TabForIndentation_EmitsCorrectSchemaFragment()
        {
            var sb = new StringBuilder();
            using (var sw = new StringWriter(sb))
            {
                var options = new FlatBuffersSchemaTypeWriterOptions { IndentCount = 1, IndentType = FlatBuffersSchemaWriterIndentType.Tabs };
                var schemaWriter = new FlatBuffersSchemaTypeWriter(sw, options);
                schemaWriter.Write<TestStruct1>();
                var expected = "struct TestStruct1 {\r\n" +
                               "\tIntProp:int;\r\n" +
                               "\tByteProp:ubyte;\r\n" +
                               "\tShortProp:short;\r\n" +
                               "}\r\n";

                Assert.AreEqual(expected, sb.ToString());
            }
        }

        [Test]
        public void Write_WithTestStruct1_AndNewLineBracing_EmitsCorrectSchemaFragment()
        {
            var sb = new StringBuilder();
            using (var sw = new StringWriter(sb))
            {
                var options = new FlatBuffersSchemaTypeWriterOptions { BracingStyle = FlatBuffersSchemaWriterBracingStyle.NewLine };
                var schemaWriter = new FlatBuffersSchemaTypeWriter(sw, options);
                schemaWriter.Write<TestStruct1>();
                var expected = "struct TestStruct1\r\n" +
                               "{\r\n" +
                               "    IntProp:int;\r\n" +
                               "    ByteProp:ubyte;\r\n" +
                               "    ShortProp:short;\r\n" +
                               "}\r\n";

                Assert.AreEqual(expected, sb.ToString());
            }
        }

        [Test]
        public void Write_WithTestTable1_EmitsCorrectSchemaFragment()
        {
            var sb = new StringBuilder();
            using (var sw = new StringWriter(sb))
            {
                var schemaWriter = new FlatBuffersSchemaTypeWriter(sw);
                schemaWriter.Write<TestTable1>();
                var expected = "table TestTable1 {\n" +
                               "    IntProp:int;\n" +
                               "    ByteProp:ubyte;\n" +
                               "    ShortProp:short;\n" +
                               "}";

                AssertExtensions.AreEquivalent(expected, sb.ToString());
            }
        }

        [Test]
        public void Write_WithTestTableWithDefaults_EmitsCorrectSchemaFragment()
        {
            var sb = new StringBuilder();
            using (var sw = new StringWriter(sb))
            {
                var schemaWriter = new FlatBuffersSchemaTypeWriter(sw);
                schemaWriter.Write<TestTableWithDefaults>();
                var expected = "table TestTableWithDefaults {\n" +
                               "    IntProp:int = 123456;\n" +
                               "    ByteProp:ubyte = 42;\n" +
                               "    ShortProp:short = 1024;\n" +
                               "}";

                AssertExtensions.AreEquivalent(expected, sb.ToString());
            }
        }

        [Test]
        public void Write_WithTestTableWithOriginalOrdering_EmitsCorrectSchemaFragment()
        {
            var sb = new StringBuilder();
            using (var sw = new StringWriter(sb))
            {
                var schemaWriter = new FlatBuffersSchemaTypeWriter(sw);
                schemaWriter.Write<TestTableWithOriginalOrdering>();
                var expected = "table TestTableWithOriginalOrdering (original_order) {\n" +
                               "    IntProp:int;\n" +
                               "    ByteProp:ubyte;\n" +
                               "    ShortProp:short;\n" +
                               "}";

                AssertExtensions.AreEquivalent(expected, sb.ToString());
            }
        }

        [Test]
        public void Write_WithTestTableWithUserOrdering_EmitsCorrectSchemaFragment()
        {
            var sb = new StringBuilder();
            using (var sw = new StringWriter(sb))
            {
                var schemaWriter = new FlatBuffersSchemaTypeWriter(sw);
                schemaWriter.Write<TestTableWithUserOrdering>();
                var expected = "table TestTableWithUserOrdering {\n" +
                               "    IntProp:int (id: 2);\n" +
                               "    ByteProp:ubyte (id: 0);\n" +
                               "    ShortProp:short (id: 1);\n" +
                               "}";

                AssertExtensions.AreEquivalent(expected, sb.ToString());
            }
        }

        [Test]
        public void Write_WithTestTableWithDeprecatedField_EmitsCorrectSchemaFragment()
        {
            var sb = new StringBuilder();
            using (var sw = new StringWriter(sb))
            {
                var schemaWriter = new FlatBuffersSchemaTypeWriter(sw);
                schemaWriter.Write<TestTableWithDeprecatedField>();
                var expected = "table TestTableWithDeprecatedField {\n" +
                               "    IntProp:int;\n" +
                               "    ByteProp:ubyte (deprecated);\n" +
                               "    ShortProp:short;\n" +
                               "}";

                AssertExtensions.AreEquivalent(expected, sb.ToString());
            }
        }

        [Test]
        public void Write_WithTestTable2_EmitsCorrectSchemaFragment()
        {
            var sb = new StringBuilder();
            using (var sw = new StringWriter(sb))
            {
                var schemaWriter = new FlatBuffersSchemaTypeWriter(sw);
                schemaWriter.Write<TestTable2>();
                var expected = "table TestTable2 {\n" +
                               "    StringProp:string;\n" +
                               "}";

                AssertExtensions.AreEquivalent(expected, sb.ToString());
            }
        }

        [Test]
        public void Write_WithTestEnum_EmitsCorrectSchemaFragment()
        {
            var sb = new StringBuilder();
            using (var sw = new StringWriter(sb))
            {
                var schemaWriter = new FlatBuffersSchemaTypeWriter(sw);
                schemaWriter.Write<TestEnum>();
                var expected = "enum TestEnum : ubyte {\n" +
                               "   Apple,\n" +
                               "   Orange,\n" +
                               "   Pear,\n" +
                               "   Banana\n" +
                               "}";

                AssertExtensions.AreEquivalent(expected, sb.ToString());
            }
        }

        [Test]
        public void Write_WithTestFlagsEnum_EmitsCorrectSchemaFragment()
        {
            var sb = new StringBuilder();
            using (var sw = new StringWriter(sb))
            {
                var schemaWriter = new FlatBuffersSchemaTypeWriter(sw);
                schemaWriter.Write<TestFlagsEnum>();
                var expected = "enum TestFlagsEnum : ubyte (bit_flags) {\n" +
                               "   None = 0,\n" +
                               "   Apple = 1,\n" +
                               "   Orange = 2,\n" +
                               "   Pear = 4,\n" +
                               "   Banana = 8\n" +
                               "}";

                AssertExtensions.AreEquivalent(expected, sb.ToString());
            }
        }

        [Test]
        public void Write_WithTestEnumWithNoDeclaredBaseType_EmitsCorrectSchemaFragment()
        {
            var sb = new StringBuilder();
            using (var sw = new StringWriter(sb))
            {
                var schemaWriter = new FlatBuffersSchemaTypeWriter(sw);
                schemaWriter.Write<TestEnumWithNoDeclaredBaseType>();
                var expected = "enum TestEnumWithNoDeclaredBaseType : int {\n" +
                               "    Apple,\n" +
                               "    Orange,\n" +
                               "    Pear,\n" +
                               "    Banana\n" +
                               "}";

                AssertExtensions.AreEquivalent(expected, sb.ToString());
            }
        }

        [Test]
        public void Write_WithTestEnumWithNoDeclaredBaseType_AndNewLineBracing_EmitsCorrectSchemaFragment()
        {
            var sb = new StringBuilder();
            using (var sw = new StringWriter(sb))
            {
                var options = new FlatBuffersSchemaTypeWriterOptions
                {
                    BracingStyle = FlatBuffersSchemaWriterBracingStyle.NewLine
                };
                var schemaWriter = new FlatBuffersSchemaTypeWriter(sw, options);
                schemaWriter.Write<TestEnumWithNoDeclaredBaseType>();
                var expected = "enum TestEnumWithNoDeclaredBaseType : int\r\n" +
                               "{\r\n" +
                               "    Apple,\r\n" +
                               "    Orange,\r\n" +
                               "    Pear,\r\n" +
                               "    Banana\r\n" +
                               "}\r\n";

                Assert.AreEqual(expected, sb.ToString());
            }
        }

        [Test]
        public void Write_WithTestEnumWithNoDeclaredBaseType_And1TabIndentation_EmitsCorrectSchemaFragment()
        {
            var sb = new StringBuilder();
            using (var sw = new StringWriter(sb))
            {
                var options = new FlatBuffersSchemaTypeWriterOptions
                {
                    IndentType = FlatBuffersSchemaWriterIndentType.Tabs,
                    IndentCount = 1
                };
                var schemaWriter = new FlatBuffersSchemaTypeWriter(sw, options);
                schemaWriter.Write<TestEnumWithNoDeclaredBaseType>();
                var expected = "enum TestEnumWithNoDeclaredBaseType : int {\r\n" +
                               "\tApple,\r\n" +
                               "\tOrange,\r\n" +
                               "\tPear,\r\n" +
                               "\tBanana\r\n" +
                               "}\r\n";

                Assert.AreEqual(expected, sb.ToString());
            }
        }

        [Test]
        public void Write_WithTestIntBasedEnum_EmitsCorrectSchemaFragment()
        {
            var sb = new StringBuilder();
            using (var sw = new StringWriter(sb))
            {
                var schemaWriter = new FlatBuffersSchemaTypeWriter(sw);
                schemaWriter.Write<TestIntBasedEnum>();
                var expected = "enum TestIntBasedEnum : int {\n" +
                               "   Apple,\n" +
                               "   Orange,\n" +
                               "   Pear,\n" +
                               "   Banana = " + int.MaxValue + "\n" +
                               "}";

                AssertExtensions.AreEquivalent(expected, sb.ToString());
            }
        }

        [Test]
        public void Write_WithAutoSizedEnum_EmitsCorrectSchemaFragment()
        {
            var sb = new StringBuilder();
            using (var sw = new StringWriter(sb))
            {
                var schemaWriter = new FlatBuffersSchemaTypeWriter(sw);
                schemaWriter.Write<TestEnumAutoSizedToByte>();
                var expected = "enum TestEnumAutoSizedToByte : ubyte {\n" +
                               "   Apple,\n" +
                               "   Orange,\n" +
                               "   Pear,\n" +
                               "   Banana = " + byte.MaxValue + "\n" +
                               "}";

                AssertExtensions.AreEquivalent(expected, sb.ToString());
            }
        }

        [Test]
        public void Write_WithTestEnumWithExplicitValues_EmitsCorrectSchemaFragment()
        {
            var sb = new StringBuilder();
            using (var sw = new StringWriter(sb))
            {
                var schemaWriter = new FlatBuffersSchemaTypeWriter(sw);
                schemaWriter.Write<TestEnumWithExplicitValues>();
                var expected = "enum TestEnumWithExplicitValues : ubyte {\n" +
                               "   Banana,\n" +
                               "   Orange,\n" +
                               "   Apple,\n" +
                               "   Pear = 5\n" +
                               "}";

                AssertExtensions.AreEquivalent(expected, sb.ToString());
            }
        }

        [Test]
        public void Write_WithTestEnumWithExplicitNonContigValues_EmitsCorrectSchemaFragment()
        {
            var sb = new StringBuilder();
            using (var sw = new StringWriter(sb))
            {
                var schemaWriter = new FlatBuffersSchemaTypeWriter(sw);
                schemaWriter.Write<TestEnumWithExplicitNonContigValues>();
                var expected = "enum TestEnumWithExplicitNonContigValues : ubyte {\n" +
                               "   Orange = 1,\n" +
                               "   Apple = 2,\n" +
                               "   Pear = 5,\n" +
                               "   Banana = 9\n" +
                               "}";

                AssertExtensions.AreEquivalent(expected, sb.ToString());
            }
        }

        [Test]
        public void Write_WithTestStructWithEnum_EmitsCorrectSchemaFragment_ContainingEnumName()
        {
            var sb = new StringBuilder();
            using (var sw = new StringWriter(sb))
            {
                var schemaWriter = new FlatBuffersSchemaTypeWriter(sw);
                schemaWriter.Write<TestStructWithEnum>();
                var expected = "struct TestStructWithEnum {\n" +
                               "    EnumProp:TestEnum;\n" +
                               "}";

                AssertExtensions.AreEquivalent(expected, sb.ToString());
            }
        }

        [Test]
        public void Write_WithTestStruct2_EmitsCorrectSchemaFragment_ContainingStructPropName()
        {
            var sb = new StringBuilder();
            using (var sw = new StringWriter(sb))
            {
                var schemaWriter = new FlatBuffersSchemaTypeWriter(sw);
                schemaWriter.Write<TestStruct2>();
                var expected = "struct TestStruct2 {\n" +
                               "    IntProp:int;\n" +
                               "    TestStructProp:TestStruct1;\n" +
                               "}";

                AssertExtensions.AreEquivalent(expected, sb.ToString());
            }
        }

        [Test]
        public void Write_WithTestTableWithTable_EmitsCorrectSchemaFragment_ContainingTablePropName()
        {
            var sb = new StringBuilder();
            using (var sw = new StringWriter(sb))
            {
                var schemaWriter = new FlatBuffersSchemaTypeWriter(sw);
                schemaWriter.Write<TestTableWithTable>();
                var expected = "table TestTableWithTable {\n" +
                               "    TableProp:TestTable1;\n" +
                               "    IntProp:int;\n" +
                               "}";

                AssertExtensions.AreEquivalent(expected, sb.ToString());
            }
        }

        [Test]
        public void Write_WithTestTableWithArray_EmitsCorrectSchemaFragment_ContainingArrayFields()
        {
            var sb = new StringBuilder();
            using (var sw = new StringWriter(sb))
            {
                var schemaWriter = new FlatBuffersSchemaTypeWriter(sw);
                schemaWriter.Write<TestTableWithArray>();
                var expected = "table TestTableWithArray {\n" +
                               "    IntArray:[int];\n" +
                               "    IntList:[int];\n" +
                               "}";

                AssertExtensions.AreEquivalent(expected, sb.ToString());
            }
        }

        [Test]
        public void Write_TableWithRequiredFields_EmitsCorrectSchemaFragment_ContainingRequiredFields()
        {
            var sb = new StringBuilder();
            using (var sw = new StringWriter(sb))
            {
                var schemaWriter = new FlatBuffersSchemaTypeWriter(sw);
                schemaWriter.Write<TableWithRequiredFields>();
                var expected = "table TableWithRequiredFields {\n" +
                               "    StringProp:string (required);\n" +
                               "    TableProp:TestTable1 (required);\n" +
                               "    VectorProp:[int] (required);\n" +
                               "}";

                AssertExtensions.AreEquivalent(expected, sb.ToString());
            }
        }

        [Test]
        public void Write_TableWithAlternativeNameFields_EmitsCorrectSchemaFragment_ContainingAlternativeFieldNames()
        {
            var sb = new StringBuilder();
            using (var sw = new StringWriter(sb))
            {
                var schemaWriter = new FlatBuffersSchemaTypeWriter(sw);
                schemaWriter.Write<TableWithAlternativeNameFields>();
                var expected = "table TableWithAlternativeNameFields {\n" +
                               "    AltIntProp:int;\n" +
                               "    AltStringProp:string;\n" +
                               "}";

                AssertExtensions.AreEquivalent(expected, sb.ToString());
            }
        }

        private abstract class TableWithRequiredAndIndexedFields
        {
            [FlatBuffersField(Id = 1, Required = true)]
            public string StringProp { get; set; }
            [FlatBuffersField(Id = 0, Required = true)]
            public List<int> VectorProp { get; set; }
        }

        [Test]
        public void Write_TableWithRequiredAndIndexedFields_EmitsCorrectSchemaFragment()
        {
            var sb = new StringBuilder();
            using (var sw = new StringWriter(sb))
            {
                var schemaWriter = new FlatBuffersSchemaTypeWriter(sw);
                schemaWriter.Write<TableWithRequiredAndIndexedFields>();
                var expected = "table TableWithRequiredAndIndexedFields {\n" +
                               "    StringProp:string (id: 1, required);\n" +
                               "    VectorProp:[int] (id: 0, required);\n" +
                               "}";

                AssertExtensions.AreEquivalent(expected, sb.ToString());
            }
        }


        [Test]
        public void Write_StructWithForceAlign_EmitsCorrectSchemaFragment()
        {
            var sb = new StringBuilder();
            using (var sw = new StringWriter(sb))
            {
                var schemaWriter = new FlatBuffersSchemaTypeWriter(sw);
                schemaWriter.Write<TestStructWithForcedAlignment>();
                var expected = "struct TestStructWithForcedAlignment (force_align: 16) {\n" +
                               "    X:float;\n" +
                               "    Y:float;\n" +
                               "    Z:float;\n" +
                               "}";

                AssertExtensions.AreEquivalent(expected, sb.ToString());
            }
        }

        [Test]
        public void Write_StructWithForceAlign_AndNewLineBracing_EmitsCorrectSchemaFragment()
        {
            var sb = new StringBuilder();
            using (var sw = new StringWriter(sb))
            {
                var options = new FlatBuffersSchemaTypeWriterOptions
                {
                    BracingStyle = FlatBuffersSchemaWriterBracingStyle.NewLine
                };
                var schemaWriter = new FlatBuffersSchemaTypeWriter(sw, options);
                schemaWriter.Write<TestStructWithForcedAlignment>();
                var expected = "struct TestStructWithForcedAlignment (force_align: 16)\r\n" +
                               "{\r\n" +
                               "    X:float;\r\n" +
                               "    Y:float;\r\n" +
                               "    Z:float;\r\n" +
                               "}\r\n";

                Assert.AreEqual(expected, sb.ToString());
            }
        }

        [Test]
        public void Write_TableWithUserMetadata_EmitsCorrectSchemaFragment()
        {
            var sb = new StringBuilder();
            using (var sw = new StringWriter(sb))
            {
                var schemaWriter = new FlatBuffersSchemaTypeWriter(sw);
                schemaWriter.Write<TableWithUserMetadata>();
                var expected = "table TableWithUserMetadata (types) {\n" +
                               "    PropA:int (priority: 1);\n" +
                               "    PropB:bool (toggle: true);\n" +
                               "    PropC:int (category: \"tests\");\n" +
                               "}";

                AssertExtensions.AreEquivalent(expected, sb.ToString());
            }
        }

        [Test]
        public void Write_TableWithKeyField_EmitsCorrectSchemaFragment()
        {
            var sb = new StringBuilder();
            using (var sw = new StringWriter(sb))
            {
                var schemaWriter = new FlatBuffersSchemaTypeWriter(sw);
                schemaWriter.Write<TestTableWithKey>();
                var expected = "table TestTableWithKey {\n" +
                               "    IntProp:int (key);\n" +
                               "    OtherProp:int;\n" +
                               "}";

                AssertExtensions.AreEquivalent(expected, sb.ToString());
            }
        }

        [Test]
        public void Write_TableWithHashField_EmitsCorrectSchemaFragment()
        {
            var sb = new StringBuilder();
            using (var sw = new StringWriter(sb))
            {
                var schemaWriter = new FlatBuffersSchemaTypeWriter(sw);
                schemaWriter.Write<TestTableWithHash>();
                var expected = "table TestTableWithHash {\n" +
                               "    IntProp:int (hash: \"fnv1_32\");\n" +
                               "    OtherProp:int;\n" +
                               "}";

                AssertExtensions.AreEquivalent(expected, sb.ToString());
            }
        }

        [Test]
        public void Write_EnumWithUserMetadata_EmitsCorrectSchemaFragment()
        {
            var sb = new StringBuilder();
            using (var sw = new StringWriter(sb))
            {
                var schemaWriter = new FlatBuffersSchemaTypeWriter(sw);
                schemaWriter.Write<EnumWithUserMetadata>();
                var expected = "enum EnumWithUserMetadata : int (magicEnum) {\n" +
                               "    Cat,\n" +
                               "    Dog,\n" +
                               "    Fish\n" +
                               "}";

                AssertExtensions.AreEquivalent(expected, sb.ToString());
            }
        }

        [Test]
        public void Write_EnumWithUserMetadata_AndNewLineBracing_EmitsCorrectSchemaFragment()
        {
            var sb = new StringBuilder();
            using (var sw = new StringWriter(sb))
            {
                var options = new FlatBuffersSchemaTypeWriterOptions
                {
                    BracingStyle = FlatBuffersSchemaWriterBracingStyle.NewLine
                };
                var schemaWriter = new FlatBuffersSchemaTypeWriter(sw, options);
                schemaWriter.Write<EnumWithUserMetadata>();
                var expected = "enum EnumWithUserMetadata : int (magicEnum)\r\n" +
                               "{\r\n" +
                               "    Cat,\r\n" +
                               "    Dog,\r\n" +
                               "    Fish\r\n" +
                               "}\r\n";

                Assert.AreEqual(expected, sb.ToString());
            }
        }

        [Test]
        public void Write_Union_EmitsCorrectSchemaFragment()
        {
            var sb = new StringBuilder();
            using (var sw = new StringWriter(sb))
            {
                var schemaWriter = new FlatBuffersSchemaTypeWriter(sw);
                schemaWriter.Write<TestUnion>();
                var expected = "union TestUnion {\n" +
                               "    TestTable1,\n" +
                               "    TestTable2\n" +
                               "}";

                AssertExtensions.AreEquivalent(expected, sb.ToString());
            }
        }

        [Test]
        public void Write_UnionWithMetadata_EmitsCorrectSchemaFragment()
        {
            var sb = new StringBuilder();
            using (var sw = new StringWriter(sb))
            {
                var schemaWriter = new FlatBuffersSchemaTypeWriter(sw);
                schemaWriter.Write<TestUnionWithMetadata>();
                var expected = "union TestUnionWithMetadata (somemeta) {\n" +
                               "    TestTable1,\n" +
                               "    TestTable2\n" +
                               "}";

                AssertExtensions.AreEquivalent(expected, sb.ToString());
            }
        }

        [Test]
        public void Write_TestTableWithUnion_EmitsCorrectSchemaFragment()
        {
            var sb = new StringBuilder();
            using (var sw = new StringWriter(sb))
            {
                var schemaWriter = new FlatBuffersSchemaTypeWriter(sw);
                schemaWriter.Write<TestTableWithUnion>();
                var expected =  "table TestTableWithUnion {\n" +
                                "    IntProp:int;\n" +
                                "    UnionProp:TestUnion;\n" +
                                "}";
                AssertExtensions.AreEquivalent(expected, sb.ToString());
            }
        }

        [Test]
        public void Write_TestTableWithNestedTestTable1_EmitsCorrectSchemaFragment()
        {
            var sb = new StringBuilder();
            using (var sw = new StringWriter(sb))
            {
                var schemaWriter = new FlatBuffersSchemaTypeWriter(sw);
                schemaWriter.Write<TestTableWithNestedTestTable1>();
                var expected = "table TestTableWithNestedTestTable1 {\n" +
                                "    IntProp:int;\n" +
                                "    Nested:[ubyte] (nested_flatbuffer: \"TestTable1\");\n" +
                                "}";
                AssertExtensions.AreEquivalent(expected, sb.ToString());
            }
        }

        [Test]
        public void Write_TestTableWithComments_EmitsCorrectSchemaFragment()
        {
            var sb = new StringBuilder();
            using (var sw = new StringWriter(sb))
            {
                var schemaWriter = new FlatBuffersSchemaTypeWriter(sw);
                schemaWriter.Write<TestTableWithComments>();
                var expected =  "/// This is a comment on a table\n" +
                                "table TestTableWithComments {\n" +
                                "    /// Comment on an int field\n" +
                                "    Field:int;\n" +
                                "    /// First comment of Multiple comments\n" +
                                "    /// Second comment of Multiple comments\n" +
                                "    StringField:string;\n" +
                                "    /// Multiline\nIs supported\ntoo\n" +
                                "    AnotherField:int;\n" +
                                "}";
                AssertExtensions.AreEquivalent(expected, sb.ToString());
            }
        }

        [Test]
        public void Write_WithStructAndFieldNamingStyleCamelCase_EmitsCorrectSchemaFragment()
        {
            var sb = new StringBuilder();
            using (var sw = new StringWriter(sb))
            {
                var schemaWriter = new FlatBuffersSchemaTypeWriter(sw, new FlatBuffersSchemaTypeWriterOptions { FieldNamingStyle = FlatBuffersSchemaWriterNamingStyle.CamelCase });
                schemaWriter.Write<TestStruct1>();
                var expected = "struct TestStruct1 {\n" +
                               "    intProp:int;\n" +
                               "    byteProp:ubyte;\n" +
                               "    shortProp:short;\n" +
                               "}";

                AssertExtensions.AreEquivalent(expected, sb.ToString());
            }
        }

        [Test]
        public void Write_WithStructAndFieldNamingStyleLowerCase_EmitsCorrectSchemaFragment()
        {
            var sb = new StringBuilder();
            using (var sw = new StringWriter(sb))
            {
                var schemaWriter = new FlatBuffersSchemaTypeWriter(sw, new FlatBuffersSchemaTypeWriterOptions { FieldNamingStyle = FlatBuffersSchemaWriterNamingStyle.LowerCase });
                schemaWriter.Write<TestStruct1>();
                var expected = "struct TestStruct1 {\n" +
                               "    intprop:int;\n" +
                               "    byteprop:ubyte;\n" +
                               "    shortprop:short;\n" +
                               "}";

                AssertExtensions.AreEquivalent(expected, sb.ToString());
            }
        }

        [Test]
        public void Write_TestTableWithArrayOfStructs_EmitsCorrectSchemaFragment()
        {
            var sb = new StringBuilder();
            using (var sw = new StringWriter(sb))
            {
                var schemaWriter = new FlatBuffersSchemaTypeWriter(sw);
                schemaWriter.Write<TestTableWithArrayOfStructs>();
                var expected = "table TestTableWithArrayOfStructs {\n" +
                               "    StructArray:[TestStruct1];\n" +
                               "}";

                AssertExtensions.AreEquivalent(expected, sb.ToString());
            }
        }

        [Test]
        public void Write_WithTestTableWithArrayOfTables_EmitsCorrectSchemaFragment()
        {
            var sb = new StringBuilder();
            using (var sw = new StringWriter(sb))
            {
                var schemaWriter = new FlatBuffersSchemaTypeWriter(sw);
                schemaWriter.Write<TestTableWithArrayOfTables>();
                var expected = "table TestTableWithArrayOfTables {\n" +
                               "    TableArrayProp:[TestTable1];\n" +
                               "    TableListProp:[TestTable1];\n" +
                               "}";

                AssertExtensions.AreEquivalent(expected, sb.ToString());
            }
        }

        [Test]
        public void Write_WithTestTableWithArrayOfStrings_EmitsCorrectSchemaFragment()
        {
            var sb = new StringBuilder();
            using (var sw = new StringWriter(sb))
            {
                var schemaWriter = new FlatBuffersSchemaTypeWriter(sw);
                schemaWriter.Write<TestTableWithArrayOfStrings>();
                var expected = "table TestTableWithArrayOfStrings {\n" +
                               "    StringArrayProp:[string];\n" +
                               "    StringListProp:[string];\n" +
                               "}";

                AssertExtensions.AreEquivalent(expected, sb.ToString());
            }
        }

        [Test]
        public void Write_WithTestTableWithArrayOfBytes_EmitsCorrectSchemaFragment()
        {
            var sb = new StringBuilder();
            using (var sw = new StringWriter(sb))
            {
                var schemaWriter = new FlatBuffersSchemaTypeWriter(sw);
                schemaWriter.Write<TestTableWithArrayOfBytes>();
                var expected = "table TestTableWithArrayOfBytes {\n" +
                               "    ByteArrayProp:[ubyte];\n" +
                               "    ByteListProp:[ubyte];\n" +
                               "}";

                AssertExtensions.AreEquivalent(expected, sb.ToString());
            }
        }
    }
}