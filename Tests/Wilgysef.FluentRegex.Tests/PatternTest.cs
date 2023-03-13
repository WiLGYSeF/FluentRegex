using System.Text.RegularExpressions;

namespace Wilgysef.FluentRegex.Tests
{
    public class PatternTest
    {
        [Fact]
        public void Concat_ToString()
        {
            var pattern = Pattern.Concat(
                Pattern.Raw("asdf"),
                Pattern.Raw("test"));

            var a = pattern.ToString();
        }
    }
}