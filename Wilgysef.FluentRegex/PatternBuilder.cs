using System.Collections.Generic;
using System.Diagnostics;
using System.Linq;
using Wilgysef.FluentRegex.Enums;
using Wilgysef.FluentRegex.Exceptions;
using Wilgysef.FluentRegex.PatternBuilders;

namespace Wilgysef.FluentRegex
{
    public class PatternBuilder : ContainerPattern
    {
        internal override bool IsSinglePattern => IsSinglePatternInternal(true);

        internal override bool IsEmpty => IsEmptyInternal();

        /// <summary>
        /// Creates a new pattern builder.
        /// </summary>
        public PatternBuilder() { }

        /// <summary>
        /// Creates a new pattern builder.
        /// </summary>
        /// <param name="character">Starting character.</param>
        public PatternBuilder(char character)
        {
            Character(character);
        }

        /// <summary>
        /// Creates a new pattern builder.
        /// </summary>
        /// <param name="literal">Starting literal string.</param>
        public PatternBuilder(string literal)
        {
            Literal(literal);
        }

        /// <summary>
        /// Creates a new pattern builder.
        /// </summary>
        /// <param name="pattern">Starting pattern.</param>
        public PatternBuilder(Pattern pattern)
        {
            Add(pattern);
        }

        private PatternBuilder(IEnumerable<Pattern> patterns) : base(patterns) { }

        #region Anchors

        /// <summary>
        /// Adds a <see cref="AnchorPattern.BeginLine"/> to the pattern.
        /// </summary>
        [DebuggerBrowsable(DebuggerBrowsableState.Never)]
        public PatternBuilder BeginLine => Add(AnchorPattern.BeginLine);

        /// <summary>
        /// Adds a <see cref="AnchorPattern.EndLine"/> to the pattern.
        /// </summary>
        [DebuggerBrowsable(DebuggerBrowsableState.Never)]
        public PatternBuilder EndLine => Add(AnchorPattern.EndLine);

        /// <summary>
        /// Adds a <see cref="AnchorPattern.Start"/> to the pattern.
        /// </summary>
        [DebuggerBrowsable(DebuggerBrowsableState.Never)]
        public PatternBuilder Start => Add(AnchorPattern.Start);

        /// <summary>
        /// Adds a <see cref="AnchorPattern.End"/> to the pattern.
        /// </summary>
        [DebuggerBrowsable(DebuggerBrowsableState.Never)]
        public PatternBuilder End => Add(AnchorPattern.End);

        /// <summary>
        /// Adds a <see cref="AnchorPattern.AbsoluteEnd"/> to the pattern.
        /// </summary>
        [DebuggerBrowsable(DebuggerBrowsableState.Never)]
        public PatternBuilder AbsoluteEnd => Add(AnchorPattern.AbsoluteEnd);

        /// <summary>
        /// Adds a <see cref="AnchorPattern.StartOfMatch"/> to the pattern.
        /// </summary>
        [DebuggerBrowsable(DebuggerBrowsableState.Never)]
        public PatternBuilder StartOfMatch => Add(AnchorPattern.StartOfMatch);

        /// <summary>
        /// Adds a <see cref="AnchorPattern.WordBoundary"/> to the pattern.
        /// </summary>
        [DebuggerBrowsable(DebuggerBrowsableState.Never)]
        public PatternBuilder WordBoundary => Add(AnchorPattern.WordBoundary);

        /// <summary>
        /// Adds a <see cref="AnchorPattern.NonWordBoundary"/> to the pattern.
        /// </summary>
        [DebuggerBrowsable(DebuggerBrowsableState.Never)]
        public PatternBuilder NonWordBoundary => Add(AnchorPattern.NonWordBoundary);

        #endregion

        /// <summary>
        /// Adds a <see cref="BackreferencePattern"/> to the pattern.
        /// </summary>
        /// <param name="group">Group number.</param>
        /// <returns>Current pattern builder.</returns>
        public PatternBuilder Backreference(int group) => Add(new BackreferencePattern(group));

        /// <summary>
        /// Adds a <see cref="BackreferencePattern"/> to the pattern.
        /// </summary>
        /// <param name="group">Group name.</param>
        /// <returns>Current pattern builder.</returns>
        public PatternBuilder Backreference(string group) => Add(new BackreferencePattern(group));

        #region Characters

        /// <summary>
        /// Adds a <see cref="CharacterPattern.Character(char)"/> to the pattern.
        /// </summary>
        /// <param name="character">Character.</param>
        /// <returns>Current pattern builder.</returns>
        public PatternBuilder Character(char character) => Add(CharacterPattern.Character(character));

        /// <summary>
        /// Adds a <see cref="CharacterPattern.Control(char)"/> to the pattern.
        /// </summary>
        /// <param name="character">Control character.</param>
        /// <returns>Current pattern builder.</returns>
        public PatternBuilder Control(char character) => Add(CharacterPattern.Control(character));

        /// <summary>
        /// Adds an <see cref="CharacterPattern.Escape"/> to the pattern.
        /// </summary>
        [DebuggerBrowsable(DebuggerBrowsableState.Never)]
        public PatternBuilder Escape => Add(CharacterPattern.Escape);

        /// <summary>
        /// Adds a <see cref="CharacterPattern.Hexadecimal(string)"/> to the pattern.
        /// </summary>
        /// <param name="hex">Hexadecimal between <c>0x00</c> and <c>0xFF</c>.</param>
        /// <returns>Current pattern builder.</returns>
        public PatternBuilder Hexadecimal(string hex) => Add(CharacterPattern.Hexadecimal(hex));

        /// <summary>
        /// Adds a <see cref="CharacterPattern.Octal(string)"/> to the pattern.
        /// </summary>
        /// <param name="octal">Octal between <c>00</c> and <c>777</c>.</param>
        /// <returns>Current pattern builder.</returns>
        public PatternBuilder Octal(string octal) => Add(CharacterPattern.Octal(octal));

        /// <summary>
        /// Adds a <see cref="CharacterPattern.Unicode(string)"/> to the pattern.
        /// </summary>
        /// <param name="hex">Unicode value between <c>0000</c> and <c>FFFF</c>.</param>
        /// <returns>Current pattern builder.</returns>
        public PatternBuilder Unicode(string hex) => Add(CharacterPattern.Unicode(hex));

        /// <summary>
        /// Adds a <see cref="CharacterPattern.Word"/> to the pattern.
        /// </summary>
        [DebuggerBrowsable(DebuggerBrowsableState.Never)]
        public PatternBuilder Word => Add(CharacterPattern.Word);

        /// <summary>
        /// Adds a <see cref="CharacterPattern.NonWord"/> to the pattern.
        /// </summary>
        [DebuggerBrowsable(DebuggerBrowsableState.Never)]
        public PatternBuilder NonWord => Add(CharacterPattern.NonWord);

        /// <summary>
        /// Adds a <see cref="CharacterPattern.Digit"/> to the pattern.
        /// </summary>
        [DebuggerBrowsable(DebuggerBrowsableState.Never)]
        public PatternBuilder Digit => Add(CharacterPattern.Digit);

        /// <summary>
        /// Adds a <see cref="CharacterPattern.NonDigit"/> to the pattern.
        /// </summary>
        [DebuggerBrowsable(DebuggerBrowsableState.Never)]
        public PatternBuilder NonDigit => Add(CharacterPattern.NonDigit);

        /// <summary>
        /// Adds a <see cref="CharacterPattern.Whitespace"/> to the pattern.
        /// </summary>
        [DebuggerBrowsable(DebuggerBrowsableState.Never)]
        public PatternBuilder Whitespace => Add(CharacterPattern.Whitespace);

        /// <summary>
        /// Adds a <see cref="CharacterPattern.NonWhitespace"/> to the pattern.
        /// </summary>
        [DebuggerBrowsable(DebuggerBrowsableState.Never)]
        public PatternBuilder NonWhitespace => Add(CharacterPattern.NonWhitespace);

        /// <summary>
        /// Adds a <see cref="CharacterPattern.Category(string)"/> to the pattern.
        /// </summary>
        /// <param name="category">Category.</param>
        /// <returns>Current pattern builder.</returns>
        public PatternBuilder Category(string category) => Add(CharacterPattern.Category(category));

        /// <summary>
        /// Adds a <see cref="CharacterPattern.NonCategory(string)"/> to the pattern.
        /// </summary>
        /// <param name="category">Category.</param>
        /// <returns>Current pattern builder.</returns>
        public PatternBuilder NonCategory(string category) => Add(CharacterPattern.NonCategory(category));

        /// <summary>
        /// Adds a <see cref="SingleCharacterPattern"/> to the pattern.
        /// </summary>
        [DebuggerBrowsable(DebuggerBrowsableState.Never)]
        public PatternBuilder Single => Add(new SingleCharacterPattern());

        #endregion

        #region Character Sets

        /// <summary>
        /// Adds a <see cref="CharacterSetPattern"/> to the pattern.
        /// </summary>
        /// <param name="characters">Characters.</param>
        /// <returns>Current pattern builder.</returns>
        public PatternBuilder CharacterSet(string characters) => Add(new CharacterSetPattern(characters));

        /// <summary>
        /// Adds a <see cref="CharacterSetPattern"/> to the pattern.
        /// </summary>
        /// <param name="characters">Characters.</param>
        /// <returns>Current pattern builder.</returns>
        public PatternBuilder CharacterSet(params char[] characters) => Add(new CharacterSetPattern(characters));

        /// <summary>
        /// Adds a <see cref="CharacterSetPattern"/> to the pattern.
        /// </summary>
        /// <param name="characters">Characters.</param>
        /// <param name="negated">Indicates if the set is negated.</param>
        /// <returns>Current pattern builder.</returns>
        public PatternBuilder CharacterSet(IEnumerable<char> characters, bool negated = false) => Add(new CharacterSetPattern(characters, negated));

        /// <summary>
        /// Adds a <see cref="CharacterSetPattern"/> to the pattern.
        /// </summary>
        /// <param name="characters">Characters.</param>
        /// <returns>Current pattern builder.</returns>
        public PatternBuilder CharacterSet(params CharacterPattern[] characters) => Add(new CharacterSetPattern(characters));

        /// <summary>
        /// Adds a <see cref="CharacterSetPattern"/> to the pattern.
        /// </summary>
        /// <param name="characters">Characters.</param>
        /// <param name="negated">Indicates if the set is negated.</param>
        /// <returns>Current pattern builder.</returns>
        public PatternBuilder CharacterSet(IEnumerable<CharacterPattern> characters, bool negated = false) => Add(new CharacterSetPattern(characters, negated));

        /// <summary>
        /// Adds a <see cref="CharacterSetPattern"/> to the pattern.
        /// </summary>
        /// <param name="characterRanges">Character ranges.</param>
        /// <returns>Current pattern builder.</returns>
        public PatternBuilder CharacterSet(params CharacterRange[] characterRanges) => Add(new CharacterSetPattern(characterRanges));

        /// <summary>
        /// Adds a <see cref="CharacterSetPattern"/> to the pattern.
        /// </summary>
        /// <param name="characterRanges">Character ranges.</param>
        /// <param name="negated">Indicates if the set is negated.</param>
        /// <returns>Current pattern builder.</returns>
        public PatternBuilder CharacterSet(IEnumerable<CharacterRange> characterRanges, bool negated = false) => Add(new CharacterSetPattern(characterRanges, negated));

        /// <summary>
        /// Adds a <see cref="CharacterSetPattern"/> to the pattern.
        /// </summary>
        /// <param name="characterRanges">Character ranges.</param>
        /// <param name="characters">Characters.</param>
        /// <param name="negated">Indicates if the set is negated.</param>
        /// <returns>Current pattern builder.</returns>
        public PatternBuilder CharacterSet(IEnumerable<CharacterRange> characterRanges, IEnumerable<CharacterPattern> characters, bool negated = false) => Add(new CharacterSetPattern(characterRanges, characters, negated));

        /// <summary>
        /// Adds a <see cref="CharacterSetPattern"/> to the pattern.
        /// </summary>
        /// <param name="characterRanges">Character ranges.</param>
        /// <param name="characters">Characters.</param>
        /// <param name="subtractedCharacters">Characters that will not match.</param>
        /// <param name="negated">Indicates if the set is negated.</param>
        /// <returns>Current pattern builder.</returns>
        public PatternBuilder CharacterSet(IEnumerable<CharacterRange> characterRanges, IEnumerable<CharacterPattern> characters, IEnumerable<CharacterPattern> subtractedCharacters, bool negated = false) => Add(new CharacterSetPattern(characterRanges, characters, subtractedCharacters, negated));

        /// <summary>
        /// Adds a <see cref="CharacterSetPattern"/> to the pattern.
        /// </summary>
        /// <param name="characterRanges">Character ranges.</param>
        /// <param name="characters">Characters.</param>
        /// <param name="subtractedCharacterRanges">Character ranges that will not match.</param>
        /// <param name="subtractedCharacters">Characters that will not match.</param>
        /// <param name="negated">Indicates if the set is negated.</param>
        /// <returns>Current pattern builder.</returns>
        public PatternBuilder CharacterSet(IEnumerable<CharacterRange> characterRanges, IEnumerable<CharacterPattern> characters, IEnumerable<CharacterRange> subtractedCharacterRanges, IEnumerable<CharacterPattern> subtractedCharacters, bool negated = false) => Add(new CharacterSetPattern(characterRanges, characters, subtractedCharacterRanges, subtractedCharacters, negated));

        #endregion

        /// <summary>
        /// Concatenates patterns as a concatenation unit to the pattern.
        /// </summary>
        /// <param name="patterns">Patterns to add.</param>
        /// <returns>Current pattern builder.</returns>
        public PatternBuilder Concat(params Pattern[] patterns) => Concat((IEnumerable<Pattern>)patterns);

        /// <summary>
        /// Concatenates patterns as a concatenation unit to the pattern.
        /// </summary>
        /// <param name="patterns">Patterns to add.</param>
        /// <returns>Current pattern builder.</returns>
        public PatternBuilder Concat(IEnumerable<Pattern> patterns)
        {
            if (HasAtLeast(patterns, 2))
            {
                Add(new ConcatPattern(patterns));
            }
            else
            {
                var first = patterns.FirstOrDefault();
                if (first != null)
                {
                    Add(first);
                }
            }

            return this;
        }

        #region Groups

        /// <summary>
        /// Adds a <see cref="AtomicGroupPattern"/> to the pattern.
        /// </summary>
        /// <param name="pattern">Pattern.</param>
        /// <returns>Current pattern builder.</returns>
        public PatternBuilder AtomicGroup(Pattern? pattern) => Add(new AtomicGroupPattern(pattern));

        /// <summary>
        /// Adds a <see cref="CommentPattern"/> to the pattern.
        /// </summary>
        /// <param name="value">Comment value.</param>
        /// <returns>Current pattern builder.</returns>
        public PatternBuilder Comment(string? value) => Add(new CommentPattern(value));

        /// <summary>
        /// Adds a <see cref="ConditionalPattern"/> to the pattern.
        /// </summary>
        /// <param name="expression">Conditional expression.</param>
        /// <param name="yes">Pattern if the conditional matches.</param>
        /// <param name="no">Pattern if the conditional does not match.</param>
        /// <param name="lookahead">Indicates if the expression is lookahead.</param>
        /// <returns>Current pattern builder.</returns>
        public PatternBuilder Conditional(Pattern expression, Pattern yes, Pattern? no, bool lookahead = true) => Add(new ConditionalPattern(expression, yes, no, lookahead));

        /// <summary>
        /// Adds a <see cref="ConditionalPattern"/> to the pattern.
        /// </summary>
        /// <param name="group">Conditional group number.</param>
        /// <param name="yes">Pattern if the conditional matches.</param>
        /// <param name="no">Pattern if the conditional does not match.</param>
        /// <returns>Current pattern builder.</returns>
        public PatternBuilder Conditional(int group, Pattern yes, Pattern? no) => Add(new ConditionalPattern(group, yes, no));

        /// <summary>
        /// Adds a <see cref="ConditionalPattern"/> to the pattern.
        /// </summary>
        /// <param name="group">Conditional group number.</param>
        /// <param name="yes">Pattern if the conditional matches.</param>
        /// <param name="no">Pattern if the conditional does not match.</param>
        /// <returns>Current pattern builder.</returns>
        public PatternBuilder Conditional(string group, Pattern yes, Pattern? no) => Add(new ConditionalPattern(group, yes, no));

        /// <summary>
        /// Adds a <see cref="GroupPattern"/> to the pattern.
        /// </summary>
        /// <param name="pattern">Pattern to match.</param>
        /// <param name="name">Group name, or <see langword="null"/>.</param>
        /// <param name="capture">Indicates if the group is capturing.</param>
        /// <returns>Current pattern builder.</returns>
        public PatternBuilder Group(Pattern? pattern, string? name = null, bool capture = true) => Add(new GroupPattern(pattern, name, capture));

        /// <summary>
        /// Adds a capturing group to the pattern.
        /// </summary>
        /// <param name="pattern">Pattern to match.</param>
        /// <returns>Current pattern builder.</returns>
        public PatternBuilder CapturingGroup(Pattern? pattern) => Add(new GroupPattern(pattern, null, true));

        /// <summary>
        /// Adds a named capturing group to the pattern.
        /// </summary>
        /// <param name="name">Group name.</param>
        /// <param name="pattern">Pattern to match.</param>
        /// <returns>Current pattern builder.</returns>
        public PatternBuilder CapturingGroup(string name, Pattern? pattern) => Add(new GroupPattern(pattern, name, true));

        /// <summary>
        /// Adds a non-capturing group to the pattern.
        /// </summary>
        /// <param name="pattern">Pattern to match.</param>
        /// <returns>Current pattern builder.</returns>
        public PatternBuilder NonCapturingGroup(Pattern? pattern) => Add(new GroupPattern(pattern, null, false));

        /// <summary>
        /// Adds a balancing group to the pattern.
        /// </summary>
        /// <param name="name1">First group name.</param>
        /// <param name="name2">Second group name.</param>
        /// <param name="pattern">Pattern to match.</param>
        /// <returns>Current pattern builder.</returns>
        public PatternBuilder BalancingGroup(string? name1, string name2, Pattern? pattern) => Add(new GroupPattern(pattern, name1, name2));

        /// <summary>
        /// Adds an <see cref="InlineModifierPattern"/> to the pattern.
        /// </summary>
        /// <param name="modifiers">Inline modifiers to enable.</param>
        /// <returns>Current pattern builder.</returns>
        public PatternBuilder Modifiers(InlineModifier modifiers) => Add(new InlineModifierPattern(null, modifiers));

        /// <summary>
        /// Adds an <see cref="InlineModifierPattern"/> to the pattern.
        /// </summary>
        /// <param name="modifiers">Inline modifiers to enable.</param>
        /// <param name="disabledModifiers">Inline modifiers to disable.</param>
        /// <returns>Current pattern builder.</returns>
        public PatternBuilder Modifiers(InlineModifier modifiers, InlineModifier disabledModifiers) => Add(new InlineModifierPattern(null, modifiers, disabledModifiers));

        /// <summary>
        /// Adds an <see cref="InlineModifierPattern"/> to the pattern.
        /// </summary>
        /// <param name="pattern">Pattern to match.</param>
        /// <param name="modifiers">Inline modifiers to enable.</param>
        /// <returns>Current pattern builder.</returns>
        public PatternBuilder Modifiers(Pattern? pattern, InlineModifier modifiers) => Add(new InlineModifierPattern(pattern, modifiers));

        /// <summary>
        /// Adds an <see cref="InlineModifierPattern"/> to the pattern.
        /// </summary>
        /// <param name="pattern">Pattern to match.</param>
        /// <param name="modifiers">Inline modifiers to enable.</param>
        /// <param name="disabledModifiers">Inline modifiers to disable.</param>
        /// <returns>Current pattern builder.</returns>
        public PatternBuilder Modifiers(Pattern? pattern, InlineModifier modifiers, InlineModifier disabledModifiers) => Add(new InlineModifierPattern(pattern, modifiers, disabledModifiers));

        /// <summary>
        /// Adds a positive lookahead to the pattern.
        /// </summary>
        /// <param name="pattern">Pattern to match.</param>
        /// <returns>Current pattern builder.</returns>
        public PatternBuilder PositiveLookahead(Pattern? pattern) => Add(LookaheadPattern.PositiveLookahead(pattern));

        /// <summary>
        /// Adds a negative lookahead to the pattern.
        /// </summary>
        /// <param name="pattern">Pattern to match.</param>
        /// <returns>Current pattern builder.</returns>
        public PatternBuilder NegativeLookahead(Pattern? pattern) => Add(LookaheadPattern.NegativeLookahead(pattern));

        /// <summary>
        /// Adds a positive lookbehind to the pattern.
        /// </summary>
        /// <param name="pattern">Pattern to match.</param>
        /// <returns>Current pattern builder.</returns>
        public PatternBuilder PositiveLookbehind(Pattern? pattern) => Add(LookaheadPattern.PositiveLookbehind(pattern));

        /// <summary>
        /// Adds a negative lookbehind to the pattern.
        /// </summary>
        /// <param name="pattern">Pattern to match.</param>
        /// <returns>Current pattern builder.</returns>
        public PatternBuilder NegativeLookbehind(Pattern? pattern) => Add(LookaheadPattern.NegativeLookbehind(pattern));

        #endregion

        /// <summary>
        /// Adds a <see cref="LiteralPattern"/> to the pattern.
        /// </summary>
        /// <param name="value">Literal value.</param>
        /// <returns>Current pattern builder.</returns>
        public PatternBuilder Literal(string value) => Add(new LiteralPattern(value));

        /// <summary>
        /// Adds a <see cref="RawPattern"/> to the pattern.
        /// </summary>
        /// <param name="regex">Regex.</param>
        /// <returns>Current pattern builder.</returns>
        public PatternBuilder Raw(string regex) => Add(new RawPattern(regex));

        /// <summary>
        /// Adds an <see cref="OrPattern"/> to the pattern.
        /// </summary>
        /// <param name="patterns">Patterns to match.</param>
        /// <returns>Current pattern builder.</returns>
        public PatternBuilder Or(params Pattern[] patterns) => Add(new OrPattern(patterns));

        /// <summary>
        /// Adds an <see cref="OrPattern"/> to the pattern.
        /// </summary>
        /// <param name="patterns">Patterns to match.</param>
        /// <returns>Current pattern builder.</returns>
        public PatternBuilder Or(IEnumerable<Pattern> patterns) => Add(new OrPattern(patterns));

        #region Quantifiers

        /// <summary>
        /// Matches the last pattern added zero or one times.
        /// </summary>
        /// <param name="pattern">Pattern to match.</param>
        /// <param name="greedy">Indicates if the match is greedy.</param>
        /// <returns>Current pattern builder.</returns>
        public PatternBuilder ZeroOrOne(Pattern pattern, bool greedy = true) => AddQuantifier(pattern, 0, 1, greedy);

        /// <summary>
        /// Matches the last pattern added zero or more times.
        /// </summary>
        /// <param name="pattern">Pattern to match.</param>
        /// <param name="greedy">Indicates if the match is greedy.</param>
        /// <returns>Current pattern builder.</returns>
        public PatternBuilder ZeroOrMore(Pattern pattern, bool greedy = true) => AddQuantifier(pattern, 0, null, greedy);

        /// <summary>
        /// Matches the last pattern added one or more times.
        /// </summary>
        /// <param name="pattern">Pattern to match.</param>
        /// <param name="greedy">Indicates if the match is greedy.</param>
        /// <returns>Current pattern builder.</returns>
        public PatternBuilder OneOrMore(Pattern pattern, bool greedy = true) => AddQuantifier(pattern, 1, null, greedy);

        /// <summary>
        /// Matches the last pattern added exactly <paramref name="number"/> times.
        /// </summary>
        /// <param name="pattern">Pattern to match.</param>
        /// <param name="number">Number of occurrences to match.</param>
        /// <returns>Current pattern builder.</returns>
        public PatternBuilder Exactly(Pattern pattern, int number) => AddQuantifier(pattern, number, number, true);

        /// <summary>
        /// Matches the last pattern added between <paramref name="min"/> and <paramref name="max"/> times.
        /// </summary>
        /// <param name="pattern">Pattern to match.</param>
        /// <param name="min">Minimum number of occurrences to match.</param>
        /// <param name="max">Maximum number of occurrences to match.</param>
        /// <param name="greedy">Indicates if the match is greedy.</param>
        /// <returns>Current pattern builder.</returns>
        public PatternBuilder Between(Pattern pattern, int min, int max, bool greedy = true) => AddQuantifier(pattern, min, max, greedy);

        /// <summary>
        /// Matches the last pattern added at least <paramref name="min"/> times.
        /// </summary>
        /// <param name="pattern">Pattern to match.</param>
        /// <param name="min">Minimum number of occurrences to match.</param>
        /// <param name="greedy">Indicates if the match is greedy.</param>
        /// <returns>Current pattern builder.</returns>
        public PatternBuilder AtLeast(Pattern pattern, int min, bool greedy = true) => AddQuantifier(pattern, min, null, greedy);

        /// <summary>
        /// Matches the last pattern added at most <paramref name="max"/> times.
        /// </summary>
        /// <param name="pattern">Pattern to match.</param>
        /// <param name="max">Maximum number of occurrences to match.</param>
        /// <param name="greedy">Indicates if the match is greedy.</param>
        /// <returns>Current pattern builder.</returns>
        public PatternBuilder AtMost(Pattern pattern, int max, bool greedy = true) => AddQuantifier(pattern, 0, max, greedy);

        /// <summary>
        /// Matches the last pattern added zero or one times.
        /// </summary>
        /// <param name="greedy">Indicates if the match is greedy.</param>
        /// <returns>Current pattern builder.</returns>
        public PatternBuilder ZeroOrOne(bool greedy = true) => AddQuantifier(0, 1, greedy);

        /// <summary>
        /// Matches the last pattern added zero or more times.
        /// </summary>
        /// <param name="greedy">Indicates if the match is greedy.</param>
        /// <returns>Current pattern builder.</returns>
        public PatternBuilder ZeroOrMore(bool greedy = true) => AddQuantifier(0, null, greedy);

        /// <summary>
        /// Matches the last pattern added one or more times.
        /// </summary>
        /// <param name="greedy">Indicates if the match is greedy.</param>
        /// <returns>Current pattern builder.</returns>
        public PatternBuilder OneOrMore(bool greedy = true) => AddQuantifier(1, null, greedy);

        /// <summary>
        /// Matches the last pattern added exactly <paramref name="number"/> times.
        /// </summary>
        /// <param name="number">Number of occurrences to match.</param>
        /// <returns>Current pattern builder.</returns>
        public PatternBuilder Exactly(int number) => AddQuantifier(number, number, true);

        /// <summary>
        /// Matches the last pattern added between <paramref name="min"/> and <paramref name="max"/> times.
        /// </summary>
        /// <param name="min">Minimum number of occurrences to match.</param>
        /// <param name="max">Maximum number of occurrences to match.</param>
        /// <param name="greedy">Indicates if the match is greedy.</param>
        /// <returns>Current pattern builder.</returns>
        public PatternBuilder Between(int min, int max, bool greedy = true) => AddQuantifier(min, max, greedy);

        /// <summary>
        /// Matches the last pattern added at least <paramref name="min"/> times.
        /// </summary>
        /// <param name="min">Minimum number of occurrences to match.</param>
        /// <param name="greedy">Indicates if the match is greedy.</param>
        /// <returns>Current pattern builder.</returns>
        public PatternBuilder AtLeast(int min, bool greedy = true) => AddQuantifier(min, null, greedy);

        /// <summary>
        /// Matches the last pattern added at most <paramref name="max"/> times.
        /// </summary>
        /// <param name="max">Maximum number of occurrences to match.</param>
        /// <param name="greedy">Indicates if the match is greedy.</param>
        /// <returns>Current pattern builder.</returns>
        public PatternBuilder AtMost(int max, bool greedy = true) => AddQuantifier(0, max, greedy);

        /// <summary>
        /// Adds a quantifier to the pattern.
        /// </summary>
        /// <param name="pattern">Pattern to match.</param>
        /// <param name="min">Minimum number of occurrences to match.</param>
        /// <param name="max">Maximum number of occurrences to match.</param>
        /// <param name="greedy">Indicates if the match is greedy.</param>
        /// <returns>Current pattern builder.</returns>
        /// <exception cref="InvalidPatternException">Cannot quantify pattern.</exception>
        private PatternBuilder AddQuantifier(Pattern pattern, int min, int? max, bool greedy) => Add(new QuantifierPattern(pattern, min, max, greedy));

        /// <summary>
        /// Adds a quantifier to the pattern, replacing the last pattern added with a quantifier.
        /// </summary>
        /// <param name="min">Minimum number of occurrences to match.</param>
        /// <param name="max">Maximum number of occurrences to match.</param>
        /// <param name="greedy">Indicates if the match is greedy.</param>
        /// <returns>Current pattern builder.</returns>
        /// <exception cref="InvalidPatternException">Cannot quantify last pattern.</exception>
        private PatternBuilder AddQuantifier(int min, int? max, bool greedy) => ReplaceLast(new QuantifierPattern(Current, min, max, greedy));

        #endregion

        private Pattern Current => _current ?? throw new InvalidPatternException(this, "A pattern is required.");
        private Pattern? _current;

        /// <summary>
        /// Gets numbered groups from the pattern in order.
        /// </summary>
        /// <returns>Numbered groups in order.</returns>
        public List<GroupPattern> GetNumberedGroups()
        {
            var groups = new List<GroupPattern>();

            foreach (var pattern in Traverse(_children))
            {
                if (pattern is GroupPattern group && group.IsNumbered)
                {
                    groups.Add(group);
                }
            }

            return groups;
        }

        /// <summary>
        /// Gets the group number of the group.
        /// </summary>
        /// <param name="group">Group.</param>
        /// <returns>Group number, or <see langword="null"/> if the group is not in the pattern or does not have a number.</returns>
        public int? GetGroupNumber(GroupPattern group)
        {
            var index = GetNumberedGroups().IndexOf(group);
            if (index == -1)
            {
                return null;
            }

            return index + 1;
        }

        /// <summary>
        /// Builds the pattern.
        /// </summary>
        /// <returns>Pattern.</returns>
        public Pattern Build()
        {
            // check if the pattern is infinitely recursive, should throw if it is.
            for (var enumerator = Traverse().GetEnumerator(); enumerator.MoveNext();) ;

            return new ConcatPattern(_children);
        }

        /// <summary>
        /// Traverses the pattern tree in depth order.
        /// </summary>
        /// <returns>Patterns.</returns>
        public IEnumerable<Pattern> Traverse()
        {
            return Traverse(_children);
        }

        public override Pattern Copy()
        {
            return new PatternBuilder(_children.Select(c => c.Copy()));
        }

        internal override void Build(PatternBuildState state)
        {
            Build().Build(state);
        }

        internal override Pattern Unwrap()
        {
            return this;
        }

        /// <summary>
        /// Adds pattern to builder.
        /// </summary>
        /// <param name="pattern">Pattern.</param>
        /// <returns>Current pattern builder.</returns>
        private PatternBuilder Add(Pattern pattern)
        {
            _children.Add(pattern);
            _current = pattern;
            return this;
        }

        /// <summary>
        /// Replaces last pattern added in builder.
        /// </summary>
        /// <param name="pattern">Pattern.</param>
        /// <returns>Current pattern builder.</returns>
        private PatternBuilder ReplaceLast(Pattern pattern)
        {
            _children[^1] = pattern;
            _current = pattern;
            return this;
        }

        /// <summary>
        /// Checks that the sequence has as least <paramref name="minimum"/> values.
        /// </summary>
        /// <typeparam name="T">Type.</typeparam>
        /// <param name="values">Values.</param>
        /// <param name="minimum">Minimum values required in the sequence.</param>
        /// <returns><see langword="true"/> if the sequence has at least <paramref name="minimum"/> values, otherwise <see langword="false"/>.</returns>
        private static bool HasAtLeast<T>(IEnumerable<T> values, int minimum)
        {
            using var enumerator = values.GetEnumerator();

            while (enumerator.MoveNext())
            {
                if (--minimum <= 0)
                {
                    return true;
                }
            }

            return false;
        }
    }
}
