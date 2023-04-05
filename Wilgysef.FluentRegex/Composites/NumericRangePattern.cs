using System;
using System.Text;

namespace Wilgysef.FluentRegex.Composites
{
    internal static class NumericRangePattern
    {
        public static Pattern NumericRange(int min, int max, LeadingZeros leadingZeros = LeadingZeros.None)
        {
            return NumericRange(min, max, leadingZeros, null);
        }

        public static Pattern NumericRange(long min, long max, LeadingZeros leadingZeros = LeadingZeros.None)
        {
            return NumericRange(min, max, leadingZeros, null);
        }

        private static Pattern NumericRange(long min, long max, LeadingZeros leadingZeros, int? maxDigitsLength)
        {
            if (min > max)
            {
                throw new ArgumentException("Minimum cannot be greater than maximum.", nameof(max));
            }

            if (min < 0)
            {
                var minAbsStr = (-min).ToString();

                if (max < 0)
                {
                    var maxAbsStr = (-max).ToString();

                    maxDigitsLength ??= Math.Max(minAbsStr.Length, maxAbsStr.Length);
                    return new ConcatPattern(
                        CharacterPattern.Character('-'),
                        NumericRange(maxAbsStr, minAbsStr, 0));
                }

                maxDigitsLength ??= Math.Max(minAbsStr.Length, max.ToString().Length);
                return new OrPattern(
                    NumericRangePattern.NumericRange(min, -1, leadingZeros, maxDigitsLength),
                    NumericRangePattern.NumericRange(0, max, leadingZeros, maxDigitsLength));
            }

            {
                // block-scope maxAsString
                var maxAsString = max.ToString();
                maxDigitsLength ??= maxAsString.Length;
                return NumericRange(min.ToString(), maxAsString, 0);
            }

            Pattern NumericRange(ReadOnlySpan<char> minStr, ReadOnlySpan<char> maxStr, int depth)
            {
                var orPattern = new OrPattern();

                if (minStr.Length < maxStr.Length)
                {
                    var last = minStr;
                    var lastOneZeros = new StringBuilder("1" + new string('0', minStr.Length - 1), maxStr.Length);
                    var mid = new StringBuilder(new string('9', minStr.Length), maxStr.Length);

                    while (mid.Length < maxStr.Length)
                    {
                        orPattern.Or(PadZero(NumericRange(last, mid.ToString(), depth + 1), maxDigitsLength.Value - mid.Length));
                        mid.Append('9');
                        lastOneZeros.Append('0');
                        last = lastOneZeros.ToString();
                    }

                    orPattern.Or(PadZero(NumericRange(last, maxStr, depth + 1), maxDigitsLength.Value - maxStr.Length));
                    return orPattern;
                }

                if (minStr.Length == 1)
                {
                    var pattern = DigitRange(minStr[0], maxStr[0]);
                    if (depth == 0)
                    {
                        pattern = PadZero(pattern, maxDigitsLength.Value - 1);
                    }

                    return pattern;
                }

                var prefix = 0;
                for (; prefix < minStr.Length && minStr[prefix] == maxStr[prefix]; prefix++) ;

                if (prefix > 0)
                {
                    if (prefix == minStr.Length)
                    {
                        return new LiteralPattern(minStr.ToString());
                    }

                    return new ConcatPattern(
                        new LiteralPattern(minStr[..prefix].ToString()),
                        NumericRange(minStr[prefix..], maxStr[prefix..], depth + 1));
                }

                // min and max are the same length of at least 2 and do not share a prefix

                var minStart = minStr.Length - 1;
                var minAllZerosAfterFirst = false;
                if (minStr[^1] == '0')
                {
                    for (; minStart > 0 && minStr[minStart] == '0'; minStart--) ;
                    minAllZerosAfterFirst = minStart == 0;
                }
                else
                {
                    orPattern.Or(DigitPattern(minStr[..^1], minStr[^1], '9', 0));
                    minStart--;
                }

                var maxEnd = maxStr.Length - 1;
                var maxAllNinesAfterFirst = false;
                for (; maxEnd > 0 && maxStr[maxEnd] == '9'; maxEnd--) ;

                if (maxEnd != maxStr.Length - 1)
                {
                    maxAllNinesAfterFirst = maxEnd == 0;
                    maxEnd++;
                }

                for (var i = minStart; i > 0; i--)
                {
                    var digit = (char)(minStr[i] + 1);
                    if (digit <= '9')
                    {
                        orPattern.Or(DigitPattern(minStr[..i], digit, '9', minStr.Length - i - 1));
                    }
                }

                if (minAllZerosAfterFirst || maxAllNinesAfterFirst || minStr[0] + 1 <= maxStr[0] - 1)
                {
                    orPattern.Or(DigitPattern(
                        "",
                        minAllZerosAfterFirst ? minStr[0] : (char)(minStr[0] + 1),
                        maxAllNinesAfterFirst ? maxStr[0] : (char)(maxStr[0] - 1),
                        minStr.Length - 1));

                    if (maxAllNinesAfterFirst)
                    {
                        return orPattern;
                    }
                }

                for (var i = 1; i < maxEnd; i++)
                {
                    var digit = (char)(maxStr[i] - 1);
                    if (digit >= '0')
                    {
                        orPattern.Or(DigitPattern(maxStr[..i], '0', digit, maxStr.Length - i - 1));
                    }
                }

                orPattern.Or(DigitPattern(
                    maxStr[..maxEnd],
                    '0',
                    maxStr[maxEnd],
                    maxStr.Length - maxEnd - 1));

                return orPattern;

                static Pattern DigitRange(char min, char max)
                {
                    return min == '0' && max == '9'
                        ? (Pattern)CharacterPattern.Digit
                        : new CharacterSetPattern(new CharacterRange(min, max));
                }

                static ConcatPattern DigitPattern(ReadOnlySpan<char> prefix, char min, char max, int digits)
                {
                    var concatPattern = new ConcatPattern();

                    if (prefix.Length > 0)
                    {
                        concatPattern.Concat(new LiteralPattern(prefix.ToString()));
                    }

                    if (min == '0' && max == '9')
                    {
                        digits++;
                    }
                    else
                    {
                        concatPattern.Concat(new CharacterSetPattern(new CharacterRange(min, max)));
                    }

                    if (digits > 0)
                    {
                        concatPattern.Concat(new QuantifierPattern(CharacterPattern.Digit, digits, digits, true));
                    }

                    return concatPattern;
                }

                Pattern PadZero(Pattern pattern, int zeros)
                {
                    if (leadingZeros == LeadingZeros.None || zeros == 0)
                    {
                        return pattern;
                    }

                    Pattern leadingZerosPattern = new LiteralPattern(new string('0', zeros));
                    if (leadingZeros == LeadingZeros.Optional)
                    {
                        leadingZerosPattern = new QuantifierPattern(leadingZerosPattern, 0, 1, true);
                    }

                    return new ConcatPattern(leadingZerosPattern, pattern);
                }
            }
        }
    }

    public enum LeadingZeros
    {
        /// <summary>
        /// Do not match leading zeros.
        /// </summary>
        None,

        /// <summary>
        /// Optionally match leading zeros.
        /// </summary>
        Optional,

        /// <summary>
        /// Must match with leading zeros.
        /// </summary>
        Required,
    }
}
