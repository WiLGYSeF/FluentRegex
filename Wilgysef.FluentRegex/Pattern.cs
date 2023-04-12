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
        /// Build pattern.
        /// </summary>
        /// <param name="state">Pattern build state.</param>
        internal abstract void Build(PatternBuildState state);

        /// <summary>
        /// Creates a copy of the pattern object.
        /// </summary>
        /// <param name="state">Pattern build state.</param>
        /// <returns>Copied pattern.</returns>
        internal abstract Pattern CopyInternal(PatternBuildState state);

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
        /// <param name="state">Pattern build state.</param>
        /// <returns><see langword="true"/> if the pattern is empty, otherwise <see langword="false"/>.</returns>
        internal abstract bool IsEmpty(PatternBuildState state);

        /// <summary>
        /// Indicates if the pattern can be treated as a single unit.
        /// </summary>
        /// <param name="state">Pattern build state.</param>
        /// <returns><see langword="true"/> if the pattern is single, otherwise <see langword="false"/>.</returns>
        internal abstract bool IsSinglePattern(PatternBuildState state);

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
        /// Creates a copy of the pattern object.
        /// </summary>
        /// <returns>Copied pattern.</returns>
        public Pattern Copy()
        {
            return CopyInternal(new PatternBuildState());
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
        /// Creates a pattern that matches numbers in the range between <paramref name="min"/> and <paramref name="max"/>.
        /// </summary>
        /// <param name="min">Minimum number to match, inclusive.</param>
        /// <param name="max">Maximum number to match, inclusive.</param>
        /// <param name="leadingZeros">Whether to match leading zeros.</param>
        /// <param name="minFractionalDigits">Minimum number of fractional digits to match.</param>
        /// <param name="maxFractionalDigits">Maximum number of fractional digits to match.</param>
        /// <param name="fractionalSeparator">Character that separates integer and fractional parts.</param>
        /// <returns>Pattern that matches numeric range.</returns>
        public static Pattern NumericRange(
            double min,
            double max,
            LeadingZeros leadingZeros = LeadingZeros.None,
            int minFractionalDigits = 0,
            int? maxFractionalDigits = null,
            char fractionalSeparator = '.')
            => NumericRangePattern.NumericRange(min, max, leadingZeros, minFractionalDigits, maxFractionalDigits, fractionalSeparator);

        /// <summary>
        /// Wraps the pattern if necessary.
        /// </summary>
        /// <param name="state">Pattern build state.</param>
        /// <param name="always">Indicates if wrapping should always be done.</param>
        internal void Wrap(PatternBuildState state, bool always = false)
        {
            if (always || !state.IsSinglePattern(this))
            {
                GroupPattern.NonCapturingGroup(state, this);
            }
            else
            {
                Build(state);
            }
        }

        internal static bool ContainsUnwrappedOrPattern(PatternBuildState state, Pattern pattern)
        {
            foreach (var current in Traverse(new[] { pattern }))
            {
                if (current is AbstractGroupPattern
                    || current is ConcatPattern
                    || current is QuantifierPattern)
                {
                    // patterns are wrapped or employ wrapping
                    return false;
                }
                else if (current is OrPattern orPattern)
                {
                    return !state.IsSinglePattern(orPattern);
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
