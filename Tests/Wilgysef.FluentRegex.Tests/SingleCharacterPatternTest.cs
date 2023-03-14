namespace Wilgysef.FluentRegex.Tests;

public class SingleCharacterPatternTest
{
    [Fact]
    public void SingleCharacter()
    {
        var pattern = Pattern.Single;

        pattern.ToString().ShouldBe(".");
    }

    [Fact]
    public void SingleCharacter_IsSingle()
    {
        var pattern = Pattern.Single.Exactly(3);

        pattern.ToString().ShouldBe(".{3}");
    }
}
