using System;
using System.Text;

namespace Wilgysef.FluentRegex
{
    internal class CharacterClassPattern : CharacterPattern
    {
        internal static new CharacterClassPattern Word => new CharacterClassPattern(CharacterType.Word);

        internal static new CharacterClassPattern NonWord => new CharacterClassPattern(CharacterType.NonWord);

        internal static new CharacterClassPattern Digit => new CharacterClassPattern(CharacterType.Digit);

        internal static new CharacterClassPattern NonDigit => new CharacterClassPattern(CharacterType.NonDigit);

        internal static new CharacterClassPattern Whitespace => new CharacterClassPattern(CharacterType.Whitespace);

        internal static new CharacterClassPattern NonWhitespace => new CharacterClassPattern(CharacterType.NonWhitespace);

        internal static new CharacterClassPattern Category(string category) => new CharacterClassPattern(CharacterType.Category, category);

        internal static new CharacterClassPattern NonCategory(string category) => new CharacterClassPattern(CharacterType.NonCategory, category);

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

        public override Pattern Copy()
        {
            return new CharacterClassPattern(Type, _category);
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

        internal override Pattern Unwrap()
        {
            return this;
        }
    }
}
