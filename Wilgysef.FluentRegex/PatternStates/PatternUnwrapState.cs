namespace Wilgysef.FluentRegex.PatternStates
{
    internal class PatternUnwrapState : PatternTraverseState<Pattern>
    {
        public PatternUnwrapState(PatternBuildState buildState) : base(buildState) { }

        public Pattern Unwrap(Pattern pattern)
        {
            return Compute(pattern, pattern.UnwrapInternal, null);
        }
    }
}
