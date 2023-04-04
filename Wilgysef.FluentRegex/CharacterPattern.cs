using Wilgysef.FluentRegex.PatternBuilders;

namespace Wilgysef.FluentRegex
{
    public abstract class CharacterPattern : Pattern
    {
        /// <summary>
        /// Matches a character.
        /// </summary>
        /// <param name="character">Character.</param>
        /// <returns>Character pattern.</returns>
        public static CharacterPattern Character(char character) => CharacterLiteralPattern.Character(character);

        /// <summary>
        /// Matches a control character.
        /// </summary>
        /// <param name="character">A control character between <c>'A'</c> and <c>'Z'</c>, case insensitive.</param>
        /// <returns>Character pattern.</returns>
        public static CharacterPattern Control(char character) => CharacterLiteralPattern.Control(character);

        /// <summary>
        /// Matches an escape character <c>\e</c>.
        /// </summary>
        public static CharacterPattern Escape => CharacterLiteralPattern.Escape;

        /// <summary>
        /// Matches a character with hexidecimal value.
        /// </summary>
        /// <param name="hex">Hexadecimal between <c>0x00</c> and <c>0xFF</c>.</param>
        /// <returns>Character pattern.</returns>
        public static CharacterPattern Hexadecimal(string hex) => CharacterLiteralPattern.Hexadecimal(hex);

        /// <summary>
        /// Matches a character with octal value.
        /// </summary>
        /// <param name="octal">Octal between <c>00</c> and <c>777</c>.</param>
        /// <returns>Character pattern.</returns>
        public static CharacterPattern Octal(string octal) => CharacterLiteralPattern.Octal(octal);

        /// <summary>
        /// Matches a character with unicode value.
        /// </summary>
        /// <param name="hex">Unicode value between <c>0000</c> and <c>FFFF</c>.</param>
        /// <returns>Character pattern.</returns>
        public static CharacterPattern Unicode(string hex) => CharacterLiteralPattern.Unicode(hex);

        /// <summary>
        /// Matches a word character.
        /// </summary>
        public static CharacterPattern Word => CharacterClassPattern.Word;

        /// <summary>
        /// Matches a non-word character.
        /// </summary>
        public static CharacterPattern NonWord => CharacterClassPattern.NonWord;

        /// <summary>
        /// Matches a digit character.
        /// </summary>
        public static CharacterPattern Digit => CharacterClassPattern.Digit;

        /// <summary>
        /// Matches a non-digit character.
        /// </summary>
        public static CharacterPattern NonDigit => CharacterClassPattern.NonDigit;

        /// <summary>
        /// Matches a whitespace character.
        /// </summary>
        public static CharacterPattern Whitespace => CharacterClassPattern.Whitespace;

        /// <summary>
        /// Matches a non-whitespace character.
        /// </summary>
        public static CharacterPattern NonWhitespace => CharacterClassPattern.NonWhitespace;

        /// <summary>
        /// Matches a character that belongs to a category.
        /// </summary>
        /// <param name="category">Category.</param>
        /// <returns>Character pattern.</returns>
        public static CharacterPattern Category(string category) => CharacterClassPattern.Category(category);

        /// <summary>
        /// Matches a character that does not belong to a category.
        /// </summary>
        /// <param name="category">Category.</param>
        /// <returns>Character pattern.</returns>
        public static CharacterPattern NonCategory(string category) => CharacterClassPattern.NonCategory(category);

        protected CharacterType Type { get; set; }

        internal override bool IsSinglePattern => true;

        /// <summary>
        /// Try to get the character value of the character pattern.
        /// </summary>
        /// <param name="character">Character value.</param>
        /// <returns><see langword="true"/> if the character pattern has a <see langword="char"/> value, otherwise <see langword="false"/>.</returns>
        public abstract bool TryGetChar(out char character);

        internal abstract void Build(PatternBuildState state, bool fromCharacterSet);

        internal abstract void Build(IPatternStringBuilder builder, bool fromCharacterSet);

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
