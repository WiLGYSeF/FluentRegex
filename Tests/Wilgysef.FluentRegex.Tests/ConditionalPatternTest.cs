namespace Wilgysef.FluentRegex.Tests;

public class ConditionalPatternTest
{
    [Fact]
    public void Expression()
    {
        var pattern = new PatternBuilder()
            .Conditional(new LiteralPattern("abc"), new LiteralPattern("abc"), new LiteralPattern("asdf"));

        pattern.ToString().ShouldBe("(?(?=abc)abc|asdf)");
    }

    [Fact]
    public void Expression_Lookbehind()
    {
        var pattern = new PatternBuilder()
            .Conditional(new LiteralPattern("abc"), new LiteralPattern("abc"), new LiteralPattern("asdf"), lookahead: false);

        pattern.ToString().ShouldBe("(?(?<=abc)abc|asdf)");
    }

    [Fact]
    public void GroupNumber()
    {
        var pattern = new PatternBuilder()
            .Group(new LiteralPattern("test"))
            .Conditional(1, new LiteralPattern("abc"), new LiteralPattern("asdf"));

        pattern.ToString().ShouldBe("(test)(?(1)abc|asdf)");
    }

    [Fact]
    public void GroupName()
    {
        var pattern = new PatternBuilder()
            .CapturingGroup("a", new LiteralPattern("test"))
            .Conditional("a", new LiteralPattern("abc"), new LiteralPattern("asdf"));

        pattern.ToString().ShouldBe("(?<a>test)(?(a)abc|asdf)");
    }

    [Fact]
    public void NoNoExpression()
    {
        var pattern = new PatternBuilder()
            .Group(new LiteralPattern("test"))
            .Conditional(1, new LiteralPattern("abc"), null);

        pattern.ToString().ShouldBe("(test)(?(1)abc)");
    }

    [Fact]
    public void NestedOr()
    {
        var pattern = new PatternBuilder()
            .Group(new LiteralPattern("test"))
            .Conditional(
                1,
                new OrPattern(new LiteralPattern("abc"), new LiteralPattern("123")),
                new LiteralPattern("asdf"));

        pattern.ToString().ShouldBe("(test)(?(1)(?:abc|123)|asdf)");
    }

    [Fact]
    public void FluentMethods()
    {
        var pattern = new ConditionalPattern(1, new LiteralPattern("a"), new LiteralPattern("b"));

        pattern.WithGroup(2);
        pattern.GroupName.ShouldBeNull();
        pattern.Expression.ShouldBeNull();
        pattern.ToString().ShouldBe("(?(2)a|b)");

        pattern.WithGroup("a");
        pattern.GroupNumber.ShouldBeNull();
        pattern.Expression.ShouldBeNull();
        pattern.ToString().ShouldBe("(?(a)a|b)");

        pattern.WithExpression(new LiteralPattern("z"));
        pattern.GroupNumber.ShouldBeNull();
        pattern.GroupName.ShouldBeNull();
        pattern.ToString().ShouldBe("(?(?=z)a|b)");

        pattern.WithYesPattern(new LiteralPattern("1"));
        pattern.ToString().ShouldBe("(?(?=z)1|b)");

        pattern.WithNoPattern(new LiteralPattern("2"));
        pattern.ToString().ShouldBe("(?(?=z)1|2)");

        pattern.WithNoPattern(null);
        pattern.ToString().ShouldBe("(?(?=z)1)");
    }

    [Fact]
    public void Copy()
    {
        ShouldCopy(typeof(int), true);
        ShouldCopy(typeof(int), false);

        ShouldCopy(typeof(string), true);
        ShouldCopy(typeof(string), false);

        ShouldCopy(typeof(Pattern), true);
        ShouldCopy(typeof(Pattern), false);

        static void ShouldCopy(Type conditionalType, bool useNoPattern)
        {
            ConditionalPattern pattern;
            Action changePatternConditional;
            string expectedConditional;

            var yesPattern = new LiteralPattern("a");
            var noPattern = new LiteralPattern("b");

            if (conditionalType == typeof(int))
            {
                pattern = new ConditionalPattern(1, yesPattern, useNoPattern ? noPattern : null);
                changePatternConditional = () => pattern.WithGroup(2);
                expectedConditional = "1";
            }
            else if (conditionalType == typeof(string))
            {
                pattern = new ConditionalPattern("abc", yesPattern, useNoPattern ? noPattern : null);
                changePatternConditional = () => pattern.WithGroup("def");
                expectedConditional = "abc";
            }
            else if (conditionalType == typeof(Pattern))
            {
                var expressionConditional = new LiteralPattern("test");
                pattern = new ConditionalPattern(expressionConditional, yesPattern, useNoPattern ? noPattern : null);
                changePatternConditional = () => expressionConditional.WithValue("asdf");
                expectedConditional = "?=test";
            }
            else
            {
                throw new ArgumentException("Invalid type.", nameof(conditionalType));
            }

            var copy = pattern.Copy();
            changePatternConditional();
            yesPattern.WithValue("y");
            noPattern.WithValue("z");

            copy.ToString().ShouldBe($"(?({expectedConditional})a{(useNoPattern ? "|b" : "")})");
        }
    }

    [Fact]
    public void Unwrap()
    {
        var pattern = new ConditionalPattern(1, new LiteralPattern("a"), null);

        pattern.Unwrap().ShouldBe(pattern);
    }
}
