using System;
using System.IO;
using System.Text;

namespace FlatBuffers
{
    /// <summary>
    /// Writes TypeModel information in fbs schema format. 
    /// Designed to write individual types, with no regard for dependency information.
    /// </summary>
    public class FlatBuffersSchemaTypeWriter
    {
        private readonly TypeModelRegistry _typeModelRegistry;
        private readonly TextWriter _writer;

        public FlatBuffersSchemaTypeWriter(TextWriter writer)
            : this(TypeModelRegistry.Default, writer)
        {
        }

        public FlatBuffersSchemaTypeWriter(TypeModelRegistry typeModelRegistry, TextWriter writer)
        {
            _typeModelRegistry = typeModelRegistry;
            _writer = writer;
        }

        public void Write<T>()
        {
            var type = typeof(T);
            Write(type);
        }

        public void Write(Type type)
        {
            var typeModel = _typeModelRegistry.GetTypeModel(type);
            if (typeModel == null)
            {
                throw new ArgumentException("Could not determine TypeModel for TypeModel");
            }
            Write(typeModel);
        }

        public void Write(TypeModel typeModel)
        {
            if (typeModel.IsEnum)
            {
                WriteEnum(typeModel);
                return;
            }

            if (typeModel.IsStruct || typeModel.IsTable)
            {
                WriteStructInternal(typeModel);
                return;
            }

            throw new NotSupportedException();
        }

        private bool IsNumberEqual(object o, int v)
        {
            var type = o.GetType();
            if (type == typeof (byte))
            {
                return (byte) o == v;
            }
            if (type == typeof(sbyte))
            {
                return (sbyte)o == v;
            }
            if (type == typeof(short))
            {
                return (short)o == v;
            }
            if (type == typeof(ushort))
            {
                return (ushort)o == v;
            }
            if (type == typeof(int))
            {
                return (int)o == v;
            }
            if (type == typeof(uint))
            {
                return (uint)o == v;
            }
            throw new ArgumentException("Unsupported type", "o");
        }

        public void WriteEnum(TypeModel typeModel)
        {
            // Note: .NET will reflect the enum based on the binary order of the VALUE.
            // We cannot reflect it in declared order
            // See: https://msdn.microsoft.com/en-us/library/system.enum.getvalues.aspx

            var values = Enum.GetValues(typeModel.Type);
            var names = Enum.GetNames(typeModel.Type);

            var emitValue = false;

            BeginEnum(typeModel);
            for (var i = 0; i < names.Length; ++i)
            {
                var enumValue = Convert.ChangeType(values.GetValue(i), Enum.GetUnderlyingType(typeModel.Type));
                if (!emitValue && !IsNumberEqual(enumValue, i))
                {
                    emitValue = true;
                }

                if (emitValue)
                {
                    _writer.WriteLine("    {0} = {1}{2}", names[i], enumValue, i == names.Length - 1 ? "" : ","); 
                }
                else
                {
                    _writer.WriteLine("    {0}{1}", names[i], i == names.Length - 1 ? "" : ","); 
                }
            }
            EndEnum();
        }

        public void WriteTable(TypeModel typeModel)
        {
            if (!typeModel.IsTable)
            {
                throw new ArgumentException();
            }
            WriteStructInternal(typeModel);
        }

        public void WriteStruct(TypeModel typeModel)
        {
            if (!typeModel.IsStruct)
            {
                throw new ArgumentException();
            }
            WriteStructInternal(typeModel);
        }

        private void WriteStructInternal(TypeModel typeModel)
        {
            var structDef = typeModel.StructDef;

            if (structDef == null)
            {
                throw new ArgumentException("Not a struct/table type", "typeModel");
            }

            BeginStruct(typeModel);
            foreach (var field in structDef.Fields)
            {
                WriteField(field);
            }
            EndObject();
        }

        protected void BeginStruct(TypeModel typeModel)
        {
            var structOrTable = typeModel.StructDef.IsFixed ? "struct" : "table";
            _writer.WriteLine("{0} {1} {{", structOrTable, typeModel.Name);
        }

        private string GetFlatBufferTypeName(TypeModel typeModel)
        {
            if (typeModel.IsEnum || typeModel.IsStruct || typeModel.IsTable)
            {
                return typeModel.Name;
            }

            var baseType = typeModel.BaseType;
            var typeName = baseType.FlatBufferTypeName();

            if (typeName != null)
            {
                return typeName;
            }

            if (baseType == BaseType.Vector)
            {
                var elementTypeName = typeModel.ElementType.FlatBufferTypeName();
                if (elementTypeName != null)
                {
                    return string.Format("[{0}]", elementTypeName);
                }
            }
            throw new NotImplementedException();
        }

        protected void WriteField(FieldTypeDefinition field)
        {
            var fieldTypeName = GetFlatBufferTypeName(field.TypeModel);

            var meta = BuildMetaData(field);

            _writer.WriteLine("    {0}:{1}{2};", field.Name, fieldTypeName, meta);
        }

        private string BuildMetaData(FieldTypeDefinition field)
        {
            var sb = new StringBuilder();

            if (field.DefaultValueProvider.IsDefaultValueSetExplicity)
            {
                sb.AppendFormat(" = {0}", field.DefaultValueProvider.GetDefaultValue(field.TypeModel.Type));
            }

            if (field.HasMetaData)
            {
                sb.Append(" (");

                if (field.IsIndexSetExplicitly)
                {
                    sb.AppendFormat("id: {0}", field.Index);
                }
                
                sb.Append(")");
            }
            return sb.ToString();
        }

        protected void EndObject()
        {
            // todo: assert nesting
            _writer.WriteLine("}");
        }

        protected void BeginEnum(TypeModel typeModel)
        {
            if (!typeModel.IsEnum)
            {
                throw new ArgumentException();
            }
            _writer.WriteLine("enum {0} : {1} {{", typeModel.Name, typeModel.BaseType.FlatBufferTypeName());
        }

        protected void EndEnum()
        {
            // todo: assert nesting
            _writer.WriteLine("}");
        }

    }
}
