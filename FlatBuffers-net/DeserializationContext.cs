using System;
using System.Collections;
using System.Collections.Generic;
using System.Text;

namespace FlatBuffers
{
    internal class DeserializationContext
    {
        private readonly TypeModelRegistry _typeModelRegistry;
        private readonly Type _rootObjectType;
        private readonly TypeModel _rootTypeModel;
        private readonly ByteBuffer _buffer;
        private readonly Dictionary<int, object> _offsetToObject = new Dictionary<int, object>(); 

        public DeserializationContext(TypeModelRegistry typeModelRegistry, Type rootObjectType, ByteBuffer buffer)
        {
            _typeModelRegistry = typeModelRegistry;
            _rootObjectType = rootObjectType;
            _rootTypeModel = _typeModelRegistry.GetTypeModel(rootObjectType);
            _buffer = buffer;
        }

        private int GetVectorLength(int structBase, int offset)
        {
            offset += structBase;
            offset += _buffer.GetInt(offset);
            return _buffer.GetInt(offset);
        }

        private int GetVectorStart(int structBase, int offset)
        {
            offset += structBase;
            return offset + _buffer.GetInt(offset) + sizeof(int);
        }

        private string GetString(int structBase, int offset)
        {
            offset += structBase;
            offset += _buffer.GetInt(offset);
            var len = _buffer.GetInt(offset);
            var startPos = offset + sizeof(int);
            return Encoding.UTF8.GetString(_buffer.Data, startPos, len);
        }

        private object DeserializeStruct(int structBase, int offset, TypeModel typeModel)
        {
            var obj = Activator.CreateInstance(typeModel.Type);
            var structDef = typeModel.StructDef;

            var fieldOffset = structBase;

            if (!structDef.IsFixed)
            {
                fieldOffset += _buffer.GetInt(offset + structBase) + offset; // indirect
            }
            else
            {
                fieldOffset += offset;
            }

            foreach (var field in structDef.Fields)
            {
                DeserializeStructField(obj, structDef, field, fieldOffset);
            }
            return obj;
        }

        private string DeserializeString(int structBase, int offset)
        {
            return GetString(structBase, offset);
        }

        private Array DeserailizeArray(TypeModel typeModel, int vectorLength, int vectorStart, int elemSize)
        {
            var array = Array.CreateInstance(typeModel.ElementType.TypeOf(), vectorLength);
            for (var i = 0; i < vectorLength; ++i)
            {
                var valuePos = vectorStart + i * elemSize;
                var value = DeserializeScalarValue(typeModel.ElementType, valuePos);
                array.SetValue(value, i);
            }
            return array;
        }

        private object DeserializeVector(int structBase, int offset, FieldTypeDefinition field)
        {
            var typeModel = field.TypeModel;
            object result = null;
            var vectorStart = GetVectorStart(structBase, offset);

            if (_offsetToObject.TryGetValue(vectorStart, out result))
            {
                return result;
            }
            
            var vectorLength = GetVectorLength(structBase, offset);
            var elemSize = typeModel.ElementType.SizeOf();

            if (typeModel.Type.BaseType == typeof(Array) || typeModel.Type.BaseType == typeof(Array))
            {
                result = DeserailizeArray(typeModel, vectorLength, vectorStart, elemSize);
            }
            else if (typeModel.Type.IsGenericType)
            {
                var genericTypeDef = typeModel.Type.GetGenericTypeDefinition();

                if (genericTypeDef == null)
                {
                    throw new NotSupportedException();
                }

                if (genericTypeDef.IsAssignableFrom(typeof(List<>)))
                {
                    var listType = genericTypeDef.MakeGenericType(typeModel.ElementType.TypeOf());
                    var list = (IList)Activator.CreateInstance(listType);
                    
                    for (var i = 0; i < vectorLength; ++i)
                    {
                        var valuePos = vectorStart + i * elemSize;
                        var value = DeserializeScalarValue(typeModel.ElementType, valuePos);
                        list.Add(value);
                    }
                    result = list;
                }
            }

            _offsetToObject.Add(vectorStart, result);
            return result;
        }



        private object DeserializeReferenceType(int structBase, int offset, FieldTypeDefinition field)
        {
            var typeModel = field.TypeModel;

            switch (typeModel.BaseType)
            {
                case BaseType.Vector:
                {
                    return DeserializeVector(structBase, offset, field);
                }
                case BaseType.Struct:
                {
                    return DeserializeStruct(structBase, offset, typeModel);
                }
                case BaseType.String:
                {
                    return DeserializeString(structBase, offset);
                }
                default:
                {
                    throw new ArgumentException("Field is not a reference type");
                }
            }
        }
        

        private object DeserializePropertyValue(int structBase, FieldTypeDefinition field)
        {
            var typeModel = field.TypeModel;
            var offset = GetFieldOffset(structBase, field.Offset);

            if (offset == 0)
            {
                // Nothing in buffer, use default value
                return field.DefaultValueProvider.GetDefaultValue(field.TypeModel.Type);
            }

            switch (typeModel.BaseType)
            {
                case BaseType.Char:
                    {
                        return _buffer.GetSbyte(offset + structBase);
                    }
                case BaseType.UChar:
                    {
                        return _buffer.Get(offset + structBase);
                    }
                case BaseType.Short:
                    {
                        return _buffer.GetShort(offset + structBase);
                    }
                case BaseType.UShort:
                    {
                        return _buffer.GetUshort(offset + structBase);
                    }
                case BaseType.Int:
                    {
                        return _buffer.GetInt(offset + structBase);
                    }
                case BaseType.UInt:
                    {
                        return _buffer.GetUint(offset + structBase);
                    }
                case BaseType.Long:
                    {
                        return _buffer.GetLong(offset + structBase);
                    }
                case BaseType.ULong:
                    {
                        return _buffer.GetUlong(offset + structBase);
                    }
                case BaseType.Float:
                    {
                        return _buffer.GetFloat(offset + structBase);
                    }
                case BaseType.Double:
                    {
                        return _buffer.GetDouble(offset + structBase);
                    }
                case BaseType.String:
                case BaseType.Struct:
                case BaseType.Vector:
                {
                    return DeserializeReferenceType(structBase, offset, field);
                }
                default:
                    {
                        throw new InvalidOperationException();
                    }
            }
        }

        private object DeserializeScalarValue(BaseType baseType, int offset)
        {
            switch (baseType)
            {
                case BaseType.Char:
                    {
                        return _buffer.GetSbyte(offset);
                    }
                case BaseType.UChar:
                    {
                        return _buffer.Get(offset);
                    }
                case BaseType.Short:
                    {
                        return _buffer.GetShort(offset);
                    }
                case BaseType.UShort:
                    {
                        return _buffer.GetUshort(offset);
                    }
                case BaseType.Int:
                    {
                        return _buffer.GetInt(offset);
                    }
                case BaseType.UInt:
                    {
                        return _buffer.GetUint(offset);
                    }
                case BaseType.Long:
                    {
                        return _buffer.GetLong(offset);
                    }
                case BaseType.ULong:
                    {
                        return _buffer.GetUlong(offset);
                    }
                case BaseType.Float:
                    {
                        return _buffer.GetFloat(offset);
                    }
                case BaseType.Double:
                    {
                        return _buffer.GetDouble(offset);
                    }
                default:
                    {
                        throw new InvalidOperationException();
                    }
            }
        }

        private object DeserializeInlineValue(int structBase, FieldTypeDefinition field)
        {
            var typeModel = field.TypeModel;
            var offset = structBase + field.Offset;

            if (typeModel.BaseType.IsScalar())
            {
                return DeserializeScalarValue(typeModel.BaseType, offset);
            }
            if (typeModel.BaseType == BaseType.Struct)
            {
                return DeserializeStruct(offset, 0, typeModel);
            }
            throw new NotImplementedException();
        }

        private void DeserializeStructField(object obj, StructTypeDefinition structDef, FieldTypeDefinition field, int structBase)
        {
            object value = null;

            if (structDef.IsFixed)
            {
                value = DeserializeInlineValue(structBase, field);
            }
            else
            {
                value = DeserializePropertyValue(structBase, field);
            }

            field.ValueProvider.SetValue(obj, value);
        }

        private int GetFieldOffset(int structBase, int vtableOffset)
        {
            var vtable = structBase - _buffer.GetInt(structBase);
            return vtableOffset < _buffer.GetShort(vtable) ? (int)_buffer.GetShort(vtable + vtableOffset) : 0;
        }

        public object Deserialize()
        {
            // TODO: Support more root types than struct?
            if (_rootTypeModel.BaseType != BaseType.Struct)
            {
                throw new ArgumentException();
            }

            return DeserializeStruct(_buffer.Position, _buffer.Position, _rootTypeModel);
        }

    }
}