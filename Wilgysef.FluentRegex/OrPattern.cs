using System.Collections.Generic;
using System.Linq;
using System.Text;

namespace Wilgysef.FluentRegex
{
    public class OrPattern : ContainerPattern
    {
        public OrPattern(params Pattern[] patterns) : this((IEnumerable<Pattern>)patterns) { }

        public OrPattern(IEnumerable<Pattern> patterns) : base(patterns) { }

        public OrPattern Or(Pattern pattern)
        {
            _children.Add(pattern);
            return this;
        }

        public override Pattern Copy()
        {
            return new OrPattern(_children.Select(c => c.Copy()));
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

            _children[0].ToString(builder);

            for (var i = 1; i < _children.Count; i++)
            {
                builder.Append('|');
                _children[i].ToString(builder);
            }
        }
    }
}
