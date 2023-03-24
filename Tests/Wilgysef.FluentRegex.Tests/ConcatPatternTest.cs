using Wilgysef.FluentRegex.Exceptions;

namespace Wilgysef.FluentRegex.Tests;

public class ConcatPatternTest
{
    [Fact]
    public void Concat()
    {
        var pattern = new PatternBuilder()
            .Concat(new LiteralPattern("asdf"))
            .Concat(new List<LiteralPattern> { new LiteralPattern("123") });

        pattern.ToString().ShouldBe("asdf123");
    }

    [Fact]
    public void FluentConcat()
    {
        var pattern = new ConcatPattern();

        pattern.Concat(new LiteralPattern("abc"));
        pattern.Concat(new LiteralPattern("def"));
        pattern.ToString().ShouldBe("abcdef");
    }

    [Fact]
    public void Copy()
    {
        var pattern = new ConcatPattern(new LiteralPattern("abc"));

        var copy = pattern.Copy();
        pattern.Concat(new LiteralPattern("def"));
        copy.ToString().ShouldBe("abc");
    }

    [Fact]
    public void Wrap()
    {
        var pattern = new ConcatPattern();
        pattern.Concat(pattern);

        var quantifierPattern = new QuantifierPattern(pattern, 0, 1, true);

        Should.Throw<PatternRecursionException>(() => quantifierPattern.ToString());
    }

    [Fact]
    public void Unwrap()
    {
        var pattern = new ConcatPattern();
        pattern.Concat(pattern);

        var orPattern = new OrPattern(pattern, new LiteralPattern("a"));

        Should.Throw<PatternRecursionException>(() => orPattern.ToString());
    }
}
