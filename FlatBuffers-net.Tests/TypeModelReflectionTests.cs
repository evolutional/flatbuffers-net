using System;
using System.Linq;
using FlatBuffers.Attributes;
using FlatBuffers.Tests.TestTypes;
using NUnit.Framework;

namespace FlatBuffers.Tests
{
    [TestFixture]
    public class TypeModelReflectionTests
    {
        private TypeModel GetTypeModel<T>()
        {
            var registry = TypeModelRegistry.Default;
            var typeModel = registry.GetTypeModel<T>();
            return typeModel;
        }

        /// <summary>
        /// This class will fail to reflect because the user has specified the same field Order multiple times
        /// </summary>
        private abstract class BrokenUserOrderingMultipleSameFieldOrder
        {
            [FlatBuffersField(Order = 0)]
            public int FieldA { get; set; }

            [FlatBuffersField(Order = 1)]
            public int FieldB { get; set; }

            [FlatBuffersField(Order = 1)]
            public int FieldC { get; set; }
        }


        /// <summary>
        /// This class will fail to reflect because the user has a gap in the field order
        /// </summary>
        private abstract class BrokenUserOrderingGapInFieldOrder
        {
            [FlatBuffersField(Order = 0)]
            public int FieldA { get; set; }

            [FlatBuffersField(Order = 1)]
            public int FieldB { get; set; }

            // Missing field 2

            [FlatBuffersField(Order = 3)]
            public int FieldC { get; set; }
        }

        /// <summary>
        /// This class will fail to reflect because the user has a gap in the field order
        /// </summary>
        private abstract class BrokenUserOrderingNotAllFieldsHaveOrder
        {
            [FlatBuffersField(Order = 0)]
            public int FieldA { get; set; }

            // Order not set for this type
            [FlatBuffersField]
            public int FieldB { get; set; }

            [FlatBuffersField(Order = 2)]
            public int FieldC { get; set; }
        }

        private abstract class TableWithDefaultValue
        {
            [FlatBuffersDefaultValue(42)]
            public int Prop { get; set; }
        }

        [Test]
        public void GetTypeModel_WithUserOrderedFields_ReflectsIndexCorrectly()
        {
            var typeModel = GetTypeModel<TestTableWithUserOrdering>();
            Assert.IsTrue(typeModel.IsTable);
            Assert.IsNotNull(typeModel.StructDef);
            var structDef = typeModel.StructDef;
            
            var orderedFields = structDef.Fields.OrderBy(i => i.Index).ToArray();
            Assert.AreEqual(3, orderedFields.Length);

            Assert.IsTrue(orderedFields.All(i=>i.IsIndexSetExplicitly));

            // Assert Field name, index, original order
            Assert.AreEqual("ByteProp", orderedFields[0].Name);
            Assert.AreEqual(0, orderedFields[0].Index);
            Assert.AreEqual(1, orderedFields[0].OriginalIndex);

            Assert.AreEqual("ShortProp", orderedFields[1].Name);
            Assert.AreEqual(1, orderedFields[1].Index);
            Assert.AreEqual(2, orderedFields[1].OriginalIndex);

            Assert.AreEqual("IntProp", orderedFields[2].Name);
            Assert.AreEqual(2, orderedFields[2].Index);
            Assert.AreEqual(0, orderedFields[2].OriginalIndex);
        }

        [ExpectedException(typeof(FlatBuffersStructFieldReflectionException), ExpectedMessage = "Order must be set on all fields")]
        [Test]
        public void GetTypeModel_BrokenUserOrderingNotAllFieldsHaveOrder_ThrowsException()
        {
            GetTypeModel<BrokenUserOrderingNotAllFieldsHaveOrder>();
        }

        [ExpectedException(typeof(FlatBuffersStructFieldReflectionException), ExpectedMessage = "Order range must be contiguous sequence from 0..N")]
        [Test]
        public void GetTypeModel_BrokenUserOrderingGapInFieldOrder_ThrowsException()
        {
            GetTypeModel<BrokenUserOrderingGapInFieldOrder>();
        }

        [ExpectedException(typeof(FlatBuffersStructFieldReflectionException), ExpectedMessage = "Order value must be unique")]
        [Test]
        public void GetTypeModel_BrokenUserOrderingMultipleSameFieldOrder_ThrowsException()
        {
            GetTypeModel<BrokenUserOrderingMultipleSameFieldOrder>();
        }

        [Test]
        public void GetTypeModel_WhenDefaultValueAttribute_DefaultValueProviderReturnsCorrectValues()
        {
            var typeModel = GetTypeModel<TableWithDefaultValue>();
            Assert.IsTrue(typeModel.IsTable);
            Assert.IsNotNull(typeModel.StructDef);
            var structDef = typeModel.StructDef;

            var fields = structDef.Fields.OrderBy(i => i.Index).ToArray();
            Assert.AreEqual(42, fields[0].DefaultValueProvider.GetDefaultValue(typeof(int)));
            Assert.IsTrue(fields[0].DefaultValueProvider.IsDefaultValueSetExplicity);
            Assert.IsTrue(fields[0].DefaultValueProvider.IsDefaultValue(42));
        }

        [Test]
        public void GetTypeModel_WithRequiredProperties_ReflectsRequiredFlagCorrectly()
        {
            var typeModel = GetTypeModel<TableWithRequiredFields>();
            Assert.IsTrue(typeModel.IsTable);
            Assert.IsNotNull(typeModel.StructDef);
            var structDef = typeModel.StructDef;

            var orderedFields = structDef.Fields.OrderBy(i => i.Index).ToArray();
            Assert.AreEqual(3, orderedFields.Length);

            Assert.IsTrue(orderedFields.All(i => i.Required));
        }

        [Test]
        public void GetTypeModel_WithForceAlign_ReflectsForceAlignValueCorrectly()
        {
            var typeModel = GetTypeModel<TestStructWithForcedAlignment>();
            Assert.IsTrue(typeModel.IsStruct);
            Assert.IsNotNull(typeModel.StructDef);
            var structDef = typeModel.StructDef;

            Assert.IsTrue(structDef.IsForceAlignSet);
            Assert.AreEqual(16, structDef.ForceAlignSize);
        }

        [Test]
        public void GetTypeModel_WithForceAlign_FieldsHaveCorrectPadding()
        {
            var typeModel = GetTypeModel<TestStructWithForcedAlignment>();
            Assert.IsTrue(typeModel.IsStruct);
            Assert.IsNotNull(typeModel.StructDef);
            var structDef = typeModel.StructDef;

            Assert.AreEqual(3, structDef.Fields.Count());

            // Check size and alignmnet
            Assert.AreEqual(16, typeModel.InlineSize);
            Assert.AreEqual(16, typeModel.InlineAlignment);

            var xField = structDef.GetFieldByName("X");
            Assert.AreEqual(4, xField.TypeModel.InlineSize);
            Assert.AreEqual(0, xField.Padding);

            var yField = structDef.GetFieldByName("Y");
            Assert.AreEqual(4, yField.TypeModel.InlineSize);
            Assert.AreEqual(0, yField.Padding);

            var zField = structDef.GetFieldByName("Z");
            Assert.AreEqual(4, zField.TypeModel.InlineSize);
            Assert.AreEqual(4, zField.Padding);     // Field should be padded to align to 16 byte boundary            
        }

        [Test]
        public void GetTypeModel_WithoutOriginalOrdering_DoesntHaveFlag()
        {
            var typeModel = GetTypeModel<TestTable1>();
            Assert.IsTrue(typeModel.IsTable);
            Assert.IsNotNull(typeModel.StructDef);
            var structDef = typeModel.StructDef;

            Assert.IsFalse(structDef.UseOriginalOrdering);
        }

        [Test]
        public void GetTypeModel_WithOriginalOrdering_ReflectsFlagCorrectly()
        {
            var typeModel = GetTypeModel<TestTableWithOriginalOrdering>();
            Assert.IsTrue(typeModel.IsTable);
            Assert.IsNotNull(typeModel.StructDef);
            var structDef = typeModel.StructDef;

            Assert.IsTrue(structDef.UseOriginalOrdering);
        }
    }
}
