using System;
using System.Collections;
using System.Collections.Generic;
using System.Linq;
using System.Reflection;
using FlatBuffers.Attributes;

namespace FlatBuffers
{
    public class TypeModelRegistry
    {
        private static readonly TypeModelRegistry s_default = new TypeModelRegistry();
        public static TypeModelRegistry Default { get { return s_default; }}

        private readonly Dictionary<Type, TypeModel> _typeModels = new Dictionary<Type, TypeModel>();

        private static readonly Dictionary<Type, BaseType> _clrTypeToBaseType;

        static TypeModelRegistry()
        {
            _clrTypeToBaseType = new Dictionary<Type, BaseType>
            {
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

        

        private BaseType DeduceBaseType(Type type)
        {
            if (type.IsPrimitive)
            {
                BaseType baseType;
                if (!_clrTypeToBaseType.TryGetValue(type, out baseType))
                {
                    throw new NotImplementedException("Haven't mapped this type");
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
                return DeduceBaseType(Enum.GetUnderlyingType(type));
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
                    throw new Exception(
                        "ForceAlign must be a power of two integer ranging from the struct's natural alignment to 16");
                }
                structTypeDef.MinAlign = align;
                structTypeDef.ForceAlignSize = align;
            }
        }

        private void ApplyTableAttributeFlags(StructTypeDefinition structTypeDef, FlatBuffersTableAttribute attribute)
        {
            structTypeDef.UseOriginalOrdering = attribute.OriginalOrdering;
        }

        private EnumTypeDefinition ReflectEnumDef(Type type)
        {
            var enumTypeDef = new EnumTypeDefinition();
            ReflectUserMetadata(type, enumTypeDef);
            return enumTypeDef;
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

                return attr.IsOrderSetExplicitly;
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

            for (var i = 0; i < members.Length; ++i)
            {
                try
                {
                    var field = ReflectStructFieldDef(members[i], i);
                    
                    if (structTypeDef.HasCustomOrdering)
                    {
                        if (!field.IsIndexSetExplicitly)
                            throw new FlatBuffersStructFieldReflectionException("Order must be set on all fields");

                        if (structTypeDef.Fields.Any(n=>n.Index == field.Index))
                            throw new FlatBuffersStructFieldReflectionException("Order value must be unique");

                    }

                    structTypeDef.AddField(field);
                }
                catch (FlatBuffersStructFieldReflectionException fieldEx)
                {
                    fieldEx.ClrType = type;
                    fieldEx.Member = members[i];
                    throw;
                }
            }

            // Pad the last field in the struct so it aligns correctly
            structTypeDef.PadLastField(structTypeDef.MinAlign);

            if (structTypeDef.HasCustomOrdering)
            {
                // Validate the sequence
                var i = 0;
                foreach (var field in structTypeDef.Fields.OrderBy(n => n.Index))
                {
                    if (field.Index != i)
                    {
                        throw new FlatBuffersStructFieldReflectionException("Order range must be contiguous sequence from 0..N");
                    }
                    ++i;
                }
            }

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
            throw new FlatBuffersStructFieldReflectionException("Member type not supported");
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

        private FieldTypeDefinition ReflectStructFieldDef(MemberInfo member, int index)
        {
            var valueProvider = CreateValueProvider(member);
            var defaultValueProvider = CreateDefaultValueProvider(member);

            var memberTypeModel = GetTypeModel(valueProvider.ValueType);

            var attr = member.Attribute<FlatBuffersFieldAttribute>();

            var field = new FieldTypeDefinition(valueProvider, defaultValueProvider)
            {
                Name = member.Name, // TODO: allow attribute override
                TypeModel = memberTypeModel,
                OriginalIndex = index
            };

            ReflectUserMetadata(member, field);
            
            if (attr != null)
            {
                if (attr.IsOrderSetExplicitly)
                {
                    field.Index = attr.Order;
                }
                field.Required = attr.Required;
                field.Deprecated = attr.Deprecated;
            }

            return field;
        }

        private void ReflectUserMetadata(ICustomAttributeProvider type, TypeDefinition typeDef)
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
        }

        public TypeModel GetTypeModel<T>()
        {
            return GetTypeModel(typeof (T));
        }

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
                var enumerableTypeModel = GetTypeModel(elementType);
                var elementBaseType = DeduceBaseType(elementType);
                typeModel = new TypeModel(this, typeName, type, baseType, elementBaseType);
            }
            else
            {
                typeModel = new TypeModel(this, typeName, type, baseType);
            }

            if (baseType == BaseType.Struct)
            {
                typeModel.StructDef = ReflectStructDef(type);
            }
            else if (typeof (Enum).IsAssignableFrom(type))
            {
                typeModel.EnumDef = ReflectEnumDef(type);
            }

            _typeModels.Add(type, typeModel);

            return typeModel;
        }

        
    }
}