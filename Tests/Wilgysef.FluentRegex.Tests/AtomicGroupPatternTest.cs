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

    [Fact]
    public void Copy()
    {
        var literal = new LiteralPattern("a");
        var pattern = new PatternBuilder().AtomicGroup(literal);

        var copy = pattern.Copy();
        literal.WithValue("b");

        copy.ToString().ShouldBe("(?>a)");

        var group = new AtomicGroupPattern(null);
        copy = group.Copy();
        group.WithPattern(copy);

        copy.ToString().ShouldBe("(?>)");
    }
}
