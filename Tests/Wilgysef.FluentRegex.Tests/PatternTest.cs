using System.Text.RegularExpressions;

namespace Wilgysef.FluentRegex.Tests;

public class PatternTest
{
    [Fact]
    public void Compile()
    {
        var pattern = new PatternBuilder()
            .BeginLine
            .Literal("test")
            .EndLine
            .Build();

        var regex = pattern.Compile();
        regex.ToString().ShouldBe("^test$");
    }

    [Fact]
    public void Compile_Options()
    {
        var pattern = new PatternBuilder()
            .BeginLine
            .Literal("test")
            .EndLine
            .Build();

        var regex = pattern.Compile(RegexOptions.IgnoreCase);

        regex.ToString().ShouldBe("^test$");
        regex.Match("tEsT").Success.ShouldBeTrue();
    }

    [Fact]
    public void Compile_Options_Timeout()
    {
        var pattern = new PatternBuilder()
            .BeginLine
            .Literal("test")
            .EndLine
            .Build();

        var regex = pattern.Compile(RegexOptions.IgnoreCase, TimeSpan.FromSeconds(1));

        regex.ToString().ShouldBe("^test$");
        regex.Match("tEsT").Success.ShouldBeTrue();
    }

    [Fact]
    public void Traverse()
    {
        var builder = new PatternBuilder()
            .CaptureGroup("z", new PatternBuilder().Group(new LiteralPattern("a")).Literal("b"))
            .Group(new LiteralPattern("c"))
            .Literal("asdf")
            .Between(1, 6);

        builder.ToString().ShouldBe("(?<z>(a)b)(c)(?:asdf){1,6}");

        var traversed = builder.Traverse().ToList();
        traversed.Count.ShouldBe(9);

        traversed[0].GetType().ShouldBe(typeof(GroupPattern));
        traversed[1].GetType().ShouldBe(typeof(PatternBuilder));
        traversed[2].GetType().ShouldBe(typeof(GroupPattern));
        traversed[3].GetType().ShouldBe(typeof(LiteralPattern));
        traversed[4].GetType().ShouldBe(typeof(LiteralPattern));
        traversed[5].GetType().ShouldBe(typeof(GroupPattern));
        traversed[6].GetType().ShouldBe(typeof(LiteralPattern));
        traversed[7].GetType().ShouldBe(typeof(QuantifierPattern));
        traversed[8].GetType().ShouldBe(typeof(LiteralPattern));
    }

    [Fact]
    public void Traverse_Recursive()
    {
        var group = new GroupPattern(new LiteralPattern("a"));
        group.WithPattern(new ConcatPattern(new LiteralPattern("b"), group));

        var builder = new PatternBuilder().CaptureGroup("z", group);

        Should.Throw<InvalidOperationException>(() => builder.Build());

        Should.Throw<InvalidOperationException>(() => group.ToString());
    }
}
