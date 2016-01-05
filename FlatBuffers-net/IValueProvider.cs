using System;

namespace FlatBuffers
{
    public interface IValueProvider
    {
        object GetValue(object obj);
        void SetValue(object obj, object value);
        Type ValueType { get; }
    }
}