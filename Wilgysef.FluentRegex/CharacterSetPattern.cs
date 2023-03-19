﻿using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;

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
        /// <param name="negated">Whether the character set is negated.</param>
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

            // TODO: remove duplicate chars?

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

        public class CharacterRange
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

            public CharacterRange Copy()
            {
                return new CharacterRange(
                    (CharacterPattern)Start.Copy(),
                    (CharacterPattern)End.Copy());
            }
        }
    }
}
