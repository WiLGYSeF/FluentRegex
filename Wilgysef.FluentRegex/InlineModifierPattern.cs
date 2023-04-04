using System;
using Wilgysef.FluentRegex.PatternBuilders;

namespace Wilgysef.FluentRegex
{
    public class InlineModifierPattern : AbstractGroupPattern
    {
        /// <summary>
        /// Inline modifiers to enable.
        /// </summary>
        public InlineModifier Modifiers
        {
            get => _modifiers;
            set => _modifiers = value;
        }
        private InlineModifier _modifiers;

        /// <summary>
        /// Inline modifiers to disable.
        /// </summary>
        public InlineModifier DisabledModifiers
        {
            get => _disabledModifiers;
            set => _disabledModifiers = value;
        }
        private InlineModifier _disabledModifiers;

        protected override bool HasContents => Modifiers != InlineModifier.None || DisabledModifiers != InlineModifier.None || Pattern != null;

        /// <summary>
        /// Creates an inline modifier pattern.
        /// </summary>
        /// <param name="pattern">Pattern to match.</param>
        /// <param name="modifiers">Inline modifiers to enable.</param>
        /// <param name="disabledModifiers">Inline modifiers to disable.</param>
        public InlineModifierPattern(
            Pattern? pattern,
            InlineModifier modifiers,
            InlineModifier disabledModifiers = InlineModifier.None)
            : base(pattern)
        {
            WithModifiers(modifiers, disabledModifiers);
        }

        #region Fluent Methods

        /// <summary>
        /// Sets the inline modifiers to enable.
        /// </summary>
        /// <param name="modifiers">Inline modifiers to enable.</param>
        /// <returns>Current inline modifiers pattern.</returns>
        public InlineModifierPattern WithModifiers(InlineModifier modifiers)
        {
            Modifiers = modifiers;
            return this;
        }

        /// <summary>
        /// Sets the inline modifiers to enable and disable.
        /// </summary>
        /// <param name="modifiers">Inline modifiers to enable.</param>
        /// <param name="disabledModifiers">Inline modifiers to disable.</param>
        /// <returns>Current inline modifiers pattern.</returns>
        public InlineModifierPattern WithModifiers(InlineModifier modifiers, InlineModifier disabledModifiers)
        {
            Modifiers = modifiers;
            DisabledModifiers = disabledModifiers;
            return this;
        }

        /// <summary>
        /// Sets the inline modifiers to disable.
        /// </summary>
        /// <param name="modifiers">Inline modifiers to disable.</param>
        /// <returns>Current inline modifiers pattern.</returns>
        public InlineModifierPattern WithDisabledModifiers(InlineModifier modifiers)
        {
            DisabledModifiers = modifiers;
            return this;
        }

        /// <summary>
        /// Sets the <see cref="InlineModifier.IgnoreCase"/> modifier.
        /// </summary>
        /// <param name="enable">Indicates if the modifier is enabled, or <see langword="null"/> if neither enabled nor disabled.</param>
        /// <returns>Current inline modifiers pattern.</returns>
        public InlineModifierPattern WithIgnoreCase(bool? enable = true) => SetModifier(InlineModifier.IgnoreCase, enable);

        /// <summary>
        /// Sets the <see cref="InlineModifier.Multiline"/> modifier.
        /// </summary>
        /// <param name="enable">Indicates if the modifier is enabled, or <see langword="null"/> if neither enabled nor disabled.</param>
        /// <returns>Current inline modifiers pattern.</returns>
        public InlineModifierPattern WithMultiline(bool? enable = true) => SetModifier(InlineModifier.Multiline, enable);

        /// <summary>
        /// Sets the <see cref="InlineModifier.ExplicitCapture"/> modifier.
        /// </summary>
        /// <param name="enable">Indicates if the modifier is enabled, or <see langword="null"/> if neither enabled nor disabled.</param>
        /// <returns>Current inline modifiers pattern.</returns>
        public InlineModifierPattern WithExplicitCapture(bool? enable = true) => SetModifier(InlineModifier.ExplicitCapture, enable);

        /// <summary>
        /// Sets the <see cref="InlineModifier.Singleline"/> modifier.
        /// </summary>
        /// <param name="enable">Indicates if the modifier is enabled, or <see langword="null"/> if neither enabled nor disabled.</param>
        /// <returns>Current inline modifiers pattern.</returns>
        public InlineModifierPattern WithSingleline(bool? enable = true) => SetModifier(InlineModifier.Singleline, enable);

        /// <summary>
        /// Sets the <see cref="InlineModifier.IgnorePatternWhitespace"/> modifier.
        /// </summary>
        /// <param name="enable">Indicates if the modifier is enabled, or <see langword="null"/> if neither enabled nor disabled.</param>
        /// <returns>Current inline modifiers pattern.</returns>
        public InlineModifierPattern WithIgnorePatternWhitespace(bool? enable = true) => SetModifier(InlineModifier.IgnorePatternWhitespace, enable);

        /// <summary>
        /// Sets the pattern to match.
        /// </summary>
        /// <param name="pattern">Pattern to match.</param>
        /// <returns>Current inline modifier pattern.</returns>
        public InlineModifierPattern WithPattern(Pattern? pattern)
        {
            Pattern = pattern;
            return this;
        }

        #endregion

        public override Pattern Copy()
        {
            return new InlineModifierPattern(Pattern?.Copy(), Modifiers, DisabledModifiers);
        }

        private protected override void GroupContents(PatternBuildState state)
        {
            state.WithPattern(this, Build);

            void Build(IPatternStringBuilder builder)
            {
                // cancel out modifiers both enabled and disabled.
                var modifiers = Modifiers & ~DisabledModifiers;
                var disabledModifiers = DisabledModifiers & ~Modifiers;

                builder.Append('?');

                if (modifiers != InlineModifier.None)
                {
                    AppendFlags(modifiers);
                }

                if (disabledModifiers != InlineModifier.None)
                {
                    builder.Append('-');
                    AppendFlags(disabledModifiers);
                }

                if (Pattern != null)
                {
                    builder.Append(':');
                    Pattern.Build(state);
                }

                void AppendFlags(InlineModifier modifiers)
                {
                    if ((modifiers & InlineModifier.IgnoreCase) != 0)
                    {
                        builder.Append('i');
                    }
                    if ((modifiers & InlineModifier.Multiline) != 0)
                    {
                        builder.Append('m');
                    }
                    if ((modifiers & InlineModifier.ExplicitCapture) != 0)
                    {
                        builder.Append('n');
                    }
                    if ((modifiers & InlineModifier.Singleline) != 0)
                    {
                        builder.Append('s');
                    }
                    if ((modifiers & InlineModifier.IgnorePatternWhitespace) != 0)
                    {
                        builder.Append('x');
                    }
                }
            }
        }

        private InlineModifierPattern SetModifier(InlineModifier modifier, bool? enable)
        {
            if (enable.HasValue)
            {
                Set(ref _modifiers, enable.Value);
                Set(ref _disabledModifiers, !enable.Value);
            }
            else
            {
                Set(ref _modifiers, false);
                Set(ref _disabledModifiers, false);
            }

            return this;

            void Set(ref InlineModifier modf, bool b)
            {
                if (b)
                {
                    modf |= modifier;
                }
                else
                {
                    modf &= ~modifier;
                }
            }
        }
    }

    [Flags]
    public enum InlineModifier
    {
        /// <summary>
        /// No modifiers.
        /// </summary>
        None = 0,
        /// <summary>
        /// Case-insensitive matching.
        /// </summary>
        IgnoreCase = 1,
        /// <summary>
        /// Multi-line. <see cref="AnchorPattern.BeginLine"/> and <see cref="AnchorPattern.EndLine"/> match the beginning and end of lines instead of the entire string.
        /// </summary>
        Multiline = 2,
        /// <summary>
        /// All unnamed capturing groups are treated as non-capturing.
        /// </summary>
        ExplicitCapture = 4,
        /// <summary>
        /// Single-line. The single character pattern matches every character, instead of every character except <c>\n</c>.
        /// </summary>
        Singleline = 16,
        /// <summary>
        /// Ignore unescaped whitespace from the pattern.
        /// </summary>
        IgnorePatternWhitespace = 32,
    }
}
