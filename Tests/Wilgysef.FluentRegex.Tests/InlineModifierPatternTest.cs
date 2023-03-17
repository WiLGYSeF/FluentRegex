﻿namespace Wilgysef.FluentRegex.Tests;

public class InlineModifierPatternTest
{
    [Fact]
    public void Simple()
    {
        var pattern = new PatternBuilder().Modifiers(InlineModifier.IgnoreCase);

        pattern.ToString().ShouldBe("(?i)");
    }

    [Theory]
    [InlineData(InlineModifier.None, InlineModifier.None, "")]
    [InlineData(InlineModifier.IgnoreCase | InlineModifier.IgnorePatternWhitespace, InlineModifier.None, "(?ix)")]
    [InlineData(InlineModifier.None, InlineModifier.Multiline | InlineModifier.ExplicitCapture, "(?-mn)")]
    [InlineData(InlineModifier.IgnoreCase | InlineModifier.Multiline, InlineModifier.ExplicitCapture | InlineModifier.Singleline | InlineModifier.IgnorePatternWhitespace, "(?im-nsx)")]
    [InlineData(InlineModifier.IgnoreCase | InlineModifier.Multiline, InlineModifier.IgnoreCase | InlineModifier.Singleline | InlineModifier.IgnorePatternWhitespace, "(?m-sx)")]
    public void Modifiers(InlineModifier modifiers, InlineModifier disabledModifiers, string expected)
    {
        var pattern = new PatternBuilder().Modifiers(modifiers, disabledModifiers);

        pattern.ToString().ShouldBe(expected);
    }

    [Fact]
    public void Modifiers_Group()
    {
        var pattern = new PatternBuilder().Modifiers(new LiteralPattern("a"), InlineModifier.IgnoreCase);

        pattern.ToString().ShouldBe("(?i:a)");

        pattern = new PatternBuilder().Modifiers(new LiteralPattern("a"), InlineModifier.IgnoreCase, InlineModifier.ExplicitCapture);
        pattern.ToString().ShouldBe("(?i-n:a)");
    }

    [Fact]
    public void FluentModifiers()
    {
        var pattern = new InlineModifierPattern(null, InlineModifier.None, InlineModifier.None);

        pattern.WithModifiers(InlineModifier.IgnoreCase);
        ShouldHaveModifier(pattern.Modifiers, InlineModifier.IgnoreCase);
        pattern.WithDisabledModifiers(InlineModifier.IgnoreCase);
        ShouldHaveModifier(pattern.DisabledModifiers, InlineModifier.IgnoreCase);

        pattern = new InlineModifierPattern(null, InlineModifier.None, InlineModifier.None);

        TestFluentSet(pattern.WithIgnoreCase, () => pattern.Modifiers, InlineModifier.IgnoreCase);
        TestFluentSet(pattern.WithMultiline, () => pattern.Modifiers, InlineModifier.Multiline);
        TestFluentSet(pattern.WithExplicitCapture, () => pattern.Modifiers, InlineModifier.ExplicitCapture);
        TestFluentSet(pattern.WithSingleline, () => pattern.Modifiers, InlineModifier.Singleline);
        TestFluentSet(pattern.WithIgnorePatternWhitespace, () => pattern.Modifiers, InlineModifier.IgnorePatternWhitespace);

        TestFluentSet(pattern.WithIgnoreCaseDisabled, () => pattern.DisabledModifiers, InlineModifier.IgnoreCase);
        TestFluentSet(pattern.WithMultilineDisabled, () => pattern.DisabledModifiers, InlineModifier.Multiline);
        TestFluentSet(pattern.WithExplicitCaptureDisabled, () => pattern.DisabledModifiers, InlineModifier.ExplicitCapture);
        TestFluentSet(pattern.WithSinglelineDisabled, () => pattern.DisabledModifiers, InlineModifier.Singleline);
        TestFluentSet(pattern.WithIgnorePatternWhitespaceDisabled, () => pattern.DisabledModifiers, InlineModifier.IgnorePatternWhitespace);

        static void TestFluentSet(Func<bool, InlineModifierPattern> action, Func<InlineModifier> getModifier, InlineModifier modifier)
        {
            action(true);
            ShouldHaveModifier(getModifier(), modifier);
            action(false);
            ShouldNotHaveModifier(getModifier(), modifier);
        }
    }

    private static void ShouldHaveModifier(InlineModifier modifiers, InlineModifier flag)
    {
        (modifiers & flag).ShouldBe(flag);
    }

    private static void ShouldNotHaveModifier(InlineModifier modifiers, InlineModifier flag)
    {
        (modifiers & flag).ShouldBe(InlineModifier.None);
    }
}