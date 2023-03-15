using System.Text;

namespace Wilgysef.FluentRegex
{
    internal class AtomicGroupPattern : AbstractGroupPattern
    {
        protected override bool HasContents => true;

        public AtomicGroupPattern(Pattern pattern) : base(pattern) { }

        protected override void GroupContents(StringBuilder builder)
        {
            builder.Append("?>");
            _pattern!.ToString(builder);
        }
    }
}
