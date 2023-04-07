namespace Wilgysef.FluentRegex.Enums
{
    public enum AnchorType
    {
        /// <summary>
        /// Matches the start of a string, or if in multiline mode, matches the beginning of a line.
        /// </summary>
        BeginLine,

        /// <summary>
        /// Matches the end of a string, or if in multiline mode, matches the end of a line.
        /// </summary>
        EndLine,

        /// <summary>
        /// Matches the start of a string.
        /// </summary>
        Start,

        /// <summary>
        /// Matches the end of a string. May match a trailing newline at the end of the string.
        /// </summary>
        End,

        /// <summary>
        /// Matches the end of a string.
        /// </summary>
        AbsoluteEnd,

        /// <summary>
        /// Match the starting point of the end of the previous sucessful match.
        /// </summary>
        StartOfMatch,

        /// <summary>
        /// Matches word boundary.
        /// </summary>
        WordBoundary,

        /// <summary>
        /// Matches non-word boundary.
        /// </summary>
        NonWordBoundary,
    }
}
