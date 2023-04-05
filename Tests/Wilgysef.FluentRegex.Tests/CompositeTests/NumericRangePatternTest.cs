using System.Text.RegularExpressions;
using Wilgysef.FluentRegex.Composites;

namespace Wilgysef.FluentRegex.Tests.CompositeTests;

public class NumericRangePatternTest
{
    [Theory]
    [InlineData(1372, 5246, @"137[2-9]|13[89]\d|1[4-9]\d{2}|[2-4]\d{3}|5[01]\d{2}|52[0-3]\d|524[0-6]")]
    [InlineData(593, 626, @"59[3-9]|6[01]\d|62[0-6]")]
    [InlineData(123, 123, "123")]
    [InlineData(79, 99, @"79|[89]\d")]
    [InlineData(100, 256, @"1\d{2}|2[0-4]\d|25[0-6]")]
    [InlineData(100, 299, @"[12]\d{2}")]
    [InlineData(701, 899, @"70[1-9]|7[1-9]\d|8\d{2}")]

    [InlineData(13, 6215, @"1[3-9]|[2-9]\d|[1-9]\d{2}|[1-5]\d{3}|6[01]\d{2}|620\d|621[0-5]")]
    [InlineData(51, 501, @"5[1-9]|[6-9]\d|[1-4]\d{2}|50[01]")]
    [InlineData(99, 100, @"99|100")]
    [InlineData(0, 100, @"\d|[1-9]\d|100")]
    [InlineData(7, 1234, @"[7-9]|[1-9]\d|[1-9]\d{2}|1(?:[01]\d{2}|2[0-2]\d|23[0-4])")]
    [InlineData(321, 12345, @"32[1-9]|3[3-9]\d|[4-9]\d{2}|[1-9]\d{3}|1(?:[01]\d{3}|2[0-2]\d{2}|23[0-3]\d|234[0-5])")]

    [InlineData(123, 125, "12[3-5]")]
    [InlineData(120, 129, @"12\d")]
    [InlineData(1269, 1287, @"12(?:69|7\d|8[0-7])")]
    [InlineData(1279, 1299, @"12(?:79|[89]\d)")]
    [InlineData(12379, 12989, @"12(?:379|3[89]\d|[4-8]\d{2}|9[0-7]\d|98\d)")]
    [InlineData(9927, 9931, @"99(?:2[7-9]|3[01])")]

    [InlineData(-271, -32, @"-(?:3[2-9]|[4-9]\d|1\d{2}|2[0-6]\d|27[01])")]
    [InlineData(-271, -3, @"-(?:[3-9]|[1-9]\d|1\d{2}|2[0-6]\d|27[01])")]
    [InlineData(-27, -3, @"-(?:[3-9]|1\d|2[0-7])")]
    [InlineData(-7, -3, "-[3-7]")]
    [InlineData(-53, 0, @"-(?:[1-9]|[1-4]\d|5[0-3])|0")]
    [InlineData(-15, 2, @"-(?:[1-9]|1[0-5])|[0-2]")]
    [InlineData(-15, 27, @"-(?:[1-9]|1[0-5])|\d|1\d|2[0-7]")]
    [InlineData(-15, 273, @"-(?:[1-9]|1[0-5])|\d|[1-9]\d|1\d{2}|2[0-6]\d|27[0-3]")]
    [InlineData(-125, -123, @"-12[3-5]")]
    public void NumericRange(int min, int max, string expected)
    {
        var pattern = Pattern.NumericRange(min, max);
        var regex = CompilePattern(pattern);

        GetTestRange(min, max, out var start, out var end);

        for (var cur = start; cur <= end; cur++)
        {
            ShouldRegexMatch(regex, cur.ToString(), cur >= min && cur <= max);
        }

        pattern.ToString().ShouldBe(expected);
    }

    [Fact]
    public void NumericRange_Long()
    {
        Pattern.NumericRange(12379L, 12989L).ToString()
            .ShouldBe(@"12(?:379|3[89]\d|[4-8]\d{2}|9[0-7]\d|98\d)");
    }

    [Fact]
    public void InvalidRange()
    {
        Should.Throw<ArgumentException>(() => Pattern.NumericRange(5, 0));
    }

    [Theory]
    [InlineData(152, 361, @"15[2-9]|1[6-9]\d|2\d{2}|3[0-5]\d|36[01]")]
    [InlineData(2, 1023, @"000[2-9]|00[1-9]\d|0[1-9]\d{2}|10(?:[01]\d|2[0-3])")]

    [InlineData(-271, -3, @"-(?:00[3-9]|0[1-9]\d|1\d{2}|2[0-6]\d|27[01])")]
    [InlineData(-53, 0, @"-(?:0[1-9]|[1-4]\d|5[0-3])|00")]
    [InlineData(-15, 27, @"-(?:0[1-9]|1[0-5])|0\d|1\d|2[0-7]")]
    [InlineData(-15, 418, @"-(?:00[1-9]|01[0-5])|00\d|0[1-9]\d|[1-3]\d{2}|40\d|41[0-8]")]
    public void LeadingZeros_Required(int min, int max, string expected)
    {
        var pattern = Pattern.NumericRange(min, max, leadingZeros: LeadingZeros.Required);
        var regex = CompilePattern(pattern);

        GetLeadingZeroTestRange(min, max, out var start, out var end);

        var maxStrLength = Math.Max(Math.Abs(min).ToString().Length, Math.Abs(max).ToString().Length);

        for (var cur = start; cur <= end; cur++)
        {
            string curStr;

            if (cur < 0)
            {
                curStr = (-cur).ToString();
                curStr = $"-{new string('0', maxStrLength - curStr.Length)}{curStr}";
            }
            else
            {
                curStr = cur.ToString();
                curStr = new string('0', maxStrLength - curStr.Length) + curStr;
            }
            
            ShouldRegexMatch(regex, curStr, cur >= min && cur <= max);
        }

        pattern.ToString().ShouldBe(expected);
    }

    [Theory]
    [InlineData(152, 361, @"15[2-9]|1[6-9]\d|2\d{2}|3[0-5]\d|36[01]")]
    [InlineData(2, 1023, @"(?:000)?[2-9]|(?:00)?[1-9]\d|0?[1-9]\d{2}|10(?:[01]\d|2[0-3])")]

    [InlineData(-271, -3, @"-(?:(?:00)?[3-9]|0?[1-9]\d|1\d{2}|2[0-6]\d|27[01])")]
    [InlineData(-53, 0, @"-(?:0?[1-9]|[1-4]\d|5[0-3])|0?0")]
    [InlineData(-15, 27, @"-(?:0?[1-9]|1[0-5])|0?\d|1\d|2[0-7]")]
    [InlineData(-15, 418, @"-(?:(?:00)?[1-9]|0?1[0-5])|(?:00)?\d|0?[1-9]\d|[1-3]\d{2}|40\d|41[0-8]")]
    public void LeadingZeros_Optional(int min, int max, string expected)
    {
        var pattern = Pattern.NumericRange(min, max, leadingZeros: LeadingZeros.Optional);
        var regex = CompilePattern(pattern);

        GetLeadingZeroTestRange(min, max, out var start, out var end);

        var maxStrLength = Math.Max(Math.Abs(min).ToString().Length, Math.Abs(max).ToString().Length);

        for (var cur = start; cur <= end; cur++)
        {
            var shouldMatch = cur >= min && cur <= max;
            string curStr;
            string paddedCurStr;

            if (cur < 0)
            {
                curStr = (-cur).ToString();
                paddedCurStr = $"-{new string('0', maxStrLength - curStr.Length)}{curStr}";
                curStr = cur.ToString();
            }
            else
            {
                curStr = cur.ToString();
                paddedCurStr = new string('0', maxStrLength - curStr.Length) + curStr;
            }

            ShouldRegexMatch(regex, curStr, shouldMatch);
            ShouldRegexMatch(regex, paddedCurStr, shouldMatch);
        }

        pattern.ToString().ShouldBe(expected);
    }

    private static Regex CompilePattern(Pattern pattern)
    {
        return new PatternBuilder().BeginLine.Concat(pattern).EndLine.Compile();
    }

    private static void ShouldRegexMatch(Regex regex, string match, bool shouldMatch)
    {
        regex.IsMatch(match).ShouldBe(shouldMatch, $"{match} {(shouldMatch ? "should" : "should not")} match");
    }

    private static void GetTestRange(int min, int max, out int start, out int end)
    {
        start = min >= 0
            ? (int)Math.Pow(10, Math.Max(0, min.ToString().Length - 2))
            : (int)-Math.Pow(10, min.ToString().Length - 1);
        end = max >= 0
            ? (int)Math.Pow(10, max.ToString().Length)
            : (int)-Math.Pow(10, Math.Max(0, max.ToString().Length - 3));
    }

    private static void GetLeadingZeroTestRange(int min, int max, out int start, out int end)
    {
        start = min >= 0
            ? (int)Math.Pow(10, Math.Max(0, min.ToString().Length - 2))
            : (int)-Math.Pow(10, min.ToString().Length - 1) + 1;
        end = max >= 0
            ? (int)Math.Pow(10, max.ToString().Length) - 1
            : (int)-Math.Pow(10, Math.Max(0, max.ToString().Length - 3)) + 1;
    }
}
