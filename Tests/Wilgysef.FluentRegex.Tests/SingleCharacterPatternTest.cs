namespace Wilgysef.FluentRegex.Tests;

public class SingleCharacterPatternTest
{
    [Fact]
    public void SingleCharacter()
    {
        var pattern = new PatternBuilder().Single.Build();

        pattern.ToString().ShouldBe(".");
    }

    [Fact]
    public void SingleCharacter_IsSingle()
    {
        var pattern = new PatternBuilder().Single.Exactly(3).Build();

        pattern.ToString().ShouldBe(".{3}");
    }
}
