using Wilgysef.FluentRegex.PatternBuilders;

namespace Wilgysef.FluentRegex
{
    public class SingleCharacterPattern : Pattern
    {
        internal override bool IsSinglePattern => true;

        internal override bool IsEmpty => false;

        public override Pattern Copy()
        {
            return new SingleCharacterPattern();
        }

        internal override void Build(PatternBuildState state)
        {
            state.WithPattern(this, Build);

            static void Build(IPatternStringBuilder builder)
            {
                builder.Append('.');
            }
        }

        internal override Pattern Unwrap()
        {
            return this;
        }
    }
}
