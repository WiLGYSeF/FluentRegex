﻿using System;
using System.Text;

namespace Wilgysef.FluentRegex
{
    internal class CharacterClassPattern : CharacterPattern
    {
        public static new CharacterClassPattern Word => new CharacterClassPattern(CharacterType.Word);

        public static new CharacterClassPattern NonWord => new CharacterClassPattern(CharacterType.NonWord);

        public static new CharacterClassPattern Digit => new CharacterClassPattern(CharacterType.Digit);

        public static new CharacterClassPattern NonDigit => new CharacterClassPattern(CharacterType.NonDigit);

        public static new CharacterClassPattern Whitespace => new CharacterClassPattern(CharacterType.Whitespace);

        public static new CharacterClassPattern NonWhitespace => new CharacterClassPattern(CharacterType.NonWhitespace);

        public static new CharacterClassPattern Category(string category) => new CharacterClassPattern(CharacterType.Category, category);

        public static new CharacterClassPattern NonCategory(string category) => new CharacterClassPattern(CharacterType.NonCategory, category);

        private readonly string? _category;

        private CharacterClassPattern(CharacterType type, string? category = null)
        {
            Type = type;
            _category = category;
        }

        public override bool TryGetChar(out char character)
        {
            character = (char)0;
            return false;
        }

        internal override void ToString(StringBuilder builder)
        {
            ToString(builder, false);
        }

        internal override void ToString(StringBuilder builder, bool fromCharacterSet)
        {
            switch (Type)
            {
                case CharacterType.Word:
                    builder.Append(@"\w");
                    break;
                case CharacterType.NonWord:
                    builder.Append(@"\W");
                    break;
                case CharacterType.Digit:
                    builder.Append(@"\d");
                    break;
                case CharacterType.NonDigit:
                    builder.Append(@"\D");
                    break;
                case CharacterType.Whitespace:
                    builder.Append(@"\s");
                    break;
                case CharacterType.NonWhitespace:
                    builder.Append(@"\S");
                    break;
                case CharacterType.Category:
                    builder.Append(@"\p{");
                    builder.Append(_category);
                    builder.Append('}');
                    break;
                case CharacterType.NonCategory:
                    builder.Append(@"\P{");
                    builder.Append(_category);
                    builder.Append('}');
                    break;
                default:
                    throw new ArgumentOutOfRangeException();
            }
        }
    }
}
