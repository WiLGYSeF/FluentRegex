using System;
using System.Text;

namespace Wilgysef.FluentRegex
{
    public class GroupPattern : AbstractGroupPattern
    {
        public string? Name { get; set; }

        public string? SecondName { get; set; }

        public bool Capture { get; set; }

        public bool IsCapturing => Name != null || Capture;

        public bool IsNumbered => Capture && Name == null;

        protected override bool HasContents => true;

        public GroupPattern(Pattern? pattern, string? name = null, bool capture = true) : base(pattern)
        {
            Name = name;
            Capture = capture;
        }

        public GroupPattern(Pattern? pattern, string? name1, string name2) : base(pattern)
        {
            Balancing(name1, name2);
        }

        public GroupPattern WithName(string? name)
        {
            Name = name;
            return this;
        }

        public GroupPattern WithSecondName(string? name)
        {
            SecondName = name;
            return this;
        }

        public GroupPattern Balancing(string? name1, string name2)
        {
            Name = name1;
            SecondName = name2;
            return this;
        }

        public GroupPattern WithCapture(bool capture = true)
        {
            Capture = capture;
            return this;
        }

        public GroupPattern WithPattern(Pattern? pattern)
        {
            Pattern = pattern;
            return this;
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

        internal static void NonCaptureGroup(StringBuilder builder, Pattern pattern)
        {
            builder.Append("(?:");
            pattern.ToString(builder);
            builder.Append(')');
        }
    }
}
