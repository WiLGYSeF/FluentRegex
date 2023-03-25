using System.Text;
using Wilgysef.FluentRegex.PatternBuilders;

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
