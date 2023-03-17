using System.Text;

namespace Wilgysef.FluentRegex
{
    public class ConditionalPattern : AbstractGroupPattern
    {
        public int? GroupNumber { get; private set; }

        public string? GroupName { get; private set; }

        public Pattern? Expression
        {
            get => _expression;
            set => SetChildPattern(value, ref _expression, ExpressionIndex);
        }

        public bool Lookahead { get; private set; }

        public Pattern YesPattern
        {
            get => _children[YesIndex];
            set => _children[YesIndex] = value;
        }

        public Pattern? NoPattern
        {
            get => _no;
            set => SetChildPattern(value, ref _no, NoIndex);
        }

        protected override bool HasContents => true;

        private int ExpressionIndex => 0;

        private int YesIndex => Expression != null ? 1 : 0;

        private int NoIndex => Expression != null ? 2 : 1;

        private Pattern? _expression;
        private Pattern? _no;

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

            YesPattern.Wrap(builder);

            if (NoPattern != null)
            {
                builder.Append('|');
                NoPattern.Wrap(builder);
            }
        }

        private void SetChildPattern(Pattern? value, ref Pattern? pattern, int index)
        {
            if (value != null)
            {
                if (pattern == null)
                {
                    pattern = value;
                    _children.Insert(index, pattern);
                }
                else
                {
                    pattern = value;
                    _children[index] = pattern;
                }
            }
            else if (pattern != null)
            {
                pattern = value;
                _children.RemoveAt(index);
            }
        }
    }
}
