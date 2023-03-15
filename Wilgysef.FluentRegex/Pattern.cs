using System;
using System.Text;
using System.Text.RegularExpressions;

namespace Wilgysef.FluentRegex
{
    public abstract class Pattern
    {
        private static readonly RegexOptions DefaultOptions = RegexOptions.Compiled;

        internal abstract bool IsSinglePattern { get; }

        internal abstract void ToString(StringBuilder builder);

        // TODO: unique group name validation
        // TODO: backreference validation

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
