using System;
using System.Text;

namespace Wilgysef.FluentRegex
{
    public class QuantifierPattern : Pattern
    {
        public Pattern Pattern { get; set; }

        public int Min { get; set; }

        public int? Max { get; set; }

        public bool Greedy { get; set; }

        internal override bool IsSinglePattern => false;

        public QuantifierPattern(Pattern pattern, int min, int? max, bool greedy)
        {
            Pattern = pattern;
            Min = min;
            Max = max;
            Greedy = greedy;
        }

        public QuantifierPattern WithZeroOrOne(bool greedy = true) => Set(0, 1, greedy);

        public QuantifierPattern WithZeroOrMore(bool greedy = true) => Set(0, null, greedy);

        public QuantifierPattern WithOneOrMore(bool greedy = true) => Set(1, null, greedy);

        public QuantifierPattern WithExactly(int number) => Set(number, number, true);

        public QuantifierPattern WithBetween(int min, int max, bool greedy = true) => Set(min, max, greedy);

        public QuantifierPattern WithAtLeast(int min, bool greedy = true) => Set(min, null, greedy);

        public QuantifierPattern WithAtMost(int max, bool greedy = true) => Set(0, max, greedy);

        public QuantifierPattern GreedyCapture(bool greedy = true)
        {
            Greedy = greedy;
            return this;
        }

        internal override void ToString(StringBuilder builder)
        {
            if (Min < 0 || Max.HasValue && Max.Value < 0)
            {
                throw new InvalidOperationException("Range cannot be negative.");
            }

            if (Max.HasValue)
            {
                if (Min > Max.Value)
                {
                    throw new InvalidOperationException("Min cannot be greater than max.");
                }

                if (Min == 0 && Max.Value == 0)
                {
                    return;
                }
            }

            Pattern.Wrap(builder);

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

        private QuantifierPattern Set(int min, int? max, bool greedy)
        {
            Min = min;
            Max = max;
            Greedy = greedy;
            return this;
        }
    }
}
