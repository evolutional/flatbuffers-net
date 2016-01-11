using System.Collections.Generic;

namespace FlatBuffers
{
    internal class IndexBasedFieldTypeDefinitionComparaer : IComparer<FieldTypeDefinition>
    {
        public int Compare(FieldTypeDefinition x, FieldTypeDefinition y)
        {
            if (x.Index == y.Index)
                return 0;
            if (x.Index < y.Index)
                return -1;
            return 1;
        }
    }
}