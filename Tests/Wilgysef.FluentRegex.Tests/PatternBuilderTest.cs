namespace Wilgysef.FluentRegex.Tests;

public class PatternBuilderTest
{
    [Fact]
    public void GetGroups_Simple()
    {
        var builder = new PatternBuilder()
            .Group(new LiteralPattern("a"))
            .Group(new LiteralPattern("b"));

        builder.ToString().ShouldBe("(a)(b)");

        var groups = builder.GetNumberedGroups();
        groups.Count.ShouldBe(2);
        groups[0].ToString().ShouldBe("(a)");
        groups[1].ToString().ShouldBe("(b)");
    }

    [Fact]
    public void GetGroups_Nested()
    {
        var builder = new PatternBuilder()
            .Group(new PatternBuilder().Group(new LiteralPattern("a")).Literal("b"))
            .Group(new LiteralPattern("c"));

        builder.ToString().ShouldBe("((a)b)(c)");

        var groups = builder.GetNumberedGroups();
        groups.Count.ShouldBe(3);
        groups[0].ToString().ShouldBe("((a)b)");
        groups[1].ToString().ShouldBe("(a)");
        groups[2].ToString().ShouldBe("(c)");
    }

    [Fact]
    public void GetGroups_Nested_With_Named()
    {
        var builder = new PatternBuilder()
            .CapturingGroup("z", new PatternBuilder().Group(new LiteralPattern("a")).Literal("b"))
            .Group(new LiteralPattern("c"))
            .Group(null);

        builder.ToString().ShouldBe("(?<z>(a)b)(c)()");

        var groups = builder.GetNumberedGroups();
        groups.Count.ShouldBe(3);
        groups[0].ToString().ShouldBe("(a)");
        groups[1].ToString().ShouldBe("(c)");
        groups[2].ToString().ShouldBe("()");
    }

    [Fact]
    public void GetGroupNumber()
    {
        var zGroup = new GroupPattern(new PatternBuilder().Group(new LiteralPattern("a")).Literal("b"), "z");
        var cGroup = new GroupPattern(new LiteralPattern("c"));

        var builder = new PatternBuilder().Concat(zGroup, cGroup);

        builder.ToString().ShouldBe("(?<z>(a)b)(c)");

        builder.GetGroupNumber(zGroup).ShouldBeNull();
        builder.GetGroupNumber(cGroup).ShouldBe(2);
    }
}
