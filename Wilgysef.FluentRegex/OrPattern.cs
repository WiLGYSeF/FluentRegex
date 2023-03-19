using System.Collections.Generic;
using System.Linq;
using System.Text;

namespace Wilgysef.FluentRegex
{
    public class OrPattern : ContainerPattern
    {
        /// <summary>
        /// Creates an or pattern.
        /// </summary>
        /// <param name="patterns">Patterns to match.</param>
        public OrPattern(params Pattern[] patterns) : this((IEnumerable<Pattern>)patterns) { }

        /// <summary>
        /// Creates an or pattern.
        /// </summary>
        /// <param name="patterns">Patterns to match.</param>
        public OrPattern(IEnumerable<Pattern> patterns) : base(patterns) { }

        /// <summary>
        /// Adds a pattern to match.
        /// </summary>
        /// <param name="pattern">Pattern to match.</param>
        /// <returns>Current or pattern.</returns>
        public OrPattern Or(Pattern pattern)
        {
            _children.Add(pattern);
            return this;
        }

        public override Pattern Copy()
        {
            return new OrPattern(_children.Select(c => c.Copy()));
        }

        internal override void Build(PatternBuildState state)
        {
            state.WithPattern(this, Build);

            void Build(StringBuilder builder)
            {
                if (_children.Count <= 1)
                {
                    if (_children.Count == 1)
                    {
                        _children[0].Build(state);
                    }

                    return;
                }

                _children[0].Build(state);

                for (var i = 1; i < _children.Count; i++)
                {
                    builder.Append('|');
                    _children[i].Build(state);
                }
            }
        }
    }
}
