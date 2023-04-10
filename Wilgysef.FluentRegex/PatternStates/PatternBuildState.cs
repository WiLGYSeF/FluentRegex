using System;
using Wilgysef.FluentRegex.PatternStringBuilders;

namespace Wilgysef.FluentRegex.PatternStates
{
    internal class PatternBuildState
    {
        private readonly PatternStringBuilder _stringBuilder = new PatternStringBuilder();
        private readonly PatternStringBuilderReplicator _replicator;

        private readonly PatternTraverseState<string> _stringResult;
        private readonly PatternTraverseState<Pattern> _unwrapState;
        private readonly PatternTraverseState<bool> _emptyState;
        private readonly PatternTraverseState<bool> _singlePatternState;

        public PatternBuildState()
        {
            _replicator = new PatternStringBuilderReplicator(_stringBuilder);
            _stringResult = new PatternTraverseState<string>(this);
            _unwrapState = new PatternTraverseState<Pattern>(this);
            _emptyState = new PatternTraverseState<bool>(this);
            _singlePatternState = new PatternTraverseState<bool>(this);
        }

        public void WithPattern(Pattern pattern, Action<IPatternStringBuilder> action)
        {
            _stringResult.Compute(pattern, Build, Append);

            string Build(PatternBuildState state)
            {
                var patternStringBuilder = new PatternStringBuilder();
                _replicator.Add(patternStringBuilder);

                action(_replicator);

                _replicator.Remove(patternStringBuilder);
                return patternStringBuilder.ToString();
            }

            void Append(string result)
            {
                _replicator.Append(result);
            }
        }

        public void WithBuilder(Action<IPatternStringBuilder> action)
        {
            action(_replicator);
        }

        public Pattern Unwrap(Pattern pattern)
        {
            return _unwrapState.Compute(pattern, pattern.UnwrapInternal, null);
        }

        public bool IsEmpty(Pattern pattern)
        {
            return _emptyState.Compute(pattern, pattern.IsEmpty, null);
        }

        public bool IsSinglePattern(Pattern pattern)
        {
            return _singlePatternState.Compute(pattern, pattern.IsSinglePattern, null);
        }

        public override string ToString()
        {
            return _stringBuilder.ToString();
        }
    }
}
