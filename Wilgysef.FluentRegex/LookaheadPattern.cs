﻿using System;
using System.Text;

namespace Wilgysef.FluentRegex
{
    public class LookaheadPattern : AbstractGroupPattern
    {
        /// <summary>
        /// Creates a positive lookahead pattern.
        /// </summary>
        /// <param name="pattern">Pattern to match.</param>
        /// <returns>Lookahead pattern.</returns>
        public static LookaheadPattern PositiveLookahead(Pattern? pattern) => new LookaheadPattern(pattern, LookaheadType.PositiveLookahead);

        /// <summary>
        /// Creates a negative lookahead pattern.
        /// </summary>
        /// <param name="pattern">Pattern to match.</param>
        /// <returns>Lookahead pattern.</returns>
        public static LookaheadPattern NegativeLookahead(Pattern? pattern) => new LookaheadPattern(pattern, LookaheadType.NegativeLookahead);

        /// <summary>
        /// Creates a positive lookbehind pattern.
        /// </summary>
        /// <param name="pattern">Pattern to match.</param>
        /// <returns>Lookahead pattern.</returns>
        public static LookaheadPattern PositiveLookbehind(Pattern? pattern) => new LookaheadPattern(pattern, LookaheadType.PositiveLookbehind);

        /// <summary>
        /// Creates a negative lookbehind pattern.
        /// </summary>
        /// <param name="pattern">Pattern to match.</param>
        /// <returns>Lookahead pattern.</returns>
        public static LookaheadPattern NegativeLookbehind(Pattern? pattern) => new LookaheadPattern(pattern, LookaheadType.NegativeLookbehind);

        protected override bool HasContents => true;

        private readonly LookaheadType _type;

        private LookaheadPattern(Pattern? pattern, LookaheadType type) : base(pattern)
        {
            _type = type;
        }

        /// <summary>
        /// Sets the pattern to match.
        /// </summary>
        /// <param name="pattern">Pattern to match.</param>
        /// <returns>Current lookahead pattern.</returns>
        public LookaheadPattern WithPattern(Pattern? pattern)
        {
            Pattern = pattern;
            return this;
        }

        public override Pattern Copy()
        {
            return new LookaheadPattern(Pattern?.Copy(), _type);
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
