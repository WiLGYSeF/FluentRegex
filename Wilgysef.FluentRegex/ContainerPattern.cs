using System.Collections.Generic;
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

        private protected bool IsSinglePatternInternal(PatternBuildState state, bool ignoreEmptyChildren)
        {
            var childrenCount = _children.Count;

            if (ignoreEmptyChildren)
            {
                foreach (var child in _children)
                {
                    if (state.IsEmpty(child))
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

            return state.IsSinglePattern(_children[0]);
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
    }
}
