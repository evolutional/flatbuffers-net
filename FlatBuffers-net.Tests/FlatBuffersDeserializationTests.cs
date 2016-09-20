using System.Collections.Generic;
using System.Linq;
using FlatBuffers.Tests.TestTypes;
using NUnit.Framework;

namespace FlatBuffers.Tests
{
    [TestFixture]
    public class FlatBuffersDeserializationTests
    {
        [Test]
        public void Deserialize_FromOracleData_WithTestStruct1()
        {
            const int intProp = 42;
            const byte byteProp = 22;
            const short shortProp = 62;

            var oracle = new SerializationTestOracle();
            var oracleResult = oracle.GenerateTestStruct1(intProp, byteProp, shortProp);

            var serializer = new FlatBuffersSerializer();
            var o = serializer.Deserialize<TestStruct1>(oracleResult, 0, oracleResult.Length);

            Assert.AreEqual(intProp, o.IntProp);
            Assert.AreEqual(byteProp, o.ByteProp);
            Assert.AreEqual(shortProp, o.ShortProp);
        }

        [Test]
        public void Deserialize_FromOracleData_WithTestStruct2()
        {
            const int intProp = 42;

            var testStruct1 = new TestStruct1()
            {
                IntProp = 100,
                ByteProp = 50,
                ShortProp = 200
            };

            var oracle = new SerializationTestOracle();
            var oracleResult = oracle.GenerateTestStruct2(intProp, testStruct1);

            var serializer = new FlatBuffersSerializer();
            var o = serializer.Deserialize<TestStruct2>(oracleResult, 0, oracleResult.Length);

            Assert.AreEqual(intProp, o.IntProp);
            Assert.AreEqual(testStruct1.IntProp, o.TestStructProp.IntProp);
            Assert.AreEqual(testStruct1.ByteProp, o.TestStructProp.ByteProp);
            Assert.AreEqual(testStruct1.ShortProp, o.TestStructProp.ShortProp);
        }

        [Test]
        public void Deserialize_FromOracleData_WithTestTable1()
        {
            const int intProp = 42;
            const byte byteProp = 22;
            const short shortProp = 62;

            var oracle = new SerializationTestOracle();
            var oracleResult = oracle.GenerateTestTable1(intProp, byteProp, shortProp);

            var serializer = new FlatBuffersSerializer();
            var o = serializer.Deserialize<TestTable1>(oracleResult, 0, oracleResult.Length);

            Assert.AreEqual(intProp, o.IntProp);
            Assert.AreEqual(byteProp, o.ByteProp);
            Assert.AreEqual(shortProp, o.ShortProp);
        }

        [Test]
        public void Deserialize_FromOracleData_WithTestTable1UsingFields()
        {
            const int intProp = 42;
            const byte byteProp = 22;
            const short shortProp = 62;

            var oracle = new SerializationTestOracle();
            var oracleResult = oracle.GenerateTestTable1(intProp, byteProp, shortProp);

            var serializer = new FlatBuffersSerializer();
            var o = serializer.Deserialize<TestTable1UsingFields>(oracleResult, 0, oracleResult.Length);

            Assert.AreEqual(intProp, o.IntProp);
            Assert.AreEqual(byteProp, o.ByteProp);
            Assert.AreEqual(shortProp, o.ShortProp);
        }

        [Test]
        public void Deserialize_FromOracleData_WithTestTableWithDefaults()
        {
            const int intProp = 123456;
            const byte byteProp = 42;
            const short shortProp = 1024;

            var oracle = new SerializationTestOracle();
            var oracleResult = oracle.GenerateTestTableWithDefaults();

            var serializer = new FlatBuffersSerializer();
            var o = serializer.Deserialize<TestTableWithDefaults>(oracleResult, 0, oracleResult.Length);

            Assert.AreEqual(intProp, o.IntProp);
            Assert.AreEqual(byteProp, o.ByteProp);
            Assert.AreEqual(shortProp, o.ShortProp);
        }

        [Test]
        public void Deserialize_FromOracleData_WithTestTableWithUserOrdering()
        {
            const int intProp = 42;
            const byte byteProp = 22;
            const short shortProp = 62;

            var oracle = new SerializationTestOracle();
            var oracleResult = oracle.GenerateTestTableWithUserOrdering(intProp, byteProp, shortProp);

            var serializer = new FlatBuffersSerializer();
            var o = serializer.Deserialize<TestTableWithUserOrdering>(oracleResult, 0, oracleResult.Length);

            Assert.AreEqual(intProp, o.IntProp);
            Assert.AreEqual(byteProp, o.ByteProp);
            Assert.AreEqual(shortProp, o.ShortProp);
        }

        [Test]
        public void Deserialize_FromOracleData_WithTestTableWithOriginalOrdering()
        {
            const int intProp = 42;
            const byte byteProp = 22;
            const short shortProp = 62;

            var oracle = new SerializationTestOracle();
            var oracleResult = oracle.GenerateTestTableWithOriginalOrdering(intProp, byteProp, shortProp);

            var serializer = new FlatBuffersSerializer();
            var o = serializer.Deserialize<TestTableWithOriginalOrdering>(oracleResult, 0, oracleResult.Length);

            Assert.AreEqual(intProp, o.IntProp);
            Assert.AreEqual(byteProp, o.ByteProp);
            Assert.AreEqual(shortProp, o.ShortProp);
        }

        [Test]
        public void Deserialize_FromOracleData_WithTestTableWithOriginalOrdering_CompatibleWithTestTable1()
        {
            const int intProp = 42;
            const byte byteProp = 22;
            const short shortProp = 62;

            var oracle = new SerializationTestOracle();
            var oracleResult = oracle.GenerateTestTableWithOriginalOrdering(intProp, byteProp, shortProp);

            var serializer = new FlatBuffersSerializer();
            var o = serializer.Deserialize<TestTable1>(oracleResult, 0, oracleResult.Length);

            Assert.AreEqual(intProp, o.IntProp);
            Assert.AreEqual(byteProp, o.ByteProp);
            Assert.AreEqual(shortProp, o.ShortProp);
        }

        [Test]
        public void Deserialize_FromOracleData_WithTestTableWithDeprecatedField_CompatibleWithTestTable1()
        {
            const int intProp = 42;
            const short shortProp = 62;

            var oracle = new SerializationTestOracle();
            var oracleResult = oracle.GenerateTestTableWithDeprecatedField(intProp, shortProp);

            var serializer = new FlatBuffersSerializer();
            var o = serializer.Deserialize<TestTable1>(oracleResult, 0, oracleResult.Length);

            Assert.AreEqual(intProp, o.IntProp);
            Assert.AreNotEqual(TestTableWithDeprecatedField.DefaultBytePropValue, o.ByteProp);  // Should not equal the default field
            Assert.AreEqual(shortProp, o.ShortProp);
        }

        [Test]
        public void Deserialize_FromOracleData_WithTestTable1_CompatibleWithTestTableWithDeprecatedField()
        {
            const int intProp = 42;
            const byte byteProp = 22;
            const short shortProp = 62;

            var oracle = new SerializationTestOracle();
            var oracleResult = oracle.GenerateTestTable1(intProp, byteProp, shortProp);

            var serializer = new FlatBuffersSerializer();
            var o = serializer.Deserialize<TestTableWithDeprecatedField>(oracleResult, 0, oracleResult.Length);

            Assert.AreEqual(intProp, o.IntProp);
            Assert.AreEqual(TestTableWithDeprecatedField.DefaultBytePropValue, o.ByteProp); // set by deserializer
            Assert.AreEqual(shortProp, o.ShortProp);
        }

        [Test]
        public void Deserialize_FromOracleData_WithTestTableWithDeprecatedField()
        {
            const int intProp = 42;
            const short shortProp = 62;

            var oracle = new SerializationTestOracle();
            var oracleResult = oracle.GenerateTestTableWithDeprecatedField(intProp, shortProp);

            var serializer = new FlatBuffersSerializer();
            var o = serializer.Deserialize<TestTableWithDeprecatedField>(oracleResult, 0, oracleResult.Length);

            Assert.AreEqual(intProp, o.IntProp);
            Assert.AreEqual(TestTableWithDeprecatedField.DefaultBytePropValue, o.ByteProp); // no value
            Assert.AreEqual(shortProp, o.ShortProp);
        }

        [Test]
        public void Deserialize_FromOracleData_WithTestTable2()
        {
            const string stringProp = "Hello, FlatBuffers!";

            var oracle = new SerializationTestOracle();
            var oracleResult = oracle.GenerateTestTable2(stringProp);

            var serializer = new FlatBuffersSerializer();
            var o = serializer.Deserialize<TestTable2>(oracleResult, 0, oracleResult.Length);

            Assert.AreEqual(stringProp, o.StringProp);
        }

        [Test]
        public void Deserialize_FromOracleData_WithTestTable2_AndEmptyString()
        {
            var oracle = new SerializationTestOracle();
            var oracleResult = oracle.GenerateTestTable2(string.Empty);

            var serializer = new FlatBuffersSerializer();
            var o = serializer.Deserialize<TestTable2>(oracleResult, 0, oracleResult.Length);

            Assert.AreEqual(string.Empty, o.StringProp);
        }

        [Test]
        public void Deserialize_FromOracleData_WithTestTable2_AndNullString()
        {
            var oracle = new SerializationTestOracle();
            var oracleResult = oracle.GenerateTestTable2(null);

            var serializer = new FlatBuffersSerializer();
            var o = serializer.Deserialize<TestTable2>(oracleResult, 0, oracleResult.Length);

            Assert.AreEqual(null, o.StringProp);
        }

        [Test]
        public void Deserialize_FromOracleData_WithTestTable3()
        {
            const bool boolProp = true;
            const long longProp = 1020304050;
            const sbyte sbyteProp = -127;
            const ushort ushortProp = 2048;
            const ulong ulongProp = 9999999999999999999;
            const TestEnum enumProp = TestEnum.Banana;
            const float floatProp = 3.14f;
            const double doubleProp = 6.22910783293;

            var oracle = new SerializationTestOracle();
            var oracleResult = oracle.GenerateTestTable3(boolProp, longProp, sbyteProp, ushortProp, ulongProp, enumProp, floatProp, doubleProp);

            var serializer = new FlatBuffersSerializer();
            var o = serializer.Deserialize<TestTable3>(oracleResult, 0, oracleResult.Length);

            Assert.AreEqual(boolProp, o.BoolProp);
            Assert.AreEqual(longProp, o.LongProp);
            Assert.AreEqual(sbyteProp, o.SByteProp);
            Assert.AreEqual(ushortProp, o.UShortProp);
            Assert.AreEqual(ulongProp, o.ULongProp);
            Assert.AreEqual(enumProp, o.EnumProp);
            Assert.AreEqual(floatProp, o.FloatProp);
            Assert.AreEqual(doubleProp, o.DoubleProp);
        }

        [Test]
        public void Deserialize_FromOracleData_WithTestTableWithArray()
        {
            var intArray = new int[] {1, 2, 3, 4, 5};
            var intList = new List<int> {6, 7, 8, 9, 0};

            var oracle = new SerializationTestOracle();
            var oracleResult = oracle.GenerateTestTableWithArray(intArray, intList);

            var serializer = new FlatBuffersSerializer();
            var o = serializer.Deserialize<TestTableWithArray>(oracleResult, 0, oracleResult.Length);

            Assert.IsTrue(o.IntArray.SequenceEqual(intArray));
            Assert.IsTrue(o.IntList.SequenceEqual(intList));
        }

        [Test]
        public void Deserialize_FromOracleData_WithTestTableWithArrayOfTables()
        {
            var tableArray = new TestTable1[]
            {
                new TestTable1 {ByteProp = 1, IntProp = 2, ShortProp = 3},
                new TestTable1 {ByteProp = 4, IntProp = 5, ShortProp = 6},
            };

            var tableList = new List<TestTable1>
            {
                new TestTable1 {ByteProp = 7, IntProp = 8, ShortProp = 9},
                new TestTable1 {ByteProp = 10, IntProp = 11, ShortProp = 12},
            };

            var oracle = new SerializationTestOracle();
            var oracleResult = oracle.GenerateTestTableWithArrayOfTables(tableArray, tableList);

            var serializer = new FlatBuffersSerializer();
            var o = serializer.Deserialize<TestTableWithArrayOfTables>(oracleResult, 0, oracleResult.Length);

            Assert.AreEqual(o.TableArrayProp[0].IntProp, tableArray[0].IntProp);
            Assert.AreEqual(o.TableArrayProp[0].ByteProp, tableArray[0].ByteProp);
            Assert.AreEqual(o.TableArrayProp[0].ShortProp, tableArray[0].ShortProp);
            Assert.AreEqual(o.TableArrayProp[1].IntProp, tableArray[1].IntProp);
            Assert.AreEqual(o.TableArrayProp[1].ByteProp, tableArray[1].ByteProp);
            Assert.AreEqual(o.TableArrayProp[1].ShortProp, tableArray[1].ShortProp);

            Assert.AreEqual(o.TableListProp[0].IntProp, tableList[0].IntProp);
            Assert.AreEqual(o.TableListProp[0].ByteProp, tableList[0].ByteProp);
            Assert.AreEqual(o.TableListProp[0].ShortProp, tableList[0].ShortProp);
            Assert.AreEqual(o.TableListProp[1].IntProp, tableList[1].IntProp);
            Assert.AreEqual(o.TableListProp[1].ByteProp, tableList[1].ByteProp);
            Assert.AreEqual(o.TableListProp[1].ShortProp, tableList[1].ShortProp);
        }

        [Test]
        public void Deserialize_FromOracleData_WithTestTableWithArrayOfStrings()
        {
            var stringArray = new string[] { "Hello", "World" };
            var stringList = new List<string> { "Lorem", "ipsum" };

            var oracle = new SerializationTestOracle();
            var oracleResult = oracle.GenerateTestTableWithArrayOfStrings(stringArray, stringList);

            var serializer = new FlatBuffersSerializer();
            var o = serializer.Deserialize<TestTableWithArrayOfStrings>(oracleResult, 0, oracleResult.Length);

            Assert.IsTrue(o.StringArrayProp.SequenceEqual(stringArray));
            Assert.IsTrue(o.StringListProp.SequenceEqual(stringList));
        }

        [Test]
        public void Deserialize_FromOracleData_WithTestTableWithStruct()
        {
            var testStruct1 = new TestStruct1()
            {
                IntProp = 42,
                ByteProp = 22,
                ShortProp = 62,
            };

            var oracle = new SerializationTestOracle();
            var oracleResult = oracle.GenerateTestTableWithStruct(testStruct1, 1024);

            var serializer = new FlatBuffersSerializer();
            var o = serializer.Deserialize<TestTableWithStruct>(oracleResult, 0, oracleResult.Length);

            Assert.AreEqual(1024, o.IntProp);
            Assert.AreEqual(testStruct1.IntProp, o.StructProp.IntProp);
            Assert.AreEqual(testStruct1.ShortProp, o.StructProp.ShortProp);
            Assert.AreEqual(testStruct1.ByteProp, o.StructProp.ByteProp);
        }

        [Test]
        public void Deserialize_FromOracleData_WithTestTableWithTable()
        {
            var testTable = new TestTable1()
            {
                IntProp = 42,
                ByteProp = 22,
                ShortProp = 62,
            };

            var oracle = new SerializationTestOracle();
            var oracleResult = oracle.GenerateTestTableWithTable(testTable, 1024);

            var serializer = new FlatBuffersSerializer();
            var o = serializer.Deserialize<TestTableWithTable>(oracleResult, 0, oracleResult.Length);

            Assert.AreEqual(1024, o.IntProp);
            Assert.AreEqual(testTable.IntProp, o.TableProp.IntProp);
            Assert.AreEqual(testTable.ShortProp, o.TableProp.ShortProp);
            Assert.AreEqual(testTable.ByteProp, o.TableProp.ByteProp);
        }

        [Test]
        public void Deserialize_FromOracleData_WithTestTableWithNestedTestTable1()
        {
            var oracle = new SerializationTestOracle();
            var oracleResult = oracle.GenerateTestTableWithNestedTestTable1(1024, 42, 22, 62);

            var serializer = new FlatBuffersSerializer();
            var o = serializer.Deserialize<TestTableWithNestedTestTable1>(oracleResult, 0, oracleResult.Length);

            Assert.IsInstanceOf<TestTable1>(o.Nested);

            var nested = o.Nested as TestTable1;

            Assert.AreEqual(1024, o.IntProp);
            Assert.AreEqual(42, nested.IntProp);
            Assert.AreEqual(22, nested.ByteProp);
            Assert.AreEqual(62, nested.ShortProp);
        }

        [Test]
        public void Deserialize_FromOracleData_WithTestTableWithUnion_And_TestTable1()
        {
            var table1 = new TestTable1()
            {
                IntProp = 42,
                ByteProp = 22,
                ShortProp = 62,
            };

            var oracle = new SerializationTestOracle();
            var oracleResult = oracle.GenerateTestTableWithUnion(1024, testTable1Prop: table1);

            var serializer = new FlatBuffersSerializer();
            var o = serializer.Deserialize<TestTableWithUnion>(oracleResult, 0, oracleResult.Length);

            Assert.AreEqual(1024, o.IntProp);

            Assert.IsInstanceOf<TestTable1>(o.UnionProp);

            var o1 = o.UnionProp as TestTable1;

            Assert.AreEqual(table1.IntProp, o1.IntProp);
            Assert.AreEqual(table1.ShortProp, o1.ShortProp);
            Assert.AreEqual(table1.ByteProp, o1.ByteProp);
        }

        [Test]
        public void Deserialize_FromOracleData_WithTestTableWithUnion_And_TestTable2()
        {
            var table2 = new TestTable2()
            {
                StringProp = "Hello, FlatBuffers!"
            };

            var oracle = new SerializationTestOracle();
            var oracleResult = oracle.GenerateTestTableWithUnion(1024, testTable2Prop: table2);

            var serializer = new FlatBuffersSerializer();
            var o = serializer.Deserialize<TestTableWithUnion>(oracleResult, 0, oracleResult.Length);

            Assert.AreEqual(1024, o.IntProp);

            Assert.IsInstanceOf<TestTable2>(o.UnionProp);

            var o2 = o.UnionProp as TestTable2;

            Assert.AreEqual(table2.StringProp, o2.StringProp);
        }

        [Test]
        public void Deserialize_FromOracleData_WithTestTableWithUnion_And_NoTables_ReturnsNullForUnion()
        {
            var oracle = new SerializationTestOracle();
            var oracleResult = oracle.GenerateTestTableWithUnion(1024);

            var serializer = new FlatBuffersSerializer();
            var o = serializer.Deserialize<TestTableWithUnion>(oracleResult, 0, oracleResult.Length);

            Assert.AreEqual(1024, o.IntProp);

            Assert.IsNull(o.UnionProp);
        }

        [Test]
        public void Deserialize_FromOracleData_WithTestTableWithUnionAndMoreFields_And_TestTable1()
        {
            var table1 = new TestTable1()
            {
                IntProp = 42,
                ByteProp = 22,
                ShortProp = 62,
            };

            var oracle = new SerializationTestOracle();
            var oracleResult = oracle.GenerateTestTableWithUnionAndMoreFields(1024, "Hello, FlatBuffers", 123.5f, 3.14, testTable1Prop: table1);

            var serializer = new FlatBuffersSerializer();
            var o = serializer.Deserialize<TestTableWithUnionAndMoreFields>(oracleResult, 0, oracleResult.Length);

            Assert.AreEqual(1024, o.IntProp);
            Assert.AreEqual("Hello, FlatBuffers", o.StringProp);
            Assert.AreEqual(123.5f, o.FloatProp);
            Assert.AreEqual(3.14, o.DoubleProp);

            Assert.IsInstanceOf<TestTable1>(o.UnionProp);

            var o1 = o.UnionProp as TestTable1;
            Assert.AreEqual(table1.IntProp, o1.IntProp);
            Assert.AreEqual(table1.ShortProp, o1.ShortProp);
            Assert.AreEqual(table1.ByteProp, o1.ByteProp);
        }

        [Test]
        public void Deserialize_FromOracleData_WithTestTableWithIdentifier()
        {
            var oracle = new SerializationTestOracle();
            var oracleResult = oracle.GenerateTestTableWithIdentifier(42);

            var serializer = new FlatBuffersSerializer();
            var o = serializer.Deserialize<TestTableWithIdentifier>(oracleResult, 0, oracleResult.Length);

            Assert.AreEqual(42, o.IntProp);
        }

    }
}