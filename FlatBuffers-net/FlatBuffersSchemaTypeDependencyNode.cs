using System.Collections.Generic;
using System.Linq;

namespace FlatBuffers
{
    public class FlatBuffersSchemaTypeDependencyNode
    {
        private readonly List<FlatBuffersSchemaTypeDependencyNode> _dependentTypes = new List<FlatBuffersSchemaTypeDependencyNode>();

        public TypeModel TypeModel { get; private set; }
        public FlatBuffersSchemaTypeDependencyNode Parent { get; private set; }

        public FlatBuffersSchemaTypeDependencyNode(TypeModel typeModel, FlatBuffersSchemaTypeDependencyNode parent)
        {
            TypeModel = typeModel;
            Parent = parent;
        }

        public void AddDependencies(IEnumerable<FlatBuffersSchemaTypeDependencyNode> typeModels)
        {
            foreach (var dep in typeModels)
            {
                AddDependency(dep);
            }
        }

        public void AddDependency(FlatBuffersSchemaTypeDependencyNode typeModel)
        {
            if (_dependentTypes.All(i => i.TypeModel.Name != typeModel.TypeModel.Name))
            {
                _dependentTypes.Add(typeModel);
            }
        }

        public IEnumerable<FlatBuffersSchemaTypeDependencyNode> DependentTypes { get { return _dependentTypes; } }
    }
}