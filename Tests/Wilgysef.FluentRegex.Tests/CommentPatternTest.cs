﻿namespace Wilgysef.FluentRegex.Tests;

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

        pattern.WithValue((string?)null);
        pattern.ToString().ShouldBe("");

        pattern.WithValue(new LiteralPattern("aaa"));
        pattern.ToString().ShouldBe("(?#aaa)");
    }

    [Fact]
    public void Copy()
    {
        var pattern = new CommentPattern("test");

        var copy = pattern.Copy();
        pattern.WithValue("abc");
        copy.ToString().ShouldBe("(?#test)");

        pattern = new CommentPattern((string?)null);
        copy = pattern.Copy();
        pattern.WithValue("abc");
        copy.ToString().ShouldBe("");
    }

    [Fact]
    public void Unwrap()
    {
        var pattern = new CommentPattern("test");

        pattern.Unwrap().ShouldBe(pattern);
    }
}
