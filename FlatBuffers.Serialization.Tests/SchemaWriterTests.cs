using System.IO;
using System.Text;
using NUnit.Framework;

namespace FlatBuffers.Serialization.Tests
{
    [TestFixture]
    public class SchemaWriterTests
    {

        [Test]
        public void Write_WithTestStruct1_EmitsCorrectSchemaFragment()
        {
            var sb = new StringBuilder();
            using (var sw = new StringWriter(sb))
            {
                var schemaWriter = new FlatBuffersSchemaWriter(sw);
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
                var schemaWriter = new FlatBuffersSchemaWriter(sw);
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
        public void Write_WithTestTable2_EmitsCorrectSchemaFragment()
        {
            var sb = new StringBuilder();
            using (var sw = new StringWriter(sb))
            {
                var schemaWriter = new FlatBuffersSchemaWriter(sw);
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
                var schemaWriter = new FlatBuffersSchemaWriter(sw);
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
                var schemaWriter = new FlatBuffersSchemaWriter(sw);
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
                var schemaWriter = new FlatBuffersSchemaWriter(sw);
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
                var schemaWriter = new FlatBuffersSchemaWriter(sw);
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
                var schemaWriter = new FlatBuffersSchemaWriter(sw);
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
    }
}