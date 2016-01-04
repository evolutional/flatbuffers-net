using System;
using System.Collections.Generic;
using System.IO;
using System.Linq;

namespace FlatBuffers
{
    public class FlatBuffersSchema
    {
        private readonly TypeModelRegistry _typeModelRegistry;
        private readonly List<FlatBuffersSchemaTypeDependencyNode> _nodes = new List<FlatBuffersSchemaTypeDependencyNode>();

        internal FlatBuffersSchema(TypeModelRegistry typeModelRegistry)
        {
            _typeModelRegistry = typeModelRegistry;
        }

        internal FlatBuffersSchema()
            : this(TypeModelRegistry.Default)
        { }

        public IEnumerable<FlatBuffersSchemaTypeDependencyNode> AllTypes { get { return _nodes; } }

        public FlatBuffersSchemaTypeDependencyNode AddType<T>()
        {
            return AddType(typeof(T));
        }

        public FlatBuffersSchemaTypeDependencyNode AddType(Type type)
        {
            var typeModel = _typeModelRegistry.GetTypeModel(type);
            return AddType(typeModel);
        }

        public FlatBuffersSchemaTypeDependencyNode AddType(TypeModel typeModel)
        {
            var node = GetDependencyNode(typeModel, null);
            return AddNode(node);
        }


        private FlatBuffersSchemaTypeDependencyNode AddNode(FlatBuffersSchemaTypeDependencyNode node)
        {
            var typeModel = node.TypeModel;
            if (!typeModel.IsObject && !typeModel.IsEnum)
            {
                return null;
            }

            // TODO: Perhaps a dictionary?
            if (_nodes.Any(i => i.TypeModel.Name == typeModel.Name))
            {
                throw new ArgumentException("TypeModel already exists");
            }
            
            var dependentTypes = GetDependentTypes(node);

            foreach (var depType in dependentTypes)
            {
                if (_nodes.All(i => i.TypeModel.Name != depType.TypeModel.Name))
                {
                    AddType(depType.TypeModel);
                }
            }

            node.AddDependencies(dependentTypes);
            _nodes.Add(node);
            return node;
        }

        private FlatBuffersSchemaTypeDependencyNode GetDependencyNode(TypeModel typeModel, FlatBuffersSchemaTypeDependencyNode parent)
        {
            var dep = _nodes.FirstOrDefault(i => i.TypeModel.Name == typeModel.Name);
            return dep ?? new FlatBuffersSchemaTypeDependencyNode(typeModel, parent);
        }

        private List<FlatBuffersSchemaTypeDependencyNode> GetDependentTypes(FlatBuffersSchemaTypeDependencyNode node)
        {
            var deps = new List<FlatBuffersSchemaTypeDependencyNode>();
            var typeModel = node.TypeModel;
            if (!typeModel.IsObject)
            {
                return deps;
            }

            var structDef = typeModel.StructDef;

            foreach (var field in structDef.Fields)
            {
                if (!field.TypeModel.IsObject && !field.TypeModel.IsEnum)
                    continue;

                if (deps.All(i => i.TypeModel.Name != field.TypeModel.Name))
                {
                    deps.Add(GetDependencyNode(field.TypeModel, node));
                }
            }
            return deps;
        }

        public void WriteTo(TextWriter writer)
        {
            var schemaWriter = new FlatBuffersSchemaTypeWriter(writer);
            var written = new List<TypeModel>();

            // These types are the 'leaf' types, eg: have no parents
            var leafNodes = _nodes.Where(i => i.Parent == null).ToList();

            // write those out with no deps first
            WriteRankedTypes(schemaWriter, leafNodes.Where(i => !i.DependentTypes.Any()), written);

            // remove already written nodes
            leafNodes.RemoveAll(i => written.Any(n => n.Name == i.TypeModel.Name));

            // resolve deps for those nodes with deps
            var seen = new List<FlatBuffersSchemaTypeDependencyNode>();
            var resolved = new List<FlatBuffersSchemaTypeDependencyNode>();
            foreach (var node in leafNodes)
            {
                ResolveDependency(node, resolved, seen);
            }
            // Write out the rest of the types in order
            WriteRankedTypes(schemaWriter, resolved, written);
        }

        private static void WriteRankedTypes(FlatBuffersSchemaTypeWriter schemaWriter, IEnumerable<FlatBuffersSchemaTypeDependencyNode> types, ICollection<TypeModel> written)
        {
            Func<TypeModel, int> typeOrderFunc = (type) =>
            {
                // Ordering func will order classes of type:
                //  enum
                //  struct
                //  table
                if (type.IsEnum)
                {
                    return 0;
                }
                return type.IsStruct ? 1 : 2;
            };

            // Ranking is as follows:
            //  'type category', 'name'

            var ranked = types.GroupBy(i => typeOrderFunc(i.TypeModel))
                .SelectMany(g => g.OrderBy(y => y.TypeModel.Name)
                .Select((x, i) => new { g.Key, Item = x, Rank = i + 1 }));

            foreach (var node in ranked.OrderBy(i => i.Key).ThenBy(i => i.Rank))
            {
                var typeModel = node.Item.TypeModel;

                if (written.Any(i => i.Name == typeModel.Name)) 
                    continue;

                schemaWriter.Write(typeModel);
                written.Add(typeModel);
            }
        }

        private void ResolveDependency(FlatBuffersSchemaTypeDependencyNode node, ICollection<FlatBuffersSchemaTypeDependencyNode> resolved, ICollection<FlatBuffersSchemaTypeDependencyNode> seen)
        {
            seen.Add(node);
            foreach (var dep in node.DependentTypes)
            {
                if (resolved.All(i => i.TypeModel.Name != dep.TypeModel.Name))
                {
                    if (seen.Any(i => i.TypeModel.Name == dep.TypeModel.Name))
                    {
                        throw new Exception(string.Format("Circular dependency detected between {0} -> {1}", node.TypeModel.Name, dep.TypeModel.Name));
                    }
                    ResolveDependency(dep, resolved, seen);
                }
            }
            resolved.Add(node);
        }
        
    }
}