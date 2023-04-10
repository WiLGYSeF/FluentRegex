using Wilgysef.FluentRegex.PatternStringBuilders;
using Wilgysef.FluentRegex.PatternStates;

namespace Wilgysef.FluentRegex
{
    public class SingleCharacterPattern : Pattern
    {
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

        internal override Pattern UnwrapInternal(PatternBuildState state)
        {
            return this;
        }

        internal override bool IsEmpty(PatternBuildState state)
        {
            return false;
        }

        internal override bool IsSinglePattern(PatternBuildState state)
        {
            return true;
        }
    }
}
