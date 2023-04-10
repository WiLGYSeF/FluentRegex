using System;
using System.Collections.Generic;
using System.Linq;
using Wilgysef.FluentRegex.Exceptions;
using Wilgysef.FluentRegex.PatternStringBuilders;
using Wilgysef.FluentRegex.PatternStates;

namespace Wilgysef.FluentRegex
{
    public class CharacterSetPattern : Pattern
    {
        /// <summary>
        /// Character ranges the set will match.
        /// </summary>
        public ICollection<CharacterRange> CharacterRanges => _characterRanges;

        /// <summary>
        /// Characters the set will match.
        /// </summary>
        public ICollection<CharacterPattern> Characters => _characters;

        /// <summary>
        /// Character ranges the set will not match.
        /// </summary>
        public ICollection<CharacterPattern> SubtractedCharacters => _subtractedCharacters;

        /// <summary>
        /// Characters the set will not match.
        /// </summary>
        public ICollection<CharacterRange> SubtractedCharacterRanges => _subtractedCharacterRanges;

        /// <summary>
        /// Indicates if the set is negated.
        /// </summary>
        public bool Negated { get; set; }

        internal override bool IsSinglePattern => true;

        internal override bool IsEmpty => _characters.Count == 0 && _characterRanges.Count == 0;

        private readonly List<CharacterRange> _characterRanges = new List<CharacterRange>();
        private readonly List<CharacterPattern> _characters = new List<CharacterPattern>();
        private readonly List<CharacterPattern> _subtractedCharacters = new List<CharacterPattern>();
        private readonly List<CharacterRange> _subtractedCharacterRanges = new List<CharacterRange>();

        #region Constructors

        /// <summary>
        /// Creates a character set with characters that will match.
        /// </summary>
        /// <param name="characters">Characters.</param>
        public CharacterSetPattern(string characters)
        {
            WithCharacters(characters);
        }

        /// <summary>
        /// Creates a character set with characters that will match.
        /// </summary>
        /// <param name="characters">Characters.</param>
        public CharacterSetPattern(params char[] characters)
        {
            WithCharacters(characters);
        }

        /// <summary>
        /// Creates a character set with characters that will match.
        /// </summary>
        /// <param name="characters">Characters.</param>
        /// <param name="negated">Indicates if the set is negated.</param>
        public CharacterSetPattern(IEnumerable<char> characters, bool negated = false)
        {
            WithCharacters(characters);
            Negated = negated;
        }

        /// <summary>
        /// Creates a character set with characters that will match.
        /// </summary>
        /// <param name="characters">Characters.</param>
        public CharacterSetPattern(params CharacterPattern[] characters)
        {
            WithCharacters(characters);
        }

        /// <summary>
        /// Creates a character set with characters that will match.
        /// </summary>
        /// <param name="characters">Characters.</param>
        /// <param name="negated">Indicates if the set is negated.</param>
        public CharacterSetPattern(IEnumerable<CharacterPattern> characters, bool negated = false)
        {
            WithCharacters(characters);
            Negated = negated;
        }

        /// <summary>
        /// Creates a character set with character ranges that will match.
        /// </summary>
        /// <param name="characterRanges">Character ranges.</param>
        public CharacterSetPattern(params CharacterRange[] characterRanges) : this(characterRanges, false) { }

        /// <summary>
        /// Creates a character set with character ranges that will match.
        /// </summary>
        /// <param name="characterRanges">Character ranges.</param>
        /// <param name="negated">Indicates if the set is negated.</param>
        public CharacterSetPattern(IEnumerable<CharacterRange> characterRanges, bool negated = false)
        {
            WithCharacterRanges(characterRanges);
            Negated = negated;
        }

        /// <summary>
        /// Creates a character set with characters that will match.
        /// </summary>
        /// <param name="characterRanges">Character ranges.</param>
        /// <param name="characters">Characters.</param>
        /// <param name="negated">Indicates if the set is negated.</param>
        public CharacterSetPattern(
            IEnumerable<CharacterRange> characterRanges,
            IEnumerable<CharacterPattern> characters,
            bool negated = false)
        {
            WithCharacterRanges(characterRanges);
            WithCharacters(characters);
            Negated = negated;
        }

        /// <summary>
        /// Creates a character set with characters that will match.
        /// </summary>
        /// <param name="characterRanges">Character ranges that will match.</param>
        /// <param name="characters">Characters that will match.</param>
        /// <param name="subtractedCharacters">Characters that will not match.</param>
        /// <param name="negated">Indicates if the set is negated.</param>
        public CharacterSetPattern(
            IEnumerable<CharacterRange> characterRanges,
            IEnumerable<CharacterPattern> characters,
            IEnumerable<CharacterPattern> subtractedCharacters,
            bool negated = false)
        {
            WithCharacterRanges(characterRanges);
            WithCharacters(characters);
            WithSubtractedCharacters(subtractedCharacters);
            Negated = negated;
        }

        /// <summary>
        /// Creates a character set with characters that will match.
        /// </summary>
        /// <param name="characterRanges">Character ranges that will match.</param>
        /// <param name="characters">Characters that will match.</param>
        /// <param name="subtractedCharacterRanges">Character ranges that will not match.</param>
        /// <param name="subtractedCharacters">Characters that will not match.</param>
        /// <param name="negated">Indicates if the set is negated.</param>
        public CharacterSetPattern(
            IEnumerable<CharacterRange> characterRanges,
            IEnumerable<CharacterPattern> characters,
            IEnumerable<CharacterRange> subtractedCharacterRanges,
            IEnumerable<CharacterPattern> subtractedCharacters,
            bool negated = false)
        {
            WithCharacterRanges(characterRanges);
            WithCharacters(characters);
            WithSubtractedCharacterRanges(subtractedCharacterRanges);
            WithSubtractedCharacters(subtractedCharacters);
            Negated = negated;
        }

        #endregion

        #region Fluent Methods

        /// <summary>
        /// Adds characters that will match to the set.
        /// </summary>
        /// <param name="characters">Characters.</param>
        /// <returns>Current character set pattern.</returns>
        public CharacterSetPattern WithCharacters(params char[] characters) => WithCharacters((IEnumerable<char>)characters);

        /// <summary>
        /// Adds characters that will match to the set.
        /// </summary>
        /// <param name="characters">Characters.</param>
        /// <returns>Current character set pattern.</returns>
        public CharacterSetPattern WithCharacters(IEnumerable<char> characters) => WithCharacters(characters.Select(c => CharacterPattern.Character(c)));

        /// <summary>
        /// Adds characters that will match to the set.
        /// </summary>
        /// <param name="characters">Characters.</param>
        /// <returns>Current character set pattern.</returns>
        public CharacterSetPattern WithCharacters(params CharacterPattern[] characters) => WithCharacters((IEnumerable<CharacterPattern>)characters);

        /// <summary>
        /// Adds characters that will match to the set.
        /// </summary>
        /// <param name="characters">Characters.</param>
        /// <returns>Current character set pattern.</returns>
        public CharacterSetPattern WithCharacters(IEnumerable<CharacterPattern> characters)
        {
            _characters.AddRange(characters);
            return this;
        }

        /// <summary>
        /// Adds character range that will match to the set.
        /// </summary>
        /// <param name="start">Start character.</param>
        /// <param name="end">End character.</param>
        /// <returns>Current character set pattern.</returns>
        public CharacterSetPattern WithCharacterRange(char start, char end) => WithCharacterRanges(new CharacterRange(start, end));

        /// <summary>
        /// Adds character range that will match to the set.
        /// </summary>
        /// <param name="start">Start character.</param>
        /// <param name="end">End character.</param>
        /// <returns>Current character set pattern.</returns>
        public CharacterSetPattern WithCharacterRange(CharacterPattern start, CharacterPattern end) => WithCharacterRanges(new CharacterRange(start, end));

        /// <summary>
        /// Adds character ranges that will match to the set.
        /// </summary>
        /// <param name="characterRanges">Character ranges.</param>
        /// <returns>Current character set pattern.</returns>
        public CharacterSetPattern WithCharacterRanges(params CharacterRange[] ranges) => WithCharacterRanges((IEnumerable<CharacterRange>)ranges);

        /// <summary>
        /// Adds character ranges that will match to the set.
        /// </summary>
        /// <param name="characterRanges">Character ranges.</param>
        /// <returns>Current character set pattern.</returns>
        public CharacterSetPattern WithCharacterRanges(IEnumerable<CharacterRange> ranges)
        {
            _characterRanges.AddRange(ranges);
            return this;
        }

        /// <summary>
        /// Adds characters that will not match to the set.
        /// </summary>
        /// <param name="characters">Characters</param>
        /// <returns>Current character set pattern.</returns>
        public CharacterSetPattern WithSubtractedCharacters(params char[] characters) => WithSubtractedCharacters((IEnumerable<char>)characters);

        /// <summary>
        /// Adds characters that will not match to the set.
        /// </summary>
        /// <param name="characters">Characters</param>
        /// <returns>Current character set pattern.</returns>
        public CharacterSetPattern WithSubtractedCharacters(IEnumerable<char> characters) => WithSubtractedCharacters(characters.Select(c => CharacterPattern.Character(c)));

        /// <summary>
        /// Adds characters that will not match to the set.
        /// </summary>
        /// <param name="characters">Characters</param>
        /// <returns>Current character set pattern.</returns>
        public CharacterSetPattern WithSubtractedCharacters(params CharacterPattern[] characters) => WithSubtractedCharacters((IEnumerable<CharacterPattern>)characters);

        /// <summary>
        /// Adds characters that will not match to the set.
        /// </summary>
        /// <param name="characters">Characters</param>
        /// <returns>Current character set pattern.</returns>
        public CharacterSetPattern WithSubtractedCharacters(IEnumerable<CharacterPattern> characters)
        {
            _subtractedCharacters.AddRange(characters);
            return this;
        }

        /// <summary>
        /// Adds character range that will not match to the set.
        /// </summary>
        /// <param name="start">Start character.</param>
        /// <param name="end">End character.</param>
        /// <returns>Current character set pattern.</returns>
        public CharacterSetPattern WithSubtractedCharacterRange(char start, char end) => WithSubtractedCharacterRanges(new CharacterRange(start, end));

        /// <summary>
        /// Adds character range that will not match to the set.
        /// </summary>
        /// <param name="start">Start character.</param>
        /// <param name="end">End character.</param>
        /// <returns>Current character set pattern.</returns>
        public CharacterSetPattern WithSubtractedCharacterRange(CharacterPattern start, CharacterPattern end) => WithSubtractedCharacterRanges(new CharacterRange(start, end));

        /// <summary>
        /// Adds character ranges that will not match to the set.
        /// </summary>
        /// <param name="characterRanges">Character ranges.</param>
        /// <returns>Current character set pattern.</returns>
        public CharacterSetPattern WithSubtractedCharacterRanges(params CharacterRange[] ranges) => WithSubtractedCharacterRanges((IEnumerable<CharacterRange>)ranges);

        /// <summary>
        /// Adds character ranges that will not match to the set.
        /// </summary>
        /// <param name="characterRanges">Character ranges.</param>
        /// <returns>Current character set pattern.</returns>
        public CharacterSetPattern WithSubtractedCharacterRanges(IEnumerable<CharacterRange> ranges)
        {
            _subtractedCharacterRanges.AddRange(ranges);
            return this;
        }

        /// <summary>
        /// Sets if the character set is negated.
        /// </summary>
        /// <param name="negated">Indicates if the character set is negated.</param>
        /// <returns>Current character set pattern.</returns>
        public CharacterSetPattern Negate(bool negated = true)
        {
            Negated = negated;
            return this;
        }

        #endregion

        public override Pattern Copy()
        {
            return new CharacterSetPattern(
                _characterRanges.Select(r => r.Copy()),
                _characters.Select(c => (CharacterPattern)c.Copy()),
                _subtractedCharacterRanges.Select(r => r.Copy()),
                _subtractedCharacters.Select(c => (CharacterPattern)c.Copy()),
                Negated);
        }

        internal static CharacterSetPattern Combine(IEnumerable<CharacterSetPattern> patterns)
        {
            var negated = patterns.FirstOrDefault()?.Negated ?? false;

            if (!patterns.All(p => p.Negated == negated))
            {
                throw new ArgumentException("Not all patterns have same negation.");
            }

            return new CharacterSetPattern(
                patterns.SelectMany(p => p.CharacterRanges),
                patterns.SelectMany(p => p.Characters),
                patterns.SelectMany(p => p.SubtractedCharacterRanges),
                patterns.SelectMany(p => p.SubtractedCharacters),
                negated);
        }

        internal override void Build(PatternBuildState state)
        {
            if (IsEmpty)
            {
                if (_subtractedCharacters.Count > 0 || _subtractedCharacterRanges.Count > 0)
                {
                    throw new InvalidPatternException(this, "Cannot have subtracted characters without characters.");
                }

                return;
            }

            state.WithPattern(this, Build);

            void Build(IPatternStringBuilder builder)
            {
                var characterRanges = CharacterRange.Overlap(_characterRanges);
                var characters = new List<CharacterPattern>(_characters);
                var subtractedCharacterRanges = CharacterRange.Overlap(_subtractedCharacterRanges);
                var subtractedCharacters = new List<CharacterPattern>(_subtractedCharacters);

                SimplifyCharacterRanges(ref characterRanges, characters);
                SimplifyCharacterRanges(ref subtractedCharacterRanges, subtractedCharacters);

                if (subtractedCharacters.Count == 0
                    && subtractedCharacterRanges.Count == 0
                    && !Negated)
                {
                    if (characters.Count == 1 && characterRanges.Count == 0)
                    {
                        characters[0].Build(state);
                        return;
                    }

                    if (characters.Count == 0 && characterRanges.Count == 1 && characterRanges[0].Single)
                    {
                        characterRanges[0].Start.Build(state);
                        return;
                    }
                }

                var tmpBuilder = new PatternStringBuilder();

                builder.Append('[');

                if (Negated)
                {
                    builder.Append('^');
                }

                AppendRanges(characterRanges);
                foreach (var pattern in characters)
                {
                    Append(pattern);
                }

                if (subtractedCharacters.Count > 0 || subtractedCharacterRanges.Count > 0)
                {
                    builder.Append("-[");

                    AppendRanges(subtractedCharacterRanges);
                    foreach (var pattern in subtractedCharacters)
                    {
                        Append(pattern);
                    }

                    builder.Append(']');
                }

                builder.Append(']');

                void Append(CharacterPattern pattern)
                {
                    pattern.Build(tmpBuilder, fromCharacterSet: true);
                    var patternString = tmpBuilder.ToString();
                    tmpBuilder.Clear();

                    if (patternString.Length == 1)
                    {
                        switch (patternString[0])
                        {
                            case '[':
                            case ']':
                            case '-':
                            case '^':
                            case '\\':
                                builder.Append('\\');
                                break;
                        }
                    }

                    builder.Append(patternString);
                }

                void AppendRanges(IEnumerable<CharacterRange> ranges)
                {
                    foreach (var range in ranges)
                    {
                        Append(range.Start);

                        if (!range.Single)
                        {
                            if (!range.Adjacent)
                            {
                                builder.Append('-');
                            }

                            Append(range.End);
                        }
                    }
                }
            }
        }

        internal override Pattern UnwrapInternal(PatternBuildState state)
        {
            if (_subtractedCharacterRanges.Count > 0
                || _subtractedCharacters.Count > 0
                || Negated)
            {
                return this;
            }

            var characterRanges = CharacterRange.Overlap(_characterRanges);
            var characters = new List<CharacterPattern>(_characters);

            SimplifyCharacterRanges(ref characterRanges, characters);

            if (characterRanges.Count == 1
                && characters.Count == 0
                && characterRanges[0].Single)
            {
                return state.UnwrapState.Unwrap(characterRanges[0].Start);
            }
            else if (characterRanges.Count == 0 && characters.Count == 1)
            {
                return state.UnwrapState.Unwrap(characters[0]);
            }

            return this;
        }

        private static void SimplifyCharacterRanges(ref List<CharacterRange> ranges, List<CharacterPattern> characters)
        {
            var overlapRanges = false;

            var characterEntries = new List<(int Value, int Index, CharacterPattern Pattern)>();

            for (var i = 0; i < characters.Count; i++)
            {
                if (characters[i] is CharacterLiteralPattern literalPattern)
                {
                    characterEntries.Add((literalPattern.GetValue(), i, literalPattern));
                }
            }

            if (characterEntries.Count >= 3)
            {
                var charsToRemove = new HashSet<int>();

                characterEntries.Sort((a, b) => a.Value - b.Value);

                // convert consecutive characters into ranges

                for (var i = 0; i < characterEntries.Count; i++)
                {
                    var start = characterEntries[i];
                    var index = i;

                    for (i++; i < characterEntries.Count && characterEntries[i].Value - characterEntries[i - 1].Value <= 1; i++) ;
                    i--;

                    if (i - index >= 3)
                    {
                        ranges.Add(new CharacterRange(start.Pattern, characterEntries[i].Pattern));
                        overlapRanges = true;

                        for (var j = index; j <= i; j++)
                        {
                            charsToRemove.Add(characterEntries[j].Index);
                        }
                    }
                }

                for (var i = characters.Count - 1; i >= 0 && charsToRemove.Count > 0; i--)
                {
                    if (charsToRemove.Contains(i))
                    {
                        characters.RemoveAt(i);
                    }
                }
            }

            int originalCharacterCount;

            // simplify characters into ranges

            do
            {
                originalCharacterCount = characters.Count;

                for (var i = 0; i < characters.Count; i++)
                {
                    for (var rangeIndex = 0; rangeIndex < ranges.Count; rangeIndex++)
                    {
                        var range = ranges[rangeIndex];
                        var removeChar = false;

                        if (range.Contains(characters[i]))
                        {
                            removeChar = true;
                        }
                        else if (range.IsAdjacentLeftOfStart(characters[i]))
                        {
                            ranges[rangeIndex] = new CharacterRange(
                                    CharacterLiteralPattern.FromInt(range.StartValue - 1),
                                    range.End);
                            overlapRanges = true;
                            removeChar = true;
                        }
                        else if (range.IsAdjacentRightOfEnd(characters[i]))
                        {
                            ranges[rangeIndex] = new CharacterRange(
                                    range.Start,
                                    CharacterLiteralPattern.FromInt(range.EndValue + 1));
                            overlapRanges = true;
                            removeChar = true;
                        }

                        if (removeChar)
                        {
                            characters.RemoveAt(i--);
                            break;
                        }
                    }
                }
            } while (characters.Count != originalCharacterCount);

            if (overlapRanges)
            {
                ranges = CharacterRange.Overlap(ranges);
            }
        }
    }
}
