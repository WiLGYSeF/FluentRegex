using System.Text;

namespace Wilgysef.FluentRegex
{
    public class CommentPattern : AbstractGroupPattern
    {
        protected override bool HasContents => true;

        public CommentPattern(string value) : base(new LiteralPattern(value)) { }

        public CommentPattern WithValue(string value)
        {
            _pattern = new LiteralPattern(value);
            return this;
        }

        protected override void GroupContents(StringBuilder builder)
        {
            builder.Append("?#");
            _pattern?.ToString(builder);
        }
    }
}
