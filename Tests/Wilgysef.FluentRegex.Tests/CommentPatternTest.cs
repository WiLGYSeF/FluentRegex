namespace Wilgysef.FluentRegex.Tests;

public class CommentPatternTest
{
    [Fact]
    public void Comment()
    {
        var pattern = new PatternBuilder().Comment("test");

        pattern.ToString().ShouldBe("(?#test)");
    }

    [Fact]
    public void FluentValue()
    {
        var pattern = new CommentPattern("test");

        pattern.WithValue("asdf");
        pattern.ToString().ShouldBe("(?#asdf)");
    }
}
