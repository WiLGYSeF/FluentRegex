using System;
using System.Collections.Generic;

namespace Wilgysef.FluentRegex
{
    public class CharacterRange
    {
        /// <summary>
        /// Start character pattern.
        /// </summary>
        public CharacterPattern Start { get; }

        /// <summary>
        /// End character pattern.
        /// </summary>
        public CharacterPattern End { get; }

        /// <summary>
        /// Indicates if the start and end characters are the same.
        /// </summary>
        public bool Single { get; }

        /// <summary>
        /// Indicates if the start and end characters are adjacent.
        /// </summary>
        public bool Adjacent { get; }

        /// <summary>
        /// Start character pattern value.
        /// </summary>
        internal int StartValue { get; }

        /// <summary>
        /// End character pattern value.
        /// </summary>
        internal int EndValue { get; }

        /// <summary>
        /// Creates a hexadecimal character range.
        /// </summary>
        /// <param name="start">Start hexadecimal.</param>
        /// <param name="end">End hexadecimal.</param>
        /// <returns>Character range.</returns>
        public static CharacterRange Hexadecimal(string start, string end)
        {
            return new CharacterRange(
                CharacterPattern.Hexadecimal(start),
                CharacterPattern.Hexadecimal(end));
        }

        /// <summary>
        /// Creates an octal character range.
        /// </summary>
        /// <param name="start">Start octal.</param>
        /// <param name="end">End octal.</param>
        /// <returns>Character range.</returns>
        public static CharacterRange Octal(string start, string end)
        {
            return new CharacterRange(
                CharacterPattern.Octal(start),
                CharacterPattern.Octal(end));
        }

        /// <summary>
        /// Creates a unicode character range.
        /// </summary>
        /// <param name="start">Start unicode.</param>
        /// <param name="end">End unicode.</param>
        /// <returns>Character range.</returns>
        public static CharacterRange Unicode(string start, string end)
        {
            return new CharacterRange(
                CharacterPattern.Unicode(start),
                CharacterPattern.Unicode(end));
        }

        /// <summary>
        /// Creates a character range.
        /// </summary>
        /// <param name="start">Start character.</param>
        /// <param name="end">End character.</param>
        public CharacterRange(char start, char end)
            : this(CharacterPattern.Character(start), CharacterPattern.Character(end)) { }

        /// <summary>
        /// Creates a character range.
        /// </summary>
        /// <param name="start">Start character pattern.</param>
        /// <param name="end">End character pattern.</param>
        /// <exception cref="ArgumentException">Pattern must be a character literal pattern.</exception>
        public CharacterRange(CharacterPattern start, CharacterPattern end)
        {
            StartValue = GetCharacterLiteralPattern(start).GetValue();
            EndValue = GetCharacterLiteralPattern(end).GetValue();

            if (StartValue > EndValue)
            {
                throw new ArgumentException("Start range cannot be greater than end range.", nameof(start));
            }

            Start = start;
            End = end;
            Single = StartValue == EndValue;
            Adjacent = EndValue - StartValue == 1;
        }

        /// <summary>
        /// Checks if the character range contains the character.
        /// </summary>
        /// <param name="character">Character.</param>
        /// <returns><see langword="true"/> if the character range contains the character, otherwise <see langword="false"/>.</returns>
        public bool Contains(char character)
        {
            return StartValue <= character && character <= EndValue;
        }

        /// <summary>
        /// Checks if the character range contains the pattern.
        /// </summary>
        /// <param name="pattern">Character pattern.</param>
        /// <returns><see langword="true"/> if the character range contains the pattern, otherwise <see langword="false"/>.</returns>
        public bool Contains(CharacterPattern pattern)
        {
            var value = GetCharacterLiteralPattern(pattern).GetValue();
            return StartValue <= value && value <= EndValue;
        }

        /// <summary>
        /// Creates a copy of the character range.
        /// </summary>
        /// <returns>Copied character range.</returns>
        public CharacterRange Copy()
        {
            return new CharacterRange((CharacterPattern)Start.Copy(), (CharacterPattern)End.Copy());
        }

        public override string ToString()
        {
            return Single
                ? Start.ToString()
                : $"{Start}-{End}";
        }

        /// <summary>
        /// Creates new character ranges with overlapped ranges simplified.
        /// </summary>
        /// <param name="ranges">Character ranges.</param>
        /// <returns>Simplified ranges.</returns>
        public static List<CharacterRange> Overlap(params CharacterRange[] ranges)
        {
            return Overlap((IEnumerable<CharacterRange>)ranges);
        }

        /// <summary>
        /// Creates new character ranges with overlapped ranges simplified.
        /// </summary>
        /// <param name="ranges">Character ranges.</param>
        /// <returns>Simplified ranges.</returns>
        public static List<CharacterRange> Overlap(IEnumerable<CharacterRange> ranges)
        {
            var points = new List<(int Value, CharacterPattern Pattern, int Index, bool IsStart)>();

            var rangeIndex = 0;
            foreach (var range in ranges)
            {
                points.Add((range.StartValue, range.Start, rangeIndex, true));
                points.Add((range.EndValue, range.End, rangeIndex, false));
                rangeIndex++;
            }

            if (points.Count == 0)
            {
                return new List<CharacterRange>();
            }

            points.Sort((a, b) => a.Value - b.Value);

            var rangeEntries = new List<(CharacterRange Range, int Index)>();
            var pointPair = 0;

            for (var i = 0; i < points.Count; i++)
            {
                var start = points[i];
                pointPair = 1;

                do
                {
                    for (i++; pointPair > 0; i++)
                    {
                        pointPair += points[i].IsStart ? 1 : -1;
                    }

                    if (i < points.Count && points[i].Value - points[i - 1].Value <= 1)
                    {
                        pointPair++;
                        i++;
                    }

                    i--;
                } while (pointPair > 0);

                rangeEntries.Add((new CharacterRange(start.Pattern, points[i].Pattern), start.Index));
            }

            rangeEntries.Sort((a, b) => a.Index - b.Index);

            var simplifiedRanges = new List<CharacterRange>(rangeEntries.Count);

            foreach (var rangeEntry in rangeEntries)
            {
                simplifiedRanges.Add(rangeEntry.Range);
            }

            return simplifiedRanges;
        }

        /// <summary>
        /// Checks if the character pattern is an adjacent character left of <see cref="Start"/>.
        /// </summary>
        /// <param name="pattern">Character pattern.</param>
        /// <returns><see langword="true"/> if the character is an adjacent character left of <see cref="Start"/>, otherwise <see langword="false"/>.</returns>
        internal bool IsAdjacentLeftOfStart(CharacterPattern pattern)
            => StartValue - GetCharacterLiteralPattern(pattern).GetValue() == 1;

        /// <summary>
        /// Checks if the character pattern is an adjacent character right of <see cref="End"/>.
        /// </summary>
        /// <param name="pattern">Character pattern.</param>
        /// <returns><see langword="true"/> if the character is an adjacent character right of <see cref="End"/>, otherwise <see langword="false"/>.</returns>
        internal bool IsAdjacentRightOfEnd(CharacterPattern pattern)
            => GetCharacterLiteralPattern(pattern).GetValue() - EndValue == 1;

        private static CharacterLiteralPattern GetCharacterLiteralPattern(CharacterPattern pattern)
        {
            return (pattern as CharacterLiteralPattern)
                ?? throw new ArgumentException("Pattern must be a character literal pattern.", nameof(pattern));
        }
    }
}
