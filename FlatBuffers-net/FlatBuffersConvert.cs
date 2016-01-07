using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;

namespace FlatBuffers
{
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

        public static T DeserializeObject<T>(byte[] buffer, int offset, int count)
        {
            return (T) DeserializeObject(typeof (T), buffer, offset, count);
        }

        public static object DeserializeObject(Type type, byte[] buffer, int offset, int count)
        {
            var serializer = new FlatBuffersSerializer();
            return serializer.Deserialize(type, buffer, offset, count);
        }

        public static T DeserializeObject<T>(byte[] buffer)
        {
            return (T)DeserializeObject(typeof(T), buffer);
        }

        public static object DeserializeObject(Type type, byte[] buffer)
        {
            var serializer = new FlatBuffersSerializer();
            return serializer.Deserialize(type, buffer, 0, buffer.Length);
        }
    }
}
