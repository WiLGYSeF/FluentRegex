using System.Text;

namespace Wilgysef.FluentRegex
{
    public class AtomicGroupPattern : AbstractGroupPattern
    {
        protected override bool HasContents => true;

        public AtomicGroupPattern(Pattern? pattern) : base(pattern) { }

        public AbstractGroupPattern WithPattern(Pattern? pattern)
        {
            Pattern = pattern;
            return this;
        }

        protected override void GroupContents(StringBuilder builder)
        {
            builder.Append("?>");
            Pattern?.ToString(builder);
        }
    }
}
