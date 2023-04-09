using Wilgysef.FluentRegex.Exceptions;

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
        
        pattern = new PatternBuilder().CharacterSet(new[] { CharacterPattern.Character('\b'), CharacterPattern.Digit });
        pattern.ToString().ShouldBe(@"[\b\d]");
    }

    [Fact]
    public void CharacterRanges()
    {
        var pattern = new PatternBuilder().CharacterSet(new CharacterRange('a', 'z'));
        pattern.ToString().ShouldBe("[a-z]");

        pattern = new PatternBuilder().CharacterSet(new[] { new CharacterRange('a', 'z') }, negated: true);
        pattern.ToString().ShouldBe("[^a-z]");

        pattern = new PatternBuilder().CharacterSet(new[] { CharacterRange.Hexadecimal("00", "FF") }, negated: true);
        pattern.ToString().ShouldBe(@"[^\x00-\xFF]");

        pattern = new PatternBuilder().CharacterSet(new[] { CharacterRange.Octal("00", "12") }, negated: true);
        pattern.ToString().ShouldBe(@"[^\00-\12]");

        pattern = new PatternBuilder().CharacterSet(new[] { CharacterRange.Unicode("0000", "FFFF") }, negated: true);
        pattern.ToString().ShouldBe(@"[^\u0000-\uFFFF]");
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
            (CharacterPattern.Character('\b'), @"[\b]"),
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
    public void AdjacentRange()
    {
        var pattern = new PatternBuilder().CharacterSet(new CharacterRange('a', 'b'));

        pattern.ToString().ShouldBe("[ab]");
    }

    [Fact]
    public void Simplify_Characters_InRange()
    {
        var pattern = new PatternBuilder().CharacterSet(
            new[] { new CharacterRange('a', 'f') },
            new[] { CharacterPattern.Character('c') });

        pattern.ToString().ShouldBe("[a-f]");
    }

    [Fact]
    public void Simplify_Characters_Adjacent()
    {
        var pattern = new PatternBuilder().CharacterSet(
            new[] { new CharacterRange('b', 'f') },
            new[]
            {
                CharacterPattern.Character('a'),
                CharacterPattern.Character('g'),
                CharacterPattern.Character('h')
            });

        pattern.ToString().ShouldBe("[a-h]");
    }

    [Fact]
    public void Simplify_Characters_Range()
    {
        var pattern = new PatternBuilder().CharacterSet(
            new[] { new CharacterRange('b', 'f'), new CharacterRange('a', 'k'), });

        pattern.ToString().ShouldBe("[a-k]");
    }

    [Fact]
    public void Simplify_Characters_ToRange()
    {
        var pattern = new PatternBuilder().CharacterSet("1def0abcz");

        pattern.ToString().ShouldBe("[a-f10z]");
    }

    [Fact]
    public void Simplify_Characters_ToRange_Special()
    {
        var pattern = new PatternBuilder().CharacterSet(new[]
        {
            CharacterPattern.Character('a'),
            CharacterPattern.Digit,
            CharacterPattern.Hexadecimal("7B"),
            CharacterPattern.Unicode("123A"),
        });

        pattern.ToString().ShouldBe(@"[a\d\x7B\u123A]");
    }

    [Fact]
    public void Simplify_Characters_Range_Subtracted()
    {
        var pattern = new PatternBuilder().CharacterSet(
            new[] { new CharacterRange('a', 'f') },
            Array.Empty<CharacterPattern>(),
            new[] { new CharacterRange('b', 'd') },
            new[] { CharacterPattern.Character('c') });

        pattern.ToString().ShouldBe("[a-f-[b-d]]");
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
        pattern.ToString().ShouldBe("[0-9a-e]");
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
        _ = new CharacterRange(CharacterPattern.Control('a'), CharacterPattern.Escape);
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
    public void Unwrap()
    {
        var pattern = new CharacterSetPattern("abc");

        pattern.Unwrap().ShouldBe(pattern);
    }

    [Fact]
    public void Unwrap_Single()
    {
        var character = CharacterPattern.Character('a');
        var pattern = new CharacterSetPattern(character);

        pattern.Unwrap().ShouldBe(character);
    }

    [Fact]
    public void Unwrap_Single_Negated()
    {
        var pattern = new CharacterSetPattern("a", negated: true);

        pattern.Unwrap().ShouldBe(pattern);
    }

    [Fact]
    public void Unwrap_RangeSingle()
    {
        var character = CharacterPattern.Character('a');
        var pattern = new CharacterSetPattern(new CharacterRange(character, character));

        pattern.Unwrap().ShouldBe(character);

        pattern = new CharacterSetPattern(
            new[] { new CharacterRange(character, character) },
            new[] { character });
        pattern.Unwrap().ShouldBe(character);
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
    public void SubtractedCharactersOnly()
    {
        var pattern = new PatternBuilder().CharacterSet(
            Array.Empty<CharacterRange>(),
            Array.Empty<CharacterPattern>(),
            new[] { CharacterPattern.Character('a') });

        Should.Throw<InvalidPatternException>(() => pattern.ToString());
    }

    [Fact]
    public void CharacterRange_NotLiteral()
    {
        Should.Throw<ArgumentException>(() => new CharacterRange(CharacterPattern.Digit, CharacterPattern.Character('a')));
        Should.Throw<ArgumentException>(() => new CharacterRange(CharacterPattern.Character('a'), CharacterPattern.Digit));
    }

    [Fact]
    public void CharacterRange_OutOfOrder()
    {
        Should.Throw<ArgumentException>(() => new CharacterRange('z', 'a'));
    }
}
