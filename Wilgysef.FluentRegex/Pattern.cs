using System;
using System.Collections.Generic;
using System.Linq;
using System.Text.RegularExpressions;
using Wilgysef.FluentRegex.Composites;
using Wilgysef.FluentRegex.Exceptions;
using Wilgysef.FluentRegex.PatternStates;

namespace Wilgysef.FluentRegex
{
    public abstract class Pattern
    {
        /// <summary>
        /// Indicates if the pattern can be treated as a single unit.
        /// </summary>
        internal abstract bool IsSinglePattern { get; }

        /// <summary>
        /// Creates a copy of the pattern object.
        /// </summary>
        /// <returns>Copied pattern.</returns>
        public abstract Pattern Copy();

        /// <summary>
        /// Build pattern.
        /// </summary>
        /// <param name="state">Pattern build state.</param>
        internal abstract void Build(PatternBuildState state);

        /// <summary>
        /// Unwraps the pattern, if possible.
        /// Gets the inner pattern if the parent pattern is transparent.
        /// </summary>
        /// <param name="state">Pattern build state.</param>
        /// <returns>Pattern.</returns>
        internal abstract Pattern UnwrapInternal(PatternBuildState state);

        /// <summary>
        /// Indicates if the pattern does not have any contents.
        /// </summary>
        internal abstract bool IsEmpty(PatternBuildState state);

        /// <summary>
        /// Compiles the pattern into a regular expression.
        /// </summary>
        /// <param name="options">Regex options.</param>
        /// <returns>Regular expression.</returns>
        public Regex Compile(RegexOptions? options = null)
        {
            return new Regex(ToString(), options ?? RegexOptions.Compiled);
        }

        /// <summary>
        /// Compiles the pattern into a regular expression.
        /// </summary>
        /// <param name="options">Regex options.</param>
        /// <param name="matchTimeout">How long a pattern match should attemp before tkming out.</param>
        /// <returns></returns>
        public Regex Compile(RegexOptions options, TimeSpan matchTimeout)
        {
            return new Regex(ToString(), options, matchTimeout);
        }

        /// <summary>
        /// Unwraps the pattern, if possible.
        /// Gets the inner pattern if the parent pattern is transparent.
        /// </summary>
        /// <returns>Pattern.</returns>
        public Pattern Unwrap()
        {
            return UnwrapInternal(new PatternBuildState());
        }

        public override string ToString()
        {
            var state = new PatternBuildState();
            Build(state);
            return state.ToString();
        }

        /// <summary>
        /// Creates a pattern that matches numbers in the range between <paramref name="min"/> and <paramref name="max"/>.
        /// </summary>
        /// <param name="min">Minimum number to match, inclusive.</param>
        /// <param name="max">Maximum number to match, inclusive.</param>
        /// <param name="leadingZeros">Whether to match leading zeros.</param>
        /// <returns>Pattern that matches numeric range.</returns>
        public static Pattern NumericRange(int min, int max, LeadingZeros leadingZeros = LeadingZeros.None)
            => NumericRangePattern.NumericRange(min, max, leadingZeros);

        /// <summary>
        /// Creates a pattern that matches numbers in the range between <paramref name="min"/> and <paramref name="max"/>.
        /// </summary>
        /// <param name="min">Minimum number to match, inclusive.</param>
        /// <param name="max">Maximum number to match, inclusive.</param>
        /// <param name="leadingZeros">Whether to match leading zeros.</param>
        /// <returns>Pattern that matches numeric range.</returns>
        public static Pattern NumericRange(long min, long max, LeadingZeros leadingZeros = LeadingZeros.None)
            => NumericRangePattern.NumericRange(min, max, leadingZeros);

        /// <summary>
        /// Wraps the pattern if necessary.
        /// </summary>
        /// <param name="state">Pattern build state.</param>
        /// <param name="always">Indicates if wrapping should always be done.</param>
        internal void Wrap(PatternBuildState state, bool always = false)
        {
            if (always || !IsSinglePattern)
            {
                GroupPattern.NonCapturingGroup(state, this);
            }
            else
            {
                Build(state);
            }
        }

        internal static bool ContainsUnwrappedOrPattern(Pattern pattern)
        {
            foreach (var current in Traverse(new[] { pattern }))
            {
                if (current is AbstractGroupPattern
                    || current is ConcatPattern
                    || current is QuantifierPattern)
                {
                    return false;
                }
                else if (current is OrPattern orPattern)
                {
                    return !orPattern.IsSinglePattern;
                }
            }

            return false;
        }

        /// <summary>
        /// Traverses the pattern in depth order.
        /// </summary>
        /// <param name="patterns">Patterns.</param>
        /// <returns>Traversed patterns.</returns>
        internal static IEnumerable<Pattern> Traverse(IReadOnlyList<Pattern> patterns)
        {
            var stack = new Stack<TraverseItem>();
            var traversed = new HashSet<Pattern>();

            stack.Push(new TraverseItem(null, patterns.GetEnumerator()));

            while (stack.Count > 0)
            {
                var current = stack.Peek();
                var skipPop = false;

                while (current.Enumerator.MoveNext())
                {
                    yield return current.Enumerator.Current;

                    if (current.Enumerator.Current is ContainerPattern container
                        && container.Children.Count > 0)
                    {
                        if (!traversed.Add(container))
                        {
                            throw new PatternRecursionException(stack.Where(i => i.Pattern != null).Select(i => i.Pattern!), container);
                        }

                        stack.Push(new TraverseItem(container, container.Children.GetEnumerator()));
                        skipPop = true;
                        break;
                    }
                }

                if (!skipPop)
                {
                    stack.Pop();

                    if (current.Pattern != null)
                    {
                        traversed.Remove(current.Pattern);
                    }
                }
            }
        }

        private class TraverseItem
        {
            public Pattern? Pattern { get; }

            public IEnumerator<Pattern> Enumerator { get; }

            public TraverseItem(Pattern? pattern, IEnumerator<Pattern> enumerator)
            {
                Pattern = pattern;
                Enumerator = enumerator;
            }
        }
    }

    internal static class PatternExtensions
    {
        public static bool IsNullOrEmpty(this Pattern? pattern, PatternBuildState state)
        {
            return pattern == null || state.IsEmpty(pattern);
        }
    }
}
