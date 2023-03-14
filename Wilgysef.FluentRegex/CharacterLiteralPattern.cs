﻿using System;
using System.Text;

namespace Wilgysef.FluentRegex
{
    internal class CharacterLiteralPattern : CharacterPattern
    {
        public static new CharacterPattern Character(char character) => new CharacterLiteralPattern(CharacterType.Character, character);

        public static new CharacterPattern Control(char character)
        {
            if (!IsControl(character))
            {
                throw new ArgumentException($"Invalid control character: {character}", nameof(character));
            }

            return new CharacterLiteralPattern(CharacterType.Control, character);
        }

        public static new CharacterPattern Escape => new CharacterLiteralPattern(CharacterType.Escape);

        public static new CharacterPattern Hexadecimal(string hex)
        {
            if (hex.Length != 2 || !IsHex(hex[0]) || !IsHex(hex[1]))
            {
                throw new ArgumentException($"Invalid hexadecimal: {hex}", nameof(hex));
            }

            return new CharacterLiteralPattern(CharacterType.Hexadecimal, hex);
        }

        public static new CharacterPattern Octal(string octal)
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

        public static new CharacterPattern Unicode(string hex)
        {
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

        internal override void ToString(StringBuilder builder)
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
                            builder.Append(@"\0");
                            break;
                        case '\\':
                            builder.Append(@"\\");
                            break;
                        case '\a':
                            builder.Append(@"\a");
                            break;
                        case '\b':
                            builder.Append(@"[\b]");
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
                            builder.Append(_character);
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
