﻿namespace Wilgysef.FluentRegex.Tests;

public class ConcatPatternTest
{
    [Fact]
    public void Concat()
    {
        var pattern = new PatternBuilder()
            .Concat(new LiteralPattern("asdf"))
            .Concat(new List<LiteralPattern> { new LiteralPattern("123") });

        pattern.ToString().ShouldBe("asdf123");
    }

    [Fact]
    public void FluentConcat()
    {
        var pattern = new ConcatPattern();

        pattern.Concat(new LiteralPattern("abc"));
        pattern.Concat(new LiteralPattern("def"));
        pattern.ToString().ShouldBe("abcdef");
    }
}