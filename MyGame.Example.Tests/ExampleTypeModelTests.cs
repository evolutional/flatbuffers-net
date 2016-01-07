using FlatBuffers;
using NUnit.Framework;

namespace MyGame.Example.Tests
{
    [TestFixture]
    public class ExampleTypeModelTests
    {
        private TypeModel GetTypeModel<T>()
        {
            var registry = TypeModelRegistry.Default;
            var typeModel = registry.GetTypeModel<T>();
            return typeModel;
        }

        [Test]
        public void GetTypeModel_Test_CorrectAlignmentAndPadding()
        {
            var typeModel = GetTypeModel<Test>();
            Assert.IsTrue(typeModel.IsStruct);
            Assert.IsNotNull(typeModel.StructDef);
            var structDef = typeModel.StructDef;

            var a = structDef.GetFieldByName("A");
            Assert.AreEqual(2, a.TypeModel.InlineSize);
            Assert.AreEqual(2, a.TypeModel.InlineAlignment);
            Assert.AreEqual(0, a.Padding);

            var b = structDef.GetFieldByName("B");
            Assert.AreEqual(1, b.TypeModel.InlineSize);
            Assert.AreEqual(1, b.TypeModel.InlineAlignment);
            Assert.AreEqual(1, b.Padding);

            Assert.AreEqual(2, structDef.MinAlign);
            Assert.AreEqual(4, structDef.ByteSize);
        }

        [Test]
        public void GetTypeModel_Vec3_StructHasCorrectAlignmentAndPadding()
        {
            var typeModel = GetTypeModel<Vec3>();
            Assert.IsTrue(typeModel.IsStruct);
            Assert.IsNotNull(typeModel.StructDef);
            var structDef = typeModel.StructDef;

            Assert.AreEqual(16, structDef.MinAlign);
            Assert.AreEqual(32, structDef.ByteSize);
        }

        [Test]
        public void GetTypeModel_Vec3_FieldsHaveCorrectSize()
        {
            var typeModel = GetTypeModel<Vec3>();
            Assert.IsTrue(typeModel.IsStruct);
            Assert.IsNotNull(typeModel.StructDef);
            var structDef = typeModel.StructDef;

            var xField = structDef.GetFieldByName("X");
            Assert.AreEqual(4, xField.TypeModel.InlineSize);
            Assert.AreEqual(0, xField.Padding);

            var yField = structDef.GetFieldByName("Y");
            Assert.AreEqual(4, yField.TypeModel.InlineSize);
            Assert.AreEqual(0, yField.Padding);

            var zField = structDef.GetFieldByName("Z");
            Assert.AreEqual(4, zField.TypeModel.InlineSize);
            Assert.AreEqual(4, zField.Padding);     // Field should be padded to align to 16 byte boundary

            var test1Field = structDef.GetFieldByName("Test1");
            Assert.AreEqual(8, test1Field.TypeModel.InlineSize);
            Assert.AreEqual(0, test1Field.Padding);

            var test2Field = structDef.GetFieldByName("Test2");
            Assert.AreEqual(1, test2Field.TypeModel.InlineSize);
            Assert.AreEqual(1, test2Field.Padding);

            var test3Field = structDef.GetFieldByName("Test3");
            Assert.AreEqual(2, test3Field.TypeModel.InlineAlignment);
            Assert.AreEqual(4, test3Field.TypeModel.InlineSize);
            Assert.AreEqual(2, test3Field.Padding);
        }
    }
}
