using System.Text;

namespace Wilgysef.FluentRegex
{
    internal class BackreferencePattern : Pattern
    {
        internal override bool IsSinglePattern => true;

        public int? GroupNumber { get; private set; }

        public string? GroupName { get; private set; }

        public BackreferenceType Type { get; private set; }

        public object Group => Type == BackreferenceType.Number ? (object)GroupNumber!.Value : GroupName!;

        public BackreferencePattern(int group)
        {
            WithGroup(group);
        }

        public BackreferencePattern(string group)
        {
            WithGroup(group);
        }

        public BackreferencePattern WithGroup(int group)
        {
            GroupNumber = group;
            GroupName = null;
            Type = BackreferenceType.Number;
            return this;
        }

        public BackreferencePattern WithGroup(string group)
        {
            GroupNumber = null;
            GroupName = group;
            Type = BackreferenceType.Name;
            return this;
        }

        internal override void ToString(StringBuilder builder)
        {
            if (Type == BackreferenceType.Number)
            {
                builder.Append(@"\g{");
                builder.Append(GroupNumber!.Value);
                builder.Append('}');
            }
            else
            {
                builder.Append(@"\k<");
                builder.Append(GroupName);
                builder.Append('>');
            }
        }

        public enum BackreferenceType
        {
            Number,
            Name,
        }
    }
}
