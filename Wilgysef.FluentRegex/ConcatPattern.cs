using System.Collections.Generic;
using System.Text;

namespace Wilgysef.FluentRegex
{
    internal class ConcatPattern : Pattern
    {
        internal override bool IsSinglePattern => _children.Count <= 1;

        private readonly List<Pattern> _children = new List<Pattern>();

        public ConcatPattern(IEnumerable<Pattern> patterns)
        {
            _children.AddRange(patterns);
        }

        internal override void ToString(StringBuilder builder)
        {
            foreach (var child in _children)
            {
                child.ToString(builder);
            }
        }
    }
}
