using System;
using System.Linq;
using System.Reflection;

namespace FlatBuffers
{
    internal class UnionTypeValueProvider : IValueProvider
    {
        private readonly IValueProvider _fieldValueProvider;
        private readonly TypeModel _valueTypeModel;
        private int _value;

        public UnionTypeValueProvider(IValueProvider fieldValueProvider, TypeModel valueTypeModel)
        {
            _fieldValueProvider = fieldValueProvider;
            _valueTypeModel = valueTypeModel;
            ValueType = typeof (byte);
        }

        public object GetValue(object obj)
        {
            var fieldValue = _fieldValueProvider.GetValue(obj);
            var unionType = _valueTypeModel.UnionDef.Fields.FirstOrDefault(i => i.MemberType != null && i.MemberType.Type == fieldValue.GetType());
            return unionType == null ? (byte)0 : (byte) unionType.Index;
        }

        public void SetValue(object obj, object value)
        {
            
        }

        public Type ValueType { get; private set; }
    }

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