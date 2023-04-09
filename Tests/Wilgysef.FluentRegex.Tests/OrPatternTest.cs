namespace Wilgysef.FluentRegex.Tests;

public class OrPatternTest
{
    [Fact]
    public void Or()
    {
        var pattern = new PatternBuilder().Or(
            new LiteralPattern("abc"),
            new LiteralPattern("123"));

        pattern.ToString().ShouldBe("abc|123");
    }

    [Fact]
    public void Single()
    {
        var pattern = new PatternBuilder().Or(new LiteralPattern("abc"));

        pattern.ToString().ShouldBe("abc");
    }

    [Fact]
    public void Empty()
    {
        var pattern = new PatternBuilder().Or();

        pattern.ToString().ShouldBe("");
    }

    [Fact]
    public void Nested()
    {
        var pattern = new PatternBuilder().Or(
            new LiteralPattern("abc"),
            new PatternBuilder().Literal("0").Or(
                new LiteralPattern("123"),
                new LiteralPattern("456")));

        pattern.ToString().ShouldBe("abc|0(?:123|456)");
    }

    [Fact]
    public void Nested_MiddleConcat()
    {
        var pattern = new PatternBuilder().Or(
            new LiteralPattern("abc"),
            new PatternBuilder().Literal("0").Or(
                new LiteralPattern("123"),
                new LiteralPattern("456")),
            new LiteralPattern("def"));

        pattern.ToString().ShouldBe("abc|0(?:123|456)|def");
    }

    [Fact]
    public void Nested_Multiple()
    {
        var pattern = new PatternBuilder().Or(
            new LiteralPattern("abc"),
            new PatternBuilder().Or(
                new PatternBuilder().Or(new List<LiteralPattern>
                {
                    new LiteralPattern("123"),
                    new LiteralPattern("456"),
                }),
                new LiteralPattern("def")),
            new LiteralPattern("ghi"));

        pattern.ToString().ShouldBe("abc|123|456|def|ghi");
    }

    [Fact]
    public void FluentOr()
    {
        var pattern = new OrPattern(
            new LiteralPattern("abc"),
            new LiteralPattern("123"));

        pattern.Or(new LiteralPattern("zxc"));
        pattern.ToString().ShouldBe("abc|123|zxc");
    }

    [Fact]
    public void Copy()
    {
        var literal = new LiteralPattern("abc");
        var pattern = new OrPattern(
            literal,
            new LiteralPattern("123"));

        var copy = pattern.Copy();
        literal.WithValue("def");
        copy.ToString().ShouldBe("abc|123");
    }

    [Fact]
    public void Unwrap()
    {
        var literal = new LiteralPattern("123");
        var pattern = new OrPattern(literal);

        pattern.Unwrap().ShouldBe(literal);
    }

    #region CharacterSetPattern combinations

    [Fact]
    public void CharacterSetPattern_Combine_All()
    {
        var pattern = new OrPattern(
            new CharacterSetPattern(new CharacterRange('a', 'z')),
            new CharacterSetPattern(new CharacterRange('0', '9')));

        pattern.ToString().ShouldBe("[a-z0-9]");
        ShouldBeSingle(pattern);
    }

    [Fact]
    public void CharacterSetPattern_Combine_Beginning()
    {
        var pattern = new OrPattern(
            new CharacterSetPattern(new CharacterRange('a', 'z')),
            new CharacterSetPattern(new CharacterRange('0', '9')),
            new LiteralPattern("abc"));

        pattern.ToString().ShouldBe("[a-z0-9]|abc");
        ShouldNotBeSingle(pattern);
    }

    [Fact]
    public void CharacterSetPattern_Combine_Middle()
    {
        var pattern = new OrPattern(
            new LiteralPattern("abc"),
            new LiteralPattern("def"),
            new CharacterSetPattern(new CharacterRange('a', 'z')),
            new CharacterSetPattern(new CharacterRange('0', '9')),
            new LiteralPattern("ghi"));

        pattern.ToString().ShouldBe("abc|def|[a-z0-9]|ghi");
        ShouldNotBeSingle(pattern);
    }

    [Fact]
    public void CharacterSetPattern_Combine_End()
    {
        var pattern = new OrPattern(
            new LiteralPattern("abc"),
            new LiteralPattern("def"),
            new CharacterSetPattern(new CharacterRange('a', 'z')),
            new CharacterSetPattern(new CharacterRange('0', '9')));

        pattern.ToString().ShouldBe("abc|def|[a-z0-9]");
        ShouldNotBeSingle(pattern);
    }

    [Fact]
    public void CharacterSetPattern_Combine_Nonconsecutive()
    {
        var pattern = new OrPattern(
            new LiteralPattern("abc"),
            new CharacterSetPattern(new CharacterRange('a', 'z')),
            new LiteralPattern("def"),
            new CharacterSetPattern(new CharacterRange('0', '9')));

        pattern.ToString().ShouldBe("abc|[a-z0-9]|def");
        ShouldNotBeSingle(pattern);
    }

    [Fact]
    public void CharacterSetPattern_Combine_CharacterPattern()
    {
        var pattern = new OrPattern(
            new CharacterSetPattern(new CharacterRange('a', 'z')),
            CharacterPattern.Character('0'));

        pattern.ToString().ShouldBe("[a-z0]");
        ShouldBeSingle(pattern);
    }

    [Fact]
    public void CharacterSetPattern_Combine_SingleLiteral()
    {
        var pattern = new OrPattern(
            new CharacterSetPattern(new CharacterRange('a', 'z')),
            new LiteralPattern("0"));

        pattern.ToString().ShouldBe("[a-z0]");
        ShouldBeSingle(pattern);
    }

    [Fact]
    public void CharacterSetPattern_Combine_CharacterPattern_SingleLiteral()
    {
        var pattern = new OrPattern(
            new CharacterSetPattern(new CharacterRange('a', 'z')),
            CharacterPattern.Character('4'),
            new LiteralPattern("0"));

        pattern.ToString().ShouldBe("[a-z40]");
        ShouldBeSingle(pattern);
    }

    [Fact]
    public void CharacterSetPattern_Combine_Negated()
    {
        var pattern = new OrPattern(
            new CharacterSetPattern(new[] { new CharacterRange('a', 'z') }, negated: true),
            new CharacterSetPattern(new[] { new CharacterRange('0', '9') }, negated: true));

        pattern.ToString().ShouldBe("[^a-z0-9]");
        ShouldBeSingle(pattern);
    }

    [Fact]
    public void CharacterSetPattern_Combine_Negated_Mixed()
    {
        var pattern = new OrPattern(
            new CharacterSetPattern(new[] { new CharacterRange('a', 'z') }, negated: true),
            new CharacterSetPattern(new[] { new CharacterRange('0', '9') }));

        pattern.ToString().ShouldBe("[0-9]|[^a-z]");
        ShouldNotBeSingle(pattern);
    }

    [Fact]
    public void SingleLiteral_Combine()
    {
        var pattern = new OrPattern(
            new LiteralPattern("0"),
            new LiteralPattern("1"));

        pattern.ToString().ShouldBe("[01]");
        ShouldBeSingle(pattern);
    }

    [Fact]
    public void CharacterPattern_Combine()
    {
        var pattern = new OrPattern(
            CharacterPattern.Character('0'),
            CharacterPattern.Character('1'));

        pattern.ToString().ShouldBe("[01]");
        ShouldBeSingle(pattern);
    }

    private static void ShouldBeSingle(Pattern pattern)
    {
        PatternLengthVsConcatPlusOneLengthDiff(pattern).ShouldBe(1);
    }

    private static void ShouldNotBeSingle(Pattern pattern)
    {
        PatternLengthVsConcatPlusOneLengthDiff(pattern).ShouldBeGreaterThan(1);
    }

    private static int PatternLengthVsConcatPlusOneLengthDiff(Pattern pattern)
    {
        return new ConcatPattern(pattern, new LiteralPattern("a")).ToString().Length - pattern.ToString().Length;
    }

    #endregion
}
