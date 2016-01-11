using System;

namespace FlatBuffers
{
    /// <summary>
    /// Generates a <see cref="FlatBuffersSchema"/> from a given .NET <see cref="Type"/>
    /// </summary>
    public class FlatBuffersSchemaGenerator
    {
        private readonly TypeModelRegistry _typeModelRegistry;

        /// <summary>
        /// Initializes an instance of the <see cref="FlatBuffersSchemaGenerator"/> class
        /// </summary>
        /// <param name="typeModelRegistry">The <see cref="TypeModelRegistry"/> used to resolve <see cref="Type"/> into <see cref="TypeModel"/></param>
        public FlatBuffersSchemaGenerator(TypeModelRegistry typeModelRegistry)
        {
            _typeModelRegistry = typeModelRegistry;
        }

        /// <summary>
        /// Initializes an instance of the <see cref="FlatBuffersSchemaGenerator"/> class using the defualt <see cref="TypeModelRegistry"/>
        /// </summary>
        public FlatBuffersSchemaGenerator()
            : this(TypeModelRegistry.Default)
        { }

        /// <summary>
        /// Creates a new <see cref="FlatBuffersSchema"/> objects
        /// </summary>
        /// <returns>New <see cref="FlatBuffersSchema"/> object</returns>
        public FlatBuffersSchema Create()
        {
            var schema = new FlatBuffersSchema(_typeModelRegistry);
            return schema;
        }

        /// <summary>
        /// Creates a new <see cref="FlatBuffersSchema"/> and automatically adds the <see cref="Type"/> to it.
        /// </summary>
        /// <param name="isRootType">A Boolean to indiciate if this type should be the root type of the schema</param>
        /// <typeparam name="T"><see cref="Type"/> of the object to reflect</typeparam>
        /// <returns>An <see cref="FlatBuffersSchema"/> object with the reflected type</returns>
        public FlatBuffersSchema Generate<T>(bool isRootType = false)
        {
            return Generate(typeof (T), isRootType);
        }

        /// <summary>
        /// Creates a new <see cref="FlatBuffersSchema"/> and automatically adds the <see cref="Type"/> to it.
        /// </summary>
        /// <param name="type"><see cref="Type"/> of the object to reflect</param>
        /// <param name="isRootType">A Boolean to indiciate if this type should be the root type of the schema</param>
        /// <returns>An <see cref="FlatBuffersSchema"/> object with the reflected type</returns>
        public FlatBuffersSchema Generate(Type type, bool isRootType = false)
        {
            var schema = Create();

            if (isRootType)
            {
                schema.SetRootType(type);
            }
            else
            {
                schema.AddType(type);
            }
            return schema;
        }

    }
}