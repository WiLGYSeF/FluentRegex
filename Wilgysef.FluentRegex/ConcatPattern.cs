using System.Collections.Generic;
using System.Linq;
using Wilgysef.FluentRegex.PatternStringBuilders;
using Wilgysef.FluentRegex.PatternStates;

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
            // TODO: recursion check?
            return new ConcatPattern(_children.Select(c => c.Copy()));
        }

        internal override void Build(PatternBuildState state)
        {
            state.WithPattern(this, Build);

            void Build(IPatternStringBuilder builder)
            {
                var childrenCount = _children.Count;
                foreach (var child in _children)
                {
                    if (state.IsEmpty(child))
                    {
                        childrenCount--;
                    }
                }

                foreach (var child in _children)
                {
                    if (childrenCount > 1 && ContainsUnwrappedOrPattern(state, child))
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

        internal override Pattern UnwrapInternal(PatternBuildState state)
        {
            var nonEmptyIndex = GetSingleNonEmptyChildIndex(state);
            return nonEmptyIndex != -1
                ? state.Unwrap(_children[nonEmptyIndex])
                : this;
        }

        internal override bool IsEmpty(PatternBuildState state)
        {
            return AreAllChildrenEmpty(state);
        }

        internal override bool IsSinglePattern(PatternBuildState state)
        {
            return IsSinglePatternInternal(state, true);
        }
    }
}
