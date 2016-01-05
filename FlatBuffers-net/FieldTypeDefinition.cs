namespace FlatBuffers
{
    public sealed class FieldTypeDefinition : TypeDefinition
    {
        private int _index;
        private object _defaultValue;

        public FieldTypeDefinition(IValueProvider valueProvider, IDefaultValueProvider defaultValueProvider)
        {
            ValueProvider = valueProvider;
            DefaultValueProvider = defaultValueProvider;
        }

        /// <summary>
        /// Gets and sets the index (order) of this field. If not set explicitly, the
        /// field will use its default reflected order
        /// </summary>
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

        /// <summary>
        /// Gets the original (reflected) index of this field
        /// </summary>
        public int OriginalIndex { get; set; }

        // key
        public int Padding { get; set; }

        public TypeModel TypeModel { get; set; }

        public int Offset { get; set; }

        public IValueProvider ValueProvider { get; private set; }
        public IDefaultValueProvider DefaultValueProvider { get; private set; }

        public bool HasMetaData 
        {
            get { return IsIndexSetExplicitly; }
        }
    }
}