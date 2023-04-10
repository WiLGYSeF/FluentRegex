using System;
using System.Collections.Generic;
using Wilgysef.FluentRegex.Exceptions;

namespace Wilgysef.FluentRegex.PatternStates
{
    internal class PatternTraverseState<TResult>
    {
        protected readonly Stack<Pattern> _stack = new Stack<Pattern>();
        protected readonly Dictionary<Pattern, TResult> _results = new Dictionary<Pattern, TResult>();

        private readonly PatternBuildState _buildState;

        public PatternTraverseState(PatternBuildState buildState)
        {
            _buildState = buildState;
        }

        public TResult Compute(
            Pattern pattern,
            Func<PatternBuildState, TResult> action,
            Action<TResult>? actionCached)
        {
            if (_stack.Contains(pattern))
            {
                throw new PatternRecursionException(_stack, pattern);
            }

            if (!_results.TryGetValue(pattern, out var result))
            {
                _stack.Push(pattern);
                result = action(_buildState);
                _stack.Pop();

                _results[pattern] = result;
            }
            else
            {
                actionCached?.Invoke(result);
            }

            return result;
        }
    }
}
