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

    [Fact]
    public void FluentPattern()
    {
        var pattern = LookaheadPattern.PositiveLookahead(new LiteralPattern("a"));

        pattern.WithPattern(new LiteralPattern("b"));
        pattern.ToString().ShouldBe("(?=b)");
    }

    [Fact]
    public void NoPattern()
    {
        var pattern = new PatternBuilder().PositiveLookahead(null);

        pattern.ToString().ShouldBe("(?=)");
    }

    [Fact]
    public void Copy()
    {
        var literal = new LiteralPattern("a");

        var pattern = LookaheadPattern.PositiveLookahead(literal);
        var copy = pattern.Copy();
        literal.WithValue("b");
        copy.ToString().ShouldBe("(?=a)");

        pattern = LookaheadPattern.PositiveLookahead(null);
        copy = pattern.Copy();
        pattern.WithPattern(literal);
        copy.ToString().ShouldBe("(?=)");
    }
}
