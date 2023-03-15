using System;
using System.Text;

namespace Wilgysef.FluentRegex
{
    internal class InlineModifierPattern : AbstractGroupPattern
    {
        private readonly InlineModifier _modifiers;

        public InlineModifierPattern(Pattern? pattern, InlineModifier modifiers) : base(pattern)
        {
            _modifiers = modifiers;
        }

        protected override void GroupContents(StringBuilder builder)
        {
            builder.Append('?');

            if ((_modifiers & InlineModifier.IgnoreCase) != 0)
            {
                builder.Append('i');
            }
            if ((_modifiers & InlineModifier.Multiline) != 0)
            {
                builder.Append('m');
            }
            if ((_modifiers & InlineModifier.ExplicitCapture) != 0)
            {
                builder.Append('n');
            }
            if ((_modifiers & InlineModifier.Singleline) != 0)
            {
                builder.Append('s');
            }
            if ((_modifiers & InlineModifier.IgnorePatternWhitespace) != 0)
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
