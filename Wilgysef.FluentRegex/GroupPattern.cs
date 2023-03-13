using System.Text;

namespace Wilgysef.FluentRegex
{
    // TODO: balancing, atomic, comment, conditional, lookahead, lookbehind groups

    internal class GroupPattern : Pattern
    {
        public virtual bool IsCapturing => true;

        internal override bool IsSinglePattern => true;

        protected readonly Pattern _pattern;

        public GroupPattern(Pattern pattern)
        {
            _pattern = pattern;
        }

        internal override void ToString(StringBuilder builder)
        {
            builder.Append('(');
            _pattern.ToString(builder);
            builder.Append(')');
        }

        internal static void NonCaptureGroup(StringBuilder builder, Pattern pattern)
        {
            builder.Append("(?:");
            pattern.ToString(builder);
            builder.Append(')');
        }
    }
}
