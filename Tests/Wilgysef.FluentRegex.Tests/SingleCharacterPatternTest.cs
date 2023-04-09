namespace Wilgysef.FluentRegex.Tests;

public class SingleCharacterPatternTest
{
    [Fact]
    public void SingleCharacter()
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

    [Fact]
    public void Unwrap()
    {
        var pattern = new SingleCharacterPattern();

        pattern.Unwrap().ShouldBe(pattern);
    }

    [Fact]
    public void Copy()
    {
        var pattern = new SingleCharacterPattern();

        pattern.Copy().ToString().ShouldBe(".");
    }
}
