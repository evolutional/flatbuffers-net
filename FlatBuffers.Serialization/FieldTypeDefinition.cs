using System;
using System.Reflection;

namespace FlatBuffers.Serialization
{
    public class FieldTypeDefinition : TypeDefinition
    {
        public FieldTypeDefinition(IValueProvider valueProvider)
        {
            ValueProvider = valueProvider;
        }
        
        public int Index { get; set; }

        // value
        public bool Deprecated { get; set; }
        public bool Required { get; set; }
        // key
        public int Padding { get; set; }

        public TypeModel TypeModel { get; set; }

        public int Offset { get; set; }

        public IValueProvider ValueProvider { get; private set; }
    }
}