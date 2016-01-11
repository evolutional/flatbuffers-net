using System;
using System.IO;

namespace FlatBuffers
{
    /// <summary>
    /// Utility class to allow simple serialization of .Net objects to FlatBuffers format
    /// </summary>
    public static class FlatBuffersConvert
    {
        /// <summary>
        /// Serializes an object into a FlatBuffer formatted byte array
        /// </summary>
        /// <param name="obj">Object to serialize</param>
        /// <returns>A sized byte buffer containing the flatbuffer data</returns>
        public static byte[] SerializeObject(object obj)
        {
            var serializer = new FlatBuffersSerializer();
            return serializer.Serialize(obj);
        }
        
        /// <summary>
        /// Serializes an object into a FlatBuffer formatted Stream
        /// </summary>
        /// <param name="obj">Object to serialize</param>
        /// <param name="writeStream">Stream to write the serialized object to</param>
        public static void SerializeObject(object obj, Stream writeStream)
        {
            var buffer = SerializeObject(obj);
            writeStream.Write(buffer, 0, buffer.Length);
        }

        /// <summary>
        /// Deserializes an object from a FlatBuffer formatted byte array
        /// </summary>
        /// <param name="buffer">Buffer containing FlatBuffer data</param>
        /// <param name="offset">Offset into the buffer to start reading from</param>
        /// <param name="count">The number of available bytes to read</param>
        /// <typeparam name="T">The type of the object to deserialize</typeparam>
        /// <returns>Deserialized object of type T</returns>
        public static T DeserializeObject<T>(byte[] buffer, int offset, int count)
        {
            return (T) DeserializeObject(typeof (T), buffer, offset, count);
        }

        /// <summary>
        /// Deserializes an object from a FlatBuffer formatted byte array
        /// </summary>
        /// <param name="type">The type of the object to deserialize</param>
        /// <param name="buffer">Buffer containing FlatBuffer data</param>
        /// <param name="offset">Offset into the buffer to start reading from</param>
        /// <param name="count">The number of available bytes to read</param>
        /// <returns>Deserialized object of type T</returns>
        public static object DeserializeObject(Type type, byte[] buffer, int offset, int count)
        {
            var serializer = new FlatBuffersSerializer();
            return serializer.Deserialize(type, buffer, offset, count);
        }

        /// <summary>
        /// Deserializes an object from a FlatBuffer formatted byte array
        /// </summary>
        /// <param name="buffer">Buffer containing FlatBuffer data</param>
        /// <typeparam name="T">The type of the object to deserialize</typeparam>
        /// <returns>Deserialized object of type T</returns>
        public static T DeserializeObject<T>(byte[] buffer)
        {
            return (T)DeserializeObject(typeof(T), buffer);
        }

        /// <summary>
        /// Deserializes an object from a FlatBuffer formatted byte array
        /// </summary>
        /// <param name="type">The type of the object to deserialize</param>
        /// <param name="buffer">Buffer containing FlatBuffer data</param>
        /// <returns>Deserialized object of type T</returns>
        public static object DeserializeObject(Type type, byte[] buffer)
        {
            var serializer = new FlatBuffersSerializer();
            return serializer.Deserialize(type, buffer, 0, buffer.Length);
        }
    }
}
