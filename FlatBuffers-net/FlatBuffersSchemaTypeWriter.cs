using System;
using System.IO;
using System.Linq;
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
        private readonly FlatBuffersSchemaTypeWriterOptions _options;
        private readonly string _indent;
        private readonly string _bracing;
        private readonly string _newline;

        public FlatBuffersSchemaTypeWriter(TextWriter writer)
            : this(TypeModelRegistry.Default, writer, FlatBuffersSchemaTypeWriterOptions.Default)
        {
        }

        public FlatBuffersSchemaTypeWriter(TextWriter writer, FlatBuffersSchemaTypeWriterOptions options)
            : this(TypeModelRegistry.Default, writer, options)
        {
        }

        public FlatBuffersSchemaTypeWriter(TypeModelRegistry typeModelRegistry, TextWriter writer, FlatBuffersSchemaTypeWriterOptions options)
        {
            _typeModelRegistry = typeModelRegistry;
            _writer = writer;
            _options = options;

            _newline = options.LineTerminator == FlatBuffersSchemaWriterLineTerminatorType.Lf ? "\n" : "\r\n";
            _writer.NewLine = _newline;

            _indent = new string(_options.IndentType == FlatBuffersSchemaWriterIndentType.Spaces ? ' ' : '\t', _options.IndentCount);
            _bracing = _options.BracingStyle == FlatBuffersSchemaWriterBracingStyle.Egyptian ? " {" : string.Format("{0}{{", _newline);
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

            if (typeModel.IsUnion)
            {
                WriteUnion(typeModel);
                return;
            }

            if (typeModel.IsObject)
            {
                WriteStructInternal(typeModel);
                return;
            }

            throw new NotSupportedException();
        }

        public void WriteTable(TypeModel typeModel)
        {
            if (!typeModel.IsTable)
            {
                throw new ArgumentException("Type is not a table");
            }
            WriteStructInternal(typeModel);
        }

        public void WriteStruct(TypeModel typeModel)
        {
            if (!typeModel.IsStruct)
            {
                throw new ArgumentException("Type is not a struct");
            }
            WriteStructInternal(typeModel);
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
            if (!typeModel.IsEnum)
            {
                throw new ArgumentException("Type is not an Enum");
            }

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
                    _writer.WriteLine("{0}{1} = {2}{3}", _indent, names[i], enumValue, i == names.Length - 1 ? "" : ","); 
                }
                else
                {
                    _writer.WriteLine("{0}{1}{2}", _indent, names[i], i == names.Length - 1 ? "" : ","); 
                }
            }
            EndEnum();
        }

        public void WriteUnion(TypeModel typeModel)
        {
            if (!typeModel.IsUnion)
            {
                throw new ArgumentException("Type is not a Union");
            }

            var unionDef = typeModel.UnionDef;

            BeginUnion(typeModel);
            var fields = unionDef.Fields.ToArray();

            for(var i = 1; i < fields.Length; ++i)
            {
                _writer.WriteLine("{0}{1}", fields[i].Name, i == fields.Length - 1 ? "" : ",");
            }

            EndUnion();
        }

        private void WriteStructInternal(TypeModel typeModel)
        {
            var structDef = typeModel.StructDef;

            if (structDef == null)
            {
                throw new ArgumentException("Not a struct/table type", "typeModel");
            }

            BeginStruct(typeModel);
            foreach (var field in structDef.Fields.OrderBy(i=>i.OriginalIndex))
            {
                if (field.TypeModel.BaseType == BaseType.UType)
                    continue;

                WriteField(field);
            }
            EndObject();
        }

        protected void BeginStruct(TypeModel typeModel)
        {
            var structOrTable = typeModel.StructDef.IsFixed ? "struct" : "table";
            var sb = new StringBuilder();
            BuildMetadata(sb, typeModel.StructDef);
            var meta = sb.ToString();
            _writer.WriteLine("{0} {1}{2}{3}", structOrTable, typeModel.Name, meta, _bracing);
        }

        protected void WriteField(FieldTypeDefinition field)
        {
            var fieldTypeName = GetFlatBufferTypeName(field.TypeModel);

            var meta = BuildMetadata(field);

            _writer.WriteLine("{0}{1}:{2}{3};", _indent, field.Name, fieldTypeName, meta);
        }

        protected void EndObject()
        {
            // todo: assert nesting
            _writer.WriteLine("}");
        }

        private string BuildMetadata(FieldTypeDefinition field)
        {
            var sb = new StringBuilder();

            if (field.DefaultValueProvider.IsDefaultValueSetExplicity)
            {
                sb.AppendFormat(" = {0}", field.DefaultValueProvider.GetDefaultValue(field.TypeModel.Type));
            }

            BuildMetadata(sb, field);
            return sb.ToString();
        }

        private void BuildMetadata(StringBuilder sb, TypeDefinition def)
        {
            if (!def.HasMetadata)
            {
                return;
            }

            // Start meta
            sb.Append(" (");

            var requiresComma = false;
            foreach (var meta in def.Metadata.Items)
            {
                var metaValue = string.Empty;

                if (meta.HasValue)
                {
                    var valueType = meta.Value.GetType();
                    if (valueType == typeof (string))
                    {
                        metaValue = string.Format("\"{0}\"", meta.Value);
                    }
                    else if (valueType == typeof (bool))
                    {
                        metaValue = ((bool) meta.Value) ? "true" : "false";
                    }
                    else
                    {
                        metaValue = meta.Value.ToString();
                    }
                }

                sb.Append(meta.HasValue
                    ? string.Format("{0}{1}: {2}", requiresComma ? ", " : "", meta.Key, metaValue)
                    : string.Format("{0}{1}", requiresComma ? ", " : "", meta.Key));

                requiresComma = true;
            }
            sb.Append(")");
        }

        private string GetFlatBufferTypeName(TypeModel typeModel)
        {
            if (typeModel.IsEnum || typeModel.IsStruct || typeModel.IsTable || typeModel.IsUnion)
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

        protected void BeginEnum(TypeModel typeModel)
        {
            if (!typeModel.IsEnum)
            {
                throw new ArgumentException();
            }

            var sb = new StringBuilder();
            BuildMetadata(sb, typeModel.EnumDef);
            var meta = sb.ToString();
            _writer.WriteLine("enum {0} : {1}{2}{3}", typeModel.Name, typeModel.BaseType.FlatBufferTypeName(), meta, _bracing);
        }

        protected void EndEnum()
        {
            // todo: assert nesting
            _writer.WriteLine("}");
        }

        protected void BeginUnion(TypeModel typeModel)
        {
            if (!typeModel.IsUnion)
            {
                throw new ArgumentException();
            }
            var sb = new StringBuilder();
            BuildMetadata(sb, typeModel.UnionDef);
            var meta = sb.ToString();
            _writer.WriteLine("union {0}{1}{2}", typeModel.Name, meta, _bracing);
        }

        protected void EndUnion()
        {
            // todo: assert nesting
            _writer.WriteLine("}");
        }

        public void WriteAttribute(string name)
        {
            // TODO: assert nesting
            _writer.WriteLine("attribute \"{0}\";", name);
        }

        public void WriteLine()
        {
            _writer.WriteLine();
        }
    }
}
