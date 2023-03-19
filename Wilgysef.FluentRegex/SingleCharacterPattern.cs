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

        internal override void ToString(StringBuilder builder)
        {
            builder.Append('.');
        }
    }
}
