namespace Wilgysef.FluentRegex.Tests;

public class LookaheadPatternTest
{
    [Fact]
    public void PositiveLookahead()
    {
        var pattern = new PatternBuilder().PositiveLookahead(new LiteralPattern("abc"));

        pattern.ToString().ShouldBe("(?=abc)");
    }

    [Fact]
    public void NegativeLookahead()
    {
        var pattern = new PatternBuilder().NegativeLookahead(new LiteralPattern("abc"));

        pattern.ToString().ShouldBe("(?!abc)");
    }

    [Fact]
    public void PositiveLookbehind()
    {
        var pattern = new PatternBuilder().PositiveLookbehind(new LiteralPattern("abc"));

        pattern.ToString().ShouldBe("(?<=abc)");
    }

    [Fact]
    public void NegativeLookbehind()
    {
        var pattern = new PatternBuilder().NegativeLookbehind(new LiteralPattern("abc"));

        pattern.ToString().ShouldBe("(?<!abc)");
    }
}
