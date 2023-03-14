using System.Collections.Generic;
using System.Text;

namespace Wilgysef.FluentRegex
{
    internal class OrPattern : ContainerPattern
    {
        public OrPattern(IEnumerable<Pattern> patterns) : base(patterns) { }

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
