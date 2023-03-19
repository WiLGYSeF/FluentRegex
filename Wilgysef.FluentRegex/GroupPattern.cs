using System;
using System.Text;

namespace Wilgysef.FluentRegex
{
    public class GroupPattern : AbstractGroupPattern
    {
        /// <summary>
        /// Group name.
        /// </summary>
        public string? Name { get; set; }

        /// <summary>
        /// Second group name, if a balancing group.
        /// </summary>
        public string? SecondName { get; set; }

        /// <summary>
        /// Indicates if the group is capturing.
        /// </summary>
        public bool Capture { get; set; }

        /// <summary>
        /// Indicates if the group will capture.
        /// </summary>
        public bool IsCapturing => Name != null || Capture;

        /// <summary>
        /// Indicates if the group is a numbered capturing group.
        /// </summary>
        public bool IsNumbered => Capture && Name == null;

        /// <summary>
        /// Indicates if the group is a balancing group.
        /// </summary>
        public bool IsBalancing => SecondName != null;

        protected override bool HasContents => true;

        /// <summary>
        /// Creates a group pattern.
        /// </summary>
        /// <param name="pattern">Pattern to match.</param>
        /// <param name="name">Group name.</param>
        /// <param name="capture">Whether the group is capturing.</param>
        public GroupPattern(Pattern? pattern, string? name = null, bool capture = true) : base(pattern)
        {
            Name = name;
            Capture = capture;
        }

        /// <summary>
        /// Creates a balancing group pattern.
        /// </summary>
        /// <param name="pattern">Pattern to match.</param>
        /// <param name="name1">First group name.</param>
        /// <param name="name2">Second group name.</param>
        public GroupPattern(Pattern? pattern, string? name1, string name2) : base(pattern)
        {
            Balancing(name1, name2);
        }

        /// <summary>
        /// Sets the group name.
        /// </summary>
        /// <param name="name">Group name.</param>
        /// <returns>Current group pattern.</returns>
        public GroupPattern WithName(string? name)
        {
            Name = name;
            return this;
        }

        /// <summary>
        /// Sets the second group name.
        /// </summary>
        /// <param name="name">Group name.</param>
        /// <returns>Current group pattern.</returns>
        public GroupPattern WithSecondName(string? name)
        {
            SecondName = name;
            return this;
        }

        /// <summary>
        /// Sets the group as a balancing group.
        /// </summary>
        /// <param name="name1">First group name.</param>
        /// <param name="name2">Second group name.</param>
        /// <returns>Current group pattern.</returns>
        public GroupPattern Balancing(string? name1, string name2)
        {
            Name = name1;
            SecondName = name2;
            return this;
        }

        /// <summary>
        /// Sets if the group is capturing.
        /// </summary>
        /// <param name="capture">Whether the group is capturing.</param>
        /// <returns>Current group pattern.</returns>
        public GroupPattern WithCapture(bool capture = true)
        {
            Capture = capture;
            return this;
        }

        /// <summary>
        /// Sets the pattern to match.
        /// </summary>
        /// <param name="pattern">Pattern to match.</param>
        /// <returns>Current group pattern.</returns>
        public GroupPattern WithPattern(Pattern? pattern)
        {
            Pattern = pattern;
            return this;
        }

        public override Pattern Copy()
        {
            return IsBalancing
                ? new GroupPattern(Pattern?.Copy(), Name, SecondName!)
                : new GroupPattern(Pattern?.Copy(), Name, Capture);
        }

        protected override void GroupContents(StringBuilder builder)
        {
            if (Name != null && !IsValidName(Name))
            {
                throw new InvalidOperationException("Group name is not valid.");
            }
            if (SecondName != null && !IsValidName(SecondName))
            {
                throw new InvalidOperationException("Group second name is not valid.");
            }

            if (IsCapturing)
            {
                if (Name != null || SecondName != null)
                {
                    builder.Append("?<");

                    if (Name != null)
                    {
                        builder.Append(Name);
                    }

                    if (SecondName != null)
                    {
                        builder.Append('-');
                        builder.Append(SecondName);
                    }

                    builder.Append('>');
                }
            }
            else
            {
                builder.Append("?:");
            }

            Pattern?.ToString(builder);
        }

        /// <summary>
        /// Checks if the name is a valid group name.
        /// </summary>
        /// <param name="name">Group name.</param>
        /// <returns><see langword="true"/> if the name is a valid group name, otherwise <see langword="false"/>.</returns>
        public static bool IsValidName(string name)
        {
            if (name.Length == 0)
            {
                return false;
            }

            foreach (var c in name)
            {
                switch (c)
                {
                    case 'A':
                    case 'B':
                    case 'C':
                    case 'D':
                    case 'E':
                    case 'F':
                    case 'G':
                    case 'H':
                    case 'I':
                    case 'J':
                    case 'K':
                    case 'L':
                    case 'M':
                    case 'N':
                    case 'O':
                    case 'P':
                    case 'Q':
                    case 'R':
                    case 'S':
                    case 'T':
                    case 'U':
                    case 'V':
                    case 'W':
                    case 'X':
                    case 'Y':
                    case 'Z':
                    case 'a':
                    case 'b':
                    case 'c':
                    case 'd':
                    case 'e':
                    case 'f':
                    case 'g':
                    case 'h':
                    case 'i':
                    case 'j':
                    case 'k':
                    case 'l':
                    case 'm':
                    case 'n':
                    case 'o':
                    case 'p':
                    case 'q':
                    case 'r':
                    case 's':
                    case 't':
                    case 'u':
                    case 'v':
                    case 'w':
                    case 'x':
                    case 'y':
                    case 'z':
                    case '0':
                    case '1':
                    case '2':
                    case '3':
                    case '4':
                    case '5':
                    case '6':
                    case '7':
                    case '8':
                    case '9':
                    case '_':
                        break;
                    default:
                        return false;
                }
            }

            return true;
        }

        /// <summary>
        /// Wraps <paramref name="pattern"/> in a non-capturing group.
        /// </summary>
        /// <param name="builder">String builder.</param>
        /// <param name="pattern">Pattern to wrap.</param>
        internal static void NonCaptureGroup(StringBuilder builder, Pattern pattern)
        {
            builder.Append("(?:");
            pattern.ToString(builder);
            builder.Append(')');
        }
    }
}
