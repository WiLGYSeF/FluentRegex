namespace Wilgysef.FluentRegex.Tests;

public class AtomicGroupPatternTest
{
    [Fact]
    public void AtomicGroup()
    {
        var pattern = new PatternBuilder().AtomicGroup(new LiteralPattern("asdf"));

        pattern.ToString().ShouldBe("(?>asdf)");
    }
}
