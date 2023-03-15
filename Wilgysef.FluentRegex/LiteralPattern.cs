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

        public static string EscapeChar(char c)
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
                    return @"\" + c;
                default:
                    return c.ToString();
            }
        }
    }
}
