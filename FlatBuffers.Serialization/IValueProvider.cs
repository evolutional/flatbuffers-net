using System;

namespace FlatBuffers.Serialization
{
    public interface IDefaultValueProvider
    {
        bool HasDefaultValue { get; }
        object GetDefaultValue();

        bool IsDefaultValue(object value);
        
    }

    public interface IValueProvider
    {
        object GetValue(object obj);
        void SetValue(object obj, object value);
        Type ValueType { get; }
    }
}