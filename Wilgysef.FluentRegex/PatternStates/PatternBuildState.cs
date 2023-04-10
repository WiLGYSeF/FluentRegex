using System;
using Wilgysef.FluentRegex.PatternStringBuilders;

namespace Wilgysef.FluentRegex.PatternStates
{
    internal class PatternBuildState
    {
        public PatternUnwrapState UnwrapState { get; }

        private readonly PatternStringBuilder _stringBuilder = new PatternStringBuilder();
        private readonly PatternStringBuilderReplicator _replicator;

        private readonly PatternTraverseState<string> _stringResult;
        private readonly PatternTraverseState<bool> _emptyState;

        public PatternBuildState()
        {
            UnwrapState = new PatternUnwrapState(this);

            _replicator = new PatternStringBuilderReplicator(_stringBuilder);
            _stringResult = new PatternTraverseState<string>(this);
            _emptyState = new PatternTraverseState<bool>(this);
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

        public bool IsEmpty(Pattern pattern)
        {
            return _emptyState.Compute(pattern, pattern.IsEmpty, null);
        }

        public override string ToString()
        {
            return _stringBuilder.ToString();
        }
    }
}
