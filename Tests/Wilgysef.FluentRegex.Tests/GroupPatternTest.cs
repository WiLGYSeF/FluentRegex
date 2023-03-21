using Wilgysef.FluentRegex.Exceptions;

namespace Wilgysef.FluentRegex.Tests;

public class GroupPatternTest
{
    [Fact]
    public void Group()
    {
        var pattern = new PatternBuilder().CapturingGroup(new LiteralPattern("abc"));

        pattern.ToString().ShouldBe("(abc)");
    }

    [Fact]
    public void NonCapture()
    {
        var pattern = new PatternBuilder().Group(new LiteralPattern("abc"), capture: false);
        pattern.ToString().ShouldBe("(?:abc)");

        pattern = new PatternBuilder().NonCapturingGroup(new LiteralPattern("abc"));
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
        var pattern = new PatternBuilder().CapturingGroup("abc", new LiteralPattern("test"));

        pattern.ToString().ShouldBe("(?<abc>test)");
    }

    [Fact]
    public void BalancingGroup()
    {
        var pattern = new PatternBuilder()
            .CapturingGroup("abc", new LiteralPattern("a"))
            .CapturingGroup("def", new LiteralPattern("b"))
            .BalancingGroup("abc", "def", new LiteralPattern("test"));

        pattern.ToString().ShouldBe("(?<abc>a)(?<def>b)(?<abc-def>test)");
    }

    [Fact]
    public void FluentNames()
    {
        var pattern = new GroupPattern(new LiteralPattern("a"));

        pattern.WithName("a");
        pattern.Name.ShouldBe("a");
        pattern.IsNumbered.ShouldBeFalse();

        pattern.WithSecondName("b");
        pattern.SecondName.ShouldBe("b");

        pattern.WithCapture(false);
        pattern.Capture.ShouldBeFalse();
        pattern.IsCapturing.ShouldBeTrue();
    }

    [Fact]
    public void FluentPattern()
    {
        var pattern = new GroupPattern(new LiteralPattern("a"));

        pattern.WithPattern(new LiteralPattern("b"));
        pattern.ToString().ShouldBe("(b)");
        pattern.IsCapturing.ShouldBeTrue();
        pattern.IsNumbered.ShouldBeTrue();
    }

    [Fact]
    public void IsCapturing_WithoutCapture()
    {
        var pattern = new GroupPattern(new LiteralPattern("a"), "abc");

        pattern.Capture = false;
        pattern.Capture.ShouldBeFalse();
        pattern.IsCapturing.ShouldBeTrue();
        pattern.IsNumbered.ShouldBeFalse();
    }

    [Fact]
    public void NoWrap()
    {
        var pattern = new PatternBuilder().Group(new LiteralPattern("abc")).Exactly(3);

        pattern.ToString().ShouldBe("(abc){3}");
    }

    [Fact]
    public void Copy()
    {
        var literal = new LiteralPattern("a");

        var pattern = new GroupPattern(literal, "test");
        var copy = pattern.Copy();
        pattern.WithName("z");
        copy.ToString().ShouldBe("(?<test>a)");

        pattern = new GroupPattern(null);
        copy = pattern.Copy();
        pattern.WithPattern(literal);
        copy.ToString().ShouldBe("()");

        pattern = new GroupPattern(literal, "test", "abc");
        copy = pattern.Copy();
        pattern.WithName("z");
        pattern.WithSecondName("y");
        copy.ToString().ShouldBe("(?<test-abc>a)");

        pattern = new GroupPattern(null, "test", "abc");
        copy = pattern.Copy();
        pattern.WithPattern(literal);
        copy.ToString().ShouldBe("(?<test-abc>)");
    }

    [Fact]
    public void Fail_InvalidName()
    {
        Should.Throw<InvalidPatternException>(() => new PatternBuilder().CapturingGroup("", new LiteralPattern("a")).ToString());
        Should.Throw<InvalidPatternException>(() => new PatternBuilder().CapturingGroup("w-", new LiteralPattern("a")).ToString());

        Should.Throw<InvalidPatternException>(() => new PatternBuilder().BalancingGroup("w", "3-", new LiteralPattern("a")).ToString());
    }
}
