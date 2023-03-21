using System;
using System.Collections.Generic;
using System.Text;
using Wilgysef.FluentRegex.Exceptions;

namespace Wilgysef.FluentRegex
{
    internal class PatternBuildState
    {
        private readonly StringBuilder _stringBuilder = new StringBuilder();
        private readonly Stack<Pattern> _buildStack = new Stack<Pattern>();

        public void WithPattern(Pattern pattern, Action<StringBuilder> action)
        {
            if (_buildStack.Contains(pattern))
            {
                throw new PatternRecursionException(_buildStack, pattern);
            }

            _buildStack.Push(pattern);
            action(_stringBuilder);
        }

        public void WithBuilder(Action<StringBuilder> action)
        {
            action(_stringBuilder);
        }

        public override string ToString()
        {
            return _stringBuilder.ToString();
        }
    }
}
