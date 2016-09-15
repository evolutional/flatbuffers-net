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
        /// This class will fail to reflect because the user has specified the same field Id multiple times
        /// </summary>
        private abstract class BrokenUserOrderingMultipleSameFieldOrder
        {
            [FlatBuffersField(Id = 0)]
            public int FieldA { get; set; }

            [FlatBuffersField(Id = 1)]
            public int FieldB { get; set; }

            [FlatBuffersField(Id = 1)]
            public int FieldC { get; set; }
        }


        /// <summary>
        /// This class will fail to reflect because the user has a gap in the field order
        /// </summary>
        private abstract class BrokenUserOrderingGapInFieldOrder
        {
            [FlatBuffersField(Id = 0)]
            public int FieldA { get; set; }

            [FlatBuffersField(Id = 1)]
            public int FieldB { get; set; }

            // Missing field 2

            [FlatBuffersField(Id = 3)]
            public int FieldC { get; set; }
        }

        /// <summary>
        /// This class will fail to reflect because the user has a gap in the field order
        /// </summary>
        private abstract class BrokenUserOrderingNotAllFieldsHaveOrder
        {
            [FlatBuffersField(Id = 0)]
            public int FieldA { get; set; }

            // Id not set for this type
            [FlatBuffersField]
            public int FieldB { get; set; }

            [FlatBuffersField(Id = 2)]
            public int FieldC { get; set; }
        }

        private abstract class TableWithDefaultValue
        {
            [FlatBuffersDefaultValue(42)]
            public int Prop { get; set; }
        }

        [FlatBuffersTable]
        [FlatBuffersStruct]
        private struct StructWithTwoAttributes
        {
            public int A { get; set; }
            public byte B { get; set; }
        }

        [FlatBuffersTable]
        [FlatBuffersStruct]
        private abstract class ClassWithTwoAttributes
        {
            public int A { get; set; }
            public byte B { get; set; }
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

        [ExpectedException(typeof(FlatBuffersStructFieldReflectionException), ExpectedMessage = "Id must be set on all fields")]
        [Test]
        public void GetTypeModel_BrokenUserOrderingNotAllFieldsHaveOrder_ThrowsException()
        {
            GetTypeModel<BrokenUserOrderingNotAllFieldsHaveOrder>();
        }

        [ExpectedException(typeof(FlatBuffersStructFieldReflectionException), ExpectedMessage = "Id range must be contiguous sequence from 0..N")]
        [Test]
        public void GetTypeModel_BrokenUserOrderingGapInFieldOrder_ThrowsException()
        {
            GetTypeModel<BrokenUserOrderingGapInFieldOrder>();
        }

        [ExpectedException(typeof(FlatBuffersStructFieldReflectionException), ExpectedMessage = "Id range must be contiguous sequence from 0..N")]
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
        public void GetTypeModel_WithNameProperties_ReflectsNameValueCorrectly()
        {
            var typeModel = GetTypeModel<TableWithAlternativeNameFields>();
            Assert.IsTrue(typeModel.IsTable);
            Assert.IsNotNull(typeModel.StructDef);
            var structDef = typeModel.StructDef;

            var orderedFields = structDef.Fields.OrderBy(i => i.Index).ToArray();
            Assert.AreEqual(2, orderedFields.Length);

            Assert.AreEqual("AltIntProp", orderedFields[0].Name);
            Assert.AreEqual("AltStringProp", orderedFields[1].Name);
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

        [Test]
        public void GetTypeModel_WithIgnoredField_ReflectsOnlyFieldsWithoutAttribute()
        {
            var typeModel = GetTypeModel<TestTableWithIgnoredField>();
            Assert.IsTrue(typeModel.IsTable);
            Assert.IsNotNull(typeModel.StructDef);
            var structDef = typeModel.StructDef;

            Assert.AreEqual(2, structDef.Fields.Count());

            var ignoredField = structDef.GetFieldByName("ByteProp");
            Assert.IsNull(ignoredField);

            var field1 = structDef.GetFieldByName("IntProp");
            Assert.AreEqual(0, field1.Index);

            var field2 = structDef.GetFieldByName("ShortProp");
            Assert.AreEqual(1, field2.Index);
        }

        [ExpectedException(typeof(FlatBuffersTypeReflectionException), ExpectedMessage = "Cannot reflect type with 'FlatBuffersIgnoreAttribute'")]
        [Test]
        public void GetTypeModel_WithIgnoredTable_ThrowsException()
        {
            var typeModel = GetTypeModel<TestIgnoredTable>();
        }

        [Test]
        public void GetTypeModel_WithDeprecatedField_ReflectsDeprecatedFlagCorrectly()
        {
            var typeModel = GetTypeModel<TestTableWithDeprecatedField>();
            Assert.IsTrue(typeModel.IsTable);
            Assert.IsNotNull(typeModel.StructDef);
            var structDef = typeModel.StructDef;

            Assert.AreEqual(3, structDef.Fields.Count());

            var deprecatedField = structDef.GetFieldByName("ByteProp");
            Assert.IsTrue(deprecatedField.Deprecated);
        }

        [Test]
        public void GetTypeModel_WithKeyField_ReflectsKeyFlagCorrectly()
        {
            var typeModel = GetTypeModel<TestTableWithKey>();
            Assert.IsTrue(typeModel.IsTable);
            Assert.IsNotNull(typeModel.StructDef);
            var structDef = typeModel.StructDef;

            Assert.IsTrue(structDef.HasKey);

            var keyField = structDef.GetFieldByName("IntProp");
            Assert.IsTrue(keyField.Key);

            var nonKeyField = structDef.GetFieldByName("OtherProp");
            Assert.IsFalse(nonKeyField.Key);
        }

        [ExpectedException(typeof(FlatBuffersStructFieldReflectionException), ExpectedMessage = "Cannot add 'OtherProp' as a key field, key already exists")]
        [Test]
        public void GetTypeModel_With2KeyField_ThrowsException()
        {
            var typeModel = GetTypeModel<TestTableWith2Keys>();
        }

        [ExpectedException(typeof(FlatBuffersStructFieldReflectionException), ExpectedMessage = "Cannot add 'OtherProp' as a key field. Type must be string or scalar")]
        [Test]
        public void GetTypeModel_WithKeyOnWrongType_ThrowsException()
        {
            var typeModel = GetTypeModel<TestTableWithKeyOnBadType>();
        }

        [Test]
        public void GetTypeModel_WithHashField_ReflectsKeyFlagCorrectly()
        {
            var typeModel = GetTypeModel<TestTableWithHash>();
            Assert.IsTrue(typeModel.IsTable);
            Assert.IsNotNull(typeModel.StructDef);
            var structDef = typeModel.StructDef;

            var hasField = structDef.GetFieldByName("IntProp");
            Assert.AreEqual(FlatBuffersHash.Fnv1_32, hasField.Hash);

            var nonHashField = structDef.GetFieldByName("OtherProp");
            Assert.AreEqual(FlatBuffersHash.None, nonHashField.Hash);
        }

        [ExpectedException(typeof(FlatBuffersStructFieldReflectionException), ExpectedMessage = "Cannot use Hash setting on 'OtherProp'. Type must be int/uint/long/ulong")]
        [Test]
        public void GetTypeModel_WithBadHashAttribute_ThrowsException()
        {
            var typeModel = GetTypeModel<TestTableWithHashOnNonIntType>();
        }

        [Test]
        public void GetTypeModel_TableWithCustomMetadata_ReflectsAttributesCorrectly()
        {
            var typeModel = GetTypeModel<TableWithUserMetadata>();
            Assert.IsTrue(typeModel.IsTable);
            Assert.IsNotNull(typeModel.StructDef);
            var structDef = typeModel.StructDef;

            var structAttr = structDef.Metadata.GetByName("types");
            Assert.IsNotNull(structAttr);
            Assert.IsFalse(structAttr.HasValue);

            var propA = structDef.GetFieldByName("PropA");
            var propAAttr = propA.Metadata.GetByName("priority");
            Assert.IsNotNull(propAAttr);
            Assert.IsTrue(propAAttr.HasValue);
            Assert.IsTrue(propAAttr.IsUserMetaData);
            Assert.AreEqual(1, propAAttr.Value);

            var propB = structDef.GetFieldByName("PropB");
            var propBAttr = propB.Metadata.GetByName("toggle");
            Assert.IsNotNull(propBAttr);
            Assert.IsTrue(propBAttr.HasValue);
            Assert.IsTrue(propBAttr.IsUserMetaData);
            Assert.AreEqual(true, propBAttr.Value);

            var propC = structDef.GetFieldByName("PropC");
            var propCAttr = propC.Metadata.GetByName("category");
            Assert.IsNotNull(propCAttr);
            Assert.IsTrue(propCAttr.HasValue);
            Assert.IsTrue(propCAttr.IsUserMetaData);
            Assert.AreEqual("tests", propCAttr.Value);
        }

        [Test]
        public void GetTypeModel_EnumWithCustomMetadata_ReflectsAttributesCorrectly()
        {
            var typeModel = GetTypeModel<EnumWithUserMetadata>();
            Assert.IsTrue(typeModel.IsEnum);
            Assert.IsNotNull(typeModel.EnumDef);
            var def = typeModel.EnumDef;

            var attr = def.Metadata.GetByName("magicEnum");
            Assert.IsNotNull(attr);
            Assert.IsFalse(attr.HasValue);
        }

        [Test]
        public void GetTypeModel_EnumWithAutoSizeToByte_ReflectsSizeCorrectly()
        {
            var typeModel = GetTypeModel<TestEnumAutoSizedToByte>();
            Assert.IsTrue(typeModel.IsEnum);
            Assert.IsNotNull(typeModel.EnumDef);
            var def = typeModel.EnumDef;
            Assert.IsTrue(def.IsAutoSized);
            Assert.AreEqual(BaseType.UChar, typeModel.BaseType);
        }

        [Test]
        public void GetTypeModel_EnumWithAutoSizeToSByte_ReflectsSizeCorrectly()
        {
            var typeModel = GetTypeModel<TestEnumAutoSizedToSByte>();
            Assert.IsTrue(typeModel.IsEnum);
            Assert.IsNotNull(typeModel.EnumDef);
            var def = typeModel.EnumDef;
            Assert.IsTrue(def.IsAutoSized);
            Assert.AreEqual(BaseType.Char, typeModel.BaseType);
        }

        [Test]
        public void GetTypeModel_EnumWithAutoSizeToShort_ReflectsSizeCorrectly()
        {
            var typeModel = GetTypeModel<TestEnumAutoSizedToShort>();
            Assert.IsTrue(typeModel.IsEnum);
            Assert.IsNotNull(typeModel.EnumDef);
            var def = typeModel.EnumDef;
            Assert.IsTrue(def.IsAutoSized);
            Assert.AreEqual(BaseType.Short, typeModel.BaseType);
        }

        [Test]
        public void GetTypeModel_EnumWithAutoSizeToUShort_ReflectsSizeCorrectly()
        {
            var typeModel = GetTypeModel<TestEnumAutoSizedToUShort>();
            Assert.IsTrue(typeModel.IsEnum);
            Assert.IsNotNull(typeModel.EnumDef);
            var def = typeModel.EnumDef;
            Assert.IsTrue(def.IsAutoSized);
            Assert.AreEqual(BaseType.UShort, typeModel.BaseType);
        }

        [Test]
        public void GetTypeModel_EnumWithAutoSizeToShortAndHasExplicitBase_UsesExplicitSize()
        {
            var typeModel = GetTypeModel<TestEnumWithExplictSizeNotAutoSized>();
            Assert.IsTrue(typeModel.IsEnum);
            Assert.IsNotNull(typeModel.EnumDef);
            var def = typeModel.EnumDef;
            Assert.IsFalse(def.IsAutoSized);
            Assert.AreEqual(BaseType.Short, typeModel.BaseType);
        }


        [Test]
        public void GetTypeModel_ClassReflectedAsAStruct_SetsIsStructFlag()
        {
            var typeModel = GetTypeModel<ClassReflectedAsAStruct>();
            Assert.IsTrue(typeModel.IsStruct);
        }

        [Test]
        public void GetTypeModel_StructReflectedAsATable_SetsIsTableFlag()
        {
            var typeModel = GetTypeModel<StructReflectedAsATable>();
            Assert.IsTrue(typeModel.IsTable);
        }

        [ExpectedException(typeof(FlatBuffersTypeReflectionException), ExpectedMessage = "Cannot use FlatBuffersStructAttribute and FlatBuffersTableAttribute on same type")]
        [Test]
        public void GetTypeModel_WhenStructAnd_StructAndTableAttributesUsed_ThrowsException()
        {
            var typeModel = GetTypeModel<StructWithTwoAttributes>();
        }

        [ExpectedException(typeof(FlatBuffersTypeReflectionException), ExpectedMessage = "Cannot use FlatBuffersStructAttribute and FlatBuffersTableAttribute on same type")]
        [Test]
        public void GetTypeModel_WhenClassAnd_StructAndTableAttributesUsed_ThrowsException()
        {
            var typeModel = GetTypeModel<ClassWithTwoAttributes>();
        }

        [Test]
        public void GetTypeModel_UnionType_ReflectsMemberTypeFieldsAndIndexes()
        {
            var typeModel = GetTypeModel<TestUnion>();
            Assert.IsTrue(typeModel.IsUnion);
            Assert.IsNotNull(typeModel.UnionDef);
            var def = typeModel.UnionDef;
            var fields = def.Fields.ToArray();

            Assert.AreEqual(3, fields.Length);
            Assert.AreEqual(0, fields[0].Index);
            Assert.AreEqual(null, fields[0].MemberType);

            Assert.AreEqual(1, fields[1].Index);
            Assert.AreEqual(typeof(TestTable1), fields[1].MemberType.Type);

            Assert.AreEqual(2, fields[2].Index);
            Assert.AreEqual(typeof(TestTable2), fields[2].MemberType.Type);
        }

        [Test]
        public void GetTypeModel_TestTableWithUnion_ReflectsUnionFieldAsUnion()
        {
            var typeModel = GetTypeModel<TestTableWithUnion>();

            Assert.IsTrue(typeModel.IsTable);
            Assert.IsNotNull(typeModel.StructDef);
            var structDef = typeModel.StructDef;

            var fields = structDef.Fields.ToArray();

            Assert.AreEqual(3, fields.Length);

            var intProp = structDef.GetFieldByName("IntProp");
            Assert.AreEqual(BaseType.Int,intProp.TypeModel.BaseType);

            var unionPropType = structDef.GetFieldByName("UnionProp_type");
            Assert.AreEqual(BaseType.UType, unionPropType.TypeModel.BaseType);

            var unionProp = structDef.GetFieldByName("UnionProp");
            Assert.AreEqual(BaseType.Union, unionProp.TypeModel.BaseType);
            var unionDef = unionProp.TypeModel.UnionDef;
            Assert.AreEqual("TestUnion", unionDef.Name);
        }

        [Test]
        public void GetTypeModel_TestTableWithComments_ReflectsAllComments()
        {
            var typeModel = GetTypeModel<TestTableWithComments>();

            Assert.IsTrue(typeModel.IsTable);
            Assert.IsNotNull(typeModel.StructDef);
            var structDef = typeModel.StructDef;

            var fields = structDef.Fields.ToArray();

            Assert.AreEqual(3, fields.Length);

            Assert.AreEqual("This is a comment on a table", structDef.Comments.FirstOrDefault());

            var intProp = structDef.GetFieldByName("Field");
            Assert.AreEqual("Comment on an int field", intProp.Comments.FirstOrDefault());

            var stringField = structDef.GetFieldByName("StringField");
            var stringFieldComments = stringField.Comments.ToArray();
            Assert.AreEqual("First comment of Multiple comments", stringFieldComments[0]);
            Assert.AreEqual("Second comment of Multiple comments", stringFieldComments[1]);

            var anotherField = structDef.GetFieldByName("AnotherField");
            Assert.AreEqual("Multiline\nIs supported\ntoo", anotherField.Comments.FirstOrDefault());
        }

        [Ignore("The logic that handles custom ordering in tables with unions is flawed. Waiting for a fix from https://github.com/google/flatbuffers/issues/3499")]
        [Test]
        public void GetTypeModel_TestTableWithUnionAndCustomOrdering_HasCorrectOrderingForTypeField()
        {
            var typeModel = GetTypeModel<TestTableWithUnionAndCustomOrdering>();

            Assert.IsTrue(typeModel.IsTable);
            Assert.IsNotNull(typeModel.StructDef);
            var structDef = typeModel.StructDef;

            var fields = structDef.Fields.ToArray();

            Assert.AreEqual(3, fields.Length);

            var intProp = structDef.GetFieldByName("IntProp");
            Assert.AreEqual(2, intProp.Index);

            var unionPropType = structDef.GetFieldByName("UnionProp_type");
            Assert.AreEqual(0, unionPropType.Index);

            var unionProp = structDef.GetFieldByName("UnionProp");
            Assert.AreEqual(1, unionProp.Index);
        }
    }
}
