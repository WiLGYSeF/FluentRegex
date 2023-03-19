using System.Text;

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

        protected override void GroupContents(StringBuilder builder)
        {
            builder.Append("?>");
            Pattern?.ToString(builder);
        }
    }
}
