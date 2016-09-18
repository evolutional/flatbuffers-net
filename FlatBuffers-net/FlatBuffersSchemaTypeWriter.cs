using System;
using System.IO;
using System.Linq;
using System.Text;
using FlatBuffers.Utilities;

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

        /// <summary>
        /// Initializes an instance of the FlatBuffersSchemaTypeWriter class
        /// </summary>
        /// <param name="writer">TextWriter write the schema to</param>
        public FlatBuffersSchemaTypeWriter(TextWriter writer)
            : this(TypeModelRegistry.Default, writer, FlatBuffersSchemaTypeWriterOptions.Default)
        {
        }

        /// <summary>
        /// Initializes an instance of the FlatBuffersSchemaTypeWriter class
        /// </summary>
        /// <param name="writer">TextWriter write the schema to</param>
        /// <param name="options">Options to use when writing the schema</param>
        public FlatBuffersSchemaTypeWriter(TextWriter writer, FlatBuffersSchemaTypeWriterOptions options)
            : this(TypeModelRegistry.Default, writer, options)
        {
        }

        /// <summary>
        /// Initializes an instance of the FlatBuffersSchemaTypeWriter class
        /// </summary>
        /// <param name="typeModelRegistry">TypeRegistry to use as the Type source</param>
        /// <param name="writer">TextWriter write the schema to</param>
        /// /// <param name="options">Options to use when writing the schema</param>
        public FlatBuffersSchemaTypeWriter(TypeModelRegistry typeModelRegistry, TextWriter writer, FlatBuffersSchemaTypeWriterOptions options)
        {
            _typeModelRegistry = typeModelRegistry;
            _writer = writer;
            _options = options;

            var newline = options.LineTerminator == FlatBuffersSchemaWriterLineTerminatorType.Lf ? "\n" : "\r\n";
            _writer.NewLine = newline;

            _indent = new string(_options.IndentType == FlatBuffersSchemaWriterIndentType.Spaces ? ' ' : '\t', _options.IndentCount);
            _bracing = _options.BracingStyle == FlatBuffersSchemaWriterBracingStyle.Egyptian ? " {" : string.Format("{0}{{", newline);
        }

        /// <summary>
        /// Writes a schema fragment of a type
        /// </summary>
        /// <typeparam name="T">Type to emit</typeparam>
        public void Write<T>()
        {
            var type = typeof(T);
            Write(type);
        }
        /// <summary>
        /// Writes a schema fragment of a type
        /// </summary>
        /// <param name="type">Type to emit</param>
        public void Write(Type type)
        {
            var typeModel = _typeModelRegistry.GetTypeModel(type);
            if (typeModel == null)
            {
                throw new ArgumentException("Could not determine TypeModel for TypeModel");
            }
            Write(typeModel);
        }

        /// <summary>
        /// Writes a schema fragment of a type
        /// </summary>
        /// <param name="typeModel">Type to emit</param>
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

        /// <summary>
        /// Writes the schema for a speficic TypeModel that contains a table
        /// </summary>
        /// <param name="typeModel">TypeModel representing a table</param>
        public void WriteTable(TypeModel typeModel)
        {
            if (!typeModel.IsTable)
            {
                throw new ArgumentException("Type is not a table");
            }
            WriteStructInternal(typeModel);
        }

        /// <summary>
        /// Writes the schema for a speficic TypeModel that contains a struct
        /// </summary>
        /// <param name="typeModel">TypeModel representing a struct</param>
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

        /// <summary>
        /// Writes the schema for a speficic TypeModel that contains an enum
        /// </summary>
        /// <param name="typeModel">TypeModel representing an enum</param>
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

            var emitValue = typeModel.EnumDef.BitFlags;

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

        /// <summary>
        /// Writes the schema for a speficic TypeModel that contains a union
        /// </summary>
        /// <param name="typeModel">TypeModel representing a union</param>
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

        private void BeginStruct(TypeModel typeModel)
        {
            var structOrTable = typeModel.StructDef.IsFixed ? "struct" : "table";
            var sb = new StringBuilder();
            BuildMetadata(sb, typeModel.StructDef);
            var meta = sb.ToString();
            WriteAllComments(typeModel.StructDef, false);
            _writer.WriteLine("{0} {1}{2}{3}", structOrTable, typeModel.Name, meta, _bracing);
        }

        private void WriteField(FieldTypeDefinition field)
        {
            var fieldTypeName = GetFlatBufferTypeName(field.TypeModel);

            if (field.HasNestedFlatBufferType)
            {
                fieldTypeName = "[ubyte]";
            }

            var meta = BuildMetadata(field);
            WriteAllComments(field, false);

            var fieldName = ApplyNameStyle(_options.FieldNamingStyle, field.Name);

            _writer.WriteLine("{0}{1}:{2}{3};", _indent, fieldName, fieldTypeName, meta);
        }

        private string ApplyNameStyle(FlatBuffersSchemaWriterNamingStyle style, string name)
        {
            switch (style)
            {
                case FlatBuffersSchemaWriterNamingStyle.CamelCase:
                {
                    return name.ToCamelCase();
                }
                case FlatBuffersSchemaWriterNamingStyle.LowerCase:
                {
                    return name.ToLower();
                }
                case FlatBuffersSchemaWriterNamingStyle.Original:
                default:
                {
                    return name;
                }
            }
        }

        private void EndObject()
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
                var elementTypeName = typeModel.ElementType.FlatBufferTypeName() ?? GetFlatBufferTypeName(typeModel.GetElementTypeModel());

                if (elementTypeName != null)
                {
                    return string.Format("[{0}]", elementTypeName);
                }
            }
            throw new NotImplementedException();
        }

        private void BeginEnum(TypeModel typeModel)
        {
            if (!typeModel.IsEnum)
            {
                throw new ArgumentException();
            }

            var sb = new StringBuilder();
            BuildMetadata(sb, typeModel.EnumDef);
            var meta = sb.ToString();
            WriteAllComments(typeModel.EnumDef, false);
            _writer.WriteLine("enum {0} : {1}{2}{3}", typeModel.Name, typeModel.BaseType.FlatBufferTypeName(), meta, _bracing);
        }

        private void EndEnum()
        {
            // todo: assert nesting
            _writer.WriteLine("}");
        }

        private void BeginUnion(TypeModel typeModel)
        {
            if (!typeModel.IsUnion)
            {
                throw new ArgumentException();
            }
            var sb = new StringBuilder();
            BuildMetadata(sb, typeModel.UnionDef);
            var meta = sb.ToString();
            WriteAllComments(typeModel.UnionDef, false);
            _writer.WriteLine("union {0}{1}{2}", typeModel.Name, meta, _bracing);
        }

        private void EndUnion()
        {
            // todo: assert nesting
            _writer.WriteLine("}");
        }

        /// <summary>
        /// Writes the schema for a speficic metadata attribute
        /// </summary>
        public void WriteAttribute(string name)
        {
            // TODO: assert nesting
            _writer.WriteLine("attribute \"{0}\";", name);
        }

        /// <summary>
        /// Writes a comment into the schema
        /// </summary>
        /// <param name="comment">Comment string to write</param>
        public void WriteComment(string comment)
        {
            _writer.WriteLine("/// {0}", comment);
        }

        /// <summary>
        /// Writes a comment into the schema using indentation
        /// </summary>
        /// <param name="comment">Comment string to write</param>
        public void WriteIndentedComment(string comment)
        {
            _writer.WriteLine("{0}/// {1}", _indent, comment);
        }

        private void WriteAllComments(TypeDefinition def, bool indent)
        {
            foreach (var comment in def.Comments)
            {
                if (indent)
                {
                    WriteIndentedComment(comment);
                }
                else
                {
                    WriteComment(comment);
                }
            }
        }

        /// <summary>
        /// Writes a newline in the schema
        /// </summary>
        public void WriteLine()
        {
            _writer.WriteLine();
        }

        /// <summary>
        /// Writes the name of the schema's root_type
        /// </summary>
        /// <param name="rootTypeName">Type name to write</param>
        public void WriteRootType(string rootTypeName)
        {
            // TODO: assert if root_type written
            _writer.WriteLine("root_type {0};", rootTypeName);
        }

        /// <summary>
        /// Writes the file_identifier from the schema's root type
        /// </summary>
        /// <param name="ident">Identifier to write</param>
        public void WriteFileIdentifier(string ident)
        {
            // TODO: assert if file_identifier written
            _writer.WriteLine("file_identifier \"{0}\";", ident);
        }
    }
}
