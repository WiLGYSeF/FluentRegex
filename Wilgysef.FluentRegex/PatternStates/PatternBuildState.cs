using System;
using Wilgysef.FluentRegex.PatternStringBuilders;

namespace Wilgysef.FluentRegex.PatternStates
{
    internal class PatternBuildState : PatternTraverseState<string, PatternBuildState>
    {
        public PatternUnwrapState UnwrapState { get; }

        private readonly PatternStringBuilder _stringBuilder = new PatternStringBuilder();
        private readonly PatternStringBuilderReplicator _replicator;

        public PatternBuildState()
        {
            UnwrapState = new PatternUnwrapState(this);

            _replicator = new PatternStringBuilderReplicator(_stringBuilder);
        }

        public void WithPattern(Pattern pattern, Action<IPatternStringBuilder> action)
        {
            Compute(pattern, Build, Append);

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

        public override string ToString()
        {
            return _stringBuilder.ToString();
        }

        protected override PatternBuildState GetState()
        {
            return this;
        }
    }
}
