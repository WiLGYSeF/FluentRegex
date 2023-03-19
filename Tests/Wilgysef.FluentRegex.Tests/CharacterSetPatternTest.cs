using static Wilgysef.FluentRegex.CharacterSetPattern;

namespace Wilgysef.FluentRegex.Tests;

public class CharacterSetPatternTest
{
    [Fact]
    public void Characters()
    {
        var pattern = new PatternBuilder().CharacterSet("ab3");
        pattern.ToString().ShouldBe("[ab3]");

        pattern = new PatternBuilder().CharacterSet('a', 'b', '3');
        pattern.ToString().ShouldBe("[ab3]");

        pattern = new PatternBuilder().CharacterSet(new[] { 'a', 'b', '3' }, negated: true);
        pattern.ToString().ShouldBe("[^ab3]");

        pattern = new PatternBuilder().CharacterSet(CharacterPattern.Digit, CharacterPattern.Word);
        pattern.ToString().ShouldBe(@"[\d\w]");

        pattern = new PatternBuilder().CharacterSet(new[] { CharacterPattern.Digit }, negated: true);
        pattern.ToString().ShouldBe(@"[^\d]");
    }

    [Fact]
    public void CharacterRanges()
    {
        var pattern = new PatternBuilder().CharacterSet(new CharacterRange('a', 'z'));
        pattern.ToString().ShouldBe("[a-z]");

        pattern = new PatternBuilder().CharacterSet(new[] { new CharacterRange('a', 'z') }, negated: true);
        pattern.ToString().ShouldBe("[^a-z]");
    }

    [Fact]
    public void CharacterRanges_Characters()
    {
        var pattern = new PatternBuilder().CharacterSet(
            new[] { new CharacterRange('a', 'z') },
            new[] { CharacterPattern.Character('0'), CharacterPattern.Character('7') },
            negated: true);

        pattern.ToString().ShouldBe("[^a-z07]");
    }

    [Fact]
    public void CharacterRanges_Characters_SubtractedCharacters()
    {
        var pattern = new PatternBuilder().CharacterSet(
            new[] { new CharacterRange('a', 'z') },
            new[] { CharacterPattern.Character('0'), CharacterPattern.Character('7') },
            new[] { CharacterPattern.Character('w'), },
            negated: true);

        pattern.ToString().ShouldBe("[^a-z07-[w]]");
    }

    [Fact]
    public void CharacterRanges_Characters_SubtractedCharacterRanges_SubtractedCharacters()
    {
        var pattern = new PatternBuilder().CharacterSet(
            new[] { new CharacterRange('a', 'z') },
            new[] { CharacterPattern.Character('0'), CharacterPattern.Character('7') },
            new[] { new CharacterRange('b', 'g') },
            new[] { CharacterPattern.Character('w'), },
            negated: true);

        pattern.ToString().ShouldBe("[^a-z07-[b-gw]]");
    }

    [Fact]
    public void Empty()
    {
        var pattern = new PatternBuilder().CharacterSet(Array.Empty<char>());

        pattern.ToString().ShouldBe("");
    }

    [Fact]
    public void Escaped()
    {
        foreach (var c in "[]-^\\")
        {
            var pattern = new PatternBuilder().CharacterSet(
                CharacterPattern.Character('a'),
                CharacterPattern.Character(c));

            pattern.ToString().ShouldBe(@$"[a\{c}]");
        }
    }

    [Fact]
    public void Single_Character()
    {
        var entries = new List<(CharacterPattern, string)>
        {
            (CharacterPattern.Character('a'), "a"),
            (CharacterPattern.Digit, @"\d"),
            (CharacterPattern.Character('['), @"\["),
            (CharacterPattern.Character('.'), @"\."),
        };

        foreach (var (character, expected) in entries)
        {
            var pattern = new PatternBuilder().CharacterSet(character);

            pattern.ToString().ShouldBe(expected);
        }
    }

    [Fact]
    public void CharacterRange_Single()
    {
        var pattern = new PatternBuilder().CharacterSet(
            new[] { new CharacterRange('a', 'a') },
            new[] { CharacterPattern.Character('e') });

        pattern.ToString().ShouldBe("[ae]");
    }

    [Fact]
    public void Single_CharacterRange_Single()
    {
        var pattern = new PatternBuilder().CharacterSet(new CharacterRange('a', 'a'));

        pattern.ToString().ShouldBe("a");
    }

    [Fact]
    public void SubtractedCharacterRange_Single()
    {
        var pattern = new PatternBuilder().CharacterSet(
            Array.Empty<CharacterRange>(),
            new[] { CharacterPattern.Character('e') },
            new[] { new CharacterRange('a', 'a') },
            Array.Empty<CharacterPattern>());

        pattern.ToString().ShouldBe("[e-[a]]");
    }

    [Fact]
    public void FluentCharacterRange()
    {
        var pattern = new CharacterSetPattern('a', 'b');
        pattern.Characters.Count.ShouldBe(2);

        pattern.WithCharacterRange('0', '9');
        pattern.ToString().ShouldBe("[0-9ab]");
        pattern.CharacterRanges.Count.ShouldBe(1);

        pattern.WithCharacterRange(CharacterPattern.Character('c'), CharacterPattern.Character('e'));
        pattern.ToString().ShouldBe("[0-9c-eab]");
        pattern.CharacterRanges.Count.ShouldBe(2);
    }

    [Fact]
    public void FluentSubtractedCharacters()
    {
        var pattern = new CharacterSetPattern('a', 'b');

        pattern.WithSubtractedCharacters('0', '9');
        pattern.ToString().ShouldBe("[ab-[09]]");
        pattern.SubtractedCharacters.Count.ShouldBe(2);

        pattern.WithSubtractedCharacters(CharacterPattern.Character('5'));
        pattern.ToString().ShouldBe("[ab-[095]]");
        pattern.SubtractedCharacters.Count.ShouldBe(3);
    }

    [Fact]
    public void FluentSubtractedCharacterRange()
    {
        var pattern = new CharacterSetPattern('a', 'b');

        pattern.WithSubtractedCharacterRange('0', '9');
        pattern.ToString().ShouldBe("[ab-[0-9]]");
        pattern.SubtractedCharacterRanges.Count.ShouldBe(1);

        pattern.WithSubtractedCharacterRange(CharacterPattern.Character('c'), CharacterPattern.Character('e'));
        pattern.ToString().ShouldBe("[ab-[0-9c-e]]");
        pattern.SubtractedCharacterRanges.Count.ShouldBe(2);
    }

    [Fact]
    public void FluentNegate()
    {
        var pattern = new CharacterSetPattern('a', 'b');

        pattern.Negate();
        pattern.ToString().ShouldBe("[^ab]");
    }

    [Fact]
    public void GetValue()
    {
        _ = new CharacterRange(CharacterPattern.Control('A'), CharacterPattern.Escape);
        _ = new CharacterRange(CharacterPattern.Hexadecimal("1e"), CharacterPattern.Octal("40"));
        _ = new CharacterRange(CharacterPattern.Unicode("1234"), CharacterPattern.Unicode("5678"));
    }

    [Fact]
    public void NoWrap()
    {
        var pattern = new PatternBuilder().CharacterSet('a', 'b').OneOrMore();

        pattern.ToString().ShouldBe("[ab]+");
    }

    [Fact]
    public void Copy()
    {
        var pattern = new CharacterSetPattern(
            new[] { new CharacterRange('0', '9') },
            new[] { CharacterPattern.Character('a') },
            new[] { new CharacterRange('5', '7') },
            new[] { CharacterPattern.Character('b') });

        var copy = pattern.Copy();
        pattern.WithCharacters('d');

        copy.ToString().ShouldBe("[0-9a-[5-7b]]");
    }

    [Fact]
    public void Fail_SubtractedCharactersOnly()
    {
        var pattern = new PatternBuilder().CharacterSet(
            Array.Empty<CharacterRange>(),
            Array.Empty<CharacterPattern>(),
            new[] { CharacterPattern.Character('a') });

        Should.Throw<InvalidOperationException>(() => pattern.ToString());
    }

    [Fact]
    public void Fail_CharacterRange_NotLiteral()
    {
        Should.Throw<ArgumentException>(() => new CharacterRange(CharacterPattern.Digit, CharacterPattern.Character('a')));
        Should.Throw<ArgumentException>(() => new CharacterRange(CharacterPattern.Character('a'), CharacterPattern.Digit));
    }

    [Fact]
    public void Fail_CharacterRange_OutOfOrder()
    {
        Should.Throw<ArgumentException>(() => new CharacterRange('z', 'a'));
    }
}
