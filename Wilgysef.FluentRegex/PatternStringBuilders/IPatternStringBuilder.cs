namespace Wilgysef.FluentRegex.PatternStringBuilders
{
    internal interface IPatternStringBuilder
    {
        int Length { get; }

        IPatternStringBuilder Append(string value);

        IPatternStringBuilder Append(char value);

        IPatternStringBuilder Append(int value);

        IPatternStringBuilder Clear();
    }
}
