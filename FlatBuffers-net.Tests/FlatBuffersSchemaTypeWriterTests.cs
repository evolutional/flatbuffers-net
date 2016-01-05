using System.IO;
using System.Text;
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

                AssertExtensions.AreEqual(expected, sb.ToString());
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

                AssertExtensions.AreEqual(expected, sb.ToString());
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

                AssertExtensions.AreEqual(expected, sb.ToString());
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

                AssertExtensions.AreEqual(expected, sb.ToString());
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

                AssertExtensions.AreEqual(expected, sb.ToString());
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

                AssertExtensions.AreEqual(expected, sb.ToString());
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
                               "   Apple,\n" +
                               "   Orange,\n" +
                               "   Pear,\n" +
                               "   Banana\n" +
                               "}";

                AssertExtensions.AreEqual(expected, sb.ToString());
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

                AssertExtensions.AreEqual(expected, sb.ToString());
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

                AssertExtensions.AreEqual(expected, sb.ToString());
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

                AssertExtensions.AreEqual(expected, sb.ToString());
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

                AssertExtensions.AreEqual(expected, sb.ToString());
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

                AssertExtensions.AreEqual(expected, sb.ToString());
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

                AssertExtensions.AreEqual(expected, sb.ToString());
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

                AssertExtensions.AreEqual(expected, sb.ToString());
            }
        }
    }
}