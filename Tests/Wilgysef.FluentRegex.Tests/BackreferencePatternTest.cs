using Wilgysef.FluentRegex.Exceptions;

namespace Wilgysef.FluentRegex.Tests;

public class BackreferencePatternTest
{
    [Fact]
    public void Number()
    {
        var pattern = new PatternBuilder()
            .Group(new LiteralPattern("a"))
            .Backreference(1);

        pattern.ToString().ShouldBe(@"(a)\1");
    }

    [Fact]
    public void Name()
    {
        var pattern = new PatternBuilder()
            .CapturingGroup("a", new LiteralPattern("z"))
            .Backreference("a");

        pattern.ToString().ShouldBe(@"(?<a>z)\k<a>");
    }

    [Fact]
    public void GetGroup()
    {
        var pattern = new BackreferencePattern(1);

        pattern.Group.ShouldBe(1);
        pattern.WithGroup("a");
        pattern.Group.ShouldBe("a");
    }

    [Fact]
    public void NoWrap()
    {
        var pattern = new PatternBuilder()
            .CapturingGroup("a", new LiteralPattern("z"))
            .Backreference("a")
            .OneOrMore();

        pattern.ToString().ShouldBe(@"(?<a>z)\k<a>+");
    }

    [Fact]
    public void Copy()
    {
        var pattern = new BackreferencePattern("a");

        var copy = pattern.Copy();
        pattern.WithGroup("b");
        copy.ToString().ShouldBe(@"\k<a>");

        pattern.WithGroup(2);
        copy = pattern.Copy();
        pattern.WithGroup(3);
        copy.ToString().ShouldBe(@"\2");
    }

    [Fact]
    public void Fail_InvalidGroupNumber()
    {
        var pattern = new BackreferencePattern(10);

        Should.Throw<InvalidPatternException>(() => pattern.ToString());
    }
}
