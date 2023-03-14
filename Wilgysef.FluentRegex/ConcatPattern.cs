using System.Collections.Generic;
using System.Text;

namespace Wilgysef.FluentRegex
{
    internal class ConcatPattern : ContainerPattern
    {
        public ConcatPattern(IEnumerable<Pattern> patterns) : base(patterns) { }

        internal override void ToString(StringBuilder builder)
        {
            foreach (var child in _children)
            {
                child.ToString(builder);
            }
        }
    }
}
