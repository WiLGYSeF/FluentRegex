using Wilgysef.FluentRegex.PatternBuilders;

namespace Wilgysef.FluentRegex
{
    public class RawPattern : Pattern
    {
        /// <summary>
        /// Regex pattern.
        /// </summary>
        public string Regex { get; set; }

        internal override bool IsSinglePattern => true;

        internal override bool IsEmpty => Regex.Length == 0;

        /// <summary>
        /// Creates a raw regex pattern.
        /// </summary>
        /// <param name="regex">Regex.</param>
        public RawPattern(string regex)
        {
            Regex = regex;
        }

        /// <summary>
        /// Sets the regex to match.
        /// </summary>
        /// <param name="regex">Regex to match.</param>
        /// <returns>Current raw pattern.</returns>
        public RawPattern WithRegex(string regex)
        {
            Regex = regex;
            return this;
        }

        public override Pattern Copy()
        {
            return new RawPattern(Regex);
        }

        internal override void Build(PatternBuildState state)
        {
            state.WithPattern(this, Build);

            void Build(IPatternStringBuilder builder)
            {
                builder.Append(Regex);
            }
        }

        internal override Pattern Unwrap()
        {
            return this;
        }
    }
}
