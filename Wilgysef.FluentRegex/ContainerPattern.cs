using System.Collections.Generic;
using System.Diagnostics.CodeAnalysis;
using Wilgysef.FluentRegex.Exceptions;

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

                            if (child.IsEmpty)
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

        protected bool IsEmptyInternal()
        {
            var path = new List<Pattern>();
            var stack = new Stack<(Pattern Pattern, int Depth)>();
            var depth = 0;

            stack.Push((this, 0));
            path.Add(this);

            while (stack.Count > 0)
            {
                var current = stack.Pop();

                for (; current.Depth <= depth; depth--)
                {
                    path.RemoveAt(path.Count - 1);
                }

                if (path.Contains(current.Pattern))
                {
                    throw new PatternRecursionException(path, current.Pattern);
                }

                path.Add(current.Pattern);
                depth++;

                if (IsContainerPattern(current.Pattern, out var container))
                {
                    for (var i = container.Children.Count - 1; i >= 0; i--)
                    {
                        stack.Push((container.Children[i], current.Depth + 1));
                    }
                }
                else if (!current.Pattern.IsEmpty)
                {
                    return false;
                }
            }

            return true;
        }

        /// <summary>
        /// Gets the index of the single non-empty child.
        /// </summary>
        /// <returns>Index of single non-empty child, or <c>-1</c> if there are none or more than one.</returns>
        protected int GetSingleNonEmptyChildIndex()
        {
            var nonEmptyIndex = -1;

            for (var i = 0; i < _children.Count; i++)
            {
                if (!_children[i].IsEmpty)
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
