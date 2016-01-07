using System;

namespace FlatBuffers.Attributes
{
    [AttributeUsage(AttributeTargets.Class|AttributeTargets.Struct, AllowMultiple = false, Inherited = true)]
    public class FlatBuffersStructAttribute : Attribute
    {
        private int _forceAlign;

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

        public bool IsForceAlignSet { get; private set; }
    }
}