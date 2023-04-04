namespace Wilgysef.FluentRegex.Tests.CompositeTests;

public class NumericRangePatternTest
{
    [Theory]
    [InlineData(1372, 5246, @"137[2-9]|13[8-9]\d|1[4-9]\d{2}|[2-4]\d{3}|5[0-1]\d{2}|52[0-3]\d|524[0-6]")]
    [InlineData(13, 6215, @"1[3-9]|[2-9]\d|[1-9]\d{2}|[1-5]\d{3}|6[0-1]\d{2}|620\d|621[0-5]")]
    [InlineData(123, 123, "123")]
    [InlineData(123, 125, "12[3-5]")]
    [InlineData(1269, 1287, @"12(?:69|7\d|8[0-7])")]
    [InlineData(593, 626, @"59[3-9]|6[0-1]\d|62[0-6]")]
    [InlineData(79, 99, @"79|[8-9]\d")]
    [InlineData(1279, 1299, @"12(?:79|[8-9]\d)")]
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
