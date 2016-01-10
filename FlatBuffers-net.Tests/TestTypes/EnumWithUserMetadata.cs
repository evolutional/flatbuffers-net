using FlatBuffers.Attributes;

namespace FlatBuffers.Tests.TestTypes
{
    [FlatBuffersMetadata("magicEnum")]
    public enum EnumWithUserMetadata
    {
        Cat, Dog, Fish
    }
}