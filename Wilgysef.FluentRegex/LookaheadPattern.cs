using System;
using Wilgysef.FluentRegex.Enums;
using Wilgysef.FluentRegex.PatternStringBuilders;
using Wilgysef.FluentRegex.PatternStates;

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

        /// <summary>
        /// Lookahead type.
        /// </summary>
        public LookaheadType Type { get; }

        private LookaheadPattern(Pattern? pattern, LookaheadType type) : base(pattern)
        {
            Type = type;
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

        internal override Pattern CopyInternal(PatternBuildState state)
        {
            return new LookaheadPattern(state.Copy(Pattern), Type);
        }

        internal override Pattern UnwrapInternal(PatternBuildState state)
        {
            return this;
        }

        internal override bool IsEmpty(PatternBuildState state)
        {
            return Pattern.IsNullOrEmpty(state);
        }

        private protected override void GroupContents(PatternBuildState state)
        {
            state.WithPattern(this, Build);

            void Build(IPatternStringBuilder builder)
            {
                builder.Append('?');

                switch (Type)
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

                Pattern?.Build(state);
            }
        }
    }
}
