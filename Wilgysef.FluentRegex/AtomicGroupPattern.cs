using System.Text;

namespace Wilgysef.FluentRegex
{
    public class AtomicGroupPattern : AbstractGroupPattern
    {
        protected override bool HasContents => true;

        public AtomicGroupPattern(Pattern? pattern) : base(pattern) { }

        public AtomicGroupPattern WithPattern(Pattern? pattern)
        {
            Pattern = pattern;
            return this;
        }

        public override Pattern Copy()
        {
            return new AtomicGroupPattern(Pattern?.Copy());
        }

        protected override void GroupContents(StringBuilder builder)
        {
            builder.Append("?>");
            Pattern?.ToString(builder);
        }
    }
}
