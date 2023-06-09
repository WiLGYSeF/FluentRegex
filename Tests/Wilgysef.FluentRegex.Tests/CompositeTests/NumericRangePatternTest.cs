﻿using System.Globalization;
using System.Runtime.CompilerServices;
using System.Text;
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
    public void NumericRange_Int()
    {
        Pattern.NumericRange(12379, 12989).ToString()
            .ShouldBe(@"12(?:379|3[89]\d|[4-8]\d{2}|9[0-7]\d|98\d)");
    }

    [Theory]
    [InlineData(1.0512, 21.23, LeadingZeros.None, 0, null, @"1\.051[2-9]\d*|1\.05[2-9]\d*|1\.0[6-9]\d*|1\.[1-9]\d*|(?:[2-9]|1\d|20)(?:\.\d*)?|21\.?|21\.[01]\d*|21\.20*|21\.2[0-2]\d*|21\.230*")]
    [InlineData(1.0599, 16.1, LeadingZeros.None, 0, null, @"1\.0599\d*|1\.0[6-9]\d*|1\.[1-9]\d*|(?:[2-9]|1[0-5])(?:\.\d*)?|16\.?|16\.0\d*|16\.10*")]
    [InlineData(1.9, 5.6, LeadingZeros.None, 0, null, @"1\.9\d*|[2-4](?:\.\d*)?|5\.?|5\.[0-5]\d*|5\.60*")]
    [InlineData(1.91, 5.69, LeadingZeros.None, 0, null, @"1\.9[1-9]\d*|[2-4](?:\.\d*)?|5\.?|5\.[0-5]\d*|5\.60*|5\.6[0-8]\d*|5\.690*")]
    [InlineData(24.932, 33, LeadingZeros.None, 0, null, @"24\.93[2-9]\d*|24\.9[4-9]\d*|(?:2[5-9]|3[0-2])(?:\.\d*)?|33(?:\.0*)?")]
    [InlineData(24, 33.152, LeadingZeros.None, 0, null, @"(?:2[4-9]|3[0-2])(?:\.\d*)?|33\.?|33\.0\d*|33\.10*|33\.1[0-4]\d*|33\.150*|33\.15[01]\d*|33\.1520*")]
    [InlineData(24, 24.5, LeadingZeros.None, 0, null, @"24(?:\.0*)?|24\.[0-4]\d*|24\.50*")]
    [InlineData(1.2, 2.489, LeadingZeros.None, 0, null, @"1\.[2-9]\d*|2\.?|2\.[0-3]\d*|2\.40*|2\.4[0-7]\d*|2\.480*|2\.48[0-8]\d*|2\.4890*")]
    [InlineData(1.2, 2.4894, LeadingZeros.None, 0, null, @"1\.[2-9]\d*|2\.?|2\.[0-3]\d*|2\.40*|2\.4[0-7]\d*|2\.480*|2\.48[0-8]\d*|2\.4890*|2\.489[0-3]\d*|2\.48940*")]
    [InlineData(5.47, 7.47, LeadingZeros.None, 0, null, @"5\.4[7-9]\d*|5\.[5-9]\d*|6(?:\.\d*)?|7\.?|7\.[0-3]\d*|7\.40*|7\.4[0-6]\d*|7\.470*")]
    [InlineData(1.234, 1.234, LeadingZeros.None, 0, null, @"1\.2340*")]
    [InlineData(1.1, 1.27, LeadingZeros.None, 0, null, @"1\.1\d*|1\.2|1\.20*|1\.2[0-6]\d*|1\.270*")]
    [InlineData(1.1, 1.37, LeadingZeros.None, 0, null, @"1\.[12]\d*|1\.3|1\.30*|1\.3[0-6]\d*|1\.370*")]
    [InlineData(1.234, 1.235, LeadingZeros.None, 0, null, @"1\.234\d*|1\.2350*")]
    [InlineData(1.234, 1.236, LeadingZeros.None, 0, null, @"1\.23[45]\d*|1\.2360*")]
    [InlineData(1.234, 1.2364, LeadingZeros.None, 0, null, @"1\.23[45]\d*|1\.236|1\.2360*|1\.236[0-3]\d*|1\.23640*")]
    [InlineData(1.234, 1.2567, LeadingZeros.None, 0, null, @"1\.23[4-9]\d*|1\.24\d*|1\.25|1\.250*|1\.25[0-5]\d*|1\.2560*|1\.256[0-6]\d*|1\.25670*")]
    [InlineData(1.2345, 1.267, LeadingZeros.None, 0, null, @"1\.234[5-9]\d*|1\.23[5-9]\d*|1\.2[45]\d*|1\.26|1\.260*|1\.26[0-6]\d*|1\.2670*")]
    [InlineData(0, 1.267, LeadingZeros.None, 0, null, @"0(?:\.\d*)?|1\.?|1\.[01]\d*|1\.20*|1\.2[0-5]\d*|1\.260*|1\.26[0-6]\d*|1\.2670*")]
    [InlineData(0, 2.267, LeadingZeros.None, 0, null, @"[01](?:\.\d*)?|2\.?|2\.[01]\d*|2\.20*|2\.2[0-5]\d*|2\.260*|2\.26[0-6]\d*|2\.2670*")]
    [InlineData(12, 12, LeadingZeros.None, 0, null, @"12(?:\.0*)?")]
    [InlineData(12, 14, LeadingZeros.None, 0, null, @"1[23](?:\.\d*)?|14(?:\.0*)?")]

    [InlineData(4.93, 233.8, LeadingZeros.Optional, 0, null, @"(?:00)?4\.9[3-9]\d*|(?:(?:00)?[5-9]|0?[1-9]\d|1\d{2}|2[0-2]\d|23[0-2])(?:\.\d*)?|233\.?|233\.[0-7]\d*|233\.80*")]
    [InlineData(4.93, 233.8, LeadingZeros.Required, 0, null, @"004\.9[3-9]\d*|(?:00[5-9]|0[1-9]\d|1\d{2}|2[0-2]\d|23[0-2])(?:\.\d*)?|233\.?|233\.[0-7]\d*|233\.80*")]

    [InlineData(1.234, 1.2567, LeadingZeros.None, 2, null, @"1\.23[4-9]\d*|1\.24\d*|1\.25|1\.250*|1\.25[0-5]\d*|1\.2560*|1\.256[0-6]\d*|1\.25670*")]
    [InlineData(1.234, 1.2567, LeadingZeros.None, 3, null, @"1\.23[4-9]\d*|1\.24\d+|1\.250+|1\.25[0-5]\d*|1\.2560*|1\.256[0-6]\d*|1\.25670*")]
    [InlineData(4.93, 233.8, LeadingZeros.None, 2, null, @"4\.9[3-9]\d*|(?:[5-9]|[1-9]\d|1\d{2}|2[0-2]\d|23[0-2])\.\d{2,}|233\.[0-7]\d+|233\.80+")]
    [InlineData(4.93, 233.8, LeadingZeros.None, 0, 3, @"4\.9[3-9]\d?|(?:[5-9]|[1-9]\d|1\d{2}|2[0-2]\d|23[0-2])(?:\.\d{0,3})?|233\.?|233\.[0-7]\d{0,2}|233\.80{0,2}")]
    [InlineData(4.93, 233.8, LeadingZeros.None, 1, 3, @"4\.9[3-9]\d?|(?:[5-9]|[1-9]\d|1\d{2}|2[0-2]\d|23[0-2])\.\d{1,3}|233\.[0-7]\d{0,2}|233\.80{0,2}")]
    [InlineData(12, 12, LeadingZeros.None, 1, null, @"12\.0+")]
    [InlineData(12, 14, LeadingZeros.None, 1, null, @"1[23]\.\d+|14\.0+")]

    [InlineData(-8.6, -5.97, LeadingZeros.None, 0, null, @"-(?:5\.9[7-9]\d*|[67](?:\.\d*)?|8\.?|8\.[0-5]\d*|8\.60*)")]
    [InlineData(-8.684, -5.3, LeadingZeros.None, 0, null, @"-(?:5\.[3-9]\d*|[67](?:\.\d*)?|8\.?|8\.[0-5]\d*|8\.60*|8\.6[0-7]\d*|8\.680*|8\.68[0-3]\d*|8\.6840*)")]
    [InlineData(-8.6, 5.97, LeadingZeros.None, 0, null, @"-(?:[0-7](?:\.\d*)?|8\.?|8\.[0-5]\d*|8\.60*)|[0-4](?:\.\d*)?|5\.?|5\.[0-8]\d*|5\.90*|5\.9[0-6]\d*|5\.970*")]
    [InlineData(-8.684, 5.3, LeadingZeros.None, 0, null, @"-(?:[0-7](?:\.\d*)?|8\.?|8\.[0-5]\d*|8\.60*|8\.6[0-7]\d*|8\.680*|8\.68[0-3]\d*|8\.6840*)|[0-4](?:\.\d*)?|5\.?|5\.[0-2]\d*|5\.30*")]
    [InlineData(-8.684, 0, LeadingZeros.None, 0, null, @"-(?:[0-7](?:\.\d*)?|8\.?|8\.[0-5]\d*|8\.60*|8\.6[0-7]\d*|8\.680*|8\.68[0-3]\d*|8\.6840*)|0(?:\.0*)?")]
    [InlineData(-8.684, 4, LeadingZeros.None, 0, null, @"-(?:[0-7](?:\.\d*)?|8\.?|8\.[0-5]\d*|8\.60*|8\.6[0-7]\d*|8\.680*|8\.68[0-3]\d*|8\.6840*)|[0-3](?:\.\d*)?|4(?:\.0*)?")]
    public void NumericRange_Double(
        double min,
        double max,
        LeadingZeros leadingZeros,
        int minFractionalDigits,
        int? maxFractionalDigits,
        string expected)
    {
        var pattern = Pattern.NumericRange(min, max, leadingZeros, minFractionalDigits, maxFractionalDigits);

        ShouldMatchNumbers(pattern, min, max, leadingZeros, minFractionalDigits, maxFractionalDigits);
        pattern.ToString().ShouldBe(expected);
    }

    [Fact]
    public void NumericRange_Double_FractionalSeparator()
    {
        var min = 24;
        var max = 33.152;

        var pattern = Pattern.NumericRange(min, max, fractionalSeparator: ',');

        ShouldMatchNumbers(pattern, min, max, LeadingZeros.None, 0, null, fractionalSeparator: ',');
        pattern.ToString()
            .ShouldBe(@"(?:2[4-9]|3[0-2])(?:,\d*)?|33,?|33,0\d*|33,10*|33,1[0-4]\d*|33,150*|33,15[01]\d*|33,1520*");
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

    private static void ShouldMatchNumbers(
        Pattern pattern,
        double min,
        double max,
        LeadingZeros leadingZeros,
        int minFractionalDigits,
        int? maxFractionalDigits,
        char fractionalSeparator = '.')
    {
        GetTestRange((long)min, (long)max, out var start, out var end);

        var minIntDigitCount = IntegerDigitsCount(min);
        var maxIntDigitCount = IntegerDigitsCount(max);
        var minFracDigitCount = FractionalDigitsCount(min);
        var maxFracDigitCount = FractionalDigitsCount(max);
        var testMaxIntegerDigits = Math.Max(minIntDigitCount, maxIntDigitCount);
        var testMaxFractionalDigits = maxFractionalDigits ?? Math.Max(minFracDigitCount, maxFracDigitCount) + 1;
        var maxFracPart = (long)Math.Pow(10, testMaxFractionalDigits) - 1;

        var minInt = (long)min;
        var maxInt = (long)max;
        var minFrac = Math.Abs(Math.Round((min - minInt) * (maxFracPart + 1), testMaxFractionalDigits));
        var maxFrac = Math.Abs(Math.Round((max - maxInt) * (maxFracPart + 1), testMaxFractionalDigits));

        var regex = CompilePattern(pattern);

        for (var cur = start; cur <= end; cur++)
        {
            var intStr = cur.ToString();
            var curIntDigitCount = cur >= 0 ? intStr.Length : intStr.Length - 1;

            var leadFracZeros = maxFracPart.ToString().Length - 1;
            var lastLength = 1;

            var numStrBuilder = new StringBuilder(intStr.Length + testMaxFractionalDigits + 1);

            for (var fracPart = 0; fracPart <= maxFracPart; fracPart++)
            {
                var fracStr = fracPart.ToString();
                if (fracStr.Length > lastLength)
                {
                    lastLength = fracStr.Length;
                    leadFracZeros--;
                }

                while (true)
                {
                    numStrBuilder.Clear();
                    numStrBuilder.Append(intStr);
                    numStrBuilder.Append(fractionalSeparator);

                    if (leadFracZeros > 0)
                    {
                        numStrBuilder.Append(new string('0', leadFracZeros));
                    }

                    numStrBuilder.Append(fracStr);

                    var numStr = numStrBuilder.ToString();
                    var numStrWithLeadingZeros = PadZero(numStr, testMaxIntegerDigits - intStr.Length);

                    var result = regex.IsMatch(numStr);
                    var resultWithLeadingZeros = regex.IsMatch(numStrWithLeadingZeros);

                    var expected = minInt <= cur && cur <= maxInt
                        && (cur != minInt || (cur >= 0 ? fracPart >= minFrac : fracPart <= minFrac))
                        && (cur != maxInt || (cur >= 0 ? fracPart <= maxFrac : fracPart >= maxFrac))
                        && fracStr.Length + leadFracZeros >= minFractionalDigits
                        && (!maxFractionalDigits.HasValue || fracStr.Length + leadFracZeros <= maxFractionalDigits);

                    bool expectedWithLeadingZeros;

                    switch (leadingZeros)
                    {
                        case LeadingZeros.Optional:
                            expectedWithLeadingZeros = expected;
                            break;
                        case LeadingZeros.Required:
                            expectedWithLeadingZeros = expected;
                            expected = expected && testMaxIntegerDigits - curIntDigitCount == 0;
                            break;
                        case LeadingZeros.None:
                            expectedWithLeadingZeros = expected && testMaxIntegerDigits - curIntDigitCount == 0;
                            break;
                        default:
                            throw new ArgumentOutOfRangeException();
                    }

                    result.ShouldBe(expected, () => $"{numStr} {(expected ? "should" : "should not")} be between {min} and {max}");
                    resultWithLeadingZeros.ShouldBe(expectedWithLeadingZeros, () => $"{numStrWithLeadingZeros} {(expectedWithLeadingZeros ? "should" : "should not")} be between {min} and {max}");

                    if (fracStr.Length > 0 && fracStr[^1] == '0')
                    {
                        fracStr = fracStr.Remove(fracStr.Length - 1);
                        continue;
                    }

                    break;
                }
            }
        }

        static string PadZero(string s, int zeros)
        {
            if (zeros <= 0)
            {
                return s;
            }

            return s[0] == '-'
                ? $"-{new string('0', zeros)}{s[1..]}"
                : new string('0', zeros) + s;
        }

        static int IntegerDigitsCount(double d)
        {
            var str = Math.Abs(d).ToString(new NumberFormatInfo
            {
                NumberDecimalSeparator = ".",
            });

            var index = str.IndexOf('.');
            return index != -1 ? index : str.Length;
        }

        static int FractionalDigitsCount(double d)
        {
            var str = Math.Abs(d).ToString(new NumberFormatInfo
            {
                NumberDecimalSeparator = ".",
            });

            var index = str.IndexOf('.');
            return index != -1 ? str.Length - index - 1 : 0;
        }
    }

    private static Regex CompilePattern(Pattern pattern)
    {
        return new PatternBuilder().BeginLine.Concat(pattern).EndLine.Compile();
    }

    private static void ShouldRegexMatch(Regex regex, string match, bool shouldMatch)
    {
        regex.IsMatch(match).ShouldBe(shouldMatch, () => $"{match} {(shouldMatch ? "should" : "should not")} match");
    }

    private static void GetTestRange(long min, long max, out long start, out long end)
    {
        start = min >= 0
            ? (long)Math.Pow(10, Math.Max(0, min.ToString().Length - 2))
            : (long)-Math.Pow(10, min.ToString().Length - 1);
        end = max >= 0
            ? (long)Math.Pow(10, max.ToString().Length)
            : (long)-Math.Pow(10, Math.Max(0, max.ToString().Length - 3));

        if (start == 1)
        {
            start = 0;
        }
    }

    private static void GetLeadingZeroTestRange(long min, long max, out long start, out long end)
    {
        start = min >= 0
            ? (long)Math.Pow(10, Math.Max(0, min.ToString().Length - 2))
            : (long)-Math.Pow(10, min.ToString().Length - 1) + 1;
        end = max >= 0
            ? (long)Math.Pow(10, max.ToString().Length) - 1
            : (long)-Math.Pow(10, Math.Max(0, max.ToString().Length - 3)) + 1;

        if (start == 1)
        {
            start = 0;
        }
    }
}

[ShouldlyMethods]
public static class ShouldlyExtensions
{
    public static void ShouldBe(
        this bool result,
        bool expected,
        Func<string> message,
        [CallerMemberName] string method = null!)
    {
        if (result != expected)
        {
            throw new ShouldAssertException(new ExpectedActualShouldlyMessage(expected, result, message(), method).ToString());
        }
    }
}
