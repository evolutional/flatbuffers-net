using System.IO;
using System.Text;
using Microsoft.VisualStudio.TestTools.UnitTesting;

namespace FlatBuffers.Serialization.Tests
{
    public static class AssertExtensions
    {
        private static string NormalizeWhitespace(string value)
        {
            var lastChar = '\0';
            var sb = new StringBuilder();
            for (var i = 0; i < value.Length; ++i)
            {
                var c = value[i];
                if (c == '\n' || c == '\r')
                {
                    c = ' ';
                }

                if (char.IsWhiteSpace(lastChar) && char.IsWhiteSpace(c))
                {
                    lastChar = c;
                    continue;
                }
                
                sb.Append(c);
                lastChar = c;
            }
            return sb.ToString().Trim();
        }

        public static void AreEqual(string expected, string actual)
        {
            expected = NormalizeWhitespace(expected);
            actual = NormalizeWhitespace(actual);
            Assert.AreEqual(expected, actual);
        }
    }

    [TestClass]
    public class SchemaWriterTests
    {

        [TestMethod]
        public void Write_WithTestStruct1_EmitsCorrectSchemaFragment()
        {
            var typeModel = TypeModelRegistry.Default.GetTypeModel(typeof (TestStruct1));

            var sb = new StringBuilder();
            var sw = new StringWriter(sb);
            var schemaWriter = new FlatBuffersSchemaWriter(sw);
            schemaWriter.Write(typeModel);
            var expected = "struct TestStruct1 {\n" +
                           "    IntProp:int;\n" +
                           "    ByteProp:ubyte;\n" +
                           "    ShortProp:short;\n" +
                           "}";

            AssertExtensions.AreEqual(expected, sb.ToString());
        }

        [TestMethod]
        public void Write_WithTestTable1_EmitsCorrectSchemaFragment()
        {
            var typeModel = TypeModelRegistry.Default.GetTypeModel(typeof(TestTable1));

            var sb = new StringBuilder();
            var sw = new StringWriter(sb);
            var schemaWriter = new FlatBuffersSchemaWriter(sw);
            schemaWriter.Write(typeModel);
            var expected = "table TestTable1 {\n" +
                           "    IntProp:int;\n" +
                           "    ByteProp:ubyte;\n" +
                           "    ShortProp:short;\n" +
                           "}";

            AssertExtensions.AreEqual(expected, sb.ToString());
        }

        [TestMethod]
        public void Write_WithTestTable2_EmitsCorrectSchemaFragment()
        {
            var typeModel = TypeModelRegistry.Default.GetTypeModel(typeof(TestTable2));

            var sb = new StringBuilder();
            var sw = new StringWriter(sb);
            var schemaWriter = new FlatBuffersSchemaWriter(sw);
            schemaWriter.Write(typeModel);
            var expected = "table TestTable2 {\n" +
                           "    StringProp:string;\n" +
                           "}";

            AssertExtensions.AreEqual(expected, sb.ToString());
        }
    }
}