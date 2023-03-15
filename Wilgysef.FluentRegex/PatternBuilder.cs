using System;
using System.Collections.Generic;
using System.Text;
using static Wilgysef.FluentRegex.CharacterSetPattern;

namespace Wilgysef.FluentRegex
{
    public class PatternBuilder : Pattern
    {
        private readonly List<Pattern> _patterns;
        private Pattern? _current;

        internal override bool IsSinglePattern => throw new NotImplementedException();

        public PatternBuilder()
        {
            _patterns = new List<Pattern>();
        }

        #region Anchors

        public PatternBuilder BeginLine => Add(AnchorPattern.BeginLine);

        public PatternBuilder EndLine => Add(AnchorPattern.EndLine);

        public PatternBuilder Start => Add(AnchorPattern.Start);

        public PatternBuilder End => Add(AnchorPattern.End);

        public PatternBuilder AbsoluteEnd => Add(AnchorPattern.AbsoluteEnd);

        public PatternBuilder StartOfMatch => Add(AnchorPattern.StartOfMatch);

        public PatternBuilder WordBoundary => Add(AnchorPattern.WordBoundary);

        public PatternBuilder NonWordBoundary => Add(AnchorPattern.NonWordBoundary);

        #endregion

        public PatternBuilder Backreference(int group) => Add(new BackreferencePattern(group));

        public PatternBuilder Backreference(string group) => Add(new BackreferencePattern(group));

        #region Characters

        public PatternBuilder Character(char character) => Add(CharacterLiteralPattern.Character(character));

        public PatternBuilder Control(char character) => Add(CharacterLiteralPattern.Control(character));

        public PatternBuilder Escape => Add(CharacterLiteralPattern.Escape);

        public PatternBuilder Hexadecimal(string hex) => Add(CharacterLiteralPattern.Hexadecimal(hex));

        public PatternBuilder Octal(string octal) => Add(CharacterLiteralPattern.Octal(octal));

        public PatternBuilder Unicode(string hex) => Add(CharacterLiteralPattern.Unicode(hex));

        public PatternBuilder Word => Add(CharacterClassPattern.Word);

        public PatternBuilder NonWord => Add(CharacterClassPattern.NonWord);

        public PatternBuilder Digit => Add(CharacterClassPattern.Digit);

        public PatternBuilder NonDigit => Add(CharacterClassPattern.NonDigit);

        public PatternBuilder Whitespace => Add(CharacterClassPattern.Whitespace);

        public PatternBuilder NonWhitespace => Add(CharacterClassPattern.NonWhitespace);

        public PatternBuilder Category(string category) => Add(CharacterClassPattern.Category(category));

        public PatternBuilder NonCategory(string category) => Add(CharacterClassPattern.NonCategory(category));

        public PatternBuilder Single => Add(new SingleCharacterPattern());

        #endregion

        #region Character Sets

        public PatternBuilder CharacterSet(params char[] characters) => Add(new CharacterSetPattern(characters));

        public PatternBuilder CharacterSet(IEnumerable<char> characters, bool negated = false) => Add(new CharacterSetPattern(characters, negated));

        public PatternBuilder CharacterSet(IEnumerable<CharacterPattern> characters, bool negated = false) => Add(new CharacterSetPattern(characters, negated));

        public PatternBuilder CharacterSet(params CharacterRange[] characterRanges) => Add(new CharacterSetPattern(characterRanges));

        public PatternBuilder CharacterSet(IEnumerable<CharacterRange> characterRanges, bool negated = false) => Add(new CharacterSetPattern(characterRanges, negated));

        public PatternBuilder CharacterSet(ICollection<CharacterRange> characterRanges, ICollection<CharacterPattern> characters, bool negated = false) => Add(new CharacterSetPattern(characterRanges, characters, negated));

        public PatternBuilder CharacterSet(ICollection<CharacterRange> characterRanges, ICollection<CharacterPattern> characters, ICollection<CharacterPattern> subtractedCharacters, bool negated = false) => Add(new CharacterSetPattern(characterRanges, characters, subtractedCharacters, negated));

        #endregion

        public PatternBuilder Concat(params Pattern[] patterns) => Add(new ConcatPattern(patterns));

        public PatternBuilder Concat(IEnumerable<Pattern> patterns) => Add(new ConcatPattern(patterns));

        #region Groups

        public PatternBuilder AtomicGroup(Pattern pattern) => Add(new AtomicGroupPattern(pattern));

        public PatternBuilder Comment(Pattern pattern) => Add(new CommentPattern(pattern));

        public PatternBuilder Comment(string value) => Add(new CommentPattern(value));

        public PatternBuilder Conditional(Pattern expression, Pattern yes, Pattern? no, bool lookahead = true) => Add(new ConditionalPattern(expression, yes, no, lookahead));

        public PatternBuilder Conditional(int group, Pattern yes, Pattern? no) => Add(new ConditionalPattern(group, yes, no));

        public PatternBuilder Conditional(string group, Pattern yes, Pattern? no) => Add(new ConditionalPattern(group, yes, no));

        public PatternBuilder Group(Pattern pattern, string? name = null, bool capture = true) => Add(new GroupPattern(pattern, name, capture));

        public PatternBuilder BalancingGroup(string name1, string name2, Pattern pattern) => Add(new GroupPattern(pattern, name1, name2));

        public PatternBuilder Modifiers(InlineModifier modifiers) => Add(new InlineModifierPattern(null, modifiers));

        public PatternBuilder Modifiers(Pattern? pattern, InlineModifier modifiers) => Add(new InlineModifierPattern(pattern, modifiers));

        public PatternBuilder PositiveLookahead(Pattern pattern) => Add(LookaheadPattern.PositiveLookahead(pattern));

        public PatternBuilder NegativeLookahead(Pattern pattern) => Add(LookaheadPattern.NegativeLookahead(pattern));

        public PatternBuilder PositiveLookbehind(Pattern pattern) => Add(LookaheadPattern.PositiveLookbehind(pattern));

        public PatternBuilder NegativeLookbehind(Pattern pattern) => Add(LookaheadPattern.NegativeLookbehind(pattern));

        #endregion

        public PatternBuilder Literal(string value) => Add(new LiteralPattern(value));

        public PatternBuilder Raw(string regex) => Add(new RawPattern(regex));

        public PatternBuilder Or(params Pattern[] patterns) => Add(new OrPattern(patterns));

        public PatternBuilder Or(IEnumerable<Pattern> patterns) => Add(new OrPattern(patterns));

        #region Quantifiers

        public PatternBuilder ZeroOrOne(bool greedy = true) => AddQuantifier(0, 1, greedy);

        public PatternBuilder ZeroOrMore(bool greedy = true) => AddQuantifier(0, null, greedy);

        public PatternBuilder OneOrMore(bool greedy = true) => AddQuantifier(1, null, greedy);

        public PatternBuilder Exactly(int number) => AddQuantifier(number, number, true);

        public PatternBuilder Between(int min, int max, bool greedy = true) => AddQuantifier(min, max, greedy);

        public PatternBuilder AtLeast(int min, bool greedy = true) => AddQuantifier(min, null, greedy);

        public PatternBuilder AtMost(int max, bool greedy = true) => AddQuantifier(0, max, greedy);

        private PatternBuilder AddQuantifier(int min, int? max, bool greedy)
        {
            if (_current == null)
            {
                throw new InvalidOperationException("A pattern is required for quantifiers.");
            }

            return ReplaceLast(new QuantifierPattern(_current, min, max, greedy));
        }

        #endregion

        public Pattern Build()
        {
            return new ConcatPattern(_patterns);
        }

        internal override void ToString(StringBuilder builder)
        {
            throw new NotImplementedException();
        }

        private PatternBuilder Add(Pattern pattern)
        {
            _patterns.Add(pattern);
            _current = pattern;
            return this;
        }

        private PatternBuilder ReplaceLast(Pattern pattern)
        {
            _patterns[^1] = pattern;
            _current = pattern;
            return this;
        }
    }
}
