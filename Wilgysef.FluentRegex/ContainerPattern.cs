using System.Collections.Generic;

namespace Wilgysef.FluentRegex
{
    internal abstract class ContainerPattern : Pattern
    {
        internal override bool IsSinglePattern
        {
            get
            {
                if (_children.Count == 0)
                {
                    return true;
                }
                else if (_children.Count == 1)
                {
                    return _children[0].IsSinglePattern;
                }
                else
                {
                    return false;
                }
            }
        }

        protected readonly List<Pattern> _children = new List<Pattern>();

        protected ContainerPattern(IEnumerable<Pattern> patterns)
        {
            _children.AddRange(patterns);
        }
    }
}
