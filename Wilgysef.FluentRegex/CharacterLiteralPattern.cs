using System;
using System.Text;

namespace Wilgysef.FluentRegex
{
    internal class CharacterLiteralPattern : CharacterPattern
    {
        internal static new CharacterPattern Character(char character) => new CharacterLiteralPattern(CharacterType.Character, character);

        internal static new CharacterPattern Control(char character)
        {
            if (!IsControl(character))
            {
                throw new ArgumentException($"Invalid control character: {character}", nameof(character));
            }

            return new CharacterLiteralPattern(CharacterType.Control, character);
        }

        internal static new CharacterPattern Escape => new CharacterLiteralPattern(CharacterType.Escape);

        internal static new CharacterPattern Hexadecimal(string hex)
        {
            if (hex.Length > 2 && hex[0] == '0' && (hex[1] == 'x' || hex[1] == 'X'))
            {
                hex = hex[2..];
            }

            if (hex.Length != 2 || !IsHex(hex[0]) || !IsHex(hex[1]))
            {
                throw new ArgumentException($"Invalid hexadecimal: {hex}", nameof(hex));
            }

            return new CharacterLiteralPattern(CharacterType.Hexadecimal, hex);
        }

        internal static new CharacterPattern Octal(string octal)
        {
            if ((octal.Length != 2 && octal.Length != 3)
                || !IsOctal(octal[0])
                || !IsOctal(octal[1])
                || (octal.Length == 3 && !IsOctal(octal[2])))
            {
                throw new ArgumentException($"Invalid octal: {octal}", nameof(octal));
            }

            return new CharacterLiteralPattern(CharacterType.Octal, octal);
        }

        internal static new CharacterPattern Unicode(string hex)
        {
            if (hex.Length > 2 && hex[0] == '0' && (hex[1] == 'x' || hex[1] == 'X'))
            {
                hex = hex[2..];
            }

            if (hex.Length != 4 || !IsHex(hex[0]) || !IsHex(hex[1]) || !IsHex(hex[2]) || !IsHex(hex[3]))
            {
                throw new ArgumentException($"Invalid unicode hexadecimal: {hex}", nameof(hex));
            }

            return new CharacterLiteralPattern(CharacterType.Unicode, hex);
        }

        private readonly char _character;
        private readonly string? _string;

        private CharacterLiteralPattern(CharacterType type)
        {
            Type = type;
        }

        private CharacterLiteralPattern(CharacterType type, char character)
        {
            Type = type;
            _character = character;
        }

        private CharacterLiteralPattern(CharacterType type, string str)
        {
            Type = type;
            _string = str;
        }

        public override bool TryGetChar(out char character)
        {
            if (Type == CharacterType.Character)
            {
                character = _character;
                return true;
            }

            character = (char)0;
            return false;
        }

        public override Pattern Copy()
        {
            return Type switch
            {
                CharacterType.Character => Character(_character),
                CharacterType.Control => Control(_character),
                CharacterType.Escape => Escape,
                CharacterType.Hexadecimal => Hexadecimal(_string!),
                CharacterType.Octal => Octal(_string!),
                CharacterType.Unicode => Unicode(_string!),
                _ => throw new ArgumentOutOfRangeException(),
            };
        }

        internal int GetValue()
        {
            return Type switch
            {
                CharacterType.Character => _character,
                CharacterType.Control => _character >= 'a' ? _character - 'a' : _character - 'A',
                CharacterType.Escape => 0x1B,
                CharacterType.Hexadecimal => int.Parse(_string, System.Globalization.NumberStyles.AllowHexSpecifier),
                CharacterType.Octal => Convert.ToInt32(_string, 8),
                CharacterType.Unicode => int.Parse(_string, System.Globalization.NumberStyles.AllowHexSpecifier),
                _ => throw new ArgumentOutOfRangeException(),
            };
        }

        internal override void Build(PatternBuildState state)
        {
            Build(state, false);
        }

        internal override void Build(PatternBuildState state, bool fromCharacterSet)
        {
            state.WithPattern(this, Build1);

            void Build1(StringBuilder builder)
            {
                Build(builder, fromCharacterSet);
            }
        }

        internal override void Build(StringBuilder builder, bool fromCharacterSet)
        {
            switch (Type)
            {
                case CharacterType.Character:
                    switch (_character)
                    {
                        case '.':
                            builder.Append(@"\.");
                            break;
                        case '\0':
                            builder.Append(@"\00");
                            break;
                        case '\\':
                            builder.Append(@"\\");
                            break;
                        case '\a':
                            builder.Append(@"\a");
                            break;
                        case '\b':
                            builder.Append(fromCharacterSet ? @"\b" : @"[\b]");
                            break;
                        case '\f':
                            builder.Append(@"\f");
                            break;
                        case '\n':
                            builder.Append(@"\n");
                            break;
                        case '\r':
                            builder.Append(@"\r");
                            break;
                        case '\t':
                            builder.Append(@"\t");
                            break;
                        case '\v':
                            builder.Append(@"\v");
                            break;
                        default:
                            if (fromCharacterSet || !LiteralPattern.EscapeChar(_character, out var escaped))
                            {
                                builder.Append(_character);
                            }
                            else
                            {
                                builder.Append(escaped);
                            }

                            break;
                    }
                    break;
                case CharacterType.Control:
                    builder.Append(@"\c");
                    builder.Append(_character);
                    break;
                case CharacterType.Escape:
                    builder.Append(@"\e");
                    break;
                case CharacterType.Hexadecimal:
                    builder.Append(@"\x");
                    builder.Append(_string);
                    break;
                case CharacterType.Octal:
                    builder.Append('\\');
                    builder.Append(_string);
                    break;
                case CharacterType.Unicode:
                    builder.Append(@"\u");
                    builder.Append(_string);
                    break;
            }
        }

        private static bool IsControl(char c)
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
                    return true;
                default:
                    return false;
            }
        }

        private static bool IsHex(char c)
        {
            switch (c)
            {
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
                case 'a':
                case 'b':
                case 'c':
                case 'd':
                case 'e':
                case 'f':
                case 'A':
                case 'B':
                case 'C':
                case 'D':
                case 'E':
                case 'F':
                    return true;
                default:
                    return false;
            }
        }

        private static bool IsOctal(char c)
        {
            switch (c)
            {
                case '0':
                case '1':
                case '2':
                case '3':
                case '4':
                case '5':
                case '6':
                case '7':
                    return true;
                default:
                    return false;
            }
        }
    }
}
