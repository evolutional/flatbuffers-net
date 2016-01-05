using System.Reflection;

namespace FlatBuffers
{
    public class FlatBuffersStructFieldReflectionException : FlatBuffersTypeReflectionException
    {
        public MemberInfo Member { get; set; }

        public FlatBuffersStructFieldReflectionException()
        {
        }

        public FlatBuffersStructFieldReflectionException(string format, params object[] args) : 
            base(format, args)
        {
        }
    }
}