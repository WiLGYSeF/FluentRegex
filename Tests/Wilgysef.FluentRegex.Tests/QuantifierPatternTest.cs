using Wilgysef.FluentRegex.Exceptions;

namespace Wilgysef.FluentRegex.Tests;

public class QuantifierPatternTest
{
    [Theory]
    [InlineData(true, "a?")]
    [InlineData(false, "a??")]
    public void ZeroOrOne(bool greedy, string expected)
    {
        ShouldCreatePattern(
            () => new PatternBuilder().Literal("a").ZeroOrOne(greedy),
            expected);

        ShouldCreatePattern(
            () => new PatternBuilder().ZeroOrOne(new LiteralPattern("a"), greedy),
            expected);

        ShouldCreatePattern(
            () => QuantifierPattern.ZeroOrOne(new LiteralPattern("a"), greedy),
            expected);
    }

    [Theory]
    [InlineData(true, "a*")]
    [InlineData(false, "a*?")]
    public void ZeroOrMore(bool greedy, string expected)
    {
        ShouldCreatePattern(
            () => new PatternBuilder().Literal("a").ZeroOrMore(greedy),
            expected);

        ShouldCreatePattern(
            () => new PatternBuilder().ZeroOrMore(new LiteralPattern("a"), greedy),
            expected);

        ShouldCreatePattern(
            () => QuantifierPattern.ZeroOrMore(new LiteralPattern("a"), greedy),
            expected);
    }

    [Theory]
    [InlineData(true, "a+")]
    [InlineData(false, "a+?")]
    public void OneOrMore(bool greedy, string expected)
    {
        ShouldCreatePattern(
            () => new PatternBuilder().Literal("a").OneOrMore(greedy),
            expected);

        ShouldCreatePattern(
            () => new PatternBuilder().OneOrMore(new LiteralPattern("a"), greedy),
            expected);

        ShouldCreatePattern(
            () => QuantifierPattern.OneOrMore(new LiteralPattern("a"), greedy),
            expected);
    }

    [Theory]
    [InlineData(3, "a{3}")]
    [InlineData(0, "")]
    [InlineData(-1, null)]
    [InlineData(1, "a")]
    public void Exactly(int count, string? expected)
    {
        ShouldCreatePattern(
            () => new PatternBuilder().Literal("a").Exactly(count),
            expected);

        ShouldCreatePattern(
            () => new PatternBuilder().Exactly(new LiteralPattern("a"), count),
            expected);

        ShouldCreatePattern(
            () => QuantifierPattern.Exactly(new LiteralPattern("a"), count),
            expected);
    }

    [Theory]
    [InlineData(0, 5, true, "a{0,5}")]
    [InlineData(1, 3, false, "a{1,3}?")]
    [InlineData(3, 1, true, null)]
    [InlineData(-3, 1, true, null)]
    public void Between(int min, int max, bool greedy, string expected)
    {
        ShouldCreatePattern(
            () => new PatternBuilder().Literal("a").Between(min, max, greedy),
            expected);

        ShouldCreatePattern(
            () => new PatternBuilder().Between(new LiteralPattern("a"), min, max, greedy),
            expected);

        ShouldCreatePattern(
            () => QuantifierPattern.Between(new LiteralPattern("a"), min, max, greedy),
            expected);
    }

    [Theory]
    [InlineData(0, true, "a*")]
    [InlineData(1, true, "a+")]
    [InlineData(2, true, "a{2,}")]
    [InlineData(2, false, "a{2,}?")]
    [InlineData(-1, true, null)]
    public void AtLeast(int min, bool greedy, string expected)
    {
        ShouldCreatePattern(
            () => new PatternBuilder().Literal("a").AtLeast(min, greedy),
            expected);

        ShouldCreatePattern(
            () => new PatternBuilder().AtLeast(new LiteralPattern("a"), min, greedy),
            expected);

        ShouldCreatePattern(
            () => QuantifierPattern.AtLeast(new LiteralPattern("a"), min, greedy),
            expected);
    }

    [Theory]
    [InlineData(0, true, "")]
    [InlineData(1, true, "a?")]
    [InlineData(2, true, "a{0,2}")]
    [InlineData(2, false, "a{0,2}?")]
    [InlineData(-1, true, null)]
    public void AtMost(int max, bool greedy, string expected)
    {
        ShouldCreatePattern(
            () => new PatternBuilder().Literal("a").AtMost(max, greedy),
            expected);

        ShouldCreatePattern(
            () => new PatternBuilder().AtMost(new LiteralPattern("a"), max, greedy),
            expected);

        ShouldCreatePattern(
            () => QuantifierPattern.AtMost(new LiteralPattern("a"), max, greedy),
            expected);
    }

    [Fact]
    public void QuantifyConcat()
    {
        ShouldCreatePattern(
            () => new PatternBuilder().Concat(
                new LiteralPattern("a"),
                new CharacterSetPattern('b', 'c'))
                .Between(1, 3),
            "(?:a[bc]){1,3}");
    }

    [Fact]
    public void QuantifyConcat_Single()
    {
        ShouldCreatePattern(
            () => new PatternBuilder().Concat(new LiteralPattern("a")).Between(1, 3),
            "a{1,3}");
    }

    [Fact]
    public void QuantifyConcat_Literal()
    {
        ShouldCreatePattern(
            () => new PatternBuilder().Concat(new LiteralPattern("abc")).Between(1, 3),
            "(?:abc){1,3}");
    }

    [Fact]
    public void QuantifyConcat_Nested()
    {
        ShouldCreatePattern(
            () => new PatternBuilder().Concat(
                new ConcatPattern(
                    new ConcatPattern(
                        new LiteralPattern("a"),
                        new CharacterSetPattern('b', 'c'))))
                .Between(1, 3),
            "(?:a[bc]){1,3}");
    }

    [Fact]
    public void QuantifyConcat_Nested_Single()
    {
        ShouldCreatePattern(
            () => new PatternBuilder().Concat(
                new ConcatPattern(
                    new ConcatPattern(
                        new LiteralPattern("abc"))))
                .Between(1, 3),
            "(?:abc){1,3}");
    }

    [Fact]
    public void FluentZeroOrOne()
    {
        var pattern = new QuantifierPattern(new LiteralPattern("a"), -1, -1, false);

        pattern.WithZeroOrOne(true);
        pattern.Min.ShouldBe(0);
        pattern.Max.ShouldBe(1);
        pattern.Greedy.ShouldBeTrue();

        pattern.WithZeroOrOne(false);
        pattern.Greedy.ShouldBeFalse();
    }

    [Fact]
    public void FluentZeroOrMore()
    {
        var pattern = new QuantifierPattern(new LiteralPattern("a"), -1, -1, false);

        pattern.WithZeroOrMore(true);
        pattern.Min.ShouldBe(0);
        pattern.Max.ShouldBeNull();
        pattern.Greedy.ShouldBeTrue();

        pattern.WithZeroOrMore(false);
        pattern.Greedy.ShouldBeFalse();
    }

    [Fact]
    public void FluentOneOrMore()
    {
        var pattern = new QuantifierPattern(new LiteralPattern("a"), -1, -1, false);

        pattern.WithOneOrMore(true);
        pattern.Min.ShouldBe(1);
        pattern.Max.ShouldBeNull();
        pattern.Greedy.ShouldBeTrue();

        pattern.WithOneOrMore(false);
        pattern.Greedy.ShouldBeFalse();
    }

    [Fact]
    public void FluentExactly()
    {
        var pattern = new QuantifierPattern(new LiteralPattern("a"), -1, -1, false);

        pattern.WithExactly(3);
        pattern.Min.ShouldBe(3);
        pattern.Max.ShouldBe(3);
        pattern.Greedy.ShouldBeTrue();
    }

    [Fact]
    public void FluentBetween()
    {
        var pattern = new QuantifierPattern(new LiteralPattern("a"), -1, -1, false);

        pattern.WithBetween(1, 3, true);
        pattern.Min.ShouldBe(1);
        pattern.Max.ShouldBe(3);
        pattern.Greedy.ShouldBeTrue();

        pattern.WithBetween(1, 3, false);
        pattern.Greedy.ShouldBeFalse();
    }

    [Fact]
    public void FluentAtLeast()
    {
        var pattern = new QuantifierPattern(new LiteralPattern("a"), -1, -1, false);

        pattern.WithAtLeast(3, true);
        pattern.Min.ShouldBe(3);
        pattern.Max.ShouldBeNull();
        pattern.Greedy.ShouldBeTrue();

        pattern.WithAtLeast(3, false);
        pattern.Greedy.ShouldBeFalse();
    }

    [Fact]
    public void FluentAtMost()
    {
        var pattern = new QuantifierPattern(new LiteralPattern("a"), -1, -1, false);

        pattern.WithAtMost(3, true);
        pattern.Min.ShouldBe(0);
        pattern.Max.ShouldBe(3);
        pattern.Greedy.ShouldBeTrue();

        pattern.WithAtMost(3, false);
        pattern.Greedy.ShouldBeFalse();
    }

    [Fact]
    public void FluentGreedy()
    {
        var pattern = new QuantifierPattern(new LiteralPattern("a"), -1, -1, false);

        pattern.GreedyCapture();
        pattern.Greedy.ShouldBeTrue();
    }

    [Fact]
    public void FluentPattern()
    {
        var pattern = new QuantifierPattern(new LiteralPattern("a"), 2, 2, true);

        pattern.WithPattern(new LiteralPattern("b"));
        pattern.ToString().ShouldBe("b{2}");
    }

    [Fact]
    public void Wrap()
    {
        var pattern = new PatternBuilder().Literal("a").Between(1, 3).Between(2, 4);

        pattern.ToString().ShouldBe("(?:a{1,3}){2,4}");
    }

    [Fact]
    public void NoWrap_ExactlyOne()
    {
        var pattern = new OrPattern(
            new QuantifierPattern(new LiteralPattern("b"), 1, 1, true),
            new LiteralPattern("a"));

        pattern.ToString().ShouldBe("[ba]");
    }

    [Fact]
    public void NoWrap_Empty()
    {
        var pattern = new OrPattern(
            new QuantifierPattern(new LiteralPattern("c"), 1, 1, true));

        pattern.ToString().ShouldBe("c");
    }

    [Fact]
    public void Unwrap()
    {
        var pattern = new QuantifierPattern(new LiteralPattern("b"), 1, 2, true);

        pattern.Unwrap().ShouldBe(pattern);
    }

    [Fact]
    public void Unwrap_ExactlyOne()
    {
        var literal = new LiteralPattern("b");
        var pattern = new QuantifierPattern(literal, 1, 1, true);

        pattern.Unwrap().ShouldBe(literal);
    }

    [Fact]
    public void Unwrap_Recursive()
    {
        var quantifier = new QuantifierPattern(new LiteralPattern("b"), 1, 1, true);
        quantifier.WithPattern(quantifier);

        var pattern = new OrPattern(quantifier, new LiteralPattern("a"));

        Should.Throw<PatternRecursionException>(() => pattern.ToString());
    }

    [Fact]
    public void Copy()
    {
        var literal = new LiteralPattern("a");
        var pattern = new QuantifierPattern(literal, 1, 4, true);

        var copy = pattern.Copy();
        literal.WithValue("b");
        pattern.WithBetween(2, 5);
        copy.ToString().ShouldBe("a{1,4}");
    }

    [Fact]
    public void NoPreviousPattern()
    {
        ShouldCreatePattern(() => new PatternBuilder().ZeroOrOne(), null);
    }

    [Fact]
    public void InvalidQuantifier()
    {
        ShouldCreatePattern(() => new PatternBuilder().BeginLine.Exactly(3), null);
    }

    [Fact]
    public void Empty()
    {
        ShouldCreatePattern(
            () => new PatternBuilder().Concat(new ConcatPattern()).Between(1, 3),
            "");
    }

    private static void ShouldCreatePattern(Func<Pattern> func, string? expected)
    {
        if (expected != null)
        {
            var pattern = func();
            pattern.ToString().ShouldBe(expected);
        }
        else
        {
            Should.Throw<InvalidPatternException>(() => func().ToString());
        }
    }
}
