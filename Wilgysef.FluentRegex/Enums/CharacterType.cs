namespace Wilgysef.FluentRegex.Enums
{
    public enum CharacterType
    {
        /// <summary>
        /// Matches a character.
        /// </summary>
        Character,

        /// <summary>
        /// Matches a control character.
        /// </summary>
        Control,

        /// <summary>
        /// Matches an escape character <c>\e</c>.
        /// </summary>
        Escape,

        /// <summary>
        /// Matches a character with hexidecimal value.
        /// </summary>
        Hexadecimal,

        /// <summary>
        /// Matches a character with octal value.
        /// </summary>
        Octal,

        /// <summary>
        /// Matches a character with unicode value.
        /// </summary>
        Unicode,

        /// <summary>
        /// Matches a word character.
        /// </summary>
        Word,

        /// <summary>
        /// Matches a non-word character.
        /// </summary>
        NonWord,

        /// <summary>
        /// Matches a digit character.
        /// </summary>
        Digit,

        /// <summary>
        /// Matches a non-digit character.
        /// </summary>
        NonDigit,

        /// <summary>
        /// Matches a whitespace character.
        /// </summary>
        Whitespace,

        /// <summary>
        /// Matches a non-whitespace character.
        /// </summary>
        NonWhitespace,

        /// <summary>
        /// Matches a character that belongs to a category.
        /// </summary>
        Category,

        /// <summary>
        /// Matches a character that does not belong to a category.
        /// </summary>
        NonCategory,
    }
}
