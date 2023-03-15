using System.Text;

namespace Wilgysef.FluentRegex
{
    internal class CommentPattern : AbstractGroupPattern
    {
        protected override bool HasContents => true;

        public CommentPattern(Pattern? pattern) : base(pattern) { }

        public CommentPattern(string value) : base(new LiteralPattern(value)) { }

        protected override void GroupContents(StringBuilder builder)
        {
            builder.Append("?#");
            _pattern?.ToString(builder);
        }
    }
}
