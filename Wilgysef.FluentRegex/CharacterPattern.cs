namespace Wilgysef.FluentRegex
{
    public abstract class CharacterPattern : Pattern
    {
        public static CharacterPattern Character(char character) => CharacterLiteralPattern.Character(character);

        public static CharacterPattern Control(char character) => CharacterLiteralPattern.Control(character);

        public static CharacterPattern Escape => CharacterLiteralPattern.Escape;

        public static CharacterPattern Hexadecimal(string hex) => CharacterLiteralPattern.Hexadecimal(hex);

        public static CharacterPattern Octal(string octal) => CharacterLiteralPattern.Octal(octal);

        public static CharacterPattern Unicode(string hex) => CharacterLiteralPattern.Unicode(hex);

        public static CharacterPattern Word => CharacterClassPattern.Word;

        public static CharacterPattern NonWord => CharacterClassPattern.NonWord;

        public static CharacterPattern Digit => CharacterClassPattern.Digit;

        public static CharacterPattern NonDigit => CharacterClassPattern.NonDigit;

        public static CharacterPattern Whitespace => CharacterClassPattern.Whitespace;

        public static CharacterPattern NonWhitespace => CharacterClassPattern.NonWhitespace;

        public static CharacterPattern Category(string category) => CharacterClassPattern.Category(category);

        public static CharacterPattern NonCategory(string category) => CharacterClassPattern.NonCategory(category);

        protected CharacterType Type { get; set; }

        internal override bool IsSinglePattern => true;

        public abstract bool TryGetChar(out char character);

        protected enum CharacterType
        {
            Character,
            Control,
            Escape,
            Hexadecimal,
            Octal,
            Unicode,

            Word,
            NonWord,
            Digit,
            NonDigit,
            Whitespace,
            NonWhitespace,
            Category,
            NonCategory,
        }
    }
}
