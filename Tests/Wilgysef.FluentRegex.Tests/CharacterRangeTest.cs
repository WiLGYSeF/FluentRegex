namespace Wilgysef.FluentRegex.Tests;

public class CharacterRangeTest
{
    [Fact]
    public void Overlap_Empty()
    {
        var ranges = CharacterRange.Overlap(Array.Empty<CharacterRange>());

        ranges.Count.ShouldBe(0);
    }

    [Fact]
    public void Overlap_None()
    {
        var ranges = CharacterRange.Overlap(new[]
        {
            new CharacterRange('a', 'f'),
            new CharacterRange('0', '9'),
        });

        ranges.Count.ShouldBe(2);
        ShouldBeRange(ranges[0], 'a', 'f');
        ShouldBeRange(ranges[1], '0', '9');
    }

    [Fact]
    public void Overlap_MultipleStart()
    {
        var ranges = CharacterRange.Overlap(new[]
        {
            new CharacterRange('a', 'f'),
            new CharacterRange('c', 'e'),
            new CharacterRange('d', 'j'),
            new CharacterRange('i', 'm'),
        });

        ranges.Count.ShouldBe(1);
        ShouldBeRange(ranges[0], 'a', 'm');
    }

    [Fact]
    public void Overlap_Start_EqualAdjacent()
    {
        var ranges = CharacterRange.Overlap(new[]
        {
            new CharacterRange('a', 'f'),
            new CharacterRange('f', 'i'),
            new CharacterRange('j', 'm'),
        });

        ranges.Count.ShouldBe(1);
        ShouldBeRange(ranges[0], 'a', 'm');
    }

    [Fact]
    public void Contains()
    {
        var range = new CharacterRange('a', 'z');

        range.Contains('m').ShouldBeTrue();
    }

    [Fact]
    public void Range_String()
    {
        var range = new CharacterRange('a', 'z');
        range.ToString().ShouldBe("a-z");

        range = new CharacterRange('a', 'a');
        range.ToString().ShouldBe("a");
    }

    private static void ShouldBeRange(CharacterRange range, char start, char end)
    {
        range.Start.TryGetChar(out var startChar).ShouldBeTrue();
        startChar.ShouldBe(start);
        range.End.TryGetChar(out var endChar).ShouldBeTrue();
        endChar.ShouldBe(end);
    }
}
