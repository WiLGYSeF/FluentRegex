using System;
using System.Text;

namespace Wilgysef.FluentRegex
{
    // TODO
    internal class InlineModifierPattern : Pattern
    {
        internal override bool IsSinglePattern => true;

        public InlineModifierPattern(InlineModifier modifier) { }

        internal override void ToString(StringBuilder builder)
        {
            throw new NotImplementedException();
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
