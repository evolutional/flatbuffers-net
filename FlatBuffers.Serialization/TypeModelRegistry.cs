using System;
using System.Collections;
using System.Collections.Generic;
using System.Linq;
using System.Reflection;
using FlatBuffers.Serialization.Attributes;

namespace FlatBuffers.Serialization
{
    public static class TypeHelpers
    {
        public static Type GetEnumerableElementType(Type enumerable)
        {
            if (enumerable == null)
            {
                throw new ArgumentException();
            }

            var genericEnumerableInterface = enumerable
                .GetInterfaces()
                .FirstOrDefault(i => i.IsGenericType && i.GetGenericTypeDefinition() == typeof(IEnumerable<>));

            if (genericEnumerableInterface == null)
            {
                throw new NotSupportedException();
            }
            var elementType = genericEnumerableInterface.GetGenericArguments()[0];
            return elementType.IsGenericType && elementType.GetGenericTypeDefinition() == typeof(Nullable<>)
                ? elementType.GetGenericArguments()[0]
                : elementType;
        }
    }

    public class TypeModelRegistry
    {
        private static TypeModelRegistry s_default = new TypeModelRegistry();
        public static TypeModelRegistry Default { get { return s_default; }}

        private Dictionary<Type, TypeModel> _typeModels = new Dictionary<Type, TypeModel>();

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

            for (var i = 0; i < members.Length; ++i)
            {
                var field = ReflectStructFieldDef(members[i], i);
                structTypeDef.AddField(field);
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
            throw new ArgumentException("Member type not supported");
        }

        private FieldTypeDefinition ReflectStructFieldDef(MemberInfo member, int index)
        {
            var valueProvider = CreateValueProvider(member);

            var memberTypeModel = GetTypeModel(valueProvider.ValueType);

            var field = new FieldTypeDefinition(valueProvider)
            {
                Name = member.Name, // TODO: allow attribute override
                TypeModel = memberTypeModel,
                Index = index   // TODO: attribute
            };
            return field;
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