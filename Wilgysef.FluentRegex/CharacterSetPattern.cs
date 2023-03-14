using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;

namespace Wilgysef.FluentRegex
{
    public class CharacterSetPattern : Pattern
    {
        public ICollection<CharacterRange> CharacterRanges => _characterRanges;

        public ICollection<CharacterPattern> Characters => _characters;

        public ICollection<CharacterPattern> SubtractedCharacters => _subtractedCharacters;

        public bool Negated { get; set; }

        internal override bool IsSinglePattern => true;

        private readonly List<CharacterRange> _characterRanges = new List<CharacterRange>();
        private readonly List<CharacterPattern> _characters = new List<CharacterPattern>();
        private readonly List<CharacterPattern> _subtractedCharacters = new List<CharacterPattern>();

        #region Constructors

        public CharacterSetPattern() { }

        public CharacterSetPattern(params char[] characters) : this(characters, false) { }

        public CharacterSetPattern(IEnumerable<char> characters, bool negated = false)
        {
            WithCharacters(characters);
            Negated = negated;
        }

        public CharacterSetPattern(IEnumerable<CharacterPattern> characters, bool negated = false)
        {
            WithCharacters(characters);
            Negated = negated;
        }

        public CharacterSetPattern(params CharacterRange[] characterRanges) : this(characterRanges, false) { }

        public CharacterSetPattern(IEnumerable<CharacterRange> characterRanges, bool negated = false)
        {
            WithCharacterRanges(characterRanges);
            Negated = negated;
        }

        public CharacterSetPattern(
            ICollection<CharacterRange> characterRanges,
            ICollection<CharacterPattern> characters,
            bool negated = false)
        {
            WithCharacterRanges(characterRanges);
            WithCharacters(characters);
            Negated = negated;
        }

        public CharacterSetPattern(
            ICollection<CharacterRange> characterRanges,
            ICollection<CharacterPattern> characters,
            ICollection<CharacterPattern> subtractedCharacters,
            bool negated = false)
        {
            WithCharacterRanges(characterRanges);
            WithCharacters(characters);
            WithSubtractedCharacters(subtractedCharacters);
            Negated = negated;
        }

        #endregion

        #region Fluent Methods

        public CharacterSetPattern WithCharacters(params char[] characters)
        {
            return WithCharacters((IEnumerable<char>)characters);
        }

        public CharacterSetPattern WithCharacters(IEnumerable<char> characters)
        {
            return WithCharacters(characters.Select(c => CharacterPattern.Character(c)));
        }

        public CharacterSetPattern WithCharacters(params CharacterPattern[] characters)
        {
            return WithCharacters((IEnumerable<CharacterPattern>)characters);
        }

        public CharacterSetPattern WithCharacters(IEnumerable<CharacterPattern> characters)
        {
            foreach (var c in characters)
            {
                _characters.Add(c);
            }

            return this;
        }

        public CharacterSetPattern WithCharacterRange(char start, char end)
        {
            return WithCharacterRanges(new CharacterRange(start, end));
        }

        public CharacterSetPattern WithCharacterRange(CharacterPattern start, CharacterPattern end)
        {
            return WithCharacterRanges(new CharacterRange(start, end));
        }

        public CharacterSetPattern WithCharacterRanges(params CharacterRange[] ranges)
        {
            return WithCharacterRanges((IEnumerable<CharacterRange>)ranges);
        }

        public CharacterSetPattern WithCharacterRanges(IEnumerable<CharacterRange> ranges)
        {
            foreach (var range in ranges)
            {
                _characterRanges.Add(range);
            }

            return this;
        }

        public CharacterSetPattern WithSubtractedCharacters(params char[] characters)
        {
            return WithSubtractedCharacters((IEnumerable<char>)characters);
        }

        public CharacterSetPattern WithSubtractedCharacters(IEnumerable<char> characters)
        {
            return WithSubtractedCharacters(characters.Select(c => CharacterPattern.Character(c)));
        }

        public CharacterSetPattern WithSubtractedCharacters(params CharacterPattern[] characters)
        {
            return WithSubtractedCharacters((IEnumerable<CharacterPattern>)characters);
        }

        public CharacterSetPattern WithSubtractedCharacters(IEnumerable<CharacterPattern> characters)
        {
            foreach (var c in characters)
            {
                _subtractedCharacters.Add(c);
            }

            return this;
        }

        public CharacterSetPattern Negate(bool negated = true)
        {
            Negated = negated;
            return this;
        }

        #endregion

        // TODO: range simplification
        internal override void ToString(StringBuilder builder)
        {
            if (_characters.Count == 0 && _characterRanges.Count == 0)
            {
                if (_subtractedCharacters.Count > 0)
                {
                    throw new InvalidOperationException("Cannot have subtracted characters without characters.");
                }

                return;
            }

            if (_characters.Count == 1
                && _characterRanges.Count == 0
                && _subtractedCharacters.Count == 0
                && !Negated)
            {
                // TODO: escape
                Append(_characters[0]);
                return;
            }

            builder.Append('[');

            if (Negated)
            {
                builder.Append('^');
            }

            foreach (var range in CharacterRanges)
            {
                Append(range.Start);
                builder.Append('-');
                Append(range.End);
            }

            foreach (var c in Characters)
            {
                Append(c);
            }

            if (SubtractedCharacters.Count > 0)
            {
                builder.Append("-[");

                foreach (var c in SubtractedCharacters)
                {
                    Append(c);
                }

                builder.Append(']');
            }

            builder.Append(']');

            void Append(CharacterPattern pattern)
            {
                if (pattern.TryGetChar(out var c))
                {
                    switch (c)
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

                pattern.ToString(builder);
            }
        }

        public struct CharacterRange
        {
            public CharacterPattern Start { get; }

            public CharacterPattern End { get; }

            public CharacterRange(char start, char end)
            {
                Start = CharacterPattern.Character(start);
                End = CharacterPattern.Character(end);
            }

            public CharacterRange(CharacterPattern start, CharacterPattern end)
            {
                Start = start;
                End = end;
            }
        }
    }
}
