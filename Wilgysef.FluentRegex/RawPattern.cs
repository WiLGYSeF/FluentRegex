using System.Text;

namespace Wilgysef.FluentRegex
{
    internal class RawPattern : Pattern
    {
        internal override bool IsSinglePattern => true;

        private readonly string _regex;

        public RawPattern(string regex)
        {
            _regex = regex;
        }

        internal override void ToString(StringBuilder builder)
        {
            builder.Append(_regex);
        }
    }
}
