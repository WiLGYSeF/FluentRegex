using System.Text;

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

            static void Build(StringBuilder builder)
            {
                builder.Append('.');
            }
        }
    }
}
