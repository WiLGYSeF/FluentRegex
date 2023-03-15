namespace Wilgysef.FluentRegex.Tests;

public class CharacterPatternTest
{
    [Theory]
    [InlineData('a', "a")]
    [InlineData('.', @"\.")]
    [InlineData('\0', @"\0")]
    [InlineData('\\', @"\\")]
    [InlineData('\a', @"\a")]
    [InlineData('\b', @"[\b]")]
    [InlineData('\f', @"\f")]
    [InlineData('\n', @"\n")]
    [InlineData('\r', @"\r")]
    [InlineData('\t', @"\t")]
    [InlineData('\v', @"\v")]
    public void Character(char character, string expected)
    {
        var pattern = new PatternBuilder().Character(character);

        pattern.ToString().ShouldBe(expected);
    }

    [Theory]
    [InlineData('a', @"\ca")]
    [InlineData('B', @"\cB")]
    [InlineData('3', null)]
    public void Control(char character, string expected)
    {
        ShouldCreatePattern(
            () => new PatternBuilder().Control(character),
            expected);
    }

    [Fact]
    public void Escape()
    {
        var pattern = new PatternBuilder().Escape;

        pattern.ToString().ShouldBe(@"\e");
    }

    [Theory]
    [InlineData("", null)]
    [InlineData("a", null)]
    [InlineData("ab", @"\xab")]
    [InlineData("abc", null)]
    [InlineData("ag", null)]
    [InlineData("31", @"\x31")]
    public void Hexadecimal(string value, string expected)
    {
        ShouldCreatePattern(
            () => new PatternBuilder().Hexadecimal(value),
            expected);
    }

    [Theory]
    [InlineData("", null)]
    [InlineData("1", null)]
    [InlineData("01", @"\01")]
    [InlineData("12", @"\12")]
    [InlineData("123", @"\123")]
    [InlineData("1234", null)]
    [InlineData("28", null)]
    [InlineData("128", null)]
    public void Octal(string value, string expected)
    {
        ShouldCreatePattern(
            () => new PatternBuilder().Octal(value),
            expected);
    }

    [Theory]
    [InlineData("", null)]
    [InlineData("a", null)]
    [InlineData("ab", null)]
    [InlineData("abc", null)]
    [InlineData("ab3d", @"\uab3d")]
    [InlineData("abcy", null)]
    public void Unicode(string value, string expected)
    {
        ShouldCreatePattern(
            () => new PatternBuilder().Unicode(value),
            expected);
    }

    [Fact]
    public void TryGetChar()
    {
        var pattern = CharacterPattern.Character('c');

        pattern.TryGetChar(out var character).ShouldBeTrue();
        character.ShouldBe('c');

        pattern = CharacterPattern.Hexadecimal("ab");
        pattern.TryGetChar(out _).ShouldBeFalse();
    }

    [Fact]
    public void Word()
    {
        var pattern = new PatternBuilder().Word;
        
        pattern.ToString().ShouldBe(@"\w");
    }

    [Fact]
    public void NonWord()
    {
        var pattern = new PatternBuilder().NonWord;

        pattern.ToString().ShouldBe(@"\W");
    }

    [Fact]
    public void Digit()
    {
        var pattern = new PatternBuilder().Digit;

        pattern.ToString().ShouldBe(@"\d");
    }

    [Fact]
    public void NonDigit()
    {
        var pattern = new PatternBuilder().NonDigit;

        pattern.ToString().ShouldBe(@"\D");
    }

    [Fact]
    public void Whitespace()
    {
        var pattern = new PatternBuilder().Whitespace;

        pattern.ToString().ShouldBe(@"\s");
    }

    [Fact]
    public void NonWhitespace()
    {
        var pattern = new PatternBuilder().NonWhitespace;

        pattern.ToString().ShouldBe(@"\S");
    }

    [Fact]
    public void Category()
    {
        var pattern = new PatternBuilder().Category("Lu");

        pattern.ToString().ShouldBe(@"\p{Lu}");
    }

    [Fact]
    public void NonCategory()
    {
        var pattern = new PatternBuilder().NonCategory("Lu");

        pattern.ToString().ShouldBe(@"\P{Lu}");
    }


    // TODO: move?
    private static void ShouldCreatePattern(Func<Pattern> func, string? expected)
    {
        if (expected != null)
        {
            var pattern = func();
            pattern.ToString().ShouldBe(expected);
        }
        else
        {
            Should.Throw<ArgumentException>(() => func().ToString());
        }
    }
}
