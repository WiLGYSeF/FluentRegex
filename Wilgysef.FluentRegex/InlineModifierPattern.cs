using System;
using System.Text;

namespace Wilgysef.FluentRegex
{
    internal class InlineModifierPattern : AbstractGroupPattern
    {
        private readonly InlineModifier _modifier;

        public InlineModifierPattern(Pattern? pattern, InlineModifier modifier) : base(pattern)
        {
            _modifier = modifier;
        }

        protected override void GroupContents(StringBuilder builder)
        {
            builder.Append('?');

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
