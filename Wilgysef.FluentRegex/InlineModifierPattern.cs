using System;
using System.Text;

namespace Wilgysef.FluentRegex
{
    internal class InlineModifierPattern : Pattern
    {
        internal override bool IsSinglePattern => true;

        private readonly Pattern? _pattern;
        private readonly InlineModifier _modifier;

        public InlineModifierPattern(Pattern pattern, InlineModifier modifier)
        {
            _pattern = pattern;
            _modifier = modifier;
        }

        public InlineModifierPattern(InlineModifier modifier)
        {
            _modifier = modifier;
        }

        internal override void ToString(StringBuilder builder)
        {
            builder.Append("(?");

            if ((_modifier & InlineModifier.IgnoreCase) != 0)
            {
                builder.Append('i');
            }
            if ((_modifier & InlineModifier.Multiline) != 0)
            {
                builder.Append('m');
            }
            if ((_modifier & InlineModifier.ExplicitCapture) != 0)
            {
                builder.Append('n');
            }
            if ((_modifier & InlineModifier.Singleline) != 0)
            {
                builder.Append('s');
            }
            if ((_modifier & InlineModifier.IgnorePatternWhitespace) != 0)
            {
                builder.Append('x');
            }

            if (_pattern != null)
            {
                builder.Append(':');
                _pattern.ToString(builder);
            }

            builder.Append(')');
        }
    }

    [Flags]
    public enum InlineModifier
    {
        IgnoreCase = 1,
        Multiline = 2,
        ExplicitCapture = 4,
        Singleline = 16,
        IgnorePatternWhitespace = 32,
    }
}
