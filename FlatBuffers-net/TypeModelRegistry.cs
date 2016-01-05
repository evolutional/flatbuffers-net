using System;
using System.Collections;
using System.Collections.Generic;
using System.Linq;
using System.Reflection;
using FlatBuffers.Attributes;

namespace FlatBuffers
{
    public abstract class FlatBuffersBaseException : Exception
    {
        protected FlatBuffersBaseException()
        { }

        protected FlatBuffersBaseException(string format, params object[] args)
            : base(string.Format(format, args))
        { }

        protected FlatBuffersBaseException(Exception innerException, string format, params object[] args)
            : base(string.Format(format, args), innerException)
        { }
    }

    public class FlatBuffersTypeReflectionException : FlatBuffersBaseException
    {
        public FlatBuffersTypeReflectionException()
        {
        }

        public FlatBuffersTypeReflectionException(string format, params object[] args)
            : base(string.Format(format, args))
        {
        }

        public Type ClrType { get; set; }
    }

    public class FlatBuffersStructFieldReflectionException : FlatBuffersTypeReflectionException
    {
        public MemberInfo Member { get; set; }

        public FlatBuffersStructFieldReflectionException()
        {
        }

        public FlatBuffersStructFieldReflectionException(string format, params object[] args) : 
            base(format, args)
        {
        }
    }

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

        private StructTypeDefinition ReflectStructDef(Type type)
        {
            var members =
               type.GetMembers(BindingFlags.Public | BindingFlags.Instance)
                   .Where(i => i.MemberType == MemberTypes.Field || i.MemberType == MemberTypes.Property).ToArray();

            var structTypeDef = new StructTypeDefinition(!type.IsClass);

            if (members.Any(i => i.IsDefined(typeof(FlatBuffersFieldAttribute), true)))
            {
                structTypeDef.HasCustomOrdering = true;
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

            
            if (attr != null)
            {
                if (attr.IsOrderSetExplicitly)
                {
                    field.Index = attr.Order;
                }
            }

            return field;
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

            // todo: get the attribute from the type
            var typeName = type.Name;   // TODO: attribute
            //var attr = type.GetCustomAttributes(typeof (FlatBuffersAttribute), true).FirstOrDefault();
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

            _typeModels.Add(type, typeModel);

            return typeModel;
        }

        
    }
}