using System;

namespace FlatBuffers
{
    public class FlatBuffersSchemaGenerator
    {
        private readonly TypeModelRegistry _typeModelRegistry;

        public FlatBuffersSchemaGenerator(TypeModelRegistry typeModelRegistry)
        {
            _typeModelRegistry = typeModelRegistry;
        }

        public FlatBuffersSchemaGenerator()
            : this(TypeModelRegistry.Default)
        { }

        public FlatBuffersSchema Create()
        {
            var schema = new FlatBuffersSchema(_typeModelRegistry);
            return schema;
        }

        public FlatBuffersSchema Generate<T>()
        {
            return Generate(typeof (T));
        }
        
        public FlatBuffersSchema Generate(Type type)
        {
            var schema = Create();
            schema.AddType(type);
            return schema;
        }

    }
}