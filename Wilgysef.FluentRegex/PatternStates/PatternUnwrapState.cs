namespace Wilgysef.FluentRegex.PatternStates
{
    internal class PatternUnwrapState : PatternTraverseState<Pattern, PatternBuildState>
    {
        private readonly PatternBuildState _buildState;

        public PatternUnwrapState(PatternBuildState buildState)
        {
            _buildState = buildState;
        }

        public Pattern Unwrap(Pattern pattern)
        {
            return Compute(pattern, pattern.UnwrapInternal, null);
        }

        protected override PatternBuildState GetState()
        {
            return _buildState;
        }
    }
}
