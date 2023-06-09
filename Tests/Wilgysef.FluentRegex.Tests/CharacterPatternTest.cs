﻿namespace Wilgysef.FluentRegex.Tests;

public class CharacterPatternTest
{
    [Theory]
    [InlineData('a', "a")]
    [InlineData('.', @"\.")]
    [InlineData('\0', @"\00")]
    [InlineData('\\', @"\\")]
    [InlineData('\a', @"\a")]
    [InlineData('\b', @"[\b]")]
    [InlineData('\f', @"\f")]
    [InlineData('\n', @"\n")]
    [InlineData('\r', @"\r")]
    [InlineData('\t', @"\t")]
    [InlineData('\v', @"\v")]
    [InlineData('^', @"\^")]
    [InlineData('[', @"\[")]
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
    [InlineData("0x31", @"\x31")]
    [InlineData("0X31", @"\x31")]
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
    [InlineData("777", @"\777")]
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
    [InlineData("0xAB3D", @"\uAB3D")]
    [InlineData("0XAB3D", @"\uAB3D")]
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

        pattern = CharacterPattern.Word;
        pattern.TryGetChar(out _).ShouldBeFalse();
    }

    [Fact]
    public void TryGetValue()
    {
        var pattern = CharacterPattern.Character('c');

        pattern.TryGetValue(out var value).ShouldBeTrue();
        value.ShouldBe('c');

        pattern = CharacterPattern.Hexadecimal("ab");
        pattern.TryGetValue(out value).ShouldBeTrue();
        value.ShouldBe(0xAB);

        pattern = CharacterPattern.Word;
        pattern.TryGetValue(out _).ShouldBeFalse();
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

    [Fact]
    public void NoWrap()
    {
        var pattern = new PatternBuilder().Digit.OneOrMore();

        pattern.ToString().ShouldBe(@"\d+");
    }

    [Fact]
    public void Unwrap()
    {
        var pattern = CharacterPattern.Character('a');

        pattern.Unwrap().ShouldBe(pattern);
    }

    [Fact]
    public void Copy()
    {
        var pattern = CharacterPattern.Word;

        pattern.ToString().ShouldBe(pattern.Copy().ToString());

        pattern = CharacterPattern.Character('a');
        pattern.ToString().ShouldBe(pattern.Copy().ToString());

        pattern = CharacterPattern.Control('a');
        pattern.ToString().ShouldBe(pattern.Copy().ToString());

        pattern = CharacterPattern.Escape;
        pattern.ToString().ShouldBe(pattern.Copy().ToString());

        pattern = CharacterPattern.Hexadecimal("a2");
        pattern.ToString().ShouldBe(pattern.Copy().ToString());

        pattern = CharacterPattern.Octal("03");
        pattern.ToString().ShouldBe(pattern.Copy().ToString());

        pattern = CharacterPattern.Unicode("1234");
        pattern.ToString().ShouldBe(pattern.Copy().ToString());
    }

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
