using System;
using System.Globalization;
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

        public static Pattern NumericRange(
            double min,
            double max,
            LeadingZeros leadingZeros = LeadingZeros.None,
            int minFractionalDigits = 0,
            int? maxFractionalDigits = null,
            char fractionalSeparator = '.')
        {
            if (double.IsNaN(min))
            {
                throw new ArgumentException("Minimum cannot be NaN");
            }
            if (double.IsNaN(max))
            {
                throw new ArgumentException("Maximum cannot be NaN");
            }
            if (double.IsInfinity(min))
            {
                throw new ArgumentException("Minimum cannot be infinite");
            }
            if (double.IsInfinity(max))
            {
                throw new ArgumentException("Maximum cannot be infinite");
            }
            if (min > max)
            {
                throw new ArgumentException("Minimum cannot be greater than maximum.", nameof(max));
            }
            if (minFractionalDigits < 0)
            {
                throw new ArgumentException("Minimum fractional digits must be at least 0", nameof(minFractionalDigits));
            }
            if (maxFractionalDigits.HasValue && minFractionalDigits > maxFractionalDigits.Value)
            {
                throw new ArgumentException("Minimum fractional digits cannot be greater than maximum fractional digits.", nameof(maxFractionalDigits));
            }

            var numberFormatInfo = new NumberFormatInfo
            {
                NumberDecimalSeparator = ".",
                NumberGroupSeparator = "",
            };

            var minStr = min.ToString(numberFormatInfo);
            var maxStr = max.ToString(numberFormatInfo);

            var minIntegerPart = GetDecimalPart(minStr, true, numberFormatInfo.NumberDecimalSeparator);
            var minFractionalPart = GetDecimalPart(minStr, false, numberFormatInfo.NumberDecimalSeparator);
            var maxIntegerPart = GetDecimalPart(maxStr, true, numberFormatInfo.NumberDecimalSeparator);
            var maxFractionalPart = GetDecimalPart(maxStr, false, numberFormatInfo.NumberDecimalSeparator);

            var maxIntegerPartLen = maxIntegerPart.Length;
            var largestFractionalDigitLength = Math.Max(minFractionalPart.Length, maxFractionalPart.Length);

            if (maxFractionalDigits < largestFractionalDigitLength)
            {
                throw new ArgumentException($"Maximum fractional digits must be at least {largestFractionalDigitLength}", nameof(maxFractionalDigits));
            }

            var orPattern = new OrPattern();

            var nextMinInt = (long)min + 1;
            var prevMaxInt = (long)max - 1;

            if (minFractionalPart.Length > 0)
            {
                orPattern.Or(FractionalPattern(
                    minIntegerPart,
                    DigitPattern(minFractionalPart[..^1], minFractionalPart[^1], '9', 0),
                    minFractionalPart.Length));
            }
            else
            {
                orPattern.Or(new ConcatPattern(
                    PadZero(
                        new LiteralPattern(minIntegerPart.ToString()),
                        maxIntegerPartLen - minIntegerPart.Length,
                        leadingZeros),
                    OptionalFractionalPart()));
            }

            for (var i = minFractionalPart.Length - 2; i >= 0; i--)
            {
                var digit = (char)(minFractionalPart[i] + 1);
                if (digit <= '9')
                {
                    orPattern.Or(FractionalPattern(
                        minIntegerPart,
                        DigitPattern(minFractionalPart[..i], digit, '9', 0),
                        i + 1));
                }
            }

            if (nextMinInt <= prevMaxInt)
            {
                orPattern.Or(new ConcatPattern(
                    NumericRange(nextMinInt, prevMaxInt, leadingZeros),
                    OptionalFractionalPart()));
            }

            if (minFractionalDigits == 0)
            {
                orPattern.Or(new ConcatPattern(
                    new LiteralPattern(maxIntegerPart.ToString()),
                    QuantifierPattern.ZeroOrOne(new LiteralPattern(fractionalSeparator))));
            }

            if (maxFractionalPart.Length > 0)
            {
                var maxEnd = maxFractionalPart.Length - 1;
                for (; maxEnd > 0 && maxFractionalPart[maxEnd] == '9'; maxEnd--) ;

                if (maxEnd != maxFractionalPart.Length - 1)
                {
                    maxEnd++;
                }

                for (var i = 0; i <= maxEnd; i++)
                {
                    var digit = (char)(maxFractionalPart[i] - 1);
                    if (digit >= '0')
                    {
                        orPattern.Or(FractionalPattern(
                            maxIntegerPart,
                            DigitPattern(maxFractionalPart[..i], '0', digit, 0),
                            i + 1));
                    }
                }
            }

            var finalPattern = new ConcatPattern(new LiteralPattern(maxStr));

            if (maxFractionalPart.Length == 0)
            {
                finalPattern.Concat(new LiteralPattern(fractionalSeparator));
            }

            finalPattern.Concat(ZeroDigits(maxFractionalPart.Length));

            orPattern.Or(finalPattern);

            return orPattern;

            Pattern FractionalPattern(ReadOnlySpan<char> integerPart, Pattern pattern, int patternDigits)
            {
                return new ConcatPattern(
                    PadZero(
                        new LiteralPattern($"{integerPart.ToString()}{fractionalSeparator}"),
                        maxIntegerPartLen - integerPart.Length,
                        leadingZeros),
                    pattern,
                    Digits(patternDigits));
            }

            Pattern OptionalFractionalPart()
            {
                return QuantifierPattern.ZeroOrOne(new ConcatPattern(
                    new LiteralPattern(fractionalSeparator),
                    Digits(0)));
            }

            Pattern Digits(int length)
            {
                return QuantifierDigits(CharacterPattern.Digit, length);
            }

            Pattern ZeroDigits(int length)
            {
                return QuantifierDigits(CharacterPattern.Character('0'), length);
            }

            Pattern QuantifierDigits(CharacterPattern pattern, int length)
            {
                return new QuantifierPattern(
                    pattern,
                    Math.Max(0, minFractionalDigits - length),
                    maxFractionalDigits - length,
                    true);
            }

            static ReadOnlySpan<char> GetDecimalPart(ReadOnlySpan<char> str, bool left, string separator)
            {
                var index = str.IndexOf(separator);
                if (left)
                {
                    return index == -1
                        ? str
                        : str[..index];
                }

                return index == -1
                    ? string.Empty
                    : str[(index + 1)..];
            }
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
                        NumericRange(maxAbsStr, minAbsStr, 0, maxDigitsLength.Value, leadingZeros));
                }

                maxDigitsLength ??= Math.Max(minAbsStr.Length, max.ToString().Length);
                return new OrPattern(
                    NumericRange(min, -1, leadingZeros, maxDigitsLength),
                    NumericRange(0, max, leadingZeros, maxDigitsLength));
            }

            var maxAsString = max.ToString();
            maxDigitsLength ??= maxAsString.Length;
            return NumericRange(min.ToString(), maxAsString, 0, maxDigitsLength.Value, leadingZeros);
        }

        private static Pattern NumericRange(
            ReadOnlySpan<char> minStr,
            ReadOnlySpan<char> maxStr,
            int depth,
            int maxDigitsLength,
            LeadingZeros leadingZeros)
        {
            var orPattern = new OrPattern();

            if (minStr.Length < maxStr.Length)
            {
                var last = minStr;
                var lastOneZeros = new StringBuilder("1" + new string('0', minStr.Length - 1), maxStr.Length);
                var mid = new StringBuilder(new string('9', minStr.Length), maxStr.Length);

                while (mid.Length < maxStr.Length)
                {
                    orPattern.Or(PadZero(
                        NumericRange(
                            last,
                            mid.ToString(),
                            depth + 1,
                            maxDigitsLength,
                            leadingZeros),
                        maxDigitsLength - mid.Length,
                        leadingZeros));
                    mid.Append('9');
                    lastOneZeros.Append('0');
                    last = lastOneZeros.ToString();
                }

                orPattern.Or(PadZero(
                    NumericRange(
                        last,
                        maxStr,
                        depth + 1,
                        maxDigitsLength,
                        leadingZeros),
                    maxDigitsLength - maxStr.Length,
                    leadingZeros));
                return orPattern;
            }

            if (minStr.Length == 1)
            {
                var pattern = DigitRange(minStr[0], maxStr[0]);
                if (depth == 0)
                {
                    pattern = PadZero(pattern, maxDigitsLength - 1, leadingZeros);
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
                    NumericRange(
                        minStr[prefix..],
                        maxStr[prefix..],
                        depth + 1,
                        maxDigitsLength,
                        leadingZeros));
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
                    return orPattern.Unwrap();
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

            return orPattern.Unwrap();

            static Pattern DigitRange(char min, char max)
            {
                return min == '0' && max == '9'
                    ? (Pattern)CharacterPattern.Digit
                    : new CharacterSetPattern(new CharacterRange(min, max));
            }
        }

        private static ConcatPattern DigitPattern(ReadOnlySpan<char> prefix, char min, char max, int digits)
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
                concatPattern.Concat(QuantifierPattern.Exactly(CharacterPattern.Digit, digits));
            }

            return concatPattern;
        }

        private static Pattern PadZero(Pattern pattern, int zeros, LeadingZeros leadingZeros)
        {
            if (leadingZeros == LeadingZeros.None || zeros == 0)
            {
                return pattern;
            }

            Pattern leadingZerosPattern = new LiteralPattern(new string('0', zeros));
            if (leadingZeros == LeadingZeros.Optional)
            {
                leadingZerosPattern = QuantifierPattern.ZeroOrOne(leadingZerosPattern);
            }

            return new ConcatPattern(leadingZerosPattern, pattern);
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
