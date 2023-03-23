using System.Collections.Generic;
using Wilgysef.FluentRegex.Exceptions;

namespace Wilgysef.FluentRegex
{
    public abstract class ContainerPattern : Pattern
    {
        /// <summary>
        /// Container pattern children.
        /// </summary>
        public IReadOnlyList<Pattern> Children => _children;

        internal override bool IsSinglePattern => IsSinglePatternInternal();

        protected readonly List<Pattern> _children = new List<Pattern>();

        protected ContainerPattern() { }

        protected ContainerPattern(IEnumerable<Pattern> patterns)
        {
            _children.AddRange(patterns);
        }

        private bool IsSinglePatternInternal()
        {
            var path = new List<Pattern>();
            var traversed = new HashSet<Pattern>();
            Pattern current = this;

            while (true)
            {
                // just in case the singular child is recursive
                path.Add(current);
                if (!traversed.Add(current))
                {
                    throw new PatternRecursionException(path, current);
                }

                if (current is ContainerPattern container)
                {
                    if (container._children.Count == 0)
                    {
                        return true;
                    }

                    if (container._children.Count > 1)
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
    }
}
