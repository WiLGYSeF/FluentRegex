using System;
using System.Text;

namespace Wilgysef.FluentRegex
{
    internal class ConditionalPattern : AbstractGroupPattern
    {
        private readonly int? _groupNumber;
        private readonly string? _groupName;
        private readonly Pattern? _expression;
        private readonly Pattern? _no;
        private readonly bool _lookahead;

        public ConditionalPattern(Pattern expression, Pattern yes, Pattern? no, bool lookahead = true)
            : base(yes)
        {
            _expression = expression;
            _no = no;
            _lookahead = lookahead;
        }

        public ConditionalPattern(int group, Pattern yes, Pattern? no) : base(yes)
        {
            _groupNumber = group;
            _no = no;
        }

        public ConditionalPattern(string group, Pattern yes, Pattern? no) : base(yes)
        {
            _groupName = group;
            _no = no;
        }

        protected override void GroupContents(StringBuilder builder)
        {
            if (_expression == null && _groupName == null && _groupNumber == null)
            {
                throw new InvalidOperationException();
            }

            builder.Append("?(");

            if (_expression != null)
            {
                if (_lookahead)
                {
                    builder.Append("(?=");
                }
                else
                {
                    builder.Append("(?<=");
                }

                _expression.ToString(builder);
                builder.Append(')');
            }
            else if (_groupName != null)
            {
                builder.Append(_groupName);
            }
            else if (_groupNumber != null)
            {
                builder.Append(_groupNumber);
            }

            builder.Append(')');

            _pattern!.Wrap(builder);

            if (_no != null)
            {
                builder.Append('|');
                _no.Wrap(builder);
            }
        }
    }
}
