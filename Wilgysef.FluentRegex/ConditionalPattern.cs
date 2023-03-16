using System;
using System.Text;

namespace Wilgysef.FluentRegex
{
    public class ConditionalPattern : AbstractGroupPattern
    {
        public int? GroupNumber { get; private set; }

        public string? GroupName { get; private set; }

        public Pattern? Expression { get; private set; }

        public bool Lookahead { get; private set; }

        public Pattern YesPattern { get => _pattern!; set => _pattern = value; }

        public Pattern? NoPattern { get; set; }

        protected override bool HasContents => true;

        public ConditionalPattern(Pattern expression, Pattern yes, Pattern? no, bool lookahead = true)
            : base(yes)
        {
            Expression = expression;
            NoPattern = no;
            Lookahead = lookahead;
        }

        public ConditionalPattern(int group, Pattern yes, Pattern? no) : base(yes)
        {
            GroupNumber = group;
            NoPattern = no;
        }

        public ConditionalPattern(string group, Pattern yes, Pattern? no) : base(yes)
        {
            GroupName = group;
            NoPattern = no;
        }

        public ConditionalPattern WithGroup(int group)
        {
            GroupNumber = group;
            GroupName = null;
            Expression = null;
            return this;
        }

        public ConditionalPattern WithGroup(string group)
        {
            GroupNumber = null;
            GroupName = group;
            Expression = null;
            return this;
        }

        public ConditionalPattern WithExpression(Pattern expression, bool lookahead = true)
        {
            GroupNumber = null;
            GroupName = null;
            Expression = expression;
            Lookahead = lookahead;
            return this;
        }

        public ConditionalPattern WithYesPattern(Pattern yes)
        {
            YesPattern = yes;
            return this;
        }

        public ConditionalPattern WithNoPattern(Pattern? no)
        {
            NoPattern = no;
            return this;
        }

        protected override void GroupContents(StringBuilder builder)
        {
            builder.Append("?(");

            if (Expression != null)
            {
                if (Lookahead)
                {
                    builder.Append("?=");
                }
                else
                {
                    builder.Append("?<=");
                }

                Expression.ToString(builder);
            }
            else if (GroupName != null)
            {
                builder.Append(GroupName);
            }
            else if (GroupNumber != null)
            {
                builder.Append(GroupNumber);
            }

            builder.Append(')');

            _pattern!.Wrap(builder);

            if (NoPattern != null)
            {
                builder.Append('|');
                NoPattern.Wrap(builder);
            }
        }
    }
}
