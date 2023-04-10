using System;
using System.Collections.Generic;
using Wilgysef.FluentRegex.Exceptions;

namespace Wilgysef.FluentRegex.PatternStates
{
    internal abstract class PatternTraverseState<TResult, TState>
    {
        protected readonly Stack<Pattern> _stack = new Stack<Pattern>();
        protected readonly Dictionary<Pattern, TResult> _results = new Dictionary<Pattern, TResult>();

        protected abstract TState GetState();

        protected TResult Compute(
            Pattern pattern,
            Func<TState, TResult> action,
            Action<TResult>? actionCached)
        {
            if (_stack.Contains(pattern))
            {
                throw new PatternRecursionException(_stack, pattern);
            }

            if (!_results.TryGetValue(pattern, out var result))
            {
                _stack.Push(pattern);
                result = action(GetState());
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
