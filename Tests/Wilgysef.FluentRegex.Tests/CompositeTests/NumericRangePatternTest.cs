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

    [Theory]
    [InlineData(1.0512, 21.23, LeadingZeros.None, 0, null, @"1\.051[2-9]\d*|1\.05[2-9]\d*|1\.0[6-9]\d*|1\.[1-9]\d*|(?:[2-9]|1\d|20)(?:\.\d*)?|21\.?|21\.[01]\d*|21\.2[0-2]\d*|21\.230*")]
    [InlineData(1.0599, 16.1, LeadingZeros.None, 0, null, @"1\.0599\d*|1\.0[6-9]\d*|1\.[1-9]\d*|(?:[2-9]|1[0-5])(?:\.\d*)?|16\.?|16\.0\d*|16\.10*")]
    [InlineData(24.932, 33, LeadingZeros.None, 0, null, @"24\.93[2-9]\d*|24\.9[4-9]\d*|(?:2[5-9]|3[0-2])(?:\.\d*)?|33\.?|33\.0*")]
    [InlineData(24, 33.152, LeadingZeros.None, 0, null, @"24(?:\.\d*)?|(?:2[5-9]|3[0-2])(?:\.\d*)?|33\.?|33\.0\d*|33\.1[0-4]\d*|33\.15[01]\d*|33\.1520*")]
    [InlineData(1.2, 2.489, LeadingZeros.None, 0, null, @"1\.[2-9]\d*|2\.?|2\.[0-3]\d*|2\.4[0-7]\d*|2\.48[0-8]\d*|2\.4890*")]
    [InlineData(1.2, 2.4894, LeadingZeros.None, 0, null, @"1\.[2-9]\d*|2\.?|2\.[0-3]\d*|2\.4[0-7]\d*|2\.48[0-8]\d*|2\.489[0-3]\d*|2\.48940*")]
    [InlineData(5.47, 7.47, LeadingZeros.None, 0, null, @"5\.4[7-9]\d*|5\.[5-9]\d*|6(?:\.\d*)?|7\.?|7\.[0-3]\d*|7\.4[0-6]\d*|7\.470*")]
    [InlineData(1.234, 1.234, LeadingZeros.None, 0, null, @"1\.2340*")]
    [InlineData(1.1, 1.27, LeadingZeros.None, 0, null, @"1\.1\d*|1\.2|1\.2[0-6]\d*|1\.270*")]
    [InlineData(1.234, 1.235, LeadingZeros.None, 0, null, @"1\.234\d*|1\.2350*")]
    [InlineData(1.234, 1.2567, LeadingZeros.None, 0, null, @"1\.23[4-9]\d*|1\.24\d*|1\.25|1\.25[0-5]\d*|1\.256[0-6]\d*|1\.25670*")]
    [InlineData(1.234, 1.2567, LeadingZeros.None, 2, null, @"1\.23[4-9]\d*|1\.24\d*|1\.25|1\.25[0-5]\d*|1\.256[0-6]\d*|1\.25670*")]
    [InlineData(1.2345, 1.267, LeadingZeros.None, 0, null, @"1\.234[5-9]\d*|1\.23[56]\d*|1\.2[45]\d*|1\.26|1\.26[0-6]\d*|1\.2670*")]

    [InlineData(4.93, 233.8, LeadingZeros.Optional, 0, null, @"(?:00)?4\.9[3-9]\d*|(?:(?:00)?[5-9]|0?[1-9]\d|1\d{2}|2[0-2]\d|23[0-2])(?:\.\d*)?|233\.?|233\.[0-7]\d*|233\.80*")]

    [InlineData(4.93, 233.8, LeadingZeros.None, 2, null, @"4\.9[3-9]\d*|(?:[5-9]|[1-9]\d|1\d{2}|2[0-2]\d|23[0-2])(?:\.\d{2,})?|233\.[0-7]\d+|233\.80+")]
    [InlineData(4.93, 233.8, LeadingZeros.None, 0, 3, @"4\.9[3-9]\d?|(?:[5-9]|[1-9]\d|1\d{2}|2[0-2]\d|23[0-2])(?:\.\d{0,3})?|233\.?|233\.[0-7]\d{0,2}|233\.80{0,2}")]
    [InlineData(4.93, 233.8, LeadingZeros.None, 1, 3, @"4\.9[3-9]\d?|(?:[5-9]|[1-9]\d|1\d{2}|2[0-2]\d|23[0-2])(?:\.\d{1,3})?|233\.[0-7]\d{0,2}|233\.80{0,2}")]

    [InlineData(-8.6, -5.97, LeadingZeros.None, 0, null, @"-(?:5\.9[7-9]\d*|[67](?:\.\d*)?|8\.?|8\.[0-5]\d*|8\.60*)")]
    [InlineData(-8.684, -5.3, LeadingZeros.None, 0, null, @"-(?:5\.[3-9]\d*|[67](?:\.\d*)?|8\.?|8\.[0-5]\d*|8\.6[0-7]\d*|8\.68[0-3]\d*|8\.6840*)")]
    [InlineData(-8.6, 5.97, LeadingZeros.None, 0, null, @"-(?:0(?:\.\d*)?|[1-7](?:\.\d*)?|8\.?|8\.[0-5]\d*|8\.60*)|0(?:\.\d*)?|[1-4](?:\.\d*)?|5\.?|5\.[0-8]\d*|5\.9[0-6]\d*|5\.970*")]
    [InlineData(-8.684, 5.3, LeadingZeros.None, 0, null, @"-(?:0(?:\.\d*)?|[1-7](?:\.\d*)?|8\.?|8\.[0-5]\d*|8\.6[0-7]\d*|8\.68[0-3]\d*|8\.6840*)|0(?:\.\d*)?|[1-4](?:\.\d*)?|5\.?|5\.[0-2]\d*|5\.30*")]
    public void NumericRange_Double(
        double min,
        double max,
        LeadingZeros leadingZeros,
        int minFractionalDigits,
        int? maxFractionalDigits,
        string expected)
    {
        var pattern = Pattern.NumericRange(min, max, leadingZeros, minFractionalDigits, maxFractionalDigits);

        pattern.ToString().ShouldBe(expected);
    }

    [Fact]
    public void NumericRange_Double_FractionalSeparator()
    {
        var pattern = Pattern.NumericRange(24, 33.152, fractionalSeparator: ',');

        pattern.ToString().ShouldBe(@"24(?:,\d*)?|(?:2[5-9]|3[0-2])(?:,\d*)?|33,?|33,0\d*|33,1[0-4]\d*|33,15[01]\d*|33,1520*");
    }

    [Fact]
    public void InvalidRange()
    {
        Should.Throw<ArgumentException>(() => Pattern.NumericRange(5, 0));
    }

    [Fact]
    public void InvalidRange_Double()
    {
        Should.Throw<ArgumentException>(() => Pattern.NumericRange(5, double.NaN));
        Should.Throw<ArgumentException>(() => Pattern.NumericRange(double.NaN, 5));
        Should.Throw<ArgumentException>(() => Pattern.NumericRange(5, double.PositiveInfinity));
        Should.Throw<ArgumentException>(() => Pattern.NumericRange(double.NegativeInfinity, 5));

        Should.Throw<ArgumentException>(() => Pattern.NumericRange(5d, 0d));

        Should.Throw<ArgumentException>(() => Pattern.NumericRange(1.23, 6.94, minFractionalDigits: -1));
        Should.Throw<ArgumentException>(() => Pattern.NumericRange(8.4, 27.634, minFractionalDigits: 4, maxFractionalDigits: 3));
        Should.Throw<ArgumentException>(() => Pattern.NumericRange(8.4, 27.634, maxFractionalDigits: 2));
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
