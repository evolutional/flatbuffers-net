using System;
using System.Security;

namespace FlatBuffers.Attributes
{
    /// <summary>
    /// Allows annotation of FlatBuffers objects. The comments will be written to the schema.
    /// </summary>
    [AttributeUsage(AttributeTargets.Field | AttributeTargets.Property | AttributeTargets.Class | AttributeTargets.Struct | AttributeTargets.Enum, AllowMultiple = true, Inherited = true)]
    public class FlatBuffersCommentAttribute : Attribute
    {
        private int _order;

        /// <summary>
        /// Gets a string of the comment text
        /// </summary>
        public string Comment { get; private set; }

        /// <summary>
        /// Gets the integer sorting order of this comment
        /// </summary>
        public int Order
        {
            get { return _order; }
            private set { _order = value;
                HasOrder = true;
            }
        }

        /// <summary>
        /// Gets a Boolean to indicate if this comment has a specific order
        /// </summary>
        public bool HasOrder { get; private set; }

        /// <summary>
        /// Initializes a new instance of the FlatBuffersCommentAttribute class
        /// </summary>
        /// <param name="comment">Comment to use</param>
        public FlatBuffersCommentAttribute(string comment)
        {
            if (string.IsNullOrEmpty(comment))
            {
                throw new ArgumentNullException();
            }

            Comment = comment;
        }

        /// <summary>
        /// Initializes a new instance of the FlatBuffersCommentAttribute class
        /// </summary>
        /// <param name="order">The order in which the comment will appear</param>
        /// <param name="comment">Comment to use</param>
        public FlatBuffersCommentAttribute(int order, string comment)
            : this(comment)
        {
            Order = order;
        }
    }
}
