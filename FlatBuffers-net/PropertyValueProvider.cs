using System;
using System.Reflection;

namespace FlatBuffers
{
    internal class PropertyValueProvider : IValueProvider
    {
        private readonly PropertyInfo _prop;

        public PropertyValueProvider(PropertyInfo prop)
        {
            _prop = prop;
        }

        public object GetValue(object obj)
        {
            return _prop.GetValue(obj, BindingFlags.Public | BindingFlags.Instance, null, null, null);
        }

        public void SetValue(object obj, object value)
        {
            _prop.SetValue(obj, value, null);
        }

        public Type ValueType { get { return _prop.PropertyType; } }
    }
}