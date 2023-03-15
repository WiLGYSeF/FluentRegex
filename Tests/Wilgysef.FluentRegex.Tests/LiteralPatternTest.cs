namespace Wilgysef.FluentRegex.Tests;

public class LiteralPatternTest
{
    [Fact]
    public void Simple()
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
}
