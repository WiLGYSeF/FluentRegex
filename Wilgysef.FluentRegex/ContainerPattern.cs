using System;
using System.Collections.Generic;

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
            var traversed = new HashSet<Pattern>();
            Pattern current = this;

            while (true)
            {
                if (!traversed.Add(current))
                {
                    throw new InvalidOperationException("Pattern is infinitely recursive.");
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
