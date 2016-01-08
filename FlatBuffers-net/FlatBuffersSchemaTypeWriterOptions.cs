using System;

namespace FlatBuffers
{
    /// <summary>
    /// Enum to direct how FlatBuffersSchemaTypeWriter should indent the schema
    /// </summary>
    public enum FlatBuffersSchemaWriterIndentType
    {
        /// <summary>
        /// Indent using spaces
        /// </summary>
        Spaces,
        /// <summary>
        /// Indent using tabs
        /// </summary>
        Tabs,
    }

    /// <summary>
    /// Enum to direct how FlatBuffersSchemaTypeWriter should position the braces in the schema
    /// </summary>
    public enum FlatBuffersSchemaWriterBracingStyle
    {
        /// <summary>
        /// Braces appear on the line of the declaration (eg: Java-style)
        /// </summary>
        Egyptian,
        /// <summary>
        /// Braces appear on a new line (eg: C# style)
        /// </summary>
        NewLine,
    }

    /// <summary>
    /// Options for the FlatBuffersSchemaTypeWriter
    /// </summary>
    public class FlatBuffersSchemaTypeWriterOptions
    {
        private int _indentCount;

        public FlatBuffersSchemaTypeWriterOptions()
        {
            IndentType = FlatBuffersSchemaWriterIndentType.Spaces;
            IndentCount = 4;
            BracingStyle = FlatBuffersSchemaWriterBracingStyle.Egyptian;
        }

        /// <summary>
        /// Gets the default set of options
        /// </summary>
        public static FlatBuffersSchemaTypeWriterOptions Default { get { return new FlatBuffersSchemaTypeWriterOptions(); } }

        /// <summary>
        /// Gets and sets the type of indentation character to use when writing the schema
        /// </summary>
        public FlatBuffersSchemaWriterIndentType IndentType { get; set; }

        /// <summary>
        /// Gets and sets the number of indentation characters to use when writing the schema
        /// </summary>
        public int IndentCount
        {
            get { return _indentCount; }
            set
            {
                if (_indentCount < 0) throw new ArgumentOutOfRangeException();
                _indentCount = value;
            }
        }

        /// <summary>
        /// Gets and sets the type of bracing style to use when writing the schema
        /// </summary>
        public FlatBuffersSchemaWriterBracingStyle BracingStyle { get; set; }
    }
}