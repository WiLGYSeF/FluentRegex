using System;
using System.Text;

namespace Wilgysef.FluentRegex
{
    public class AnchorPattern : Pattern
    {
        public static AnchorPattern BeginLine => new AnchorPattern(AnchorType.BeginLine);

        public static AnchorPattern EndLine => new AnchorPattern(AnchorType.EndLine);

        public static AnchorPattern Start => new AnchorPattern(AnchorType.Start);

        public static AnchorPattern End => new AnchorPattern(AnchorType.End);

        public static AnchorPattern AbsoluteEnd => new AnchorPattern(AnchorType.AbsoluteEnd);

        public static AnchorPattern StartOfMatch => new AnchorPattern(AnchorType.StartOfMatch);

        public static AnchorPattern WordBoundary => new AnchorPattern(AnchorType.WordBoundary);

        public static AnchorPattern NonWordBoundary => new AnchorPattern(AnchorType.NonWordBoundary);

        internal override bool IsSinglePattern => true;

        private readonly AnchorType _type;

        private AnchorPattern(AnchorType type)
        {
            _type = type;
        }

        public override Pattern Copy()
        {
            return new AnchorPattern(_type);
        }

        internal override void ToString(StringBuilder builder)
        {
            switch (_type)
            {
                case AnchorType.BeginLine:
                    builder.Append(@"^");
                    break;
                case AnchorType.EndLine:
                    builder.Append(@"$");
                    break;
                case AnchorType.Start:
                    builder.Append(@"\A");
                    break;
                case AnchorType.End:
                    builder.Append(@"\Z");
                    break;
                case AnchorType.AbsoluteEnd:
                    builder.Append(@"\z");
                    break;
                case AnchorType.StartOfMatch:
                    builder.Append(@"\G");
                    break;
                case AnchorType.WordBoundary:
                    builder.Append(@"\b");
                    break;
                case AnchorType.NonWordBoundary:
                    builder.Append(@"\B");
                    break;
                default:
                    throw new ArgumentOutOfRangeException();
            }
        }

        private enum AnchorType
        {
            BeginLine,
            EndLine,
            Start,
            End,
            AbsoluteEnd,
            StartOfMatch,
            WordBoundary,
            NonWordBoundary,
        }
    }
}
