using System.Text;

namespace Wilgysef.FluentRegex
{
    internal class QuantifierPattern : Pattern
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

        internal override void ToString(StringBuilder builder)
        {
            Pattern.Wrap(builder);

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
                builder.Append(',');

                if (Max.HasValue)
                {
                    builder.Append(Max.Value);
                }

                builder.Append('}');
            }

            if (!Greedy)
            {
                builder.Append('?');
            }
        }
    }
}
