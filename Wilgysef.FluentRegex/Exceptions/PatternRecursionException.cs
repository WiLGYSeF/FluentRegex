using System;
using System.Collections.Generic;
using System.Text;

namespace Wilgysef.FluentRegex.Exceptions
{
    public class PatternRecursionException : Exception
    {
        public IEnumerable<Pattern> Path { get; }

        public Pattern RecursivePattern { get; }

        public PatternRecursionException(IEnumerable<Pattern> path, Pattern recursivePattern)
            : base("Pattern is infinitely recursive")
        {
            Path = path;
            RecursivePattern = recursivePattern;
        }

        /// <summary>
        /// Gets the pattern path string where the recursion occurred.
        /// </summary>
        /// <returns>Pattern path string.</returns>
        public string GetPatternPath()
        {
            var builder = new StringBuilder();

            using var enumerator = Path.GetEnumerator();

            if (enumerator.MoveNext())
            {
                Append(enumerator.Current);
            }

            while (enumerator.MoveNext())
            {
                builder.Append(" -> ");
                Append(enumerator.Current);
            }

            return builder.ToString();

            void Append(Pattern pattern)
            {
                if (pattern == RecursivePattern)
                {
                    builder.Append('*');
                }

                builder.Append(pattern.GetType().Name);
            }
        }
    }
}
