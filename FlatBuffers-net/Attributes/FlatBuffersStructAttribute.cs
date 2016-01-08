using System;

namespace FlatBuffers.Attributes
{
    /// <summary>
    /// Attribute to signify that this type is to be serialized as a struct
    /// </summary>
    public class FlatBuffersStructAttribute : Attribute
    {
        private int _forceAlign;

        /// <summary>
        /// Gets and sets the alignment value that the struct will be forced to. 
        /// Must be a power of 2 between 1 and 16.
        /// </summary>
        public int ForceAlign
        {
            get
            {
                return _forceAlign;
            }
            set { IsForceAlignSet = true;
                _forceAlign = value;
            }
        }

        /// <summary>
        /// Gets if struct has a forced alignment
        /// </summary>
        public bool IsForceAlignSet { get; private set; }
    }
}