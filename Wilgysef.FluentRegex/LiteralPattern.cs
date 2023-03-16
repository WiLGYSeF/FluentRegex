using System.Diagnostics.CodeAnalysis;
using System.Text;

namespace Wilgysef.FluentRegex
{
    public class LiteralPattern : Pattern
    {
        public string Value { get; set; }

        internal override bool IsSinglePattern => true;

        public LiteralPattern(string value)
        {
            Value = value;
        }

        public LiteralPattern WithValue(string value)
        {
            Value = value;
            return this;
        }

        internal override void ToString(StringBuilder builder)
        {
            EscapeString(builder, Value);
        }

        public static string EscapeString(string pattern)
        {
            var builder = new StringBuilder(pattern.Length);
            EscapeString(builder, pattern);
            return builder.ToString();
        }

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

        public static bool EscapeChar(char c, [MaybeNullWhen(false)] out string escaped)
        {
            switch (c)
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

        public static string EscapeChar(char c)
        {
            return EscapeChar(c, out var escaped) ? escaped : c.ToString();
        }
    }
}
