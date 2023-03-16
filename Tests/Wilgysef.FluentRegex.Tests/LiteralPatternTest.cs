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
}
