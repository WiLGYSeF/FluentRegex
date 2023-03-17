namespace Wilgysef.FluentRegex.Tests;

public class GroupPatternTest
{
    [Fact]
    public void Group()
    {
        var pattern = new PatternBuilder().CaptureGroup(new LiteralPattern("abc"));

        pattern.ToString().ShouldBe("(abc)");
    }

    [Fact]
    public void NonCapture()
    {
        var pattern = new PatternBuilder().Group(new LiteralPattern("abc"), capture: false);
        pattern.ToString().ShouldBe("(?:abc)");

        pattern = new PatternBuilder().NonCaptureGroup(new LiteralPattern("abc"));
        pattern.ToString().ShouldBe("(?:abc)");
    }

    [Fact]
    public void Empty()
    {
        var pattern = new PatternBuilder().Group(null);

        pattern.ToString().ShouldBe("()");
    }

    [Fact]
    public void NamedGroup()
    {
        var pattern = new PatternBuilder().CaptureGroup("abc", new LiteralPattern("test"));

        pattern.ToString().ShouldBe("(?<abc>test)");
    }

    [Fact]
    public void BalancingGroup()
    {
        var pattern = new PatternBuilder()
            .CaptureGroup("abc", new LiteralPattern("a"))
            .CaptureGroup("def", new LiteralPattern("b"))
            .BalancingGroup("abc", "def", new LiteralPattern("test"));

        pattern.ToString().ShouldBe("(?<abc>a)(?<def>b)(?<abc-def>test)");
    }

    [Fact]
    public void FluentNames()
    {
        var pattern = new GroupPattern(new LiteralPattern("a"));

        pattern.WithName("a");
        pattern.Name.ShouldBe("a");

        pattern.WithSecondName("b");
        pattern.SecondName.ShouldBe("b");

        pattern.Capture(false);
        pattern.IsCapturing.ShouldBeFalse();
    }

    [Fact]
    public void FluentPattern()
    {
        var pattern = new GroupPattern(new LiteralPattern("a"));

        pattern.WithPattern(new LiteralPattern("b"));
        pattern.ToString().ShouldBe("(b)");
    }

    [Fact]
    public void Fail_InvalidName()
    {
        Should.Throw<InvalidOperationException>(() => new PatternBuilder().CaptureGroup("", new LiteralPattern("a")).ToString());
        Should.Throw<InvalidOperationException>(() => new PatternBuilder().CaptureGroup("w-", new LiteralPattern("a")).ToString());

        Should.Throw<InvalidOperationException>(() => new PatternBuilder().BalancingGroup("w", "3-", new LiteralPattern("a")).ToString());
    }
}
