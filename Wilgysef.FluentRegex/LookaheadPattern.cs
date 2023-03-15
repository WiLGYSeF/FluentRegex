using System;
using System.Text;

namespace Wilgysef.FluentRegex
{
    internal class LookaheadPattern : AbstractGroupPattern
    {
        internal static Pattern PositiveLookahead(Pattern pattern) => new LookaheadPattern(pattern, LookaheadType.PositiveLookahead);

        internal static Pattern NegativeLookahead(Pattern pattern) => new LookaheadPattern(pattern, LookaheadType.NegativeLookahead);

        internal static Pattern PositiveLookbehind(Pattern pattern) => new LookaheadPattern(pattern, LookaheadType.PositiveLookbehind);

        internal static Pattern NegativeLookbehind(Pattern pattern) => new LookaheadPattern(pattern, LookaheadType.NegativeLookbehind);

        protected override bool HasContents => true;

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
                default:
                    throw new ArgumentOutOfRangeException();
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
