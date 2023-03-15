namespace Wilgysef.FluentRegex.Tests;

public class OrPatternTest
{
    [Fact]
    public void Simple()
    {
        var pattern = new PatternBuilder().Or(
            new LiteralPattern("abc"),
            new LiteralPattern("123"));

        pattern.ToString().ShouldBe("abc|123");
    }

    [Fact]
    public void Single()
    {
        var pattern = new PatternBuilder().Or(new LiteralPattern("abc"));

        pattern.ToString().ShouldBe("abc");
    }

    [Fact]
    public void Nested()
    {
        var pattern = new PatternBuilder().Or(
            new LiteralPattern("abc"),
            new PatternBuilder().Literal("0").Or(
                new LiteralPattern("123"),
                new LiteralPattern("456")));

        pattern.ToString().ShouldBe("abc|0(?:123|456)");
    }

    [Fact]
    public void Nested_MiddleConcat()
    {
        var pattern = new PatternBuilder().Or(
            new LiteralPattern("abc"),
            new PatternBuilder().Literal("0").Or(
                new LiteralPattern("123"),
                new LiteralPattern("456")),
            new LiteralPattern("def"));

        pattern.ToString().ShouldBe("abc|0(?:123|456)|def");
    }

    [Fact]
    public void Nested_Multiple()
    {
        var pattern = new PatternBuilder().Or(
            new LiteralPattern("abc"),
            new PatternBuilder().Or(
                new PatternBuilder().Or(new[]
                {
                    new LiteralPattern("123"),
                    new LiteralPattern("456"),
                }),
                new LiteralPattern("def")),
            new LiteralPattern("ghi"));

        pattern.ToString().ShouldBe("abc|123|456|def|ghi");
    }
}
