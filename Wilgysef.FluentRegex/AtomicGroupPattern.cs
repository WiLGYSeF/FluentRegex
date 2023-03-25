using System.Text;
using Wilgysef.FluentRegex.PatternBuilders;

namespace Wilgysef.FluentRegex
{
    public class AtomicGroupPattern : AbstractGroupPattern
    {
        protected override bool HasContents => true;

        /// <summary>
        /// Creates an atomic group with a pattern.
        /// </summary>
        /// <param name="pattern">Pattern.</param>
        public AtomicGroupPattern(Pattern? pattern) : base(pattern) { }

        /// <summary>
        /// Sets the atomic group pattern.
        /// </summary>
        /// <param name="pattern">Pattern.</param>
        /// <returns>Current atomic group.</returns>
        public AtomicGroupPattern WithPattern(Pattern? pattern)
        {
            Pattern = pattern;
            return this;
        }

        public override Pattern Copy()
        {
            return new AtomicGroupPattern(Pattern?.Copy());
        }

        private protected override void GroupContents(PatternBuildState state)
        {
            state.WithPattern(this, Build);

            void Build(IPatternStringBuilder builder)
            {
                builder.Append("?>");
                Pattern?.Build(state);
            }
        }
    }
}
