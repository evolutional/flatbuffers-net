using System.IO;
using System.Linq;
using FlatBuffers;
using NUnit.Framework;

namespace MyGame.Example.Tests
{
    [TestFixture]
    public class ExampleSerializationTests
    {
        [Test]
        public void Vec3_CanonicalBuffer_Matches_SerializedResult()
        {
            // Declare a Vec3 in .NET
            var vec3 = new Vec3()
            {
                X = 1.0f,
                Y = 2.0f,
                Z = 3.0f,
                Test1 = 4.0,
                Test2 = Color.Green,
                Test3 = new Test() {A = 5, B = 6}
            };

            // Create a buffer using the generated API
            var fbb = new FlatBufferBuilder(32);
            var vec3Offset = MyGame.Example.Vec3.CreateVec3(fbb, 1.0f, 2.0f, 3.0f, 4.0, MyGame.Example.Color.Green, 5, 6);

            // Serialize Vec3 using FlatBuffers-net
            var buffer = FlatBuffersConvert.SerializeObject(vec3);

            // Verify buffers are identical
            Assert.IsTrue(buffer.SequenceEqual(fbb.DataBuffer.Data));
        }

        [Ignore("Highlighted a bug deserializing arrays of unions")]
        [Test]
        public void Deserialize_ExampleMonster_Buffer()
        {
            var buffer = GetBuffer();
            var root = FlatBuffersConvert.DeserializeObject<Monster>(buffer);
        }

        private byte[] GetBuffer()
        {
            return File.ReadAllBytes(@"monsterdata_test.mon");
        }
    }
}
