using System;
using System.Text;

namespace Wilgysef.FluentRegex
{
    public class InlineModifierPattern : AbstractGroupPattern
    {
        public InlineModifier Modifiers { get; set; }

        public InlineModifier DisabledModifiers { get; set; }

        protected override bool HasContents => Modifiers != InlineModifier.None || DisabledModifiers != InlineModifier.None;

        public InlineModifierPattern(
            Pattern? pattern,
            InlineModifier modifiers,
            InlineModifier disabledModifiers = InlineModifier.None)
            : base(pattern)
        {
            WithModifiers(modifiers, disabledModifiers);
        }

        #region Fluent Methods

        public InlineModifierPattern WithModifiers(InlineModifier modifiers)
        {
            Modifiers = modifiers;
            return this;
        }

        public InlineModifierPattern WithModifiers(InlineModifier modifiers, InlineModifier disabledModifiers)
        {
            Modifiers = modifiers;
            DisabledModifiers = disabledModifiers;
            return this;
        }

        public InlineModifierPattern WithDisabledModifiers(InlineModifier modifiers)
        {
            DisabledModifiers = modifiers;
            return this;
        }

        public InlineModifierPattern WithIgnoreCase(bool enable = true) => SetModifier(InlineModifier.IgnoreCase, enable);

        public InlineModifierPattern WithMultiline(bool enable = true) => SetModifier(InlineModifier.Multiline, enable);

        public InlineModifierPattern WithExplicitCapture(bool enable = true) => SetModifier(InlineModifier.ExplicitCapture, enable);

        public InlineModifierPattern WithSingleline(bool enable = true) => SetModifier(InlineModifier.Singleline, enable);

        public InlineModifierPattern WithIgnorePatternWhitespace(bool enable = true) => SetModifier(InlineModifier.IgnorePatternWhitespace, enable);

        public InlineModifierPattern WithIgnoreCaseDisabled(bool enable = true) => SetDisabledModifier(InlineModifier.IgnoreCase, enable);

        public InlineModifierPattern WithMultilineDisabled(bool enable = true) => SetDisabledModifier(InlineModifier.Multiline, enable);

        public InlineModifierPattern WithExplicitCaptureDisabled(bool enable = true) => SetDisabledModifier(InlineModifier.ExplicitCapture, enable);

        public InlineModifierPattern WithSinglelineDisabled(bool enable = true) => SetDisabledModifier(InlineModifier.Singleline, enable);

        public InlineModifierPattern WithIgnorePatternWhitespaceDisabled(bool enable = true) => SetDisabledModifier(InlineModifier.IgnorePatternWhitespace, enable);

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

        protected override void GroupContents(StringBuilder builder)
        {
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
                Pattern.ToString(builder);
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

        private InlineModifierPattern SetModifier(InlineModifier modifier, bool enable)
        {
            if (enable)
            {
                Modifiers |= modifier;
            }
            else
            {
                Modifiers &= ~modifier;
            }

            return this;
        }

        private InlineModifierPattern SetDisabledModifier(InlineModifier modifier, bool enable)
        {
            if (enable)
            {
                DisabledModifiers |= modifier;
            }
            else
            {
                DisabledModifiers &= ~modifier;
            }

            return this;
        }
    }

    [Flags]
    public enum InlineModifier
    {
        None = 0,
        IgnoreCase = 1,
        Multiline = 2,
        ExplicitCapture = 4,
        Singleline = 16,
        IgnorePatternWhitespace = 32,
    }
}
