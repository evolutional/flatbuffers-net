using System;

namespace FlatBuffers.Attributes
{
    public enum FlatBuffersHash
    {
        None,
        Fnv1_32,
        Fnv1a_32,
        Fnv1_64,
        Fnv1a_64,
    }

    public static class FlatBuffersHashUtils
    {
        public static string HashName(this FlatBuffersHash hash)
        {
            switch (hash)
            {
                case FlatBuffersHash.None:
                {
                    return null;
                }
                case FlatBuffersHash.Fnv1_32:
                {
                    return "fnv1_32";
                }
                case FlatBuffersHash.Fnv1a_32:
                {
                    return "fnv1a_32";
                }
                case FlatBuffersHash.Fnv1_64:
                {
                    return "fnv1_64";
                }
                case FlatBuffersHash.Fnv1a_64:
                {
                    return "fnv1a_64";
                }
                default:
                    throw new ArgumentException();
            }
        }
    }
}