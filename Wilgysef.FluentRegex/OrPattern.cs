using System.Collections.Generic;
using System.Text;

namespace Wilgysef.FluentRegex
{
    internal class OrPattern : Pattern
    {
        internal override bool IsSinglePattern => _children.Count <= 1;

        private readonly List<Pattern> _children = new List<Pattern>();

        public OrPattern(IEnumerable<Pattern> patterns)
        {
            _children.AddRange(patterns);
        }

        internal override void ToString(StringBuilder builder)
        {
            if (_children.Count <= 1)
            {
                if (_children.Count == 1)
                {
                    _children[0].ToString(builder);
                }

                return;
            }

            _children[0].Wrap(builder);

            for (var i = 1; i < _children.Count; i++)
            {
                builder.Append('|');
                _children[i].Wrap(builder);
            }
        }
    }
}
