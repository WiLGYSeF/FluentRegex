using System;
using System.Text;

namespace Wilgysef.FluentRegex.Composites
{
    internal static class NumericRangePattern
    {
        public static Pattern NumericRange(int min, int max)
        {
            // TODO: negatives
            // TODO: leading zeroes
            // TODO: \d vs [0-9]?

            if (min > max)
            {
                throw new ArgumentException("Minimum cannot be greater than maximum.", nameof(max));
            }

            return NumericRange(min.ToString(), max.ToString(), false);

            static Pattern NumericRange(string minStr, string maxStr, bool isSubstring)
            {
                var orPattern = new OrPattern();

                if (minStr.Length < maxStr.Length)
                {
                    var last = minStr;
                    var lastOneZeros = new StringBuilder("10", maxStr.Length - minStr.Length + 2);
                    var mid = new StringBuilder(new string('9', minStr.Length), maxStr.Length);

                    while (mid.Length < maxStr.Length)
                    {
                        orPattern.Or(NumericRange(last, mid.ToString(), isSubstring));
                        mid.Append('9');
                        lastOneZeros.Append('0');
                        last = lastOneZeros.ToString();
                    }

                    orPattern.Or(NumericRange(last, maxStr, isSubstring));
                }
                else
                {
                    if (minStr.Length == 1)
                    {
                        return new CharacterSetPattern(new CharacterRange(minStr[0], maxStr[0]));
                    }

                    var prefix = 0;
                    for (; prefix < minStr.Length && minStr[prefix] == maxStr[prefix]; prefix++) ;

                    if (prefix > 0)
                    {
                        if (prefix == minStr.Length)
                        {
                            return new LiteralPattern(minStr);
                        }

                        return new ConcatPattern(
                            new LiteralPattern(minStr[..prefix]),
                            NumericRange(minStr[prefix..], maxStr[prefix..], true));
                    }

                    var digits = new QuantifierPattern(CharacterPattern.Digit, 0, 0, true);

                    var maxAllNines = true;
                    for (var i = 0; i < maxStr.Length; i++)
                    {
                        if (maxStr[i] != '9')
                        {
                            maxAllNines = false;
                            break;
                        }
                    }

                    if (minStr[0] < maxStr[0])
                    {
                        var start = minStr.Length - 1;

                        if (minStr[^1] == '0')
                        {
                            for (; start > 0 && minStr[start] == '0'; start--) ;
                        }
                        else
                        {
                            orPattern.Or(new ConcatPattern(
                                new LiteralPattern(minStr[..^1]),
                                new CharacterSetPattern(new CharacterRange(minStr[^1], '9'))));
                            start--;
                        }

                        for (var i = start; i > 0; i--)
                        {
                            var digit = (char)(minStr[i] + 1);
                            if (digit <= '9')
                            {
                                orPattern.Or(new ConcatPattern(
                                    new LiteralPattern(minStr[..i]),
                                    new CharacterSetPattern(new CharacterRange(digit, '9')),
                                    digits.WithExactly(minStr.Length - i - 1).Copy()));
                            }
                        }

                        if (start == 0 && minStr[1] == '0')
                        {
                            orPattern.Or(new ConcatPattern(
                                new CharacterSetPattern(new CharacterRange(minStr[0], maxAllNines ? '9' : (char)(maxStr[0] - 1))),
                                digits.WithExactly(minStr.Length - 1).Copy()));
                        }
                        else if (minStr[0] + 1 <= maxStr[0] - 1)
                        {
                            orPattern.Or(new ConcatPattern(
                                new CharacterSetPattern(new CharacterRange((char)(minStr[0] + 1), maxAllNines ? '9' : (char)(maxStr[0] - 1))),
                                digits.WithExactly(minStr.Length - 1).Copy()));
                        }
                    }

                    if (!maxAllNines)
                    {
                        for (var i = 1; i < maxStr.Length - 1; i++)
                        {
                            var digit = (char)(maxStr[i] - 1);
                            if (digit >= '0')
                            {
                                orPattern.Or(new ConcatPattern(
                                    new LiteralPattern(maxStr[..i]),
                                    new CharacterSetPattern(new CharacterRange('0', digit)),
                                    digits.WithExactly(maxStr.Length - i - 1).Copy()));
                            }
                        }

                        orPattern.Or(new ConcatPattern(
                            new LiteralPattern(maxStr[..^1]),
                            new CharacterSetPattern(new CharacterRange('0', maxStr[^1]))));
                    }
                }

                return orPattern;
            }
        }
    }
}
