﻿using Wilgysef.FluentRegex.PatternStringBuilders;
using Wilgysef.FluentRegex.PatternStates;

namespace Wilgysef.FluentRegex
{
    public abstract class AbstractGroupPattern : ContainerPattern
    {
        /// <summary>
        /// Group pattern.
        /// </summary>
        protected Pattern? Pattern
        {
            get => _children.Count != 0 ? _children[0] : null;
            set
            {
                if (value != null)
                {
                    if (_children.Count == 0)
                    {
                        _children.Add(value);
                    }
                    else
                    {
                        _children[0] = value;
                    }
                }
                else
                {
                    _children.Clear();
                }
            }
        }

        protected AbstractGroupPattern(Pattern? pattern)
        {
            Pattern = pattern;
        }

        /// <summary>
        /// Writes the group contents to the string builder.
        /// </summary>
        /// <param name="state">Pattern build state.</param>
        private protected abstract void GroupContents(PatternBuildState state);

        internal sealed override void Build(PatternBuildState state)
        {
            if (state.IsEmpty(this))
            {
                return;
            }

            state.WithBuilder(Build);

            void Build(IPatternStringBuilder builder)
            {
                builder.Append('(');
                GroupContents(state);
                builder.Append(')');
            }
        }

        internal sealed override bool IsSinglePattern(PatternBuildState state)
        {
            return true;
        }
    }
}
