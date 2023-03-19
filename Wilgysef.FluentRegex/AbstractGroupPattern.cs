using System.Text;

namespace Wilgysef.FluentRegex
{
    public abstract class AbstractGroupPattern : ContainerPattern
    {
        internal sealed override bool IsSinglePattern => true;

        /// <summary>
        /// Indicates if the group has contents.
        /// </summary>
        protected abstract bool HasContents { get; }

        /// <summary>
        /// Group pattern.
        /// </summary>
        protected Pattern? Pattern
        {
            get => _children.Count != 0 ? _children[0] : null;
            set
            {
                if (value != null)
                {
                    if (_children.Count == 0)
                    {
                        _children.Add(value);
                    }
                    else
                    {
                        _children[0] = value;
                    }
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

        /// <summary>
        /// Writes the group contents to the string builder.
        /// </summary>
        /// <param name="builder">String builder.</param>
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
