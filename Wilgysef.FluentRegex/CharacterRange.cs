using System;

namespace Wilgysef.FluentRegex
{
    public class CharacterRange
    {
        public CharacterPattern Start { get; }

        public CharacterPattern End { get; }

        public bool Single { get; }

        public static CharacterRange Hexadecimal(string start, string end)
        {
            return new CharacterRange(
                CharacterPattern.Hexadecimal(start),
                CharacterPattern.Hexadecimal(end));
        }

        public static CharacterRange Octal(string start, string end)
        {
            return new CharacterRange(
                CharacterPattern.Octal(start),
                CharacterPattern.Octal(end));
        }

        public static CharacterRange Unicode(string start, string end)
        {
            return new CharacterRange(
                CharacterPattern.Unicode(start),
                CharacterPattern.Unicode(end));
        }

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
