using System.Collections.Generic;
using System.Globalization;
using System.Linq;

namespace FlatBuffers
{
    public sealed class FieldTypeDefinition : TypeDefinition
    {
        private int _index;
        private bool _required;

        private readonly List<FieldTypeMetadata> _metadata = new List<FieldTypeMetadata>();

        public FieldTypeDefinition(IValueProvider valueProvider, IDefaultValueProvider defaultValueProvider)
        {
            ValueProvider = valueProvider;
            DefaultValueProvider = defaultValueProvider;
        }

        public void SetMetaData(string key)
        {
            SetMetaData(key, null);
        }

        public void RemoveMetaData(string key)
        {
            _metadata.RemoveAll(i => i.Key == key);
        }

        public void SetMetaData(string key, string value)
        {
            _metadata.RemoveAll(i => i.Key == key);
            _metadata.Add(new FieldTypeMetadata() { Key = key, Value = value});
        }

        public IEnumerable<FieldTypeMetadata> MetaData { get { return _metadata; }} 

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
                SetMetaData(FlatBuffersKnownMetaData.Index, _index.ToString(CultureInfo.InvariantCulture));
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
            get { return _metadata.Count > 0; }
        }

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
                    SetMetaData(FlatBuffersKnownMetaData.Required);
                }
                else
                {
                    RemoveMetaData(FlatBuffersKnownMetaData.Required);
                }
            }
            
        }
    }
}