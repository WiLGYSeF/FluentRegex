using System;

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
        /// <exception cref="ArgumentException"></exception>
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
            Adjacent = endValue - startValue == 1;
        }

        /// <summary>
        /// Creates a copy of the character range.
        /// </summary>
        /// <returns>Copied character range.</returns>
        public CharacterRange Copy()
        {
            return new CharacterRange(
                (CharacterPattern)Start.Copy(),
                (CharacterPattern)End.Copy());
        }
    }
}
