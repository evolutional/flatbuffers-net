using System;
using System.Collections.Generic;
using System.IO;
using System.Linq;

namespace FlatBuffers
{
    /// <summary>
    /// Class that represents a single FlatBuffers schema (fbs) file
    /// </summary>
    public class FlatBuffersSchema
    {
        private FlatBuffersSchemaTypeDependencyNode _rootTypeNode;
        private readonly TypeModelRegistry _typeModelRegistry;
        private readonly List<FlatBuffersSchemaTypeDependencyNode> _nodes = new List<FlatBuffersSchemaTypeDependencyNode>();

        private readonly HashSet<string> _metadataAttributes = new HashSet<string>();

        /// <summary>
        /// Gets the RootType for this schema
        /// </summary>
        public FlatBuffersSchemaTypeDependencyNode RootType { get { return _rootTypeNode; } }

        /// <summary>
        /// Gets a Boolean to indiciate if this schema has a <see cref="RootType"/> set
        /// </summary>
        public bool HasRootType { get { return _rootTypeNode != null; } }

        /// <summary>
        /// Gets an enumerable of the user-specified attibutes used in this schema
        /// </summary>
        public IEnumerable<string> UserMetadataAttributes { get { return _metadataAttributes; } } 

        internal FlatBuffersSchema(TypeModelRegistry typeModelRegistry)
        {
            _typeModelRegistry = typeModelRegistry;
        }

        internal FlatBuffersSchema()
            : this(TypeModelRegistry.Default)
        { }

        /// <summary>
        /// Gets an enumerable of the types contained in this schema
        /// </summary>
        public IEnumerable<FlatBuffersSchemaTypeDependencyNode> AllTypes { get { return _nodes; } }

        /// <summary>
        /// Adds a .NET type to the schema. Type will be reflected into a <see cref="TypeModel"/>
        /// </summary>
        /// <typeparam name="T">The type to add to the schema</typeparam>
        /// <returns>A dependency node for the type</returns>
        public FlatBuffersSchemaTypeDependencyNode AddType<T>()
        {
            return AddType(typeof(T));
        }

        /// <summary>
        /// Adds a .NET type to the schema. Type will be reflected into a <see cref="TypeModel"/>
        /// </summary>
        /// <param name="type">The type to add to the schema</param>
        /// <returns>A dependency node for the type</returns>
        public FlatBuffersSchemaTypeDependencyNode AddType(Type type)
        {
            var typeModel = _typeModelRegistry.GetTypeModel(type);
            return AddType(typeModel);
        }

        /// <summary>
        /// Adds a <see cref="TypeModel"/> to the schema
        /// </summary>
        /// <param name="typeModel">The type to add to the schema</param>
        /// <returns>A dependency node for the type</returns>
        public FlatBuffersSchemaTypeDependencyNode AddType(TypeModel typeModel)
        {
            var node = GetDependencyNode(typeModel, null);
            return AddNode(node);
        }

        /// <summary>
        /// Sets the Root <see cref="TypeModel"/> of this schema
        /// </summary>
        /// <typeparam name="T">The root type</typeparam>
        /// <returns>A dependency node for the root type</returns>
        public FlatBuffersSchemaTypeDependencyNode SetRootType<T>()
        {
            return SetRootType(typeof (T));
        }

        /// <summary>
        /// Sets the Root <see cref="TypeModel"/> of this schema
        /// </summary>
        /// <param name="type">The root type</param>
        /// <returns>A dependency node for the root type</returns>
        public FlatBuffersSchemaTypeDependencyNode SetRootType(Type type)
        {
            var typeModel = _typeModelRegistry.GetTypeModel(type);
            return SetRootType(typeModel);
        }

        /// <summary>
        /// Sets the Root <see cref="TypeModel"/> of this schema
        /// </summary>
        /// <param name="typeModel">The root type</param>
        /// <returns>A dependency node for the root type</returns>
        public FlatBuffersSchemaTypeDependencyNode SetRootType(TypeModel typeModel)
        {
            if (HasRootType)
            {
                throw new FlatBuffersSchemaException("Schema already has a root type");
            }

            if (!typeModel.IsObject)
            {
                throw new FlatBuffersSchemaException("Type must be a Table or Struct type to be used as a root type");
            }

            var node = AddType(typeModel);
            _rootTypeNode = node;
            return node;
        }

        private FlatBuffersSchemaTypeDependencyNode AddNode(FlatBuffersSchemaTypeDependencyNode node)
        {
            var typeModel = node.TypeModel;
            if (!typeModel.IsObject && !typeModel.IsEnum && !typeModel.IsUnion)
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
                    if (depType.TypeModel != null)
                        AddType(depType.TypeModel);
                }
            }

            node.AddDependencies(dependentTypes);
            _nodes.Add(node);
            return node;
        }

        private void CollectMetadata(TypeDefinition def)
        {
            foreach (var meta in def.Metadata.Items.Where(i=>i.IsUserMetaData))
            {
                _metadataAttributes.Add(meta.Key);
            }
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
                if (field.TypeModel.IsVector && field.TypeModel.ElementType == BaseType.Struct)
                {
                    var elementType = field.TypeModel.GetElementTypeModel();

                    if (elementType != null)
                    {
                        deps.Add(GetDependencyNode(elementType, node));
                    }

                    continue;
                }

                if (!field.TypeModel.IsObject && !field.TypeModel.IsEnum && !field.TypeModel.IsUnion && !field.TypeModel.IsUnion)
                    continue;

                if (field.TypeModel.IsUnion)
                {
                    var unionDeps = field.TypeModel.UnionDef.Fields.Where(i=>i.MemberType != null).Select(i => GetDependencyNode(i.MemberType, node));
                    foreach (var u in unionDeps)
                    {
                        if (deps.All(i => i.TypeModel != null && i.TypeModel.Name != u.TypeModel.Name))
                        {
                            deps.Add(GetDependencyNode(u.TypeModel, node));
                        }
                    }
                }

                if (field.TypeModel.IsVector && field.TypeModel.ElementType == BaseType.Struct)
                {
                    var elementType = field.TypeModel.GetElementTypeModel();
                
                    if (elementType != null)
                    {
                        deps.Add(GetDependencyNode(elementType, node));
                    }
                
                    continue;
                }

                if (field.HasNestedFlatBufferType)
                {
                    if (deps.All(i => i.TypeModel.Name != field.NestedFlatBufferType.Name))
                    {
                        deps.Add(GetDependencyNode(field.NestedFlatBufferType, node));
                    }
                }

                if (deps.All(i => i.TypeModel.Name != field.TypeModel.Name))
                {
                    deps.Add(GetDependencyNode(field.TypeModel, node));
                }

 
            }
            return deps;
        }

        /// <summary>
        /// Writes this schema to the specified <see cref="TextWriter"/> using the default options
        /// </summary>
        /// <param name="writer">TextWriter to emit the schema to</param>
        public void WriteTo(TextWriter writer)
        {
            WriteTo(writer, FlatBuffersSchemaTypeWriterOptions.Default);
        }

        /// <summary>
        /// Writes this schema to the specified <see cref="TextWriter"/>
        /// </summary>
        /// <param name="writer">TextWriter to emit the schema to</param>
        /// <param name="options">Options to use when writing the schema</param>
        public void WriteTo(TextWriter writer, FlatBuffersSchemaTypeWriterOptions options)
        {
            var schemaWriter = new FlatBuffersSchemaTypeWriter(writer, options);

            var resolved = TraverseTypeGraph();

            WriteUserMetadata(schemaWriter);

            foreach (var node in resolved)
            {
                schemaWriter.Write(node.TypeModel);
            }

            WriteFooter(schemaWriter);
        }

        private void WriteFooter(FlatBuffersSchemaTypeWriter schemaWriter)
        {
            if (HasRootType)
            {
                schemaWriter.WriteRootType(RootType.TypeModel.Name);
                if (RootType.TypeModel.IsTable)
                {
                    var structDef = RootType.TypeModel.StructDef;
                    if (structDef.HasIdentifier)
                    {
                        schemaWriter.WriteFileIdentifier(structDef.Identifier);
                    }
                }
            }
        }


        /// <summary>
        /// Traverses the type graph, resolving dependencies.
        /// </summary>
        /// <returns>Resolved, ordered list of types</returns>
        private IEnumerable<FlatBuffersSchemaTypeDependencyNode> TraverseTypeGraph()
        {
            var results = new List<FlatBuffersSchemaTypeDependencyNode>();
            var visited = new List<TypeModel>();
            // These types are the 'leaf' types, eg: have no parents
            var leafNodes = _nodes.Where(i => i.Parent == null).ToList();

            // write those out with no deps first
            VisitRankedTypes(results, leafNodes.Where(i => !i.DependentTypes.Any()), visited);

            // remove already visited nodes
            leafNodes.RemoveAll(i => visited.Any(n => n.Name == i.TypeModel.Name));

            // resolve deps for those nodes with deps
            var seen = new List<FlatBuffersSchemaTypeDependencyNode>();
            var resolved = new List<FlatBuffersSchemaTypeDependencyNode>();
            foreach (var node in leafNodes)
            {
                ResolveDependency(node, resolved, seen);
            }
            // Write out the rest of the types in order
            VisitRankedTypes(results, resolved, visited);
            return results;
        }

        private void WriteUserMetadata(FlatBuffersSchemaTypeWriter schemaTypeWriter)
        {
            foreach (var meta in _metadataAttributes.OrderBy(i => i))
            {
                schemaTypeWriter.WriteAttribute(meta);
            }
            if (_metadataAttributes.Any())
            {
                schemaTypeWriter.WriteLine();
            }
        }

        private void VisitRankedTypes(ICollection<FlatBuffersSchemaTypeDependencyNode> results, IEnumerable<FlatBuffersSchemaTypeDependencyNode> types, ICollection<TypeModel> visited)
        {
            Func<TypeModel, int> typeOrderFunc = (type) =>
            {
                // Ordering func will order classes of type:
                //  enum
                // union
                //  struct
                //  table
                if (type.IsEnum)
                {
                    return 0;
                }
                if (type.IsUnion)
                {
                    return 1;
                }
                return type.IsStruct ? 2 : 3;
            };

            // Ranking is as follows:
            //  'type category', 'name'

            var ranked = types.GroupBy(i => typeOrderFunc(i.TypeModel))
                .SelectMany(g => g.OrderBy(y => y.TypeModel.Name)
                .Select((x, i) => new { g.Key, Item = x, Rank = i + 1 }));

            foreach (var node in ranked.OrderBy(i => i.Key).ThenBy(i => i.Rank))
            {
                var typeModel = node.Item.TypeModel;

                if (visited.Any(i => i.Name == typeModel.Name)) 
                    continue;

                // apply user types
                CollectMetadata(typeModel);

                results.Add(node.Item);
                visited.Add(typeModel);
            }
        }

        private void CollectMetadata(TypeModel typeModel)
        {
            if (typeModel.IsObject)
            {
                CollectMetadata(typeModel.StructDef);
                foreach (var field in typeModel.StructDef.Fields)
                {
                    CollectMetadata(field);
                }
            }
            else if (typeModel.IsEnum)
            {
                CollectMetadata(typeModel.EnumDef);
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