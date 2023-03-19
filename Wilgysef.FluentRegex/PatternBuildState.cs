using System;
using System.Collections.Generic;
using System.Text;

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
                var path = Pattern.GetPatternPath(_buildStack, pattern);
                throw new InvalidOperationException($"Pattern is infinitely recursive: {path}");
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
