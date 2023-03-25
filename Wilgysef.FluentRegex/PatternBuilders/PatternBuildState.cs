using System;
using System.Collections.Generic;
using Wilgysef.FluentRegex.Exceptions;

namespace Wilgysef.FluentRegex.PatternBuilders
{
    internal class PatternBuildState
    {
        private readonly PatternStringBuilder _stringBuilder = new PatternStringBuilder();
        private readonly Stack<Pattern> _buildStack = new Stack<Pattern>();
        private readonly Dictionary<Pattern, string> _patternStrings = new Dictionary<Pattern, string>();

        private readonly PatternStringBuilderReplicator _replicator;

        public PatternBuildState()
        {
            _replicator = new PatternStringBuilderReplicator(_stringBuilder);
        }

        public void WithPattern(Pattern pattern, Action<IPatternStringBuilder> action)
        {
            if (_buildStack.Contains(pattern))
            {
                throw new PatternRecursionException(_buildStack, pattern);
            }

            if (!_patternStrings.TryGetValue(pattern, out var value))
            {
                var patternStringBuilder = new PatternStringBuilder();
                _replicator.Add(patternStringBuilder);
                _buildStack.Push(pattern);

                action(_replicator);

                _buildStack.Pop();
                _replicator.Remove(patternStringBuilder);
                _patternStrings[pattern] = patternStringBuilder.ToString();
            }
            else
            {
                _replicator.Append(value);
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
    }
}
