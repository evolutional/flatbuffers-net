using System;

namespace FlatBuffers
{
    /// <summary>
    /// Serializers objects into FlatBuffers format
    /// </summary>
    public class FlatBuffersSerializer
    {
        private readonly TypeModelRegistry _typeModelRegistry;

        /// <summary>
        /// Initializes an instance of the <see cref="FlatBuffersSerializer"/> class uing the deault options
        /// </summary>
        public FlatBuffersSerializer() : this(TypeModelRegistry.Default)
        {}

        /// <summary>
        /// Initializes an instance of the <see cref="FlatBuffersSerializer"/> class
        /// </summary>
        /// <param name="typeModelRegistry"><see cref="TypeModelRegistry"/> used for type resolution</param>
        public FlatBuffersSerializer(TypeModelRegistry typeModelRegistry)
        {
            _typeModelRegistry = typeModelRegistry;
        }

        /// <summary>
        /// Deserializes an object from a byte array containing data FlatBuffers format
        /// </summary>
        /// <param name="buffer">Buffer containing FlatBuffers formatted data</param>
        /// <param name="offset">Offset in the buffer that the FlatBuffers data starts</param>
        /// <param name="count">The number of bytes allowed to be deserialized</param>
        /// <typeparam name="T">The type of the object to deserialize from the buffer</typeparam>
        /// <returns>The deserialized object</returns>
        public T Deserialize<T>(byte[] buffer, int offset, int count)
        {
            return (T)Deserialize(typeof(T), buffer, offset, count);
        }

        /// <summary>
        /// Deserializes an object from a byte array containing data FlatBuffers format
        /// </summary>
        /// <param name="buffer">Buffer containing FlatBuffers formatted data</param>
        /// <param name="offset">Offset in the buffer that the FlatBuffers data starts</param>
        /// <param name="count">The number of bytes allowed to be deserialized</param>
        /// <param name="type">The type of the object to deserialize from the buffer</param>
        /// <returns>The deserialized object</returns>
        public object Deserialize(Type type, byte[] buffer, int offset, int count)
        {
            var bufferPos = offset;
            var bb = new ByteBuffer(buffer, bufferPos);
            var context = new DeserializationContext(_typeModelRegistry, type, bb);
            return context.Deserialize();
        }

        /// <summary>
        /// Serializes an object as FlatBuffers formatted data into a byte array
        /// </summary>
        /// <param name="obj">The object to serialize into FlatBuffers format</param>
        /// <param name="buffer">The buffer to write the FlatBuffers formatted data to</param>
        /// <param name="offset">The offset in the buffer to write the data to</param>
        /// <param name="count">The number of bytes allowed to be written into the buffer</param>
        /// <returns>The size (in bytes) of the FlatBuffer data written</returns>
        public int Serialize(object obj, byte[] buffer, int offset, int count)
        {
            var builder = new FlatBufferBuilder(count);
            var context = new SerializationContext(_typeModelRegistry, obj, builder);
            context.Serialize();
            Buffer.BlockCopy(builder.DataBuffer.Data, builder.DataBuffer.Length - builder.Offset, buffer, offset, builder.Offset);
            return builder.Offset;
        }

        /// <summary>
        /// Serializes an object as FlatBuffers formatted data into a byte array
        /// </summary>
        /// <param name="obj">The object to serialize into FlatBuffers format</param>
        /// <returns>A sized buffer containing the serialized data</returns>
        public byte[] Serialize(object obj)
        {
            var builder = new FlatBufferBuilder(64);
            var context = new SerializationContext(_typeModelRegistry, obj, builder);
            context.Serialize();
            return builder.SizedByteArray();
        }
    }
}
