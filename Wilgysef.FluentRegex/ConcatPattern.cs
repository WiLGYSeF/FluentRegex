using System.Collections.Generic;
using System.Text;

namespace Wilgysef.FluentRegex
{
    public class ConcatPattern : ContainerPattern
    {
        public ConcatPattern(params Pattern[] patterns) : base(patterns) { }

        public ConcatPattern(IEnumerable<Pattern> patterns) : base(patterns) { }

        public ConcatPattern Concat(Pattern pattern)
        {
            _children.Add(pattern);
            return this;
        }

        internal override void ToString(StringBuilder builder)
        {
            foreach (var child in _children)
            {
                if (child is OrPattern orPattern
                    && !orPattern.IsSinglePattern
                    && _children.Count > 1)
                {
                    orPattern.Wrap(builder, always: true);
                }
                else
                {
                    child.ToString(builder);
                }
            }
        }
    }
}
