using FlatBuffers.Tests.TestTypes;
using NUnit.Framework;

namespace FlatBuffers.Tests
{
    /// <summary>
    /// Tests around the Test Oracle - here to verify that certain situations work and it's not a 
    /// bug in the oracle code
    /// </summary>
    [TestFixture]
    public class SerializationTestOracleTests
    {
        [Test]
        public void Oracle_WithTestTableWithTable_CanReadGeneratedData()
        {
            const int intProp = 42;
            const byte byteProp = 22;
            const short shortProp = 62;

            var testTable = new TestTable1() { IntProp = intProp, ShortProp = shortProp, ByteProp = byteProp };

            var obj = new TestTableWithTable() { TableProp = testTable, IntProp = 1024 };

            var oracle = new SerializationTestOracle();

            var buffer = oracle.GenerateTestTableWithTable(testTable, 1024);

            var oracleResult = oracle.ReadTestTableWithTable(buffer);

            Assert.AreEqual(obj.IntProp, oracleResult.IntProp);
            Assert.AreEqual(obj.TableProp.IntProp, oracleResult.TableProp.IntProp);
            Assert.AreEqual(obj.TableProp.ByteProp, oracleResult.TableProp.ByteProp);
            Assert.AreEqual(obj.TableProp.ShortProp, oracleResult.TableProp.ShortProp);
        }
    }
}