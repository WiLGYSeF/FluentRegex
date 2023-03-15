namespace Wilgysef.FluentRegex.Tests;

public class SingleCharacterPatternTest
{
    [Fact]
    public void Simple()
    {
        var pattern = new PatternBuilder().Single;

        pattern.ToString().ShouldBe(".");
    }

    [Fact]
    public void NoWrap()
    {
        var pattern = new PatternBuilder().Single.Exactly(3);

        pattern.ToString().ShouldBe(".{3}");
    }
}
