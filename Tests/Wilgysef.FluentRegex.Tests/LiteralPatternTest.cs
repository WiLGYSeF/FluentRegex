namespace Wilgysef.FluentRegex.Tests;

public class LiteralPatternTest
{
    [Fact]
    public void Literal()
    {
        var pattern = new PatternBuilder().Literal("test");

        pattern.ToString().ShouldBe("test");
    }

    [Fact]
    public void Escaped()
    {
        var pattern = new PatternBuilder().Literal(@"test$()*+.?[\]^{|}");

        pattern.ToString().ShouldBe(@"test\$\(\)\*\+\.\?\[\\\]\^\{\|\}");
    }

    [Fact]
    public void FluentValue()
    {
        var pattern = new LiteralPattern("a");

        pattern.WithValue("test");
        pattern.ToString().ShouldBe("test");
    }

    [Fact]
    public void Copy()
    {
        var pattern = new LiteralPattern("a");

        var copy = pattern.Copy();
        pattern.WithValue("b");
        copy.ToString().ShouldBe("a");
    }

    [Fact]
    public void Unwrap()
    {
        var pattern = new LiteralPattern("a");

        pattern.Unwrap().ShouldBe(pattern);
    }

    [Fact]
    public void EscapeString()
    {
        LiteralPattern.EscapeString(@"test$()*+.?[\]^{|}")
            .ShouldBe(@"test\$\(\)\*\+\.\?\[\\\]\^\{\|\}");
    }

    [Fact]
    public void EscapeChar()
    {
        LiteralPattern.EscapeChar('$').ShouldBe(@"\$");
        LiteralPattern.EscapeChar('(').ShouldBe(@"\(");
        LiteralPattern.EscapeChar(')').ShouldBe(@"\)");
        LiteralPattern.EscapeChar('*').ShouldBe(@"\*");
        LiteralPattern.EscapeChar('+').ShouldBe(@"\+");
        LiteralPattern.EscapeChar('.').ShouldBe(@"\.");
        LiteralPattern.EscapeChar('?').ShouldBe(@"\?");
        LiteralPattern.EscapeChar('[').ShouldBe(@"\[");
        LiteralPattern.EscapeChar('\\').ShouldBe(@"\\");
        LiteralPattern.EscapeChar(']').ShouldBe(@"\]");
        LiteralPattern.EscapeChar('^').ShouldBe(@"\^");
        LiteralPattern.EscapeChar('{').ShouldBe(@"\{");
        LiteralPattern.EscapeChar('|').ShouldBe(@"\|");
        LiteralPattern.EscapeChar('}').ShouldBe(@"\}");
        LiteralPattern.EscapeChar('a').ShouldBe("a");
    }

    [Fact]
    public void IsSpecialCharacter()
    {
        LiteralPattern.IsSpecialCharacter('$').ShouldBeTrue();
        LiteralPattern.IsSpecialCharacter('(').ShouldBeTrue();
        LiteralPattern.IsSpecialCharacter(')').ShouldBeTrue();
        LiteralPattern.IsSpecialCharacter('*').ShouldBeTrue();
        LiteralPattern.IsSpecialCharacter('+').ShouldBeTrue();
        LiteralPattern.IsSpecialCharacter('.').ShouldBeTrue();
        LiteralPattern.IsSpecialCharacter('?').ShouldBeTrue();
        LiteralPattern.IsSpecialCharacter('[').ShouldBeTrue();
        LiteralPattern.IsSpecialCharacter('\\').ShouldBeTrue();
        LiteralPattern.IsSpecialCharacter(']').ShouldBeTrue();
        LiteralPattern.IsSpecialCharacter('^').ShouldBeTrue();
        LiteralPattern.IsSpecialCharacter('{').ShouldBeTrue();
        LiteralPattern.IsSpecialCharacter('|').ShouldBeTrue();
        LiteralPattern.IsSpecialCharacter('}').ShouldBeTrue();
        LiteralPattern.IsSpecialCharacter('a').ShouldBeFalse();
    }
}
