using System.Text;

namespace Wilgysef.FluentRegex
{
    internal class LookaheadPattern : Pattern
    {
        public static Pattern PositiveLookahead(Pattern pattern) => new LookaheadPattern(pattern, LookaheadType.PositiveLookahead);

        public static Pattern NegativeLookahead(Pattern pattern) => new LookaheadPattern(pattern, LookaheadType.NegativeLookahead);

        public static Pattern PositiveLookbehind(Pattern pattern) => new LookaheadPattern(pattern, LookaheadType.PositiveLookbehind);

        public static Pattern NegativeLookbehind(Pattern pattern) => new LookaheadPattern(pattern, LookaheadType.NegativeLookbehind);

        internal override bool IsSinglePattern => true;

        private readonly Pattern _pattern;
        private readonly LookaheadType _type;

        private LookaheadPattern(Pattern pattern, LookaheadType type)
        {
            _pattern = pattern;
            _type = type;
        }

        internal override void ToString(StringBuilder builder)
        {
            builder.Append("(?");

            switch (_type)
            {
                case LookaheadType.PositiveLookahead:
                    builder.Append('=');
                    break;
                case LookaheadType.NegativeLookahead:
                    builder.Append('!');
                    break;
                case LookaheadType.PositiveLookbehind:
                    builder.Append("<=");
                    break;
                case LookaheadType.NegativeLookbehind:
                    builder.Append("<!");
                    break;
            }

            _pattern.ToString(builder);

            builder.Append(')');
        }

        private enum LookaheadType
        {
            PositiveLookahead,
            NegativeLookahead,
            PositiveLookbehind,
            NegativeLookbehind,
        }
    }
}
