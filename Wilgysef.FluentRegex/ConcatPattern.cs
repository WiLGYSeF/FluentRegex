using System.Collections.Generic;
using System.Linq;
using System.Text;
using Wilgysef.FluentRegex.PatternBuilders;

namespace Wilgysef.FluentRegex
{
    public class ConcatPattern : ContainerPattern
    {
        /// <summary>
        /// Concatenates patterns.
        /// </summary>
        /// <param name="patterns">Patterns.</param>
        public ConcatPattern(params Pattern[] patterns) : base(patterns) { }

        /// <summary>
        /// Concatenates patterns.
        /// </summary>
        /// <param name="patterns">Patterns.</param>
        public ConcatPattern(IEnumerable<Pattern> patterns) : base(patterns) { }

        /// <summary>
        /// Adds a pattern to concatenate.
        /// </summary>
        /// <param name="pattern">Pattern.</param>
        /// <returns>Current concatenation object.</returns>
        public ConcatPattern Concat(Pattern pattern)
        {
            _children.Add(pattern);
            return this;
        }

        public override Pattern Copy()
        {
            return new ConcatPattern(_children.Select(c => c.Copy()));
        }

        internal override void Build(PatternBuildState state)
        {
            state.WithPattern(this, Build);

            void Build(IPatternStringBuilder builder)
            {
                foreach (var child in _children)
                {
                    if (_children.Count > 1
                        && ContainsUnwrappedOrPattern(child))
                    {
                        child.Wrap(state, always: true);
                    }
                    else
                    {
                        child.Build(state);
                    }
                }
            }
        }
    }
}
