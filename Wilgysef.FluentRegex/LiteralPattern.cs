using System.Diagnostics.CodeAnalysis;
using System.Text;
using Wilgysef.FluentRegex.PatternBuilders;

namespace Wilgysef.FluentRegex
{
    public class LiteralPattern : Pattern
    {
        /// <summary>
        /// Literal value.
        /// </summary>
        public string Value { get; set; }

        internal override bool IsSinglePattern => Value.Length <= 1;

        internal override bool IsEmpty => Value.Length == 0;

        /// <summary>
        /// Creates a literal pattern.
        /// </summary>
        /// <param name="value">Literal value.</param>
        public LiteralPattern(string value)
        {
            Value = value;
        }

        /// <summary>
        /// Sets the literal value.
        /// </summary>
        /// <param name="value">Literal value.</param>
        /// <returns>Current literal pattern.</returns>
        public LiteralPattern WithValue(string value)
        {
            Value = value;
            return this;
        }

        public override Pattern Copy()
        {
            return new LiteralPattern(Value);
        }

        internal override void Build(PatternBuildState state)
        {
            state.WithPattern(this, Build);

            void Build(IPatternStringBuilder builder)
            {
                EscapeString(builder, Value);
            }
        }

        internal override Pattern Unwrap()
        {
            return this;
        }

        /// <summary>
        /// Escapes a pattern string.
        /// </summary>
        /// <param name="pattern">Pattern string.</param>
        /// <returns>Escaped string.</returns>
        public static string EscapeString(string pattern)
        {
            var builder = new StringBuilder(pattern.Length);
            EscapeString(builder, pattern);
            return builder.ToString();
        }

        /// <summary>
        /// Escapes a pattern string.
        /// </summary>
        /// <param name="builder">String builder.</param>
        /// <param name="pattern">Pattern string.</param>
        public static void EscapeString(StringBuilder builder, string pattern)
        {
            foreach (var c in pattern)
            {
                switch (c)
                {
                    case '$':
                    case '(':
                    case ')':
                    case '*':
                    case '+':
                    case '.':
                    case '?':
                    case '[':
                    case '\\':
                    case ']':
                    case '^':
                    case '{':
                    case '|':
                    case '}':
                        builder.Append('\\');
                        break;
                }

                builder.Append(c);
            }
        }

        /// <summary>
        /// Escapes a pattern string.
        /// </summary>
        /// <param name="builder">String builder.</param>
        /// <param name="pattern">Pattern string.</param>
        internal static void EscapeString(IPatternStringBuilder builder, string pattern)
        {
            foreach (var c in pattern)
            {
                switch (c)
                {
                    case '$':
                    case '(':
                    case ')':
                    case '*':
                    case '+':
                    case '.':
                    case '?':
                    case '[':
                    case '\\':
                    case ']':
                    case '^':
                    case '{':
                    case '|':
                    case '}':
                        builder.Append('\\');
                        break;
                }

                builder.Append(c);
            }
        }

        /// <summary>
        /// Escapes a pattern character.
        /// </summary>
        /// <param name="character">Pattern character.</param>
        /// <param name="escaped">Escaped character string, or <see langword="null"/> if character does not need to be escaped.</param>
        /// <returns><see langword="true"/> if the character needs to be escaped, otherwise <see langword="false"/>.</returns>
        public static bool EscapeChar(char character, [MaybeNullWhen(false)] out string escaped)
        {
            switch (character)
            {
                case '$':
                    escaped = @"\$";
                    return true;
                case '(':
                    escaped = @"\(";
                    return true;
                case ')':
                    escaped = @"\)";
                    return true;
                case '*':
                    escaped = @"\*";
                    return true;
                case '+':
                    escaped = @"\+";
                    return true;
                case '.':
                    escaped = @"\.";
                    return true;
                case '?':
                    escaped = @"\?";
                    return true;
                case '[':
                    escaped = @"\[";
                    return true;
                case '\\':
                    escaped = @"\\";
                    return true;
                case ']':
                    escaped = @"\]";
                    return true;
                case '^':
                    escaped = @"\^";
                    return true;
                case '{':
                    escaped = @"\{";
                    return true;
                case '|':
                    escaped = @"\|";
                    return true;
                case '}':
                    escaped = @"\}";
                    return true;
                default:
                    escaped = null;
                    return false;
            }
        }

        /// <summary>
        /// Escapes a pattern character.
        /// </summary>
        /// <param name="character">Character.</param>
        /// <returns>Escaped pattern character.</returns>
        public static string EscapeChar(char character)
        {
            return EscapeChar(character, out var escaped) ? escaped : character.ToString();
        }

        /// <summary>
        /// Checks if a character is special.
        /// </summary>
        /// <param name="character">Character.</param>
        /// <returns><see langword="true"/> if the character is special, otherwise <see langword="false"/>.</returns>
        public static bool IsSpecialCharacter(char character)
        {
            switch (character)
            {
                case '$':
                case '(':
                case ')':
                case '*':
                case '+':
                case '.':
                case '?':
                case '[':
                case '\\':
                case ']':
                case '^':
                case '{':
                case '|':
                case '}':
                    return true;
                default:
                    return false;
            }
        }
    }
}
