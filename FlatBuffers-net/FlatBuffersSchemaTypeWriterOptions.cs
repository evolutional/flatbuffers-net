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
    /// Enum to direct how FlatBuffersSchemaTypeWriter should terminate a line
    /// </summary>
    public enum FlatBuffersSchemaWriterLineTerminatorType
    {
        /// <summary>
        /// Terminate with Windows style CrLf
        /// </summary>
        CrLf,
        /// <summary>
        /// Terminate with Linux style Lf
        /// </summary>
        Lf,
    }

    /// <summary>
    /// Enum to direct how Type/Field names are emitted by the FlatBuffersSchemaTypeWriter
    /// </summary>
    public enum FlatBuffersSchemaWriterNamingStyle
    {
        /// <summary>
        /// Use original style
        /// </summary>
        Original,
        /// <summary>
        /// Emit names in camelCase
        /// </summary>
        CamelCase,
        /// <summary>
        /// Emit names in lowercase
        /// </summary>
        LowerCase,
    }

    /// <summary>
    /// Options for the FlatBuffersSchemaTypeWriter
    /// </summary>
    public class FlatBuffersSchemaTypeWriterOptions
    {
        private int _indentCount;

        /// <summary>
        /// Initializes an instance of the FlatBuffersSchemaTypeWriterOptions class
        /// </summary>
        public FlatBuffersSchemaTypeWriterOptions()
        {
            IndentType = FlatBuffersSchemaWriterIndentType.Spaces;
            IndentCount = 4;
            BracingStyle = FlatBuffersSchemaWriterBracingStyle.Egyptian;
            LineTerminator = FlatBuffersSchemaWriterLineTerminatorType.CrLf;
            FieldNamingStyle = FlatBuffersSchemaWriterNamingStyle.Original;
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

        /// <summary>
        /// Gets and sets the type of line terminator to use when writing the schema
        /// </summary>
        public FlatBuffersSchemaWriterLineTerminatorType LineTerminator { get; set; }

        /// <summary>
        /// Gets and sets the naming style to use for fields
        /// </summary>
        public FlatBuffersSchemaWriterNamingStyle FieldNamingStyle { get; set; }
    }
}