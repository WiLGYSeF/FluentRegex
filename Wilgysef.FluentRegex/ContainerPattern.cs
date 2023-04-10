using System.Collections.Generic;
using System.Diagnostics.CodeAnalysis;
using Wilgysef.FluentRegex.Exceptions;
using Wilgysef.FluentRegex.PatternStates;

namespace Wilgysef.FluentRegex
{
    public abstract class ContainerPattern : Pattern
    {
        /// <summary>
        /// Container pattern children.
        /// </summary>
        public IReadOnlyList<Pattern> Children => _children;

        protected readonly List<Pattern> _children = new List<Pattern>();

        protected ContainerPattern() { }

        protected ContainerPattern(IEnumerable<Pattern> patterns)
        {
            _children.AddRange(patterns);
        }

        protected bool IsSinglePatternInternal(bool ignoreEmptyChildren)
        {
            var path = new List<Pattern>();
            var traversed = new HashSet<Pattern>();
            Pattern current = this;

            while (true)
            {
                path.Add(current);
                if (!traversed.Add(current))
                {
                    throw new PatternRecursionException(path, current);
                }

                if (IsContainerPattern(current, out var container))
                {
                    var childrenCount = container._children.Count;

                    if (ignoreEmptyChildren)
                    {
                        foreach (var child in container._children)
                        {
                            if (traversed.Contains(child))
                            {
                                path.Add(child);
                                throw new PatternRecursionException(path, child);
                            }

                            // TODO: fix
                            if (child.IsEmpty(new PatternBuildState()))
                            {
                                childrenCount--;
                            }
                        }
                    }

                    if (childrenCount == 0)
                    {
                        return true;
                    }

                    if (childrenCount > 1)
                    {
                        return false;
                    }

                    current = container._children[0];
                }
                else
                {
                    return current.IsSinglePattern;
                }
            }
        }

        /// <summary>
        /// Gets the index of the single non-empty child.
        /// </summary>
        /// <returns>Index of single non-empty child, or <c>-1</c> if there are none or more than one.</returns>
        private protected int GetSingleNonEmptyChildIndex(PatternBuildState state)
        {
            var nonEmptyIndex = -1;

            for (var i = 0; i < _children.Count; i++)
            {
                if (!state.IsEmpty(_children[i]))
                {
                    if (nonEmptyIndex != -1)
                    {
                        return -1;
                    }

                    nonEmptyIndex = i;
                }
            }

            return nonEmptyIndex;
        }

        private protected bool AreAllChildrenEmpty(PatternBuildState state)
        {
            foreach (var child in _children)
            {
                if (!state.IsEmpty(child))
                {
                    return false;
                }
            }

            return true;
        }

        private bool IsContainerPattern(
            Pattern pattern,
            [MaybeNullWhen(false)] out ContainerPattern container)
        {
            if (pattern is ContainerPattern c)
            {
                container = c;
                return true;
            }

            container = null;
            return false;
        }
    }
}
