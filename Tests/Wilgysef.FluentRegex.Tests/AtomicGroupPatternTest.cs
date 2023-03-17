namespace Wilgysef.FluentRegex.Tests;

public class AtomicGroupPatternTest
{
    [Fact]
    public void AtomicGroup()
    {
        var pattern = new PatternBuilder().AtomicGroup(new LiteralPattern("asdf"));

        pattern.ToString().ShouldBe("(?>asdf)");
    }

    [Fact]
    public void FluentPattern()
    {
        var pattern = new AtomicGroupPattern(new LiteralPattern("a"));

        pattern.WithPattern(new LiteralPattern("b"));
        pattern.ToString().ShouldBe("(?>b)");
    }
}
