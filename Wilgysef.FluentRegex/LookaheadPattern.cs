using System.Text;

namespace Wilgysef.FluentRegex
{
    internal class LookaheadPattern : AbstractGroupPattern
    {
        public static Pattern PositiveLookahead(Pattern pattern) => new LookaheadPattern(pattern, LookaheadType.PositiveLookahead);

        public static Pattern NegativeLookahead(Pattern pattern) => new LookaheadPattern(pattern, LookaheadType.NegativeLookahead);

        public static Pattern PositiveLookbehind(Pattern pattern) => new LookaheadPattern(pattern, LookaheadType.PositiveLookbehind);

        public static Pattern NegativeLookbehind(Pattern pattern) => new LookaheadPattern(pattern, LookaheadType.NegativeLookbehind);

        private readonly LookaheadType _type;

        private LookaheadPattern(Pattern pattern, LookaheadType type) : base(pattern)
        {
            _type = type;
        }

        protected override void GroupContents(StringBuilder builder)
        {
            builder.Append('?');

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

            _pattern!.ToString(builder);
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
