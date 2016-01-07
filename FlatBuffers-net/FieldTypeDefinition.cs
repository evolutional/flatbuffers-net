using System.Globalization;
using System.Linq;

namespace FlatBuffers
{
    public sealed class FieldTypeDefinition : TypeDefinition
    {
        private int _index;
        private bool _required;

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
                MetaData.Add(FieldTypeMetaData.Index, _index.ToString(CultureInfo.InvariantCulture));
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

        

        /// <summary>
        /// Gets and sets whether this field is required to be set during serialization
        /// </summary>
        public bool Required
        {
            get { return _required; }
            set
            {
                _required = value;
                if (_required)
                {
                   MetaData.Add(FieldTypeMetaData.Required);
                }
                else
                {
                    MetaData.Remove(FieldTypeMetaData.Required);
                }
            }
            
        }
    }
}