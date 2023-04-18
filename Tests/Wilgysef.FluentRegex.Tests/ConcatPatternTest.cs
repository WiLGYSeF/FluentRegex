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
    public void Copy_Recursive()
    {
        var pattern = new ConcatPattern();
        pattern.Concat(pattern);

        Should.Throw<PatternRecursionException>(() => pattern.Copy());
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
    public void NoWrap_Multiple_WithEmpty()
    {
        var pattern = new PatternBuilder(new ConcatPattern(
            new LiteralPattern("b"),
            new GroupPattern(null, capture: false)))
                .ZeroOrMore();

        pattern.ToString().ShouldBe("b*");
    }

    [Fact]
    public void NoWrap_OrPattern_NoWrapSingle_Empty()
    {
        var pattern = new ConcatPattern(
            new OrPattern(
                new LiteralPattern("zxc"),
                new LiteralPattern("vbn")),
            new GroupPattern(null, capture: false));

        pattern.ToString().ShouldBe("zxc|vbn");
    }

    [Fact]
    public void Unwrap()
    {
        var literal = new LiteralPattern("a");
        var pattern = new ConcatPattern(literal);

        pattern.Unwrap().ShouldBe(literal);
    }

    [Fact]
    public void Unwrap_Recursive()
    {
        var pattern = new ConcatPattern();
        pattern.Concat(pattern);

        Should.Throw<PatternRecursionException>(() => pattern.Unwrap());
    }
}
