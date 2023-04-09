using Wilgysef.FluentRegex.PatternBuilders;

namespace Wilgysef.FluentRegex
{
    public class CommentPattern : AbstractGroupPattern
    {
        internal override bool IsEmpty => Pattern.IsNullOrEmpty();

        /// <summary>
        /// Creates a comment.
        /// </summary>
        /// <param name="value">Comment value.</param>
        public CommentPattern(string? value)
            : this(value != null ? new LiteralPattern(value) : null) { }

        /// <summary>
        /// Creates a comment.
        /// </summary>
        /// <param name="literal">Comment literal value.</param>
        public CommentPattern(LiteralPattern? literal) : base(literal) { }

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

        /// <summary>
        /// Sets the comment value.
        /// </summary>
        /// <param name="literal">Comment literal value.</param>
        /// <returns>Current comment pattern.</returns>
        public CommentPattern WithValue(LiteralPattern literal)
        {
            Pattern = literal;
            return this;
        }

        public override Pattern Copy()
        {
            return new CommentPattern((Pattern as LiteralPattern)?.Value);
        }

        public override Pattern Unwrap()
        {
            return this;
        }

        private protected override void GroupContents(PatternBuildState state)
        {
            state.WithPattern(this, Build);

            void Build(IPatternStringBuilder builder)
            {
                builder.Append("?#");
                Pattern?.Build(state);
            }
        }
    }
}
