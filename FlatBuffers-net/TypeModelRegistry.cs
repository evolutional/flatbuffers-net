using System;
using System.Collections;
using System.Collections.Generic;
using System.Linq;
using System.Reflection;
using FlatBuffers.Attributes;
using FlatBuffers.Utilities;

namespace FlatBuffers
{
    /// <summary>
    /// Class that contains a collection of reflected types for use in FlatBuffers serialization and schema generation
    /// </summary>
    public class TypeModelRegistry
    {
        private static readonly TypeModelRegistry s_default = new TypeModelRegistry();
        
        /// <summary>
        /// Gets the globally shared-Default instance of the TypeModelRegistry
        /// </summary>
        public static TypeModelRegistry Default { get { return s_default; }}

        private readonly Dictionary<Type, TypeModel> _typeModels = new Dictionary<Type, TypeModel>();

        private static readonly Dictionary<Type, BaseType> _clrTypeToBaseType;

        static TypeModelRegistry()
        {
            _clrTypeToBaseType = new Dictionary<Type, BaseType>
            {
                {typeof(UnionFieldType), BaseType.UType},
                {typeof(bool), BaseType.Bool},
                {typeof(sbyte), BaseType.Char},
                {typeof(byte), BaseType.UChar},
                {typeof(short), BaseType.Short},
                {typeof(ushort), BaseType.UShort},
                {typeof(int), BaseType.Int},
                {typeof(uint), BaseType.UInt},
                {typeof(long), BaseType.Long},
                {typeof(ulong), BaseType.ULong},
                {typeof(float), BaseType.Float},
                {typeof(double), BaseType.Double},
            };
        }

        /// <summary>
        /// Initializes an instance of the TypeModelRegistry class
        /// </summary>
        public TypeModelRegistry()
        {
            var type = typeof (UnionFieldType);
            var uft = new TypeModel(this, type.Name, type, BaseType.UType);
            _typeModels.Add(type, uft);
        }

        private BaseType DeduceBaseType(Type type)
        {
            if (type.IsPrimitive)
            {
                BaseType baseType;
                if (!_clrTypeToBaseType.TryGetValue(type, out baseType))
                {
                    throw new FlatBuffersTypeReflectionException("BaseType of Type '{0}' cannot be deduced", type.FullName);
                }
                return baseType;
            }
            if (type == typeof (string))
            {
                return BaseType.String;
            }
            if (typeof (IEnumerable).IsAssignableFrom(type))
            {
                return BaseType.Vector;
            }
            if (type.IsEnum)
            {
                if (type.Defined<FlatBuffersUnionAttribute>())
                {
                    return BaseType.Union;
                }
                return DeduceBaseTypeForEnum(type);
            }
            if (type.IsClass || type.IsValueType)
            {
                return BaseType.Struct;
            }
            
            return BaseType.None;
        }

        private void ApplyStructAttributeFlags(StructTypeDefinition structTypeDef, FlatBuffersStructAttribute attribute)
        {
            // check force align makes sense
            if (attribute.IsForceAlignSet)
            {
                var align = attribute.ForceAlign;
                if (align < structTypeDef.MinAlign ||
                    align > 16 ||
                    (align & (align - 1)) != 0)
                {
                    throw new FlatBuffersTypeReflectionException(
                        "ForceAlign must be a power of two integer ranging from the struct's natural alignment to 16");
                }
                structTypeDef.MinAlign = align;
                structTypeDef.ForceAlignSize = align;
            }
        }

        private void ApplyTableAttributeFlags(StructTypeDefinition structTypeDef, FlatBuffersTableAttribute attribute)
        {
            structTypeDef.UseOriginalOrdering = attribute.OriginalOrdering;
            if (attribute.HasIdentitifer)
            {
                structTypeDef.Identifier = attribute.Identifier;
            }
        }

        private EnumTypeDefinition ReflectEnumDef(Type type)
        {
            var enumTypeDef = new EnumTypeDefinition {UnderlyingType = Enum.GetUnderlyingType(type)};

            var enumAttr = type.Attribute<FlatBuffersEnumAttribute>();
            enumTypeDef.BitFlags = enumAttr != null && enumAttr.BitFlags || type.Defined<FlagsAttribute>();
            
            ReflectUserMetadata(type, enumTypeDef);
            return enumTypeDef;
        }

        private UnionTypeDefinition ReflectUnionDef(Type type)
        {
            var unionTypeDef = new UnionTypeDefinition() { Name = type.Name };

            var members = type.GetMembers(BindingFlags.Public|BindingFlags.Static).Where(i=>i.MemberType == MemberTypes.Field).ToArray();
            for (var i = 0; i < members.Length; ++i)
            {
                var member = members[i];
                var attr = member.Attribute<FlatBuffersUnionMemberAttribute>();
                var memberTypeModel = GetTypeModel(attr.MemberType);
                unionTypeDef.AddField(i + 1, member.Name, memberTypeModel);
            }

            ReflectUserMetadata(type, unionTypeDef);
            return unionTypeDef;
        }

        private BaseType DeduceBaseTypeForEnum(Type type)
        {
            var underlyingType = Enum.GetUnderlyingType(type);

            if (underlyingType != typeof (int))
            {
                // Always use the base member if it's been set explicitly
                return DeduceBaseType(underlyingType);
            }

            if (!type.Defined<FlatBuffersEnumAttribute>())
            {
                // no attribute, so use the underlying member
                return DeduceBaseType(underlyingType);
            }

            var attr = type.Attribute<FlatBuffersEnumAttribute>();
            if (!attr.AutoSizeEnum)
            {
                // attribute not set, use the underlying member
                return DeduceBaseType(underlyingType);
            }

            var values = Enum.GetValues(type);

            var minValue = (long)Convert.ChangeType(values.GetValue(0), typeof(long));
            var maxValue = (long)Convert.ChangeType(values.GetValue(values.Length-1), typeof(long));

            var autoFitType = typeof (int);

            if (minValue >= sbyte.MinValue && maxValue <= sbyte.MaxValue)
            {
                autoFitType = typeof(sbyte);
            }
            else if (minValue >= byte.MinValue && maxValue <= byte.MaxValue)
            {
                autoFitType = typeof (byte);
            }
            else if(minValue >= short.MinValue && maxValue <= short.MaxValue)
            {
                autoFitType = typeof(short);
            }
            else if (minValue >= ushort.MinValue && maxValue <= ushort.MaxValue)
            {
                autoFitType = typeof(ushort);
            }

            return DeduceBaseType(autoFitType);
        }

        private StructTypeDefinition ReflectStructDef(Type type)
        {
            var members =
               type.GetMembers(BindingFlags.Public | BindingFlags.Instance)
                   .Where(i => i.MemberType == MemberTypes.Field || i.MemberType == MemberTypes.Property)
                   .Where(i=>!i.Defined<FlatBuffersIgnoreAttribute>())
                   .ToArray();

            var isFixed = !type.IsClass;

            if (type.IsClass && type.Defined<FlatBuffersStructAttribute>())
            {
                isFixed = true;
            }

            if (!type.IsClass && type.Defined<FlatBuffersTableAttribute>())
            {
                isFixed = false;
            }

            if (type.Defined<FlatBuffersTableAttribute>() && type.Defined<FlatBuffersStructAttribute>())
            {
                throw new FlatBuffersTypeReflectionException("Cannot use FlatBuffersStructAttribute and FlatBuffersTableAttribute on same type");
            }

            var structTypeDef = new StructTypeDefinition(isFixed);

            ReflectUserMetadata(type, structTypeDef);

            if (members.Any(i =>
            { 
                var attr = i.Attribute<FlatBuffersFieldAttribute>();
                if (attr == null)
                    return false;

                return attr.IsIdSetExplicitly;
            }))
            {
                structTypeDef.HasCustomOrdering = true;
            }

            if (structTypeDef.IsFixed)
            {
                var structAttr = type.Attribute<FlatBuffersStructAttribute>();
                if (structAttr != null)
                {
                    ApplyStructAttributeFlags(structTypeDef, structAttr);
                }
            }
            else
            {
                var tableAttr = type.Attribute<FlatBuffersTableAttribute>();
                if (tableAttr != null)
                {
                    ApplyTableAttributeFlags(structTypeDef, tableAttr);
                }
            }

            foreach (var member in members)
            {
                try
                {
                    ReflectStructFieldDef(structTypeDef, member);                   
                }
                catch (FlatBuffersStructFieldReflectionException fieldEx)
                {
                    fieldEx.ClrType = type;
                    fieldEx.Member = member;
                    throw;
                }
            }

            // Pad the last field in the struct so it aligns correctly
            structTypeDef.PadLastField(structTypeDef.MinAlign);
            structTypeDef.FinalizeFieldDefinition();
            return structTypeDef;
        }

        
        private IValueProvider CreateValueProvider(MemberInfo member)
        {
            if (member.MemberType == MemberTypes.Property)
            {
                return new PropertyValueProvider(member as PropertyInfo);
            }
            else if (member.MemberType == MemberTypes.Field)
            {
                return new FieldValueProvider(member as FieldInfo);
            }
            throw new FlatBuffersStructFieldReflectionException("Member member not supported");
        }

        private IDefaultValueProvider CreateDefaultValueProvider(MemberInfo member)
        {
            var attr = member.Attribute<FlatBuffersDefaultValueAttribute>();
            if (attr == null)
            {
                return TypeDefaultValueProvider.Instance;
            }
            if (attr.Value == null)
            {
                throw new FlatBuffersStructFieldReflectionException("Default value attribute used with null Value");
            }
            return new AttributeDefaultValueProvider(attr);
        }

        private void ReflectStructFieldDef(StructTypeDefinition structDef, MemberInfo member)
        {
            var valueProvider = CreateValueProvider(member);
            var defaultValueProvider = CreateDefaultValueProvider(member);
            var attr = member.Attribute<FlatBuffersFieldAttribute>();

            var valueType = valueProvider.ValueType;

            TypeModel memberTypeModel = null;
            TypeModel nestedTypeModel = null;

            if (valueType == typeof (object))
            {
                if (attr == null || (!attr.IsUnionField && !attr.HasNestedFlatBufferType))
                {
                    throw new FlatBuffersStructFieldReflectionException("Field with 'object' member must have a UnionType or NestedFlatBufferType declared");
                }

                if (attr.HasNestedFlatBufferType)
                {
                    memberTypeModel = nestedTypeModel = GetTypeModel(attr.NestedFlatBufferType);
                }

                if (attr.IsUnionField)
                {
                    memberTypeModel = GetTypeModel(attr.UnionType);
                }
            }
            else
            {
                if (attr != null && attr.HasNestedFlatBufferType)
                {
                    throw new FlatBuffersStructFieldReflectionException("HasNestedFlatBufferType can only be used on fields with 'object' member");
                }

                memberTypeModel = GetTypeModel(valueType);
            }

            FieldTypeDefinition unionTypeField = null;

            if (memberTypeModel.IsUnion)
            {
                var unionTypeFieldValueProvider = new UnionTypeValueProvider(valueProvider, memberTypeModel);
                var unionTypeFieldDefaultValueProvider = TypeDefaultValueProvider.Instance;
                unionTypeField = new FieldTypeDefinition(unionTypeFieldValueProvider,
                    unionTypeFieldDefaultValueProvider)
                {
                    Name = string.Format("{0}_type", member.Name),
                    TypeModel = GetTypeModel<UnionFieldType>()
                };
            }

            var field = new FieldTypeDefinition(valueProvider, defaultValueProvider)
            {
                Name = member.Name, // TODO: allow attribute override
                TypeModel = memberTypeModel,
            };

            if (nestedTypeModel != null)
            {
                field.NestedFlatBufferType = nestedTypeModel;
            }

            ReflectUserMetadata(member, field);
            
            if (attr != null)
            {
                if (!string.IsNullOrEmpty(attr.Name))
                {
                    field.Name = attr.Name;
                }

                if (attr.IsIdSetExplicitly)
                {
                    field.UserIndex = attr.Id;
                    if (unionTypeField != null)
                    {
                        unionTypeField.UserIndex = attr.Id - 1;
                    }
                }
                field.Required = attr.Required;
                field.Deprecated = attr.Deprecated;

                if (attr.Key)
                {
                    if (!ValidKeyType(valueType))
                    {
                        throw new FlatBuffersStructFieldReflectionException("Cannot add '{0}' as a key field. Type must be string or scalar", member.Name);
                    }
                    field.Key = attr.Key;
                }

                if (attr.Hash != FlatBuffersHash.None)
                {
                    if (!ValidHashType(valueType))
                    {
                        throw new FlatBuffersStructFieldReflectionException("Cannot use Hash setting on '{0}'. Type must be int/uint/long/ulong", member.Name);
                    }
                    field.Hash = attr.Hash;
                }
            }

            if (unionTypeField != null)
            {
                structDef.AddField(unionTypeField);
                field.UnionTypeField = unionTypeField;
            }
            
            structDef.AddField(field);
        }

        private bool ValidKeyType(Type type)
        {
            if (type == typeof (string) || type.IsPrimitive)
            {
                return true;
            }
            return false;
        }

        private bool ValidHashType(Type type)
        {
            if (type == typeof(int) || type == typeof(uint) || type == typeof(long) || type == typeof(ulong))
            {
                return true;
            }
            return false;
        }

        private void ReflectUserMetadata(Type type, TypeDefinition typeDef)
        {
            foreach (var attr in type.Attributes<FlatBuffersMetadataAttribute>())
            {
                if (!attr.HasValue)
                {
                    typeDef.Metadata.Add(attr.Name, true);
                }
                else
                {
                    typeDef.Metadata.Add(attr.Name, attr.Value, true);
                }
            }

            foreach (var attr in type.Attributes<FlatBuffersCommentAttribute>().OrderBy(i => i.Order))
            {
                typeDef.AddComment(attr.Comment);
            }
        }

        private void ReflectUserMetadata(MemberInfo member, TypeDefinition typeDef)
        {
            foreach (var attr in member.Attributes<FlatBuffersMetadataAttribute>())
            {
                if (!attr.HasValue)
                {
                    typeDef.Metadata.Add(attr.Name, true);
                }
                else
                {
                    typeDef.Metadata.Add(attr.Name, attr.Value, true);
                }
            }

            foreach (var attr in member.Attributes<FlatBuffersCommentAttribute>().OrderBy(i=>i.Order))
            {
                typeDef.AddComment(attr.Comment);
            }
        }

        /// <summary>
        /// Gets a TypeModel for a given member, creating it via relfection if one doesn't exist
        /// </summary>
        /// <typeparam name="T">Type to reflect</typeparam>
        /// <returns>TypeModel</returns>
        public TypeModel GetTypeModel<T>()
        {
            return GetTypeModel(typeof (T));
        }


        /// <summary>
        /// Gets a TypeModel for a given member, creating it via relfection if one doesn't exist
        /// </summary>
        /// <param name="type">Type to reflect</param>
        /// <returns>TypeModel</returns>
        public TypeModel GetTypeModel(Type type)
        {
            TypeModel typeModel = null;

            if (_typeModels.TryGetValue(type, out typeModel))
            {
                return typeModel;
            }
            
            if (type.Defined<FlatBuffersIgnoreAttribute>())
            {
                throw new FlatBuffersTypeReflectionException("Cannot reflect type with 'FlatBuffersIgnoreAttribute'") {ClrType = type};
            }

            

            var typeName = type.Name;   // TODO: attribute

            var baseType = DeduceBaseType(type);

            if (baseType == BaseType.Vector)
            {
                var elementType = TypeHelpers.GetEnumerableElementType(type);
                var elementBaseType = DeduceBaseType(elementType);
                typeModel = new TypeModel(this, typeName, type, baseType, elementBaseType);
            }
            else
            {
                typeModel = new TypeModel(this, typeName, type, baseType);
            }

            _typeModels.Add(type, typeModel);

            if (baseType == BaseType.Struct)
            {
                typeModel.StructDef = ReflectStructDef(type);
            }
            else if (baseType == BaseType.Union)
            {
                typeModel.UnionDef = ReflectUnionDef(type);
            }
            else if (typeModel.IsEnum)
            {
                typeModel.EnumDef = ReflectEnumDef(type);
                if (typeModel.BaseType != BaseType.Int 
                    && typeModel.EnumDef.UnderlyingType == typeof(int))
                {
                    typeModel.EnumDef.IsAutoSized = true;
                }
            }

            
            return typeModel;
        }
    }
}