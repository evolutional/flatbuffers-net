namespace FlatBuffers
{
    public class FieldTypeDefinition : TypeDefinition
    {
        private int _index;

        public FieldTypeDefinition(IValueProvider valueProvider)
        {
            ValueProvider = valueProvider;
        }

        public int Index
        {
            get
            {
                if (!IsIndexSetExplicitly)
                {
                    return OriginalIndex;
                }
                return _index;
            }
            set
            {
                _index = value;
                IsIndexSetExplicitly = true;
            }
        }

        public bool IsIndexSetExplicitly { get; private set; }

        public int OriginalIndex { get; set; }

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