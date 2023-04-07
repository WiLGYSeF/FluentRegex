using System.Collections.Generic;
using System.Linq;
using Wilgysef.FluentRegex.PatternBuilders;

namespace Wilgysef.FluentRegex
{
    public class OrPattern : ContainerPattern
    {
        internal override bool IsSinglePattern
        {
            get
            {
                if (IsSinglePatternInternal(false))
                {
                    return true;
                }

                var categories = CategorizePatterns();
                return categories.IsSingle;
            }
        }

        internal override bool IsEmpty => IsEmptyInternal();

        /// <summary>
        /// Creates an or pattern.
        /// </summary>
        /// <param name="patterns">Patterns to match.</param>
        public OrPattern(params Pattern[] patterns) : this((IEnumerable<Pattern>)patterns) { }

        /// <summary>
        /// Creates an or pattern.
        /// </summary>
        /// <param name="patterns">Patterns to match.</param>
        public OrPattern(IEnumerable<Pattern> patterns) : base(patterns) { }

        /// <summary>
        /// Adds a pattern to match.
        /// </summary>
        /// <param name="pattern">Pattern to match.</param>
        /// <returns>Current or pattern.</returns>
        public OrPattern Or(Pattern pattern)
        {
            _children.Add(pattern);
            return this;
        }

        public override Pattern Copy()
        {
            // TODO: recursion check?
            return new OrPattern(_children.Select(c => c.Copy()));
        }

        internal override void Build(PatternBuildState state)
        {
            state.WithPattern(this, Build);

            void Build(IPatternStringBuilder builder)
            {
                if (_children.Count <= 1)
                {
                    if (_children.Count == 1)
                    {
                        _children[0].Build(state);
                    }

                    return;
                }

                var categories = CategorizePatterns();

                for (var i = 0; i < categories.Uncategorized.Count; i++)
                {
                    if (i == categories.FirstCharacterSetIndex)
                    {
                        AppendCharacterSets();
                        builder.Append('|');
                    }

                    categories.Uncategorized[i].Build(state);

                    if (i < categories.Uncategorized.Count - 1)
                    {
                        builder.Append('|');
                    }
                }

                if (categories.FirstCharacterSetIndex == categories.Uncategorized.Count)
                {
                    if (categories.FirstCharacterSetIndex != 0)
                    {
                        builder.Append('|');
                    }

                    AppendCharacterSets();
                }

                void AppendCharacterSets()
                {
                    if (categories.HasCharacterSet)
                    {
                        CharacterSetPattern.Combine(categories.CharacterSetPatterns)
                            .WithCharacters(categories.CharacterPatterns)
                            .WithCharacters(categories.SingleLiterals.Select(p => p.Value[0]))
                            .Build(state);

                        if (categories.HasNegatedCharacterSet)
                        {
                            builder.Append('|');
                        }
                    }

                    if (categories.HasNegatedCharacterSet)
                    {
                        CharacterSetPattern.Combine(categories.NegatedCharacterSetPatterns).Build(state);
                    }
                }
            }
        }

        internal override Pattern Unwrap()
        {
            return UnwrapInternal();
        }

        private CategorizedPatterns CategorizePatterns()
        {
            var uncategorized = new List<Pattern>();
            var characterSetPatterns = new List<CharacterSetPattern>();
            var negatedCharacterSetPatterns = new List<CharacterSetPattern>();
            var singleLiterals = new List<LiteralPattern>();
            var characterPatterns = new List<CharacterPattern>();
            var firstCharacterSetIndex = -1;

            for (var i = 0; i < _children.Count; i++)
            {
                var unwrapped = _children[i].Unwrap();
                if (unwrapped is CharacterSetPattern characterSet)
                {
                    if (firstCharacterSetIndex == -1)
                    {
                        firstCharacterSetIndex = i;
                    }

                    if (characterSet.Negated)
                    {
                        negatedCharacterSetPatterns.Add(characterSet);
                    }
                    else
                    {
                        characterSetPatterns.Add(characterSet);
                    }
                }
                else if (unwrapped is CharacterPattern character)
                {
                    if (firstCharacterSetIndex == -1)
                    {
                        firstCharacterSetIndex = i;
                    }

                    characterPatterns.Add(character);
                }
                else if (unwrapped is LiteralPattern literal && literal.Value.Length == 1)
                {
                    if (firstCharacterSetIndex == -1)
                    {
                        firstCharacterSetIndex = i;
                    }

                    singleLiterals.Add(literal);
                }
                else
                {
                    uncategorized.Add(unwrapped);
                }
            }

            return new CategorizedPatterns(
                uncategorized,
                characterSetPatterns,
                negatedCharacterSetPatterns,
                characterPatterns,
                singleLiterals,
                firstCharacterSetIndex);
        }

        private class CategorizedPatterns
        {
            public List<Pattern> Uncategorized { get; }

            public List<CharacterSetPattern> CharacterSetPatterns { get; }

            public List<CharacterSetPattern> NegatedCharacterSetPatterns { get; }

            public List<CharacterPattern> CharacterPatterns { get; }

            public List<LiteralPattern> SingleLiterals { get; }

            public int FirstCharacterSetIndex { get; }

            public bool HasCharacterSet => CharacterSetPatterns.Count + CharacterPatterns.Count + SingleLiterals.Count > 0;

            public bool HasNegatedCharacterSet => NegatedCharacterSetPatterns.Count > 0;

            public bool IsSingle => Uncategorized.Count == 0 && HasCharacterSet != HasNegatedCharacterSet;

            public CategorizedPatterns(
                List<Pattern> uncategorized,
                List<CharacterSetPattern> characterSetPatterns,
                List<CharacterSetPattern> negatedCharacterSetPatterns,
                List<CharacterPattern> characterPatterns,
                List<LiteralPattern> singleLiterals,
                int firstCharacterSetIndex)
            {
                Uncategorized = uncategorized;
                CharacterSetPatterns = characterSetPatterns;
                NegatedCharacterSetPatterns = negatedCharacterSetPatterns;
                CharacterPatterns = characterPatterns;
                SingleLiterals = singleLiterals;
                FirstCharacterSetIndex = firstCharacterSetIndex;
            }
        }
    }
}
