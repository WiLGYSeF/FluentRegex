using System.Text;

namespace Wilgysef.FluentRegex
{
    internal class NamedGroupPattern : GroupPattern
    {
        // TODO: group name validation, alternate group syntax?
        public string Name { get; set; }

        public NamedGroupPattern(string name, Pattern pattern) : base(pattern)
        {
            Name = name;
        }

        internal override void ToString(StringBuilder builder)
        {
            builder.Append("(?<");
            builder.Append(Name);
            builder.Append('>');
            _pattern.ToString(builder);
            builder.Append(')');
        }
    }
}
