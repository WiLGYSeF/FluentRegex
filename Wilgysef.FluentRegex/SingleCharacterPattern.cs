using Wilgysef.FluentRegex.PatternStringBuilders;
using Wilgysef.FluentRegex.PatternStates;

namespace Wilgysef.FluentRegex
{
    public class SingleCharacterPattern : Pattern
    {
        internal override bool IsSinglePattern => true;

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
    }
}
