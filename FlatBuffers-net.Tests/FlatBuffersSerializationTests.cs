using System.Collections.Generic;
using System.Linq;
using FlatBuffers.Tests.TestTypes;
using NUnit.Framework;

namespace FlatBuffers.Tests
{
    [TestFixture]
    public class FlatBuffersSerializationTests
    {
        [Test]
        public void Serialize_WithTestStruct1_CanBeReadByOracle()
        {
            const int intProp = 42;
            const byte byteProp = 22;
            const short shortProp = 62;

            var serializer = new FlatBuffersSerializer();

            var obj = new TestStruct1() { IntProp = intProp, ByteProp = byteProp, ShortProp = shortProp };

            var buffer = new byte[32];
            serializer.Serialize(obj, buffer, 0, buffer.Length);

            var oracle = new SerializationTestOracle();
            var oracleResult = oracle.ReadTestStruct1(buffer);

            Assert.AreEqual(obj.IntProp, oracleResult.IntProp);
            Assert.AreEqual(obj.ByteProp, oracleResult.ByteProp);
            Assert.AreEqual(obj.ShortProp, oracleResult.ShortProp);
        }

        [Test]
        public void Serialize_WithTestStruct2_CanBeReadByOracle()
        {
            const int outerIntProp = 102;
            const int intProp = 42;
            const byte byteProp = 22;
            const short shortProp = 62;

            var serializer = new FlatBuffersSerializer();

            var obj = new TestStruct1() { IntProp = intProp, ByteProp = byteProp, ShortProp = shortProp };
            var root = new TestStruct2() {IntProp = outerIntProp, TestStructProp = obj};

            var buffer = new byte[32];
            serializer.Serialize(root, buffer, 0, buffer.Length);

            var oracle = new SerializationTestOracle();
            var oracleResult = oracle.ReadTestStruct2(buffer);

            Assert.AreEqual(root.IntProp, oracleResult.IntProp);

            Assert.AreEqual(root.TestStructProp.IntProp, oracleResult.TestStructProp.IntProp);
            Assert.AreEqual(root.TestStructProp.ByteProp, oracleResult.TestStructProp.ByteProp);
            Assert.AreEqual(root.TestStructProp.ShortProp, oracleResult.TestStructProp.ShortProp);
        }

        [Test]
        public void Serialize_WithTestTable1_CanBeReadByOracle()
        {
            const int intProp = 42;
            const byte byteProp = 22;
            const short shortProp = 62;

            var serializer = new FlatBuffersSerializer();

            var obj = new TestTable1() { IntProp = intProp, ByteProp = byteProp, ShortProp = shortProp };

            var buffer = new byte[32];
            serializer.Serialize(obj, buffer, 0, buffer.Length);

            var oracle = new SerializationTestOracle();
            var oracleResult = oracle.ReadTestTable1(buffer);

            Assert.AreEqual(obj.IntProp, oracleResult.IntProp);
            Assert.AreEqual(obj.ByteProp, oracleResult.ByteProp);
            Assert.AreEqual(obj.ShortProp, oracleResult.ShortProp);
        }

        [Test]
        public void Serialize_WithTestTableUsingFields_CanBeReadByOracle()
        {
            const int intProp = 42;
            const byte byteProp = 22;
            const short shortProp = 62;

            var serializer = new FlatBuffersSerializer();

            var obj = new TestTable1UsingFields() { IntProp = intProp, ByteProp = byteProp, ShortProp = shortProp };

            var buffer = new byte[32];
            serializer.Serialize(obj, buffer, 0, buffer.Length);

            var oracle = new SerializationTestOracle();
            var oracleResult = oracle.ReadTestTable1(buffer);

            Assert.AreEqual(obj.IntProp, oracleResult.IntProp);
            Assert.AreEqual(obj.ByteProp, oracleResult.ByteProp);
            Assert.AreEqual(obj.ShortProp, oracleResult.ShortProp);
        }

        [Test]
        public void Serialize_WithTestTableWithDeprecatedField_CanBeReadByOracle()
        {
            const int intProp = 42;
            const short shortProp = 62;

            var serializer = new FlatBuffersSerializer();

            var obj = new TestTableWithDeprecatedField {IntProp = intProp, ShortProp = shortProp, ByteProp = 255};

            var buffer = new byte[32];
            serializer.Serialize(obj, buffer, 0, buffer.Length);

            var oracle = new SerializationTestOracle();
            var oracleResult = oracle.ReadTestTableWithDeprecatedField(buffer);

            Assert.AreEqual(obj.IntProp, oracleResult.IntProp);
            Assert.AreEqual(TestTableWithDeprecatedField.DefaultBytePropValue, oracleResult.ByteProp);   // default value
            Assert.AreEqual(obj.ShortProp, oracleResult.ShortProp);
        }

        [Test]
        public void Serialize_WithTestTableWithDeprecatedField_CanBeReadByOracle_And_CompatibleWithTestTable1()
        {
            const int intProp = 42;
            const short shortProp = 62;

            var serializer = new FlatBuffersSerializer();

            var obj = new TestTableWithDeprecatedField() { IntProp = intProp, ShortProp = shortProp };

            var buffer = new byte[32];
            serializer.Serialize(obj, buffer, 0, buffer.Length);

            var oracle = new SerializationTestOracle();
            var oracleResult = oracle.ReadTestTable1(buffer);

            Assert.AreEqual(obj.IntProp, oracleResult.IntProp);
            Assert.AreNotEqual(TestTableWithDeprecatedField.DefaultBytePropValue, oracleResult.ByteProp);   // default value
            Assert.AreEqual(obj.ShortProp, oracleResult.ShortProp);
        }
        
        [Test]
        public void Serialize_WithTestTable2_CanBeReadByOracle()
        {
            const string stringProp = "Hello, FlatBuffers!";
            var serializer = new FlatBuffersSerializer();

            var obj = new TestTable2() { StringProp = stringProp };

            var buffer = new byte[64];
            serializer.Serialize(obj, buffer, 0, buffer.Length);

            var oracle = new SerializationTestOracle();
            var oracleResult = oracle.ReadTestTable2(buffer);

            Assert.AreEqual(obj.StringProp, oracleResult.StringProp);
        }

        [Test]
        public void Serialize_WithTestTable2_AndEmptyString_CanBeReadByOracle()
        {
            var serializer = new FlatBuffersSerializer();

            var obj = new TestTable2() { StringProp = string.Empty };

            var buffer = new byte[64];
            serializer.Serialize(obj, buffer, 0, buffer.Length);

            var oracle = new SerializationTestOracle();
            var oracleResult = oracle.ReadTestTable2(buffer);

            Assert.IsEmpty(oracleResult.StringProp);
        }

        [Test]
        public void Serialize_WithTestTable2_AndNullString_CanBeReadByOracle()
        {
            var serializer = new FlatBuffersSerializer();

            var obj = new TestTable2();

            var buffer = new byte[64];
            serializer.Serialize(obj, buffer, 0, buffer.Length);

            var oracle = new SerializationTestOracle();
            var oracleResult = oracle.ReadTestTable2(buffer);

            Assert.IsNull(oracleResult.StringProp);
        }

        [Test]
        public void Serialize_WithTestTable3_CanBeReadByOracle()
        {
            var serializer = new FlatBuffersSerializer();

            var obj = new TestTable3()
            {
                EnumProp = TestEnum.Banana,
                BoolProp = true,
                LongProp = 1020304050,
                SByteProp = -127,
                ULongProp = 9999999999999999999,
                UShortProp = 2048,
                FloatProp = 3.14f,
                DoubleProp = 6.22910783293
            };

            var buffer = new byte[128];
            serializer.Serialize(obj, buffer, 0, buffer.Length);

            
            var oracle = new SerializationTestOracle();
            var oracleResult = oracle.ReadTestTable3(buffer);

            Assert.AreEqual(obj.BoolProp, oracleResult.BoolProp);
            Assert.AreEqual(obj.LongProp, oracleResult.LongProp);
            Assert.AreEqual(obj.DoubleProp, oracleResult.DoubleProp);
            Assert.AreEqual(obj.FloatProp, oracleResult.FloatProp);
            Assert.AreEqual(obj.EnumProp, oracleResult.EnumProp);
            Assert.AreEqual(obj.ULongProp, oracleResult.ULongProp);
            Assert.AreEqual(obj.SByteProp, oracleResult.SByteProp);
            Assert.AreEqual(obj.UShortProp, oracleResult.UShortProp);
        }

        [Test]
        public void Serialize_WithTestTableWithArray_CanBeReadByOracle()
        {
            var serializer = new FlatBuffersSerializer();
            var obj = new TestTableWithArray()
            {
                IntArray = new[] {1, 2, 3, 4, 5},
                IntList = new List<int>(new[] {6, 7, 8, 9, 0})
            };
            
            var buffer = new byte[128];

            serializer.Serialize(obj, buffer, 0, buffer.Length);

            var oracle = new SerializationTestOracle();
            var oracleResult = oracle.ReadTestTableWithArray(buffer);

            Assert.IsTrue(obj.IntArray.SequenceEqual(oracleResult.IntArray));
            Assert.IsTrue(obj.IntList.SequenceEqual(oracleResult.IntList));
        }

        [Test]
        public void Serialize_WithTestTableWithStruct_CanBeReadByOracle()
        {
            const int intProp = 42;
            const byte byteProp = 22;
            const short shortProp = 62;
            
            var serializer = new FlatBuffersSerializer();

            var testStruct1 = new TestStruct1() {IntProp = intProp, ShortProp = shortProp, ByteProp = byteProp};

            var obj = new TestTableWithStruct() { StructProp = testStruct1, IntProp = 1024 };

            var buffer = new byte[64];
            serializer.Serialize(obj, buffer, 0, buffer.Length);

            var oracle = new SerializationTestOracle();
            var oracleResult = oracle.ReadTestTableWithStruct(buffer);

            Assert.AreEqual(obj.IntProp, oracleResult.IntProp);
            Assert.AreEqual(obj.StructProp.IntProp, oracleResult.StructProp.IntProp);
            Assert.AreEqual(obj.StructProp.ByteProp, oracleResult.StructProp.ByteProp);
            Assert.AreEqual(obj.StructProp.ShortProp, oracleResult.StructProp.ShortProp);
        }

        [Test]
        public void Serialize_WithTestTableWithTable_CanBeReadByOracle()
        {
            const int intProp = 42;
            const byte byteProp = 22;
            const short shortProp = 62;

            var serializer = new FlatBuffersSerializer();

            var testTable = new TestTable1() { IntProp = intProp, ShortProp = shortProp, ByteProp = byteProp };

            var obj = new TestTableWithTable() { TableProp = testTable, IntProp = 1024 };

            var buffer = new byte[256];
            serializer.Serialize(obj, buffer, 0, buffer.Length);

            var oracle = new SerializationTestOracle();
            var oracleResult = oracle.ReadTestTableWithTable(buffer);

            Assert.AreEqual(obj.IntProp, oracleResult.IntProp);
            Assert.AreEqual(obj.TableProp.IntProp, oracleResult.TableProp.IntProp);
            Assert.AreEqual(obj.TableProp.ByteProp, oracleResult.TableProp.ByteProp);
            Assert.AreEqual(obj.TableProp.ShortProp, oracleResult.TableProp.ShortProp);
        }

        [Test]
        public void Serialize_WithTestTableWithArrayOfTables_CanBeReadByOracle()
        {
            var serializer = new FlatBuffersSerializer();
            
            var obj = new TestTableWithArrayOfTables()
            {
                TableArrayProp = new []
                {
                    new TestTable1() { IntProp = 1, ShortProp = 2, ByteProp = 3 },
                    new TestTable1() { IntProp = 4, ShortProp = 5, ByteProp = 6 }
                },
                TableListProp = new List<TestTable1>()
                {
                    new TestTable1() { IntProp = 7, ShortProp = 8, ByteProp = 9 },
                    new TestTable1() { IntProp = 10, ShortProp = 11, ByteProp = 12 }
                }
            };

            var buffer = new byte[256];
            serializer.Serialize(obj, buffer, 0, buffer.Length);

            var oracle = new SerializationTestOracle();
            var oracleResult = oracle.ReadTestTableWithArrayOfTables(buffer);
            
            Assert.AreEqual(obj.TableArrayProp[0].IntProp, oracleResult.TableArrayProp[0].IntProp);
            Assert.AreEqual(obj.TableArrayProp[0].ByteProp, oracleResult.TableArrayProp[0].ByteProp);
            Assert.AreEqual(obj.TableArrayProp[0].ShortProp, oracleResult.TableArrayProp[0].ShortProp);
            Assert.AreEqual(obj.TableArrayProp[1].IntProp, oracleResult.TableArrayProp[1].IntProp);
            Assert.AreEqual(obj.TableArrayProp[1].ByteProp, oracleResult.TableArrayProp[1].ByteProp);
            Assert.AreEqual(obj.TableArrayProp[1].ShortProp, oracleResult.TableArrayProp[1].ShortProp);

            Assert.AreEqual(obj.TableListProp[0].IntProp, oracleResult.TableListProp[0].IntProp);
            Assert.AreEqual(obj.TableListProp[0].ByteProp, oracleResult.TableListProp[0].ByteProp);
            Assert.AreEqual(obj.TableListProp[0].ShortProp, oracleResult.TableListProp[0].ShortProp);
            Assert.AreEqual(obj.TableListProp[1].IntProp, oracleResult.TableListProp[1].IntProp);
            Assert.AreEqual(obj.TableListProp[1].ByteProp, oracleResult.TableListProp[1].ByteProp);
            Assert.AreEqual(obj.TableListProp[1].ShortProp, oracleResult.TableListProp[1].ShortProp);
        }

        [Test]
        public void Serialize_WithTestTableWithArrayOfStructs_CanBeReadByOracle()
        {
            var serializer = new FlatBuffersSerializer();
            var obj = new TestTableWithArrayOfStructs()
            {
                StructArray = new[]
                {
                    new TestStruct1() {  ByteProp = 42, IntProp = 100, ShortProp = -50},
                    new TestStruct1() {  ByteProp = 142, IntProp = 1000, ShortProp = -150},
                }
            };

            var buffer = new byte[128];

            serializer.Serialize(obj, buffer, 0, buffer.Length);

            var oracle = new SerializationTestOracle();
            var oracleResult = oracle.ReadTestTableWithArrayOfStructs(buffer);

            Assert.IsTrue(oracleResult.StructArray[0].Equals(obj.StructArray[0]));
            Assert.IsTrue(oracleResult.StructArray[1].Equals(obj.StructArray[1]));
        }

        [Test]
        public void Serialize_WithTestTableWithArrayOfStrings_CanBeReadByOracle()
        {
            var serializer = new FlatBuffersSerializer();
            var obj = new TestTableWithArrayOfStrings()
            {
                StringArrayProp = new []
                {
                    "Hello",
                    "World"
                },
                StringListProp = new List<string>
                {
                    "Lorem",
                    "ipsum"
                }
            };

            var buffer = new byte[128];

            serializer.Serialize(obj, buffer, 0, buffer.Length);

            var oracle = new SerializationTestOracle();
            var oracleResult = oracle.ReadTestTableWithArrayOfStrings(buffer);

            Assert.IsTrue(oracleResult.StringArrayProp[0].Equals(obj.StringArrayProp[0]));
            Assert.IsTrue(oracleResult.StringArrayProp[1].Equals(obj.StringArrayProp[1]));
            Assert.IsTrue(oracleResult.StringListProp[0].Equals(obj.StringListProp[0]));
            Assert.IsTrue(oracleResult.StringListProp[1].Equals(obj.StringListProp[1]));
        }

        [Test]
        public void Serialize_WithTestTable1WithUserOrdering_CanBeReadByOracle()
        {
            const int intProp = 42;
            const byte byteProp = 22;
            const short shortProp = 62;

            var serializer = new FlatBuffersSerializer();
            var obj = new TestTableWithUserOrdering() { IntProp = intProp, ShortProp = shortProp, ByteProp = byteProp };
            var buffer = new byte[32];
            serializer.Serialize(obj, buffer, 0, buffer.Length);

            var oracle = new SerializationTestOracle();
            var oracleResult = oracle.ReadTestTable1WithUserOrdering(buffer);

            Assert.AreEqual(byteProp, oracleResult.ByteProp);
            Assert.AreEqual(shortProp, oracleResult.ShortProp);
            Assert.AreEqual(intProp, oracleResult.IntProp);
        }


        [Test]
        public void Serialize_WithTestTableWithDefaultValues_CanBeReadByOracle()
        {
            const int intProp = 123456;
            const byte byteProp = 42;
            const short shortProp = 1024;

            var serializer = new FlatBuffersSerializer();

            var obj = new TestTableWithDefaults();  // relying on the flatbuffers defaults

            var buffer = new byte[32];
            serializer.Serialize(obj, buffer, 0, buffer.Length);

            var oracle = new SerializationTestOracle();
            var oracleResult = oracle.ReadTestTableWithDefaults(buffer);

            Assert.AreEqual(intProp, oracleResult.IntProp);
            Assert.AreEqual(byteProp, oracleResult.ByteProp);
            Assert.AreEqual(shortProp, oracleResult.ShortProp);
        }

        [Test]
        public void Serialize_TestTable1UsingFieldsAndDefaultFieldValues_CanBeReadByOracleAsTestTable1()
        {
            var serializer = new FlatBuffersSerializer();

            var obj = new TestTable1UsingFieldsAndDefaultFieldValues();  // relying on the flatbuffers defaults

            var buffer = new byte[32];
            serializer.Serialize(obj, buffer, 0, buffer.Length);

            var oracle = new SerializationTestOracle();
            var oracleResult = oracle.ReadTestTable1(buffer);

            Assert.AreEqual(0, oracleResult.IntProp);   // values are all zero as the defaults matched the values in the struct
            Assert.AreEqual(0, oracleResult.ByteProp);
            Assert.AreEqual(0, oracleResult.ShortProp);
        }

        [Test]
        public void Serialize_WithTestTableWithOriginalOrdering_CanBeReadByOracle()
        {
            const int intProp = 123456;
            const byte byteProp = 42;
            const short shortProp = 1024;

            var serializer = new FlatBuffersSerializer();

            var obj = new TestTableWithOriginalOrdering()
            {
                IntProp = intProp,
                ByteProp = byteProp,
                ShortProp = shortProp
            };

            var buffer = new byte[32];
            serializer.Serialize(obj, buffer, 0, buffer.Length);

            var oracle = new SerializationTestOracle();
            var oracleResult = oracle.ReadTestTableWithOriginalOrdering(buffer);

            Assert.AreEqual(intProp, oracleResult.IntProp);
            Assert.AreEqual(byteProp, oracleResult.ByteProp);
            Assert.AreEqual(shortProp, oracleResult.ShortProp);
        }

        [Test]
        public void Serialize_WithTestTableWithOriginalOrdering_CanBeReadByOracle_And_CompatibleWithTestTable1()
        {
            const int intProp = 123456;
            const byte byteProp = 42;
            const short shortProp = 1024;

            var serializer = new FlatBuffersSerializer();

            var obj = new TestTableWithOriginalOrdering()
            {
                IntProp = intProp,
                ByteProp = byteProp,
                ShortProp = shortProp
            };

            var buffer = new byte[32];
            serializer.Serialize(obj, buffer, 0, buffer.Length);

            var oracle = new SerializationTestOracle();
            var oracleResult = oracle.ReadTestTable1(buffer);

            Assert.AreEqual(intProp, oracleResult.IntProp);
            Assert.AreEqual(byteProp, oracleResult.ByteProp);
            Assert.AreEqual(shortProp, oracleResult.ShortProp);
        }

        [Test]
        public void Serialize_TestTable1_And_TestTableWithOriginalOrdering_EmitDifferentBuffers()
        {
            const int intProp = 123456;
            const byte byteProp = 42;
            const short shortProp = 1024;

            var obj = new TestTableWithOriginalOrdering()
            {
                IntProp = intProp,
                ByteProp = byteProp,
                ShortProp = shortProp
            };

            var buffer = FlatBuffersConvert.SerializeObject(obj);

            var obj2 = new TestTable1()
            {
                IntProp = intProp,
                ByteProp = byteProp,
                ShortProp = shortProp
            };

            var buffer2 = FlatBuffersConvert.SerializeObject(obj2);
            // Buffers will be different as they've been written in different orders
            Assert.IsFalse(buffer.SequenceEqual(buffer2));
        }

        [Test]
        public void Serialize_TestTableWithIdentifier_CanBeReadByOracle()
        {
            const int intProp = 123456;

            var obj = new TestTableWithIdentifier()
            {
                IntProp = intProp,
            };

            var buffer = FlatBuffersConvert.SerializeObject(obj);
            var oracle = new SerializationTestOracle();
            var oracleResult = oracle.ReadTestTableWithIdentifier(buffer);
            Assert.AreEqual(intProp, oracleResult.IntProp);
        }

        [ExpectedException(typeof(FlatBuffersSerializationException), ExpectedMessage = "Required field 'StringProp' is not set")]
        [Test]
        public void Serialize_TableWithRequiredFields_WhenStringPropNull_ThrowsException()
        {
            var serializer = new FlatBuffersSerializer();

            var obj = new TableWithRequiredFields()
            {
                StringProp = null,
                TableProp = new TestTable1(),
                VectorProp = new List<int>() {1, 2, 3, 4}
            };

            var buffer = new byte[128];
            serializer.Serialize(obj, buffer, 0, buffer.Length);
        }

        [ExpectedException(typeof(FlatBuffersSerializationException), ExpectedMessage = "Required field 'TableProp' is not set")]
        [Test]
        public void Serialize_TableWithRequiredFields_WhenTablePropNull_ThrowsException()
        {
            var serializer = new FlatBuffersSerializer();

            var obj = new TableWithRequiredFields()
            {
                StringProp = "Hello",
                TableProp = null,
                VectorProp = new List<int>() { 1, 2, 3, 4 }
            };

            var buffer = new byte[64];
            serializer.Serialize(obj, buffer, 0, buffer.Length);
        }

        [ExpectedException(typeof(FlatBuffersSerializationException), ExpectedMessage = "Required field 'VectorProp' is not set")]
        [Test]
        public void Serialize_TableWithRequiredFields_WhenVectorPropNull_ThrowsException()
        {
            var serializer = new FlatBuffersSerializer();

            var obj = new TableWithRequiredFields()
            {
                StringProp = "Hello",
                TableProp = new TestTable1(),
                VectorProp = null
            };

            var buffer = new byte[64];
            serializer.Serialize(obj, buffer, 0, buffer.Length);
        }

        [Test]
        public void Serialize_TestTableWithUnion_And_TestTable1_CanBeReadByOracle()
        {
            const int intProp = 123456;
            const byte byteProp = 42;
            const short shortProp = 1024;

            var table1 = new TestTable1()
            {
                IntProp = intProp,
                ByteProp = byteProp,
                ShortProp = shortProp
            };

            var obj = new TestTableWithUnion()
            {
                IntProp = 512,
                UnionProp = table1
            };

            var buffer = FlatBuffersConvert.SerializeObject(obj);

            var oracle = new SerializationTestOracle();
            var oracleResult = oracle.ReadTestTableWithUnion(buffer);

            Assert.AreEqual(512, oracleResult.IntProp);
            Assert.AreEqual(typeof(TestTable1), oracleResult.UnionProp.GetType());

            var oracleResult1 = oracleResult.UnionProp as TestTable1;
            Assert.IsNotNull(oracleResult1);
            Assert.AreEqual(intProp, oracleResult1.IntProp);
            Assert.AreEqual(byteProp, oracleResult1.ByteProp);
            Assert.AreEqual(shortProp, oracleResult1.ShortProp);
        }

        [Test]
        public void Serialize_TestTableWithUnion_And_TestTable2_CanBeReadByOracle()
        {
            const string stringProp = "Hello, FlatBuffers!";

            var table2 = new TestTable2()
            {
                StringProp = stringProp
            };

            var obj = new TestTableWithUnion()
            {
                IntProp = 512,
                UnionProp = table2
            };

            var buffer = FlatBuffersConvert.SerializeObject(obj);

            var oracle = new SerializationTestOracle();
            var oracleResult = oracle.ReadTestTableWithUnion(buffer);

            Assert.AreEqual(512, oracleResult.IntProp);
            Assert.AreEqual(typeof(TestTable2), oracleResult.UnionProp.GetType());

            var oracleResult2 = oracleResult.UnionProp as TestTable2;
            Assert.IsNotNull(oracleResult2);
            Assert.AreEqual(stringProp, oracleResult2.StringProp);
        }

        [Test]
        public void Serialize_TestTableWithUnionAndMoreFields_And_TestTable1_CanBeReadByOracle()
        {
            const int intProp = 123456;
            const byte byteProp = 42;
            const short shortProp = 1024;

            var table1 = new TestTable1()
            {
                IntProp = intProp,
                ByteProp = byteProp,
                ShortProp = shortProp
            };

            var obj = new TestTableWithUnionAndMoreFields()
            {
                IntProp = 512,
                StringProp = "Hello, world!",
                FloatProp = 3.125f,
                DoubleProp = 3.14,
                UnionProp = table1
            };

            var buffer = FlatBuffersConvert.SerializeObject(obj);

            var oracle = new SerializationTestOracle();
            var oracleResult = oracle.ReadTestTableWithUnionAndMoreFields(buffer);

            Assert.AreEqual(512, oracleResult.IntProp);
            Assert.AreEqual("Hello, world!", oracleResult.StringProp);
            Assert.AreEqual(3.125f, oracleResult.FloatProp);
            Assert.AreEqual(3.14, oracleResult.DoubleProp);
            Assert.IsInstanceOf<TestTable1>(oracleResult.UnionProp);

            var oracleResult1 = oracleResult.UnionProp as TestTable1;
            Assert.IsNotNull(oracleResult1);
            Assert.AreEqual(intProp, oracleResult1.IntProp);
            Assert.AreEqual(byteProp, oracleResult1.ByteProp);
            Assert.AreEqual(shortProp, oracleResult1.ShortProp);
        }

        [Test]
        public void Serialize_TestTableWithNestedTestTable1_CanBeReadByOracle()
        {
            const int intProp = 123456;
            const byte byteProp = 42;
            const short shortProp = 1024;

            var nested = new TestTable1()
            {
                IntProp = intProp,
                ByteProp = byteProp,
                ShortProp = shortProp
            };

            var obj = new TestTableWithNestedTestTable1()
            {
                IntProp = 2048,
                Nested = nested
            };

            var buffer = FlatBuffersConvert.SerializeObject(obj);

            var oracle = new SerializationTestOracle();
            var oracleResult = oracle.ReadTestTableWithNestedTable1(buffer);

            Assert.AreEqual(2048, oracleResult.IntProp);

            var oracleResult1 = oracleResult.Nested as TestTable1;
            Assert.IsNotNull(oracleResult1);
            Assert.AreEqual(intProp, oracleResult1.IntProp);
            Assert.AreEqual(byteProp, oracleResult1.ByteProp);
            Assert.AreEqual(shortProp, oracleResult1.ShortProp);
        }

        // Tests to implement
        // structs cannot contain vectors, string, table (only scalars or structs)
        // serialization of all basic types (char, short, int, long, float, double, + unsigned variants, string)

        // lists of structs
        // lists of tables
        // deep nesting of structs containing structs
        // shared ptr objects (string, table)
    }
}
