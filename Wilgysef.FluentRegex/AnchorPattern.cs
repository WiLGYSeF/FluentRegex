﻿using System;
using System.Text;

namespace Wilgysef.FluentRegex
{
    public class AnchorPattern : Pattern
    {
        /// <summary>
        /// Matches the start of a string, or if in multiline mode, matches the beginning of a line.
        /// </summary>
        public static AnchorPattern BeginLine => new AnchorPattern(AnchorType.BeginLine);

        /// <summary>
        /// Matches the end of a string, or if in multiline mode, matches the end of a line.
        /// </summary>
        public static AnchorPattern EndLine => new AnchorPattern(AnchorType.EndLine);

        /// <summary>
        /// Matches the start of a string.
        /// </summary>
        public static AnchorPattern Start => new AnchorPattern(AnchorType.Start);

        /// <summary>
        /// Matches the end of a string. May match a trailing newline at the end of the string.
        /// </summary>
        public static AnchorPattern End => new AnchorPattern(AnchorType.End);

        /// <summary>
        /// Matches the end of a string.
        /// </summary>
        public static AnchorPattern AbsoluteEnd => new AnchorPattern(AnchorType.AbsoluteEnd);

        /// <summary>
        /// Match the starting point of the end of the previous sucessful match.
        /// </summary>
        public static AnchorPattern StartOfMatch => new AnchorPattern(AnchorType.StartOfMatch);

        /// <summary>
        /// Matches word boundary.
        /// </summary>
        public static AnchorPattern WordBoundary => new AnchorPattern(AnchorType.WordBoundary);

        /// <summary>
        /// Matches non-word boundary.
        /// </summary>
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

        internal override void Build(PatternBuildState state)
        {
            state.WithPattern(this, Build);

            void Build(StringBuilder builder)
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
