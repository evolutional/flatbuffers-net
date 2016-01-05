using System;

namespace FlatBuffers
{
    public interface IDefaultValueProvider
    {
        object GetDefaultValue(Type valueType);

        bool IsDefaultValue(object value);

        /// <summary>
        /// Gets if the default value has been set explicitly by the user
        /// </summary>
        bool IsDefaultValueSetExplicity { get; }
    }
}