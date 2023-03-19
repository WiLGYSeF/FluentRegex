using System.Text;

namespace Wilgysef.FluentRegex
{
    public class CommentPattern : AbstractGroupPattern
    {
        protected override bool HasContents => true;

        /// <summary>
        /// Creates a comment.
        /// </summary>
        /// <param name="value">Comment value.</param>
        public CommentPattern(string? value)
            : base(value != null ? new LiteralPattern(value) : null) { }

        /// <summary>
        /// Sets the comment value.
        /// </summary>
        /// <param name="value">Comment value.</param>
        /// <returns>Current comment pattern.</returns>
        public CommentPattern WithValue(string? value)
        {
            Pattern = value != null
                ? new LiteralPattern(value)
                : null;
            return this;
        }

        public override Pattern Copy()
        {
            return new CommentPattern((Pattern as LiteralPattern)?.Value);
        }

        protected override void GroupContents(StringBuilder builder)
        {
            builder.Append("?#");
            Pattern?.ToString(builder);
        }
    }
}
