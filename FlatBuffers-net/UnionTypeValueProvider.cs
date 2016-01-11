using System;
using System.Linq;

namespace FlatBuffers
{
    internal class UnionTypeValueProvider : IValueProvider
    {
        private readonly IValueProvider _fieldValueProvider;
        private readonly TypeModel _valueTypeModel;

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
}