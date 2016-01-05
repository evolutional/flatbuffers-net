using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using FlatBuffers.Attributes;
using FlatBuffers.Tests.TestTypes;
using NUnit.Framework;

namespace FlatBuffers.Tests
{
    [TestFixture]
    public class TypeModelReflectionTests
    {

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
            var registry = TypeModelRegistry.Default;
            var typeModel = registry.GetTypeModel<TestTableWithUserOrdering>();
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
            var registry = TypeModelRegistry.Default;
            registry.GetTypeModel<BrokenUserOrderingNotAllFieldsHaveOrder>();
        }

        [ExpectedException(typeof(FlatBuffersStructFieldReflectionException), ExpectedMessage = "Order range must be contiguous sequence from 0..N")]
        [Test]
        public void GetTypeModel_BrokenUserOrderingGapInFieldOrder_ThrowsException()
        {
            var registry = TypeModelRegistry.Default;
            registry.GetTypeModel<BrokenUserOrderingGapInFieldOrder>();
        }

        [ExpectedException(typeof(FlatBuffersStructFieldReflectionException), ExpectedMessage = "Order value must be unique")]
        [Test]
        public void GetTypeModel_BrokenUserOrderingMultipleSameFieldOrder_ThrowsException()
        {
            var registry = TypeModelRegistry.Default;
            registry.GetTypeModel<BrokenUserOrderingMultipleSameFieldOrder>();
        }

        [Test]
        public void GetTypeModel_WhenDefaultValueAttribute_DefaultValueProviderReturnsCorrectValues()
        {
            var registry = TypeModelRegistry.Default;
            var typeModel = registry.GetTypeModel<TableWithDefaultValue>();
            Assert.IsTrue(typeModel.IsTable);
            Assert.IsNotNull(typeModel.StructDef);
            var structDef = typeModel.StructDef;

            var fields = structDef.Fields.OrderBy(i => i.Index).ToArray();
            Assert.AreEqual(42, fields[0].DefaultValueProvider.GetDefaultValue(typeof(int)));
            Assert.IsTrue(fields[0].DefaultValueProvider.IsDefaultValueSetExplicity);
            Assert.IsTrue(fields[0].DefaultValueProvider.IsDefaultValue(42));
        }
    }
}
