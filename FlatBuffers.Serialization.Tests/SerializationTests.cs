using System.Collections.Generic;
using System.Linq;
using Microsoft.VisualStudio.TestTools.UnitTesting;

namespace FlatBuffers.Serialization.Tests
{
    [TestClass]
    public class SerializationTests
    {
        [TestMethod]
        public void Serialize_WithTestStruct1_CanBeReadByOracle()
        {
            const int intProp = 42;
            const byte byteProp = 22;
            const short shortProp = 62;

            var serializer = new FlatBuffersSerializer();

            var obj = new TestStruct1() { IntProp = intProp, ByteProp = byteProp, ShortProp = shortProp };

            var buffer = new byte[32];
            var bytesWritten = serializer.Serialize(obj, buffer, 0, buffer.Length);

            var oracle = new TestOracle();
            var oracleResult = oracle.ReadTestStruct1(buffer);

            Assert.AreEqual(obj.IntProp, oracleResult.IntProp);
            Assert.AreEqual(obj.ByteProp, oracleResult.ByteProp);
            Assert.AreEqual(obj.ShortProp, oracleResult.ShortProp);
        }

        [TestMethod]
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
            var bytesWritten = serializer.Serialize(root, buffer, 0, buffer.Length);

            var oracle = new TestOracle();
            var oracleResult = oracle.ReadTestStruct2(buffer);

            Assert.AreEqual(root.IntProp, oracleResult.IntProp);

            Assert.AreEqual(root.TestStructProp.IntProp, oracleResult.TestStructProp.IntProp);
            Assert.AreEqual(root.TestStructProp.ByteProp, oracleResult.TestStructProp.ByteProp);
            Assert.AreEqual(root.TestStructProp.ShortProp, oracleResult.TestStructProp.ShortProp);
        }

        [TestMethod]
        public void Serialize_WithTestTable1_CanBeReadByOracle()
        {
            const int intProp = 42;
            const byte byteProp = 22;
            const short shortProp = 62;

            var serializer = new FlatBuffersSerializer();

            var obj = new TestTable1() { IntProp = intProp, ByteProp = byteProp, ShortProp = shortProp };

            var buffer = new byte[32];
            var bytesWritten = serializer.Serialize(obj, buffer, 0, buffer.Length);

            var oracle = new TestOracle();
            var oracleResult = oracle.ReadTestTable1(buffer);

            Assert.AreEqual(obj.IntProp, oracleResult.IntProp);
            Assert.AreEqual(obj.ByteProp, oracleResult.ByteProp);
            Assert.AreEqual(obj.ShortProp, oracleResult.ShortProp);
        }

        [TestMethod]
        public void Serialize_WithTestTable2_CanBeReadByOracle()
        {
            const string stringProp = "Hello, FlatBuffers!";
            var serializer = new FlatBuffersSerializer();

            var obj = new TestTable2() { StringProp = stringProp };

            var buffer = new byte[64];
            var bytesWritten = serializer.Serialize(obj, buffer, 0, buffer.Length);

            var oracle = new TestOracle();
            var oracleResult = oracle.ReadTestTable2(buffer);

            Assert.AreEqual(obj.StringProp, oracleResult.StringProp);
        }

        [TestMethod]
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
            var bytesWritten = serializer.Serialize(obj, buffer, 0, buffer.Length);

            
            var oracle = new TestOracle();
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

        [TestMethod]
        public void Serialize_WithTestTableWithArray_CanBeReadByOracle()
        {
            var serializer = new FlatBuffersSerializer();
            var obj = new TestTableWithArray()
            {
                IntArray = new[] {1, 2, 3, 4, 5},
                IntList = new List<int>(new[] {6, 7, 8, 9, 0})
            };
            
            var buffer = new byte[128];

            var bytesWritten = serializer.Serialize(obj, buffer, 0, buffer.Length);

            var oracle = new TestOracle();
            var oracleResult = oracle.ReadTestTableWithArray(buffer);

            Assert.IsTrue(obj.IntArray.SequenceEqual(oracleResult.IntArray));
            Assert.IsTrue(obj.IntList.SequenceEqual(oracleResult.IntList));
        }

        [TestMethod]
        public void Serialize_WithTestTableWithStruct_CanBeReadByOracle()
        {
            const int intProp = 42;
            const byte byteProp = 22;
            const short shortProp = 62;
            
            var serializer = new FlatBuffersSerializer();

            var testStruct1 = new TestStruct1() {IntProp = intProp, ShortProp = shortProp, ByteProp = byteProp};

            var obj = new TestTableWithStruct() { StructProp = testStruct1, IntProp = 1024 };

            var buffer = new byte[64];
            var bytesWritten = serializer.Serialize(obj, buffer, 0, buffer.Length);

            var oracle = new TestOracle();
            var oracleResult = oracle.ReadTestTableWithStruct(buffer);

            Assert.AreEqual(obj.IntProp, oracleResult.IntProp);
            Assert.AreEqual(obj.StructProp.IntProp, oracleResult.StructProp.IntProp);
            Assert.AreEqual(obj.StructProp.ByteProp, oracleResult.StructProp.ByteProp);
            Assert.AreEqual(obj.StructProp.ShortProp, oracleResult.StructProp.ShortProp);
        }

        [TestMethod]
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

            var bytesWritten = serializer.Serialize(obj, buffer, 0, buffer.Length);

            var oracle = new TestOracle();
            var oracleResult = oracle.ReadTestTableWithArrayOfStructs(buffer);

            Assert.IsTrue(oracleResult.StructArray[0].Equals(obj.StructArray[0]));
            Assert.IsTrue(oracleResult.StructArray[1].Equals(obj.StructArray[1]));
        }

        // Tests to implement
        // structs cannot contain vectors, string, table (only scalars or structs)
        // serialization of all basic types (char, short, int, long, float, double, + unsigned variants, string)

        // lists of structs
        // lists of tables
        // table containing table
        // table containing struct
        // deep nesting of structs containing structs
        // shared ptr objects (string, table)
        // enum
        // unions
    }
}
