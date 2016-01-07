using System;

namespace FlatBuffers
{
    public class FlatBuffersSerializer
    {
        private readonly TypeModelRegistry _typeModelRegistry;

        public FlatBuffersSerializer() : this(TypeModelRegistry.Default)
        {}

        public FlatBuffersSerializer(TypeModelRegistry typeModelRegistry)
        {
            _typeModelRegistry = typeModelRegistry;
        }

        public T Deserialize<T>(byte[] buffer, int offset, int count)
        {
            return (T)Deserialize(typeof(T), buffer, offset, count);
        }

        public object Deserialize(Type type, byte[] buffer, int offset, int count)
        {
            var bufferPos = offset;
            var bb = new ByteBuffer(buffer, bufferPos);
            var context = new DeserializationContext(_typeModelRegistry, type, bb);
            return context.Deserialize();
        }

        public int Serialize(object obj, byte[] buffer, int offset, int count)
        {
            var builder = new FlatBufferBuilder(count);
            var context = new SerializationContext(_typeModelRegistry, obj, builder);
            context.Serialize();
            Buffer.BlockCopy(builder.DataBuffer.Data, builder.DataBuffer.Length - builder.Offset, buffer, offset, builder.Offset);
            return builder.Offset;
        }

        public byte[] Serialize(object obj)
        {
            var builder = new FlatBufferBuilder(64);
            var context = new SerializationContext(_typeModelRegistry, obj, builder);
            context.Serialize();
            return builder.SizedByteArray();
        }
    }
}
