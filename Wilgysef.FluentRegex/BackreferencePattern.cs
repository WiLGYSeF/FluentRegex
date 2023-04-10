using System;
using Wilgysef.FluentRegex.Exceptions;
using Wilgysef.FluentRegex.PatternStringBuilders;
using Wilgysef.FluentRegex.PatternStates;
using Wilgysef.FluentRegex.Enums;

namespace Wilgysef.FluentRegex
{
    public class BackreferencePattern : Pattern
    {
        /// <summary>
        /// Backreference group number.
        /// </summary>
        public int? GroupNumber { get; private set; }

        /// <summary>
        /// Backreference group name.
        /// </summary>
        public string? GroupName { get; private set; }

        /// <summary>
        /// Backreference type.
        /// </summary>
        public BackreferenceType Type { get; private set; }

        /// <summary>
        /// Backreference group value.
        /// <para>Type is <see langword="int"/> if <see cref="GroupNumber"/> is set.</para>
        /// <para>Type is <see langword="string"/> if <see cref="GroupName"/> is set.</para>
        /// </summary>
        public object Group => Type switch
        {
            BackreferenceType.Number => GroupNumber!.Value,
            BackreferenceType.Name => GroupName!,
            _ => throw new ArgumentOutOfRangeException(),
        };

        internal override bool IsSinglePattern => true;

        /// <summary>
        /// Creates a backreference with group number.
        /// </summary>
        /// <param name="group">Group number</param>
        public BackreferencePattern(int group)
        {
            WithGroup(group);
        }

        /// <summary>
        /// Creates a backreference with group name.
        /// </summary>
        /// <param name="group">Group name.</param>
        public BackreferencePattern(string group)
        {
            WithGroup(group);
        }

        /// <summary>
        /// Sets the group number.
        /// </summary>
        /// <param name="group">Group number.</param>
        /// <returns>Current backreference pattern.</returns>
        public BackreferencePattern WithGroup(int group)
        {
            GroupNumber = group;
            GroupName = null;
            Type = BackreferenceType.Number;
            return this;
        }

        /// <summary>
        /// Sets the group name.
        /// </summary>
        /// <param name="group">Group name.</param>
        /// <returns>Current backreference pattern.</returns>
        public BackreferencePattern WithGroup(string group)
        {
            GroupNumber = null;
            GroupName = group;
            Type = BackreferenceType.Name;
            return this;
        }

        public override Pattern Copy()
        {
            return Type switch
            {
                BackreferenceType.Number => new BackreferencePattern(GroupNumber!.Value),
                BackreferenceType.Name => new BackreferencePattern(GroupName!),
                _ => throw new ArgumentOutOfRangeException(),
            };
        }

        internal override void Build(PatternBuildState state)
        {
            state.WithPattern(this, Build);

            void Build(IPatternStringBuilder builder)
            {
                if (Type == BackreferenceType.Number)
                {
                    if (GroupNumber!.Value > 9)
                    {
                        throw new InvalidPatternException(this, "Backreference number exceeds limit of 9.");
                    }

                    builder.Append('\\');
                    builder.Append(GroupNumber!.Value);
                }
                else
                {
                    builder.Append(@"\k<");
                    builder.Append(GroupName!);
                    builder.Append('>');
                }
            }
        }

        internal override Pattern UnwrapInternal(PatternBuildState state)
        {
            return this;
        }

        internal override bool IsEmpty(PatternBuildState state)
        {
            return false;
        }
    }
}
