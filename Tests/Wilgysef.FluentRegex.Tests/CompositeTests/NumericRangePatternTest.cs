﻿using System.Text.RegularExpressions;
using Wilgysef.FluentRegex.Composites;

namespace Wilgysef.FluentRegex.Tests.CompositeTests;

public class NumericRangePatternTest
{
    [Theory]
    [InlineData(1372, 5246, @"137[2-9]|13[8-9]\d|1[4-9]\d{2}|[2-4]\d{3}|5[0-1]\d{2}|52[0-3]\d|524[0-6]")]
    [InlineData(593, 626, @"59[3-9]|6[0-1]\d|62[0-6]")]
    [InlineData(123, 123, "123")]
    [InlineData(79, 99, @"79|[8-9]\d")]
    [InlineData(100, 256, @"1\d{2}|2[0-4]\d|25[0-6]")]
    [InlineData(100, 299, @"[1-2]\d{2}")]
    [InlineData(701, 899, @"70[1-9]|7[1-9]\d|8\d{2}")]

    [InlineData(13, 6215, @"1[3-9]|[2-9]\d|[1-9]\d{2}|[1-5]\d{3}|6[0-1]\d{2}|620\d|621[0-5]")]
    [InlineData(51, 501, @"5[1-9]|[6-9]\d|[1-4]\d{2}|50[0-1]")]
    [InlineData(99, 100, @"99|100")]
    [InlineData(7, 1234, @"[7-9]|[1-9]\d|[1-9]\d{2}|1(?:[0-1]\d{2}|2[0-2]\d|23[0-4])")]
    [InlineData(321, 12345, @"32[1-9]|3[3-9]\d|[4-9]\d{2}|[1-9]\d{3}|1(?:[0-1]\d{3}|2[0-2]\d{2}|23[0-3]\d|234[0-5])")]

    [InlineData(123, 125, "12[3-5]")]
    [InlineData(120, 129, @"12\d")]
    [InlineData(1269, 1287, @"12(?:69|7\d|8[0-7])")]
    [InlineData(1279, 1299, @"12(?:79|[8-9]\d)")]
    [InlineData(12379, 12989, @"12(?:379|3[8-9]\d|[4-8]\d{2}|9[0-7]\d|98\d)")]
    [InlineData(9927, 9931, @"99(?:2[7-9]|3[0-1])")]

    [InlineData(5, 0, null)]
    public void NumericRange(int min, int max, string? expected)
    {
        if (expected == null)
        {
            Should.Throw<Exception>(() => Pattern.NumericRange(min, max));
            return;
        }

        var pattern = Pattern.NumericRange(min, max);
        var regex = BuildPattern(pattern);

        var start = Math.Pow(10, Math.Max(0, min.ToString().Length - 2));
        var end = Math.Pow(10, max.ToString().Length);

        for (var cur = start; cur <= end; cur++)
        {
            ShouldRegexMatch(regex, cur.ToString(), cur >= min && cur <= max);
        }

        pattern.ToString().ShouldBe(expected);
    }

    [Theory]
    [InlineData(152, 361, @"15[2-9]|1[6-9]\d|2\d{2}|3[0-5]\d|36[0-1]")]
    [InlineData(2, 1023, @"000[2-9]|00(?:[1-9]\d)|0(?:[1-9]\d{2})|10(?:[0-1]\d|2[0-3])")]
    public void LeadingZeros_Required(int min, int max, string expected)
    {
        var pattern = Pattern.NumericRange(min, max, leadingZeros: LeadingZeros.Required);
        var regex = BuildPattern(pattern);

        var start = Math.Pow(10, Math.Max(0, min.ToString().Length - 2));
        var end = Math.Pow(10, max.ToString().Length) - 1;

        var maxStr = max.ToString();

        for (var cur = start; cur <= end; cur++)
        {
            var curStr = cur.ToString();
            curStr = new string('0', maxStr.Length - curStr.Length) + curStr;
            ShouldRegexMatch(regex, curStr, cur >= min && cur <= max);
        }

        pattern.ToString().ShouldBe(expected);
    }

    [Theory]
    [InlineData(152, 361, @"15[2-9]|1[6-9]\d|2\d{2}|3[0-5]\d|36[0-1]")]
    [InlineData(2, 1023, @"(?:000)?[2-9]|(?:00)?(?:[1-9]\d)|0?(?:[1-9]\d{2})|10(?:[0-1]\d|2[0-3])")]
    public void LeadingZeros_Optional(int min, int max, string expected)
    {
        var pattern = Pattern.NumericRange(min, max, leadingZeros: LeadingZeros.Optional);
        var regex = BuildPattern(pattern);

        var start = Math.Pow(10, Math.Max(0, min.ToString().Length - 2));
        var end = Math.Pow(10, max.ToString().Length) - 1;

        var maxStr = max.ToString();

        for (var cur = start; cur <= end; cur++)
        {
            var shouldMatch = cur >= min && cur <= max;
            var curStr = cur.ToString();
            var paddedCurStr = new string('0', maxStr.Length - curStr.Length) + curStr;

            ShouldRegexMatch(regex, curStr, shouldMatch);
            ShouldRegexMatch(regex, paddedCurStr, shouldMatch);
        }

        pattern.ToString().ShouldBe(expected);
    }

    private static Regex BuildPattern(Pattern pattern)
    {
        return new PatternBuilder().BeginLine.Concat(pattern).EndLine.Compile();
    }

    private static void ShouldRegexMatch(Regex regex, string match, bool shouldMatch)
    {
        regex.IsMatch(match).ShouldBe(shouldMatch, $"{match} {(shouldMatch ? "should" : "should not")} match");
    }
}
