using System.Collections.Generic;
using System.Linq;

namespace FlatBuffers
{
    /// <summary>
    /// Class representing a TypeModel and its direct dependencies. Used within the <see cref="FlatBuffersSchema" /> to order and resolve dependincies.
    /// </summary>
    public sealed class FlatBuffersSchemaTypeDependencyNode
    {
        private readonly List<FlatBuffersSchemaTypeDependencyNode> _dependentTypes = new List<FlatBuffersSchemaTypeDependencyNode>();

        /// <summary>
        /// Gets the <see cref="TypeModel"/> that this node holds
        /// </summary>
        public TypeModel TypeModel { get; private set; }

        /// <summary>
        /// Get the parent node
        /// </summary>
        public FlatBuffersSchemaTypeDependencyNode Parent { get; private set; }

        internal FlatBuffersSchemaTypeDependencyNode(TypeModel typeModel, FlatBuffersSchemaTypeDependencyNode parent)
        {
            TypeModel = typeModel;
            Parent = parent;
        }

        internal void AddDependencies(IEnumerable<FlatBuffersSchemaTypeDependencyNode> typeModels)
        {
            foreach (var dep in typeModels)
            {
                AddDependency(dep);
            }
        }

        internal void AddDependency(FlatBuffersSchemaTypeDependencyNode typeModel)
        {
            if (_dependentTypes.All(i => i.TypeModel.Name != typeModel.TypeModel.Name))
            {
                _dependentTypes.Add(typeModel);
            }
        }

        /// <summary>
        /// Gets an enumerable of the nodes that this node has dependencies on
        /// </summary>
        public IEnumerable<FlatBuffersSchemaTypeDependencyNode> DependentTypes { get { return _dependentTypes; } }
    }
}