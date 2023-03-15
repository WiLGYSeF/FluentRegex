using System.Collections.Generic;

namespace Wilgysef.FluentRegex
{
    public abstract class ContainerPattern : Pattern
    {
        internal override bool IsSinglePattern => _children.Count == 0
            || (_children.Count == 1 && _children[0].IsSinglePattern);

        protected readonly List<Pattern> _children = new List<Pattern>();

        protected ContainerPattern() { }

        protected ContainerPattern(IEnumerable<Pattern> patterns)
        {
            _children.AddRange(patterns);
        }
    }
}
