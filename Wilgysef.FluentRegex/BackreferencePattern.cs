using System;
using System.Text;

namespace Wilgysef.FluentRegex
{
    public class BackreferencePattern : Pattern
    {
        public int? GroupNumber { get; private set; }

        public string? GroupName { get; private set; }

        public BackreferenceType Type { get; private set; }

        public object Group => Type == BackreferenceType.Number ? (object)GroupNumber!.Value : GroupName!;

        internal override bool IsSinglePattern => true;

        // TODO: backreference group pattern

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
                if (GroupNumber!.Value > 9)
                {
                    throw new InvalidOperationException("Backreference number exceeds limit of 9.");
                }

                builder.Append('\\');
                builder.Append(GroupNumber!.Value);
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
