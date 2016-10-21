using System;
using System.Collections;
using System.Collections.Generic;
using System.Linq;
using FlatBuffers.Utilities;

namespace FlatBuffers
{
    internal class SerializationContext
    {
        private readonly TypeModelRegistry _typeModelRegistry;
        private readonly object _rootObject;
        private readonly TypeModel _rootTypeModel;
        private readonly FlatBufferBuilder _builder;
        private readonly Dictionary<object, int> _objectOffsets = new Dictionary<object, int>();
        private readonly Dictionary<object, int> _nestedOffsets = new Dictionary<object, int>();

        public SerializationContext(TypeModelRegistry typeModelRegistry, object rootObject, FlatBufferBuilder builder)
        {
            _typeModelRegistry = typeModelRegistry;
            _rootObject = rootObject;
            _rootTypeModel = _typeModelRegistry.GetTypeModel(rootObject.GetType());
            _builder = builder;
        }

        /// <summary>
        /// Serializes a value as inline
        /// </summary>
        private int SerializeInlineValue(object obj, TypeModel typeModel)
        {
            switch (typeModel.BaseType)
            {
                case BaseType.Bool:
                {
                    _builder.AddBool((bool)obj);
                    break;
                }
                case BaseType.Char:
                {
                    _builder.AddSbyte((sbyte)obj);
                    break;
                }
                case BaseType.UChar:
                {
                    _builder.AddByte((byte)obj);
                    break;
                }
                case BaseType.Short:
                {
                    _builder.AddShort((short)obj);
                    break;
                }
                case BaseType.UShort:
                {
                    _builder.AddUshort((ushort)obj);
                    break;
                }
                case BaseType.Int:
                {
                    _builder.AddInt((int)obj);
                    break;
                }
                case BaseType.UInt:
                {
                    _builder.AddUint((uint)obj);
                    break;
                }
                case BaseType.Long:
                {
                    _builder.AddLong((long)obj);
                    break;
                }
                case BaseType.ULong:
                {
                    _builder.AddUlong((ulong)obj);
                    break;
                }
                case BaseType.Float:
                {
                    _builder.AddFloat((float)obj);
                    break;
                }
                case BaseType.Double:
                {
                    _builder.AddDouble((double)obj);
                    break;
                }
                case BaseType.Struct:
                {
                    return SerializeStruct(obj, typeModel);
                }
                default:
                {
                    throw new InvalidOperationException();
                }
            }
            return _builder.Offset;
        }

        private int SerializePropertyValue(object obj, FieldTypeDefinition field)
        {
            var typeModel = field.TypeModel;

            if (typeModel.IsReferenceType && obj == null)
            {
                if (field.Required)
                {
                    throw new FlatBuffersSerializationException("Required field '{0}' is not set", field.Name);
                }
            }

            if (field.DefaultValueProvider.IsDefaultValue(obj))
            {
                return _builder.Offset;
            }

            switch (typeModel.BaseType)
            {
                case BaseType.Bool:
                {
                    _builder.AddBool(field.Index, (bool)obj, false);
                    break;
                }
                case BaseType.Char:
                {
                    _builder.AddSbyte(field.Index, (sbyte)obj, 0);
                    break;
                }
                case BaseType.UType:
                case BaseType.UChar:
                {
                    _builder.AddByte(field.Index, (byte)obj, 0);
                    break;
                }
                case BaseType.Short:
                {
                    _builder.AddShort(field.Index, (short)obj, 0);
                    break;
                }
                case BaseType.UShort:
                {
                    _builder.AddUshort(field.Index, (ushort)obj, 0);
                    break;
                }
                case BaseType.Int:
                {
                    _builder.AddInt(field.Index, (int)obj, 0);
                    break;
                }
                case BaseType.UInt:
                {
                    _builder.AddUint(field.Index, (uint)obj, 0);
                    break;
                }
                case BaseType.Long:
                {
                    _builder.AddLong(field.Index, (long)obj, 0);
                    break;
                }
                case BaseType.ULong:
                {
                    _builder.AddUlong(field.Index, (ulong)obj, 0);
                    break;
                }
                case BaseType.Float:
                {
                    _builder.AddFloat(field.Index, (float)obj, 0);
                    break;
                }
                case BaseType.Double:
                {
                    _builder.AddDouble(field.Index, (double)obj, 0);
                    break;
                }
                case BaseType.Struct:
                {
                    if (typeModel.IsStruct)
                    {
                        // Structs are serialized inline
                        var structOffset = SerializeStruct(obj, typeModel);
                        _builder.AddStruct(field.Index, structOffset, 0);
                    }
                    else
                    {
                        // Is a table, so grab the offset
                        AddReferenceFieldOffset(obj, field);
                    }
                    
                    break;
                }
                case BaseType.String:
                case BaseType.Vector:
                case BaseType.Union:
                {
                    AddReferenceFieldOffset(obj, field);
                    break;
                }
                default:
                {
                    throw new InvalidOperationException();
                }
            }
            return _builder.Offset;
        }

        private void AddReferenceFieldOffset(object obj, FieldTypeDefinition field)
        {
            var fieldBufferOffset = 0;

            var dict = field.HasNestedFlatBufferType ? _nestedOffsets : _objectOffsets;

            if (!dict.TryGetValue(obj, out fieldBufferOffset))
            {
                throw new FlatBuffersSerializationException("Offset for object not found in map");
            }

            _builder.AddOffset(field.Index, fieldBufferOffset, 0);
        }

        private void SerializeFieldValue(object obj, StructTypeDefinition structDef, FieldTypeDefinition field)
        {
            if (structDef.IsFixed)
            {
                SerializeInlineValue(obj, field.TypeModel);
            }
            else
            {
                SerializePropertyValue(obj, field);
            }
        }

        private void SerializeStructField(object obj, StructTypeDefinition structDef, FieldTypeDefinition field)
        {
            if (field.Deprecated)
            {
                return;
            }

            if (field.Padding > 0)
            {
                _builder.Pad(field.Padding);
            }
            var val = field.ValueProvider.GetValue(obj);
            SerializeFieldValue(val, structDef, field);
        }

        private int SerializeUnion(object obj, TypeModel typeModel)
        {
            var unionTypeModel = GetUnionFieldTypeModel(obj, typeModel);
            return SerializeStruct(obj, unionTypeModel);
        }

        private int SerializeStruct(object obj, TypeModel typeModel)
        {
            var structDef = typeModel.StructDef;

            if (structDef.IsFixed)
            {
                _builder.Prep(structDef.MinAlign, structDef.ByteSize);
                foreach (var field in structDef.Fields.Reverse())
                {
                    SerializeStructField(obj, structDef, field);
                }
            }
            else
            {
                _builder.StartObject(structDef.FieldCount);

                var enumerable = structDef.UseOriginalOrdering
                    ? structDef.Fields
                    : structDef.Fields.OrderByDescending(i => i.TypeModel.InlineSize);

                foreach (var field in enumerable)
                {
                    SerializeStructField(obj, structDef, field);
                }
                return _builder.EndObject();
            }
            return _builder.Offset;
        }

        private int SerializeVector(ICollection collection)
        {
            var elementType = TypeHelpers.GetEnumerableElementType(collection.GetType());
            var elementTypeModel = _typeModelRegistry.GetTypeModel(elementType);
            _builder.StartVector(elementTypeModel.InlineSize, collection.Count, elementTypeModel.InlineAlignment);
    
            foreach(var element in collection.Cast<object>().Reverse())
            {
                if (elementTypeModel.IsReferenceType)
                {
                    _builder.AddOffset(_objectOffsets[element]);
                }
                else
                {
                    SerializeInlineValue(element, elementTypeModel);
                }
            }

            return _builder.EndVector().Value;
        }

        private int SerializeVector(object obj, TypeModel typeModel)
        {
            var collection = obj as ICollection;
            if (collection != null)
            {
                return SerializeVector(collection);
            }
            throw new NotSupportedException("Vector type not supported");
        }

        private int SerializeReferenceType(object obj, TypeModel typeModel)
        {
            if (typeModel.IsString)
            {
                return _builder.CreateString((string)obj).Value;
            }

            if (typeModel.IsVector)
            {
                return SerializeVector(obj, typeModel);
            }

            if (typeModel.IsTable)
            {
                return SerializeStruct(obj, typeModel);
            }

            if (typeModel.IsUnion)
            {
                return SerializeUnion(obj, typeModel);
            }

            throw new NotSupportedException();
        }

        private TypeModel GetUnionFieldTypeModel(object obj, TypeModel typeModel)
        {
            var unionDef = typeModel.UnionDef;
            var unionType = unionDef.Fields.FirstOrDefault(i => i.MemberType != null && i.MemberType.Type == obj.GetType());
            return unionType.MemberType;
        }
        
        private void SerializeReferenceTypeEnumerable(IEnumerable collection)
        {
            foreach (var element in collection)
            {
                var elementTypeModel = _typeModelRegistry.GetTypeModel(element.GetType());

                var fieldBufferOffset = 0;

                if (!_objectOffsets.TryGetValue(element, out fieldBufferOffset))
                {
                    fieldBufferOffset = SerializeReferenceType(element, elementTypeModel);
                    _objectOffsets.Add(element, fieldBufferOffset);
                }
            }
        }

        private void SerializeReferenceTypeFields(object obj, TypeModel typeModel)
        {
            var structDef = typeModel.StructDef;

            foreach (var field in structDef.Fields.Where(i => i.TypeModel.IsReferenceType))
            {
                // get object field
                var val = field.ValueProvider.GetValue(obj);

                if (val == null)
                {
                    continue;
                }

                if (field.TypeModel.IsTable)
                {
                    SerializeReferenceTypeFields(val, field.TypeModel);
                }
                else if (field.TypeModel.IsUnion)
                {
                    SerializeReferenceTypeFields(val, GetUnionFieldTypeModel(val, field.TypeModel));
                }
                else if (field.TypeModel.IsVector)
                {
                    var elementType = field.TypeModel.GetElementTypeModel();

                    if (elementType != null && elementType.IsReferenceType)
                    {
                        var collection = val as ICollection;

                        if (collection != null)
                        {
                            SerializeReferenceTypeEnumerable(collection);
                        }
                    }
                }

                var fieldBufferOffset = 0;

                if (!field.HasNestedFlatBufferType)
                {
                    if (!_objectOffsets.TryGetValue(val, out fieldBufferOffset))
                    {
                        fieldBufferOffset = SerializeReferenceType(val, field.TypeModel);
                        _objectOffsets.Add(val, fieldBufferOffset);
                    }
                }
                else
                {
                    if (!_nestedOffsets.TryGetValue(val, out fieldBufferOffset))
                    {
                        var start = _builder.Offset;
                        var objOffset = SerializeStruct(val, field.TypeModel);
                        _builder.Finish(objOffset);
                        var len = _builder.Offset - start;
                        _builder.AddInt(len);
                        fieldBufferOffset = _builder.Offset;
                        _nestedOffsets.Add(val, fieldBufferOffset);
                    }
                }
            }
        }

        public int Serialize()
        {
            // Todo: support more root types?
            if (!_rootTypeModel.IsObject)
            {
                throw new NotSupportedException();
            }

            var hasRefTypes = !_rootTypeModel.StructDef.IsFixed 
                && _rootTypeModel.StructDef.Fields.Any(i => i.TypeModel.IsReferenceType);

            if (hasRefTypes)
            {
                // if we have ref types, we want to serialize them first and store their offsets in an object map
                SerializeReferenceTypeFields(_rootObject, _rootTypeModel);
            }

            var rootTable = SerializeStruct(_rootObject, _rootTypeModel);

            if (_rootTypeModel.IsTable)
            {
                if (_rootTypeModel.StructDef.HasIdentifier)
                {
                    _builder.Finish(rootTable, _rootTypeModel.StructDef.Identifier);
                }
                else
                {
                    _builder.Finish(rootTable);
                }
                
            }
            return _builder.Offset;
        }
    }
}