using System.Text;

namespace Wilgysef.FluentRegex
{
    internal class SingleCharacterPattern : Pattern
    {
        internal override bool IsSinglePattern => true;

        internal override void ToString(StringBuilder builder)
        {
            builder.Append('.');
        }
    }
}
