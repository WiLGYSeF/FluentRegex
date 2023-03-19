using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Text.RegularExpressions;

namespace Wilgysef.FluentRegex
{
    public abstract class Pattern
    {
        /// <summary>
        /// Copies the pattern object.
        /// </summary>
        /// <returns>Copied pattern.</returns>
        public abstract Pattern Copy();

        /// <summary>
        /// Indicates if the pattern can be treated as a single unit.
        /// </summary>
        internal abstract bool IsSinglePattern { get; }

        /// <summary>
        /// Build pattern.
        /// </summary>
        /// <param name="state">Pattern build state.</param>
        internal abstract void Build(PatternBuildState state);

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

        public override string ToString()
        {
            var state = new PatternBuildState();
            Build(state);
            return state.ToString();
        }

        /// <summary>
        /// Wraps the pattern if necessary.
        /// </summary>
        /// <param name="builder">String builder.</param>
        /// <param name="always">Whether wrapping should always be done.</param>
        internal void Wrap(PatternBuildState state, bool always = false)
        {
            if (always || !IsSinglePattern)
            {
                GroupPattern.NonCaptureGroup(state, this);
            }
            else
            {
                Build(state);
            }
        }

        /// <summary>
        /// Traverses the pattern in depth order.
        /// </summary>
        /// <param name="patterns">Pattern.</param>
        /// <returns>Traversed patterns.</returns>
        internal static IEnumerable<Pattern> Traverse(Pattern pattern)
        {
            return Traverse(new[] { pattern });
        }

        /// <summary>
        /// Traverses the pattern in depth order.
        /// </summary>
        /// <param name="patterns">Patterns.</param>
        /// <returns>Traversed patterns.</returns>
        /// <exception cref="InvalidOperationException">Pattern is infinitely recursive.</exception>
        internal static IEnumerable<Pattern> Traverse(IReadOnlyList<Pattern> patterns)
        {
            var stack = new Stack<TraverseItem>();
            stack.Push(new TraverseItem(null, patterns.GetEnumerator()));

            while (stack.Count > 0)
            {
                var item = stack.Peek();
                var skipPop = false;

                while (item.Enumerator.MoveNext())
                {
                    yield return item.Enumerator.Current;

                    if (item.Enumerator.Current is ContainerPattern container)
                    {
                        if (container.Children.Count > 0)
                        {
                            if (stack.Any(i => i.Pattern == container))
                            {
                                var path = GetPatternPath(stack.Where(i => i.Pattern != null).Select(i => i.Pattern!), container);
                                throw new InvalidOperationException($"Pattern is infinitely recursive: {path}");
                            }

                            stack.Push(new TraverseItem(container, container.Children.GetEnumerator()));
                            skipPop = true;
                            break;
                        }
                    }
                }

                if (!skipPop)
                {
                    stack.Pop();
                }
            }
        }

        internal static string GetPatternPath(IEnumerable<Pattern> patterns, Pattern? search)
        {
            var builder = new StringBuilder();

            using var enumerator = patterns.GetEnumerator();

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
                if (pattern == search)
                {
                    builder.Append('*');
                }

                builder.Append(pattern.GetType().Name);
            }
        }

        private struct TraverseItem
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
}
