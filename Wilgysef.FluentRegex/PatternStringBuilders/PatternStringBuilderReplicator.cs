using System.Collections.Generic;

namespace Wilgysef.FluentRegex.PatternStringBuilders
{
    internal class PatternStringBuilderReplicator : IPatternStringBuilder
    {
        public int Length => _builders.Count > 0 ? _builders[0].Length : 0;

        private readonly List<IPatternStringBuilder> _builders;

        public PatternStringBuilderReplicator(IPatternStringBuilder builder)
        {
            _builders = new List<IPatternStringBuilder>
            {
                builder
            };
        }

        public void Add(IPatternStringBuilder builder)
        {
            _builders.Add(builder);
        }

        public void Remove(IPatternStringBuilder builder)
        {
            _builders.Remove(builder);
        }

        public IPatternStringBuilder Append(string value)
        {
            foreach (var builder in _builders)
            {
                builder.Append(value);
            }

            return this;
        }

        public IPatternStringBuilder Append(char value)
        {
            foreach (var builder in _builders)
            {
                builder.Append(value);
            }

            return this;
        }

        public IPatternStringBuilder Append(int value)
        {
            foreach (var builder in _builders)
            {
                builder.Append(value);
            }

            return this;
        }

        public IPatternStringBuilder Clear()
        {
            foreach (var builder in _builders)
            {
                builder.Clear();
            }

            return this;
        }

        public override string ToString()
        {
            return _builders.Count > 0
                ? _builders[0].ToString()
                : "";
        }
    }
}
