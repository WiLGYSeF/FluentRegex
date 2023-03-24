namespace Wilgysef.FluentRegex.Tests;

public class AnchorPatternTest
{
    [Fact]
    public void BeginLine()
    {
        var pattern = new PatternBuilder().BeginLine;

        pattern.ToString().ShouldBe("^");
    }

    [Fact]
    public void EndLine()
    {
        var pattern = new PatternBuilder().EndLine;

        pattern.ToString().ShouldBe("$");
    }

    [Fact]
    public void Start()
    {
        var pattern = new PatternBuilder().Start;

        pattern.ToString().ShouldBe(@"\A");
    }

    [Fact]
    public void End()
    {
        var pattern = new PatternBuilder().End;

        pattern.ToString().ShouldBe(@"\Z");
    }

    [Fact]
    public void AbsoluteEnd()
    {
        var pattern = new PatternBuilder().AbsoluteEnd;

        pattern.ToString().ShouldBe(@"\z");
    }

    [Fact]
    public void StartOfMatch()
    {
        var pattern = new PatternBuilder().StartOfMatch;

        pattern.ToString().ShouldBe(@"\G");
    }

    [Fact]
    public void WordBoundary()
    {
        var pattern = new PatternBuilder().WordBoundary;

        pattern.ToString().ShouldBe(@"\b");
    }

    [Fact]
    public void NonWordBoundary()
    {
        var pattern = new PatternBuilder().NonWordBoundary;

        pattern.ToString().ShouldBe(@"\B");
    }

    [Fact]
    public void Copy()
    {
        var pattern = new PatternBuilder().BeginLine;

        pattern.ToString().ShouldBe(pattern.Copy().ToString());
    }

    [Fact]
    public void Unwrap()
    {
        var pattern = new OrPattern(AnchorPattern.BeginLine, new LiteralPattern("a"));

        pattern.ToString().ShouldBe("^|a");
    }
}
