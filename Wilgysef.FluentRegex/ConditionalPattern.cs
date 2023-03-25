using System.Text;
using Wilgysef.FluentRegex.PatternBuilders;

namespace Wilgysef.FluentRegex
{
    public class ConditionalPattern : AbstractGroupPattern
    {
        /// <summary>
        /// Conditional group number.
        /// </summary>
        public int? GroupNumber { get; private set; }

        /// <summary>
        /// Conditional group name.
        /// </summary>
        public string? GroupName { get; private set; }

        /// <summary>
        /// Conditional expression.
        /// </summary>
        public Pattern? Expression
        {
            get => _expression;
            set => SetChildPattern(value, ref _expression, ExpressionIndex);
        }

        /// <summary>
        /// Indicates if the conditional expression is lookahead.
        /// </summary>
        public bool Lookahead { get; private set; }

        /// <summary>
        /// Pattern if the conditional matches.
        /// </summary>
        public Pattern YesPattern
        {
            get => _children[YesIndex];
            set => _children[YesIndex] = value;
        }

        /// <summary>
        /// Pattern if the conditional does not match.
        /// </summary>
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

        /// <summary>
        /// Creates a conditional pattern.
        /// </summary>
        /// <param name="expression">Conditional expression.</param>
        /// <param name="yes">Pattern if the conditional matches.</param>
        /// <param name="no">Pattern if the conditional does not match.</param>
        /// <param name="lookahead">Indicates if the expression is lookahead.</param>
        public ConditionalPattern(Pattern expression, Pattern yes, Pattern? no, bool lookahead = true)
            : base(yes)
        {
            Expression = expression;
            NoPattern = no;
            Lookahead = lookahead;
        }

        /// <summary>
        /// Creates a conditional pattern.
        /// </summary>
        /// <param name="group">Conditional group number.</param>
        /// <param name="yes">Pattern if the conditional matches.</param>
        /// <param name="no">Pattern if the conditional does not match.</param>
        public ConditionalPattern(int group, Pattern yes, Pattern? no) : base(yes)
        {
            GroupNumber = group;
            NoPattern = no;
        }

        /// <summary>
        /// Creates a conditional pattern.
        /// </summary>
        /// <param name="group">Conditional group name.</param>
        /// <param name="yes">Pattern if the conditional matches.</param>
        /// <param name="no">Pattern if the conditional does not match.</param>
        public ConditionalPattern(string group, Pattern yes, Pattern? no) : base(yes)
        {
            GroupName = group;
            NoPattern = no;
        }

        /// <summary>
        /// Sets the conditional group number.
        /// </summary>
        /// <param name="group">Group number.</param>
        /// <returns>Current conditional pattern.</returns>
        public ConditionalPattern WithGroup(int group)
        {
            GroupNumber = group;
            GroupName = null;
            Expression = null;
            return this;
        }

        /// <summary>
        /// Sets the conditional group name.
        /// </summary>
        /// <param name="group">Group name.</param>
        /// <returns>Current conditional pattern.</returns>
        public ConditionalPattern WithGroup(string group)
        {
            GroupNumber = null;
            GroupName = group;
            Expression = null;
            return this;
        }

        /// <summary>
        /// Sets the conditional expression.
        /// </summary>
        /// <param name="expression">Expressino.</param>
        /// <param name="lookahead">Indicates if the expression is lookahead.</param>
        /// <returns>Current conditional pattern.</returns>
        public ConditionalPattern WithExpression(Pattern expression, bool lookahead = true)
        {
            GroupNumber = null;
            GroupName = null;
            Expression = expression;
            Lookahead = lookahead;
            return this;
        }

        /// <summary>
        /// Sets the pattern if the conditional matches.
        /// </summary>
        /// <param name="yes">Pattern if the conditional matches.</param>
        /// <returns>Current conditional pattern.</returns>
        public ConditionalPattern WithYesPattern(Pattern yes)
        {
            YesPattern = yes;
            return this;
        }

        /// <summary>
        /// Sets the pattern if the conditional does not match.
        /// </summary>
        /// <param name="no">Pattern if the conditional does not match.</param>
        /// <returns>Current conditional pattern.</returns>
        public ConditionalPattern WithNoPattern(Pattern? no)
        {
            NoPattern = no;
            return this;
        }

        public override Pattern Copy()
        {
            if (Expression != null)
            {
                return new ConditionalPattern(Expression.Copy(), YesPattern.Copy(), NoPattern?.Copy(), Lookahead);
            }

            if (GroupNumber.HasValue)
            {
                return new ConditionalPattern(GroupNumber.Value, YesPattern.Copy(), NoPattern?.Copy());
            }

            return new ConditionalPattern(GroupName!, YesPattern.Copy(), NoPattern?.Copy());
        }

        private protected override void GroupContents(PatternBuildState state)
        {
            state.WithPattern(this, Build);

            void Build(IPatternStringBuilder builder)
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

                    Expression.Build(state);
                }
                else if (GroupName != null)
                {
                    builder.Append(GroupName);
                }
                else if (GroupNumber != null)
                {
                    builder.Append(GroupNumber.Value);
                }

                builder.Append(')');

                Wrap(YesPattern);

                if (NoPattern != null)
                {
                    builder.Append('|');
                    Wrap(NoPattern);
                }

                void Wrap(Pattern pattern)
                {
                    if (ContainsUnwrappedOrPattern(pattern))
                    {
                        pattern.Wrap(state);
                    }
                    else
                    {
                        pattern.Build(state);
                    }
                }
            }
        }

        /// <summary>
        /// Sets the child pattern, ensuring that <see cref="Expression"/>, <see cref="YesPattern"/>, <see cref="NoPattern"/> are stored in the correct order.
        /// </summary>
        /// <param name="value">Value.</param>
        /// <param name="pattern">Pattern to change to <paramref name="value"/>.</param>
        /// <param name="index">Pattern index.</param>
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
