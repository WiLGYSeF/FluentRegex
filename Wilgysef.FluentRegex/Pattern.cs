using System;
using System.Collections.Generic;
using System.Text;
using System.Text.RegularExpressions;

namespace Wilgysef.FluentRegex
{
    public abstract class Pattern
    {
        private static readonly RegexOptions DefaultOptions = RegexOptions.Compiled;

        internal abstract bool IsSinglePattern { get; }

        internal abstract void ToString(StringBuilder builder);

        #region Static Pattern Builders

        public static Pattern Concat(params Pattern[] patterns)
        {
            return new ConcatPattern(patterns);
        }

        public static Pattern Concat(IEnumerable<Pattern> patterns)
        {
            return new ConcatPattern(patterns);
        }

        public static Pattern Or(params Pattern[] patterns)
        {
            return new OrPattern(patterns);
        }

        public static Pattern Or(IEnumerable<Pattern> patterns)
        {
            return new OrPattern(patterns);
        }

        public static Pattern Raw(string regex)
        {
            return new RawPattern(regex);
        }

        #endregion

        #region Quantifiers

        public Pattern ZeroOrOne(bool greedy = true) => new QuantifierPattern(this, 0, 1, greedy);

        public Pattern ZeroOrMore(bool greedy = true) => new QuantifierPattern(this, 0, null, greedy);

        public Pattern OneOrMore(bool greedy = true) => new QuantifierPattern(this, 1, null, greedy);

        public Pattern Exactly(int number) => new QuantifierPattern(this, number, number, true);

        public Pattern Between(int min, int max, bool greedy = true) => new QuantifierPattern(this, min, max, greedy);

        public Pattern AtLeast(int min, bool greedy = true) => new QuantifierPattern(this, min, null, greedy);

        public Pattern AtMost(int max, bool greedy = true) => new QuantifierPattern(this, 0, max, greedy);

        #endregion

        public Regex Compile()
        {
            return new Regex(ToString(), DefaultOptions);
        }

        public Regex Compile(RegexOptions options)
        {
            return new Regex(ToString(), options);
        }

        public Regex Compile(RegexOptions options, TimeSpan matchTimeout)
        {
            return new Regex(ToString(), options, matchTimeout);
        }

        public override string ToString()
        {
            var builder = new StringBuilder();
            ToString(builder);
            return builder.ToString();
        }

        internal void Wrap(StringBuilder builder, bool always = false)
        {
            if (always || !IsSinglePattern)
            {
                GroupPattern.NonCaptureGroup(builder, this);
            }
            else
            {
                ToString(builder);
            }
        }
    }
}
