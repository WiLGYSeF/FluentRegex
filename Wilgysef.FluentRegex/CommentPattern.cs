using System.Text;

namespace Wilgysef.FluentRegex
{
    public class CommentPattern : AbstractGroupPattern
    {
        protected override bool HasContents => true;

        public CommentPattern(string? value)
            : base(value != null ? new LiteralPattern(value) : null) { }

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
