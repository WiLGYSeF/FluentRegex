using System.Text;

namespace Wilgysef.FluentRegex.PatternStringBuilders
{
    internal class PatternStringBuilder : IPatternStringBuilder
    {
        public int Length => _builder.Length;

        private readonly StringBuilder _builder = new StringBuilder();

        public IPatternStringBuilder Append(string value)
        {
            _builder.Append(value);
            return this;
        }

        public IPatternStringBuilder Append(char value)
        {
            _builder.Append(value);
            return this;
        }

        public IPatternStringBuilder Append(int value)
        {
            _builder.Append(value);
            return this;
        }

        public IPatternStringBuilder Clear()
        {
            _builder.Clear();
            return this;
        }

        public override string ToString()
        {
            return _builder.ToString();
        }
    }
}
