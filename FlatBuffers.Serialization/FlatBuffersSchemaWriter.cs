using System;
using System.Collections.Generic;
using System.IO;
using System.Linq;
using System.Text;

namespace FlatBuffers.Serialization
{
    public class FlatBuffersSchemaWriter
    {
        private readonly TypeModelRegistry _typeModelRegistry;
        private readonly TextWriter _writer;

        public FlatBuffersSchemaWriter(TextWriter writer)
            : this(TypeModelRegistry.Default, writer)
        {
        }

        public FlatBuffersSchemaWriter(TypeModelRegistry typeModelRegistry, TextWriter writer)
        {
            _typeModelRegistry = typeModelRegistry;
            _writer = writer;
        }

        public void Write(TypeModel typeModel)
        {
            switch (typeModel.BaseType)
            {
                case BaseType.Struct:
                {
                    WriteStruct(typeModel);
                    break;
                }
                default:
                {
                    throw new NotImplementedException();
                }
            }
        }

        public void WriteStruct(TypeModel typeModel)
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
            // TODO: Attributes
            _writer.WriteLine("    {0}:{1};", field.Name, fieldTypeName);
        }

        protected void EndObject()
        {
            // todo: assert nesting
            _writer.WriteLine("}");
        }

    }
}
