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

        public ICollection<CharacterRange> SubtractedCharacterRanges => _subtractedCharacterRanges;

        public bool Negated { get; set; }

        internal override bool IsSinglePattern => true;

        private readonly List<CharacterRange> _characterRanges = new List<CharacterRange>();
        private readonly List<CharacterPattern> _characters = new List<CharacterPattern>();
        private readonly List<CharacterPattern> _subtractedCharacters = new List<CharacterPattern>();
        private readonly List<CharacterRange> _subtractedCharacterRanges = new List<CharacterRange>();

        #region Constructors

        public CharacterSetPattern(params char[] characters)
        {
            WithCharacters(characters);
        }

        public CharacterSetPattern(IEnumerable<char> characters, bool negated = false)
        {
            WithCharacters(characters);
            Negated = negated;
        }

        public CharacterSetPattern(params CharacterPattern[] characters)
        {
            WithCharacters(characters);
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
            IEnumerable<CharacterRange> characterRanges,
            IEnumerable<CharacterPattern> characters,
            bool negated = false)
        {
            WithCharacterRanges(characterRanges);
            WithCharacters(characters);
            Negated = negated;
        }

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

        public CharacterSetPattern WithCharacters(params char[] characters) => WithCharacters((IEnumerable<char>)characters);

        public CharacterSetPattern WithCharacters(IEnumerable<char> characters) => WithCharacters(characters.Select(c => CharacterPattern.Character(c)));

        public CharacterSetPattern WithCharacters(params CharacterPattern[] characters) => WithCharacters((IEnumerable<CharacterPattern>)characters);

        public CharacterSetPattern WithCharacters(IEnumerable<CharacterPattern> characters)
        {
            _characters.AddRange(characters);
            return this;
        }

        public CharacterSetPattern WithCharacterRange(char start, char end) => WithCharacterRanges(new CharacterRange(start, end));

        public CharacterSetPattern WithCharacterRange(CharacterPattern start, CharacterPattern end) => WithCharacterRanges(new CharacterRange(start, end));

        public CharacterSetPattern WithCharacterRanges(params CharacterRange[] ranges) => WithCharacterRanges((IEnumerable<CharacterRange>)ranges);

        public CharacterSetPattern WithCharacterRanges(IEnumerable<CharacterRange> ranges)
        {
            _characterRanges.AddRange(ranges);
            return this;
        }

        public CharacterSetPattern WithSubtractedCharacters(params char[] characters) => WithSubtractedCharacters((IEnumerable<char>)characters);

        public CharacterSetPattern WithSubtractedCharacters(IEnumerable<char> characters) => WithSubtractedCharacters(characters.Select(c => CharacterPattern.Character(c)));

        public CharacterSetPattern WithSubtractedCharacters(params CharacterPattern[] characters) => WithSubtractedCharacters((IEnumerable<CharacterPattern>)characters);

        public CharacterSetPattern WithSubtractedCharacters(IEnumerable<CharacterPattern> characters)
        {
            _subtractedCharacters.AddRange(characters);
            return this;
        }

        public CharacterSetPattern WithSubtractedCharacterRange(char start, char end) => WithSubtractedCharacterRanges(new CharacterRange(start, end));

        public CharacterSetPattern WithSubtractedCharacterRange(CharacterPattern start, CharacterPattern end) => WithSubtractedCharacterRanges(new CharacterRange(start, end));

        public CharacterSetPattern WithSubtractedCharacterRanges(params CharacterRange[] ranges) => WithSubtractedCharacterRanges((IEnumerable<CharacterRange>)ranges);

        public CharacterSetPattern WithSubtractedCharacterRanges(IEnumerable<CharacterRange> ranges)
        {
            _subtractedCharacterRanges.AddRange(ranges);
            return this;
        }

        public CharacterSetPattern Negate(bool negated = true)
        {
            Negated = negated;
            return this;
        }

        #endregion

        internal override void ToString(StringBuilder builder)
        {
            if (_characters.Count == 0 && _characterRanges.Count == 0)
            {
                if (_subtractedCharacters.Count > 0 || _subtractedCharacterRanges.Count > 0)
                {
                    throw new InvalidOperationException("Cannot have subtracted characters without characters.");
                }

                return;
            }

            if (_subtractedCharacters.Count == 0
                && _subtractedCharacterRanges.Count == 0
                && !Negated)
            {
                if (_characters.Count == 1 && _characterRanges.Count == 0)
                {
                    _characters[0].ToString(builder);
                    return;
                }

                if (_characters.Count == 0 && _characterRanges.Count == 1 && _characterRanges[0].Single)
                {
                    _characterRanges[0].Start.ToString(builder);
                    return;
                }
            }

            var tmpBuilder = new StringBuilder();

            builder.Append('[');

            if (Negated)
            {
                builder.Append('^');
            }

            AppendRanges(_characterRanges);
            foreach (var pattern in _characters)
            {
                Append(pattern);
            }

            if (_subtractedCharacters.Count > 0 || _subtractedCharacterRanges.Count > 0)
            {
                builder.Append("-[");

                AppendRanges(_subtractedCharacterRanges);
                foreach (var pattern in _subtractedCharacters)
                {
                    Append(pattern);
                }

                builder.Append(']');
            }

            builder.Append(']');

            void Append(CharacterPattern pattern)
            {
                pattern.ToString(tmpBuilder, fromCharacterSet: true);
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
                        builder.Append('-');
                        Append(range.End);
                    }
                }
            }
        }

        // TODO: make class?
        public struct CharacterRange
        {
            public CharacterPattern Start { get; }

            public CharacterPattern End { get; }

            public bool Single { get; }

            public CharacterRange(char start, char end)
                : this(CharacterPattern.Character(start), CharacterPattern.Character(end)) { }

            public CharacterRange(CharacterPattern start, CharacterPattern end)
            {
                if (!(start is CharacterLiteralPattern startLiteral))
                {
                    throw new ArgumentException("Start range must be a character literal pattern.", nameof(start));
                }

                if (!(end is CharacterLiteralPattern endLiteral))
                {
                    throw new ArgumentException("End range must be a character literal pattern.", nameof(end));
                }

                var startValue = startLiteral.GetValue();
                var endValue = endLiteral.GetValue();

                if (startValue > endValue)
                {
                    throw new ArgumentException("Start range cannot be greater than end range.", nameof(start));
                }

                Start = start;
                End = end;
                Single = startValue == endValue;
            }
        }
    }
}
