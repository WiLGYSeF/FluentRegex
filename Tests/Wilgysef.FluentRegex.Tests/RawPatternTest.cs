namespace Wilgysef.FluentRegex.Tests;

public class RawPatternTest
{
    [Fact]
    public void Simple()
    {
        var pattern = new PatternBuilder().Raw("ab[c").Raw("o]de");

        pattern.ToString().ShouldBe("ab[co]de");
    }

    [Fact]
    public void FluentRegex()
    {
        var pattern = new RawPattern("asdf");

        pattern.WithRegex("test");
        pattern.ToString().ShouldBe("test");
    }

    [Fact]
    public void NoWrap()
    {
        var pattern = new PatternBuilder().Or(
            new PatternBuilder().Raw("ab[c"),
            new PatternBuilder().Raw("o]de"));

        pattern.ToString().ShouldBe("ab[c|o]de");
    }
}
