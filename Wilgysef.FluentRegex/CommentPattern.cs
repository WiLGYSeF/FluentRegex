using Wilgysef.FluentRegex.PatternStringBuilders;
using Wilgysef.FluentRegex.PatternStates;

namespace Wilgysef.FluentRegex
{
    public class CommentPattern : AbstractGroupPattern
    {
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

        internal override Pattern CopyInternal(PatternBuildState state)
        {
            return new CommentPattern((Pattern as LiteralPattern)?.Value);
        }

        internal override Pattern UnwrapInternal(PatternBuildState state)
        {
            return this;
        }

        internal override bool IsEmpty(PatternBuildState state)
        {
            return Pattern.IsNullOrEmpty(state);
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
