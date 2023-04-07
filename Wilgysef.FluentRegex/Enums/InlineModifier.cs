using System;

namespace Wilgysef.FluentRegex.Enums
{
    [Flags]
    public enum InlineModifier
    {
        /// <summary>
        /// No modifiers.
        /// </summary>
        None = 0,

        /// <summary>
        /// Case-insensitive matching.
        /// </summary>
        IgnoreCase = 1,

        /// <summary>
        /// Multi-line. <see cref="AnchorPattern.BeginLine"/> and <see cref="AnchorPattern.EndLine"/> match the beginning and end of lines instead of the entire string.
        /// </summary>
        Multiline = 2,

        /// <summary>
        /// All unnamed capturing groups are treated as non-capturing.
        /// </summary>
        ExplicitCapture = 4,

        /// <summary>
        /// Single-line. The single character pattern matches every character, instead of every character except <c>\n</c>.
        /// </summary>
        Singleline = 16,

        /// <summary>
        /// Ignore unescaped whitespace from the pattern.
        /// </summary>
        IgnorePatternWhitespace = 32,
    }
}
