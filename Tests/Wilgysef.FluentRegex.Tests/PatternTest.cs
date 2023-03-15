using System.Text.RegularExpressions;

namespace Wilgysef.FluentRegex.Tests
{
    public class PatternTest
    {
        [Fact]
        public void Compile()
        {
            var pattern = new PatternBuilder()
                .BeginLine
                .Literal("test")
                .EndLine
                .Build();

            var regex = pattern.Compile();
            regex.ToString().ShouldBe("^test$");
        }

        [Fact]
        public void Compile_Options()
        {
            var pattern = new PatternBuilder()
                .BeginLine
                .Literal("test")
                .EndLine
                .Build();

            var regex = pattern.Compile(RegexOptions.IgnoreCase);

            regex.ToString().ShouldBe("^test$");
            regex.Match("tEsT").Success.ShouldBeTrue();
        }

        [Fact]
        public void Compile_Options_Timeout()
        {
            var pattern = new PatternBuilder()
                .BeginLine
                .Literal("test")
                .EndLine
                .Build();

            var regex = pattern.Compile(RegexOptions.IgnoreCase, TimeSpan.FromSeconds(1));

            regex.ToString().ShouldBe("^test$");
            regex.Match("tEsT").Success.ShouldBeTrue();
        }
    }
}