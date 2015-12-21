using System.Collections.Generic;
using System.Linq;
using Microsoft.VisualStudio.TestTools.UnitTesting;

namespace FlatBuffers.Serialization.Tests
{
    [TestClass]
    public class DeserializationTests
    {
        [TestMethod]
        public void Deserialize_WithTestStruct1_FromOracleData()
        {
            const int intProp = 42;
            const byte byteProp = 22;
            const short shortProp = 62;

            var oracle = new TestOracle();
            var oracleResult = oracle.GenerateTestStruct1(intProp, byteProp, shortProp);

            var serializer = new FlatBuffersSerializer();
            var o = serializer.Deserialize<TestStruct1>(oracleResult, 0, oracleResult.Length);

            Assert.AreEqual(intProp, o.IntProp);
            Assert.AreEqual(byteProp, o.ByteProp);
            Assert.AreEqual(shortProp, o.ShortProp);
        }

        [TestMethod]
        public void Deserialize_WithTestStruct2_FromOracleData()
        {
            const int intProp = 42;

            var testStruct1 = new TestStruct1()
            {
                IntProp = 100,
                ByteProp = 50,
                ShortProp = 200
            };

            var oracle = new TestOracle();
            var oracleResult = oracle.GenerateTestStruct2(intProp, testStruct1);

            var serializer = new FlatBuffersSerializer();
            var o = serializer.Deserialize<TestStruct2>(oracleResult, 0, oracleResult.Length);

            Assert.AreEqual(intProp, o.IntProp);
            Assert.AreEqual(testStruct1.IntProp, o.TestStructProp.IntProp);
            Assert.AreEqual(testStruct1.ByteProp, o.TestStructProp.ByteProp);
            Assert.AreEqual(testStruct1.ShortProp, o.TestStructProp.ShortProp);
        }

        [TestMethod]
        public void Deserialize_WithTestTable1_FromOracleData()
        {
            const int intProp = 42;
            const byte byteProp = 22;
            const short shortProp = 62;

            var oracle = new TestOracle();
            var oracleResult = oracle.GenerateTestTable1(intProp, byteProp, shortProp);

            var serializer = new FlatBuffersSerializer();
            var o = serializer.Deserialize<TestTable1>(oracleResult, 0, oracleResult.Length);

            Assert.AreEqual(intProp, o.IntProp);
            Assert.AreEqual(byteProp, o.ByteProp);
            Assert.AreEqual(shortProp, o.ShortProp);
        }

        [TestMethod]
        public void Deserialize_WithTestTable2_FromOracleData()
        {
            const string stringProp = "Hello, FlatBuffers!";

            var oracle = new TestOracle();
            var oracleResult = oracle.GenerateTestTable2(stringProp);

            var serializer = new FlatBuffersSerializer();
            var o = serializer.Deserialize<TestTable2>(oracleResult, 0, oracleResult.Length);

            Assert.AreEqual(stringProp, o.StringProp);
        }

        [TestMethod]
        public void Deserialize_WithTestTableWithArray_FromOracleData()
        {
            var intArray = new int[] {1, 2, 3, 4, 5};
            var intList = new List<int> {6, 7, 8, 9, 0};

            var oracle = new TestOracle();
            var oracleResult = oracle.GenerateTestTableWithArray(intArray, intList);

            var serializer = new FlatBuffersSerializer();
            var o = serializer.Deserialize<TestTableWithArray>(oracleResult, 0, oracleResult.Length);

            Assert.IsTrue(o.IntArray.SequenceEqual(intArray));
            Assert.IsTrue(o.IntList.SequenceEqual(intList));
        }

        [TestMethod]
        public void Deserialize_WithTestTableWithStruct_FromOracleData()
        {
            var testStruct1 = new TestStruct1()
            {
                IntProp = 42,
                ByteProp = 22,
                ShortProp = 62,
            };

            var oracle = new TestOracle();
            var oracleResult = oracle.GenerateTestTableWithStruct(testStruct1, 1024);

            var serializer = new FlatBuffersSerializer();
            var o = serializer.Deserialize<TestTableWithStruct>(oracleResult, 0, oracleResult.Length);


        }
    }
}