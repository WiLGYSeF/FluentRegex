using System.Text;

namespace Wilgysef.FluentRegex
{
    public abstract class AbstractGroupPattern : Pattern
    {
        internal sealed override bool IsSinglePattern => true;

        protected Pattern? _pattern;

        protected AbstractGroupPattern(Pattern? pattern)
        {
            _pattern = pattern;
        }

        protected abstract void GroupContents(StringBuilder builder);

        internal sealed override void ToString(StringBuilder builder)
        {
            builder.Append('(');
            GroupContents(builder);
            builder.Append(')');
        }
    }
}
