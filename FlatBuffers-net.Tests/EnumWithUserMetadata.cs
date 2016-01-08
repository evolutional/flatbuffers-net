using FlatBuffers.Attributes;

namespace FlatBuffers.Tests
{
    [FlatBuffersMetadata("magicEnum")]
    public enum EnumWithUserMetadata
    {
        Cat, Dog, Fish
    }
}