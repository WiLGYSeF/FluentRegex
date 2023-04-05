namespace Wilgysef.FluentRegex.Tests.CompositeTests;

public class NumericRangePatternTest
{
    [Theory]
    [InlineData(1372, 5246, @"137[2-9]|13[8-9]\d|1[4-9]\d{2}|[2-4]\d{3}|5[0-1]\d{2}|52[0-3]\d|524[0-6]")]
    [InlineData(593, 626, @"59[3-9]|6[0-1]\d|62[0-6]")]
    [InlineData(123, 123, "123")]
    [InlineData(79, 99, @"79|[8-9]\d")]

    [InlineData(13, 6215, @"1[3-9]|[2-9]\d|[1-9]\d{2}|[1-5]\d{3}|6[0-1]\d{2}|620\d|621[0-5]")]
    [InlineData(51, 501, @"5[1-9]|[6-9]\d|[1-4]\d{2}|50[0-1]")]
    [InlineData(99, 100, @"99|100")]
    [InlineData(100, 256, @"1\d{2}|2[0-4]\d|25[0-6]")]
    [InlineData(100, 299, @"[1-2]\d{2}")]
    [InlineData(701, 899, @"70[1-9]|7[1-9]\d|8\d{2}")]

    [InlineData(123, 125, "12[3-5]")]
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
        var regex = new PatternBuilder().BeginLine.Concat(pattern).EndLine.Compile();

        var start = Math.Pow(10, Math.Max(0, min.ToString().Length - 2));
        var end = Math.Pow(10, max.ToString().Length);

        for (var cur = start; cur <= end; cur++)
        {
            var shouldMatch = cur >= min && cur <= max;
            regex.IsMatch(cur.ToString()).ShouldBe(shouldMatch, $"{cur} {(shouldMatch ? "should" : "should not")} match");
        }

        pattern.ToString().ShouldBe(expected);
    }
}
