using System;

namespace Wilgysef.FluentRegex.Exceptions
{
    public class InvalidPatternException : Exception
    {
        public Pattern Pattern { get; }

        public InvalidPatternException(Pattern pattern, string message) : base(message)
        {
            Pattern = pattern;
        }
    }
}
