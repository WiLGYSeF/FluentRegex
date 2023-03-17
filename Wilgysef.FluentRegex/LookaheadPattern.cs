using System;
using System.Text;

namespace Wilgysef.FluentRegex
{
    public class LookaheadPattern : AbstractGroupPattern
    {
        public static LookaheadPattern PositiveLookahead(Pattern? pattern) => new LookaheadPattern(pattern, LookaheadType.PositiveLookahead);

        public static LookaheadPattern NegativeLookahead(Pattern? pattern) => new LookaheadPattern(pattern, LookaheadType.NegativeLookahead);

        public static LookaheadPattern PositiveLookbehind(Pattern? pattern) => new LookaheadPattern(pattern, LookaheadType.PositiveLookbehind);

        public static LookaheadPattern NegativeLookbehind(Pattern? pattern) => new LookaheadPattern(pattern, LookaheadType.NegativeLookbehind);

        protected override bool HasContents => true;

        private readonly LookaheadType _type;

        private LookaheadPattern(Pattern? pattern, LookaheadType type) : base(pattern)
        {
            _type = type;
        }

        public LookaheadPattern WithPattern(Pattern? pattern)
        {
            Pattern = pattern;
            return this;
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
                default:
                    throw new ArgumentOutOfRangeException();
            }

            Pattern?.ToString(builder);
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
