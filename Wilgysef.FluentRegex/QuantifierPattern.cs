using System.Text;
using Wilgysef.FluentRegex.Exceptions;

namespace Wilgysef.FluentRegex
{
    public class QuantifierPattern : ContainerPattern
    {
        /// <summary>
        /// Pattern to match.
        /// </summary>
        public Pattern Pattern
        {
            get => _children[0];
            set
            {
                if (value is AnchorPattern)
                {
                    throw new InvalidPatternException(value, "Cannot quantify an anchor.");
                }

                if (_children.Count == 0)
                {
                    _children.Add(value);
                }
                else
                {
                    _children[0] = value;
                }
            }
        }

        /// <summary>
        /// Minimum number of occurrences to match.
        /// </summary>
        public int Min { get; set; }

        /// <summary>
        /// Maximum number of occurrences to match.
        /// </summary>
        public int? Max { get; set; }

        /// <summary>
        /// Indicates if the match is greedy.
        /// </summary>
        public bool Greedy { get; set; }

        internal override bool IsSinglePattern => false;

        /// <summary>
        /// Creates a quantifier pattern.
        /// </summary>
        /// <param name="pattern">Pattern to match.</param>
        /// <param name="min">Minimum number of occurrences to match.</param>
        /// <param name="max">Maximum number of occurrences to match.</param>
        /// <param name="greedy">Indicates if the match is greedy.</param>
        public QuantifierPattern(Pattern pattern, int min, int? max, bool greedy)
        {
            Pattern = pattern;
            Min = min;
            Max = max;
            Greedy = greedy;
        }

        /// <summary>
        /// Sets the quantifier to match zero or one occurrences.
        /// </summary>
        /// <param name="greedy">Indicates if the match is greedy.</param>
        /// <returns>Current quantifier pattern.</returns>
        public QuantifierPattern WithZeroOrOne(bool greedy = true) => Set(0, 1, greedy);

        /// <summary>
        /// Sets the quantifier to match zero or more occurrences.
        /// </summary>
        /// <param name="greedy">Indicates if the match is greedy.</param>
        /// <returns>Current quantifier pattern.</returns>
        public QuantifierPattern WithZeroOrMore(bool greedy = true) => Set(0, null, greedy);

        /// <summary>
        /// Sets the quantifier to match one or more occurrences.
        /// </summary>
        /// <param name="greedy">Indicates if the match is greedy.</param>
        /// <returns>Current quantifier pattern.</returns>
        public QuantifierPattern WithOneOrMore(bool greedy = true) => Set(1, null, greedy);

        /// <summary>
        /// Sets the quantifier to match exactly <paramref name="number"/> occurrences.
        /// </summary>
        /// <param name="number">Number of occurrences to match.</param>
        /// <returns>Current quantifier pattern.</returns>
        public QuantifierPattern WithExactly(int number) => Set(number, number, true);

        /// <summary>
        /// Sets the quantifier to match between <paramref name="min"/> and <paramref name="max"/> occurrences.
        /// </summary>
        /// <param name="min">Minimum number of occurrences to match.</param>
        /// <param name="max">Maximum number of occurrences to match.</param>
        /// <param name="greedy">Indicates if the match is greedy.</param>
        /// <returns>Current quantifier pattern.</returns>
        public QuantifierPattern WithBetween(int min, int max, bool greedy = true) => Set(min, max, greedy);

        /// <summary>
        /// Sets the quantifier to match at least <paramref name="min"/> occurrences.
        /// </summary>
        /// <param name="min">Minimum number of occurrences to match.</param>
        /// <param name="greedy">Indicates if the match is greedy.</param>
        /// <returns>Current quantifier pattern.</returns>
        public QuantifierPattern WithAtLeast(int min, bool greedy = true) => Set(min, null, greedy);

        /// <summary>
        /// Sets the quantifier to match at most <paramref name="max"/> occurrences.
        /// </summary>
        /// <param name="max">Maximum number of occurrences to match.</param>
        /// <param name="greedy">Indicates if the match is greedy.</param>
        /// <returns>Current quantifier pattern.</returns>
        public QuantifierPattern WithAtMost(int max, bool greedy = true) => Set(0, max, greedy);

        /// <summary>
        /// Sets if the quantifier is greedy.
        /// </summary>
        /// <param name="greedy">Indicates if the match is greedy.</param>
        /// <returns>Current quantifier pattern.</returns>
        public QuantifierPattern GreedyCapture(bool greedy = true)
        {
            Greedy = greedy;
            return this;
        }

        /// <summary>
        /// Sets the pattern to match.
        /// </summary>
        /// <param name="pattern">Pattern to match.</param>
        /// <returns>Current quantifier pattern.</returns>
        public QuantifierPattern WithPattern(Pattern pattern)
        {
            Pattern = pattern;
            return this;
        }

        public override Pattern Copy()
        {
            return new QuantifierPattern(Pattern.Copy(), Min, Max, Greedy);
        }

        internal override void Build(PatternBuildState state)
        {
            if (Min < 0 || Max.HasValue && Max.Value < 0)
            {
                throw new InvalidPatternException(this, "Range cannot be negative.");
            }

            if (Max.HasValue)
            {
                if (Min > Max.Value)
                {
                    throw new InvalidPatternException(this, "Min cannot be greater than max.");
                }

                if (Min == 0 && Max.Value == 0)
                {
                    return;
                }
            }

            state.WithPattern(this, Build);

            void Build(StringBuilder builder)
            {
                var startLength = builder.Length;

                Pattern.Wrap(state);

                if (builder.Length == startLength)
                {
                    throw new InvalidPatternException(this, "Quantified pattern cannot be empty.");
                }

                if (Min == 1 && Max.HasValue && Max.Value == 1)
                {
                    return;
                }

                if (Min == 0)
                {
                    if (Max.HasValue)
                    {
                        if (Max.Value == 1)
                        {
                            builder.Append('?');
                        }
                        else
                        {
                            builder.Append("{0,");
                            builder.Append(Max.Value);
                            builder.Append('}');
                        }
                    }
                    else
                    {
                        builder.Append('*');
                    }
                }
                else if (Min == 1 && !Max.HasValue)
                {
                    builder.Append('+');
                }
                else
                {
                    builder.Append('{');
                    builder.Append(Min);

                    if (!Max.HasValue || Min != Max.Value)
                    {
                        builder.Append(',');

                        if (Max.HasValue)
                        {
                            builder.Append(Max.Value);
                        }
                    }

                    builder.Append('}');
                }

                if (!Greedy)
                {
                    builder.Append('?');
                }
            }
        }

        private QuantifierPattern Set(int min, int? max, bool greedy)
        {
            Min = min;
            Max = max;
            Greedy = greedy;
            return this;
        }
    }
}
