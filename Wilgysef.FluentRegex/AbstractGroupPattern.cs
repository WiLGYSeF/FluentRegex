using System.Text;

namespace Wilgysef.FluentRegex
{
    public abstract class AbstractGroupPattern : ContainerPattern
    {
        internal sealed override bool IsSinglePattern => true;

        protected abstract bool HasContents { get; }

        protected Pattern? Pattern
        {
            get => _children.Count != 0 ? _children[0] : null;
            set
            {
                if (value != null)
                {
                    _children.Insert(0, value);
                }
                else
                {
                    _children.Clear();
                }
            }
        }

        protected AbstractGroupPattern(Pattern? pattern)
        {
            Pattern = pattern;
        }

        protected abstract void GroupContents(StringBuilder builder);

        internal sealed override void ToString(StringBuilder builder)
        {
            if (!HasContents)
            {
                return;
            }

            builder.Append('(');
            GroupContents(builder);
            builder.Append(')');
        }
    }
}
