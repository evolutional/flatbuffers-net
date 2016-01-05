using System;

namespace FlatBuffers.Attributes
{
    [AttributeUsage(AttributeTargets.Field | AttributeTargets.Property)]
    public class FlatBuffersFieldAttribute : Attribute
    {
        /// <summary>
        /// Gets and sets the order of serialization and deserialization of the member
        /// </summary>
        public int Order
        {
            get { return _order; }
            set { _hasOrderSet = true;
                _order = value;
            }
        }

        /// <summary>
        /// Gets if the Order value has been set explicitly
        /// </summary>
        public bool IsOrderSetExplicitly { get { return _hasOrderSet; } }
        
        private bool _hasOrderSet;
        private int _order = -1;
    }
}
