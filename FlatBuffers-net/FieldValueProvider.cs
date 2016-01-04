using System;
using System.Reflection;

namespace FlatBuffers
{
    internal class FieldValueProvider : IValueProvider
    {
        private readonly FieldInfo _field;

        public FieldValueProvider(FieldInfo field)
        {
            _field = field;
        }

        public object GetValue(object obj)
        {
            return _field.GetValue(obj);
        }

        public void SetValue(object obj, object value)
        {
            _field.SetValue(obj, value);
        }

        public Type ValueType { get { return _field.FieldType; } }
    }
}