using System;
using System.Collections.Generic;
using System.Diagnostics;
using System.Linq;
using System.Text;
using static Wilgysef.FluentRegex.CharacterSetPattern;

namespace Wilgysef.FluentRegex
{
    public class PatternBuilder : ContainerPattern
    {
        public PatternBuilder() { }

        private PatternBuilder(IEnumerable<Pattern> patterns) : base(patterns) { }

        #region Anchors

        [DebuggerBrowsable(DebuggerBrowsableState.Never)]
        public PatternBuilder BeginLine => Add(AnchorPattern.BeginLine);

        [DebuggerBrowsable(DebuggerBrowsableState.Never)]
        public PatternBuilder EndLine => Add(AnchorPattern.EndLine);

        [DebuggerBrowsable(DebuggerBrowsableState.Never)]
        public PatternBuilder Start => Add(AnchorPattern.Start);

        [DebuggerBrowsable(DebuggerBrowsableState.Never)]
        public PatternBuilder End => Add(AnchorPattern.End);

        [DebuggerBrowsable(DebuggerBrowsableState.Never)]
        public PatternBuilder AbsoluteEnd => Add(AnchorPattern.AbsoluteEnd);

        [DebuggerBrowsable(DebuggerBrowsableState.Never)]
        public PatternBuilder StartOfMatch => Add(AnchorPattern.StartOfMatch);

        [DebuggerBrowsable(DebuggerBrowsableState.Never)]
        public PatternBuilder WordBoundary => Add(AnchorPattern.WordBoundary);

        [DebuggerBrowsable(DebuggerBrowsableState.Never)]
        public PatternBuilder NonWordBoundary => Add(AnchorPattern.NonWordBoundary);

        #endregion

        public PatternBuilder Backreference(int group) => Add(new BackreferencePattern(group));

        public PatternBuilder Backreference(string group) => Add(new BackreferencePattern(group));

        #region Characters

        public PatternBuilder Character(char character) => Add(CharacterPattern.Character(character));

        public PatternBuilder Control(char character) => Add(CharacterPattern.Control(character));

        [DebuggerBrowsable(DebuggerBrowsableState.Never)]
        public PatternBuilder Escape => Add(CharacterPattern.Escape);

        public PatternBuilder Hexadecimal(string hex) => Add(CharacterPattern.Hexadecimal(hex));

        public PatternBuilder Octal(string octal) => Add(CharacterPattern.Octal(octal));

        public PatternBuilder Unicode(string hex) => Add(CharacterPattern.Unicode(hex));

        [DebuggerBrowsable(DebuggerBrowsableState.Never)]
        public PatternBuilder Word => Add(CharacterPattern.Word);

        [DebuggerBrowsable(DebuggerBrowsableState.Never)]
        public PatternBuilder NonWord => Add(CharacterPattern.NonWord);

        [DebuggerBrowsable(DebuggerBrowsableState.Never)]
        public PatternBuilder Digit => Add(CharacterPattern.Digit);

        [DebuggerBrowsable(DebuggerBrowsableState.Never)]
        public PatternBuilder NonDigit => Add(CharacterPattern.NonDigit);

        [DebuggerBrowsable(DebuggerBrowsableState.Never)]
        public PatternBuilder Whitespace => Add(CharacterPattern.Whitespace);

        [DebuggerBrowsable(DebuggerBrowsableState.Never)]
        public PatternBuilder NonWhitespace => Add(CharacterPattern.NonWhitespace);

        public PatternBuilder Category(string category) => Add(CharacterPattern.Category(category));

        public PatternBuilder NonCategory(string category) => Add(CharacterPattern.NonCategory(category));

        [DebuggerBrowsable(DebuggerBrowsableState.Never)]
        public PatternBuilder Single => Add(new SingleCharacterPattern());

        #endregion

        #region Character Sets

        public PatternBuilder CharacterSet(params char[] characters) => Add(new CharacterSetPattern(characters));

        public PatternBuilder CharacterSet(IEnumerable<char> characters, bool negated = false) => Add(new CharacterSetPattern(characters, negated));

        public PatternBuilder CharacterSet(params CharacterPattern[] characters) => Add(new CharacterSetPattern(characters));

        public PatternBuilder CharacterSet(IEnumerable<CharacterPattern> characters, bool negated = false) => Add(new CharacterSetPattern(characters, negated));

        public PatternBuilder CharacterSet(params CharacterRange[] characterRanges) => Add(new CharacterSetPattern(characterRanges));

        public PatternBuilder CharacterSet(IEnumerable<CharacterRange> characterRanges, bool negated = false) => Add(new CharacterSetPattern(characterRanges, negated));

        public PatternBuilder CharacterSet(IEnumerable<CharacterRange> characterRanges, IEnumerable<CharacterPattern> characters, bool negated = false) => Add(new CharacterSetPattern(characterRanges, characters, negated));

        public PatternBuilder CharacterSet(IEnumerable<CharacterRange> characterRanges, IEnumerable<CharacterPattern> characters, IEnumerable<CharacterPattern> subtractedCharacters, bool negated = false) => Add(new CharacterSetPattern(characterRanges, characters, subtractedCharacters, negated));

        public PatternBuilder CharacterSet(IEnumerable<CharacterRange> characterRanges, IEnumerable<CharacterPattern> characters, IEnumerable<CharacterRange> subtractedCharacterRanges, IEnumerable<CharacterPattern> subtractedCharacters, bool negated = false) => Add(new CharacterSetPattern(characterRanges, characters, subtractedCharacterRanges, subtractedCharacters, negated));

        #endregion

        public PatternBuilder Concat(params Pattern[] patterns) => Add(new ConcatPattern(patterns));

        public PatternBuilder Concat(IEnumerable<Pattern> patterns) => Add(new ConcatPattern(patterns));

        #region Groups

        public PatternBuilder AtomicGroup(Pattern? pattern) => Add(new AtomicGroupPattern(pattern));

        public PatternBuilder Comment(string? value) => Add(new CommentPattern(value));

        public PatternBuilder Conditional(Pattern expression, Pattern yes, Pattern? no, bool lookahead = true) => Add(new ConditionalPattern(expression, yes, no, lookahead));

        public PatternBuilder Conditional(int group, Pattern yes, Pattern? no) => Add(new ConditionalPattern(group, yes, no));

        public PatternBuilder Conditional(string group, Pattern yes, Pattern? no) => Add(new ConditionalPattern(group, yes, no));

        public PatternBuilder Group(Pattern? pattern, string? name = null, bool capture = true) => Add(new GroupPattern(pattern, name, capture));

        public PatternBuilder CaptureGroup(Pattern? pattern) => Add(new GroupPattern(pattern, null, true));

        public PatternBuilder CaptureGroup(string name, Pattern? pattern) => Add(new GroupPattern(pattern, name, true));

        public PatternBuilder NonCaptureGroup(Pattern? pattern) => Add(new GroupPattern(pattern, null, false));

        public PatternBuilder BalancingGroup(string? name1, string name2, Pattern? pattern) => Add(new GroupPattern(pattern, name1, name2));

        public PatternBuilder Modifiers(InlineModifier modifiers) => Add(new InlineModifierPattern(null, modifiers));

        public PatternBuilder Modifiers(InlineModifier modifiers, InlineModifier disabledModifiers) => Add(new InlineModifierPattern(null, modifiers, disabledModifiers));

        public PatternBuilder Modifiers(Pattern? pattern, InlineModifier modifiers) => Add(new InlineModifierPattern(pattern, modifiers));

        public PatternBuilder Modifiers(Pattern? pattern, InlineModifier modifiers, InlineModifier disabledModifiers) => Add(new InlineModifierPattern(pattern, modifiers, disabledModifiers));

        public PatternBuilder PositiveLookahead(Pattern? pattern) => Add(LookaheadPattern.PositiveLookahead(pattern));

        public PatternBuilder NegativeLookahead(Pattern? pattern) => Add(LookaheadPattern.NegativeLookahead(pattern));

        public PatternBuilder PositiveLookbehind(Pattern? pattern) => Add(LookaheadPattern.PositiveLookbehind(pattern));

        public PatternBuilder NegativeLookbehind(Pattern? pattern) => Add(LookaheadPattern.NegativeLookbehind(pattern));

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
            var current = Current;

            if (current is AnchorPattern)
            {
                throw new InvalidOperationException("Cannot quantify an anchor.");
            }

            if (current is ConcatPattern concat && !concat.IsSinglePattern)
            {
                current = new GroupPattern(current, capture: false);
            }

            return ReplaceLast(new QuantifierPattern(current, min, max, greedy));
        }

        #endregion

        private Pattern Current => _current ?? throw new InvalidOperationException("A pattern is required for quantifiers.");
        private Pattern? _current;

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

        public int? GetGroupNumber(GroupPattern group, IList<GroupPattern>? groups = null)
        {
            var index = (groups ?? GetNumberedGroups()).IndexOf(group);
            if (index == -1)
            {
                return null;
            }

            return index + 1;
        }

        public Pattern Build()
        {
            return new ConcatPattern(_children);
        }

        public override Pattern Copy()
        {
            return new PatternBuilder(_children.Select(c => c.Copy()));
        }

        internal override void ToString(StringBuilder builder)
        {
            Build().ToString(builder);
        }

        private PatternBuilder Add(Pattern pattern)
        {
            _children.Add(pattern);
            _current = pattern;
            return this;
        }

        private PatternBuilder ReplaceLast(Pattern pattern)
        {
            _children[^1] = pattern;
            _current = pattern;
            return this;
        }

        private static IEnumerable<Pattern> Traverse(IReadOnlyList<Pattern> patterns)
        {
            var stack = new Stack<IEnumerator<Pattern>>();
            stack.Push(patterns.GetEnumerator());

            while (stack.Count > 0)
            {
                var currentPatterns = stack.Peek();
                var skipPop = false;

                while (currentPatterns.MoveNext())
                {
                    yield return currentPatterns.Current;

                    if (currentPatterns.Current is ContainerPattern container)
                    {
                        if (container.Children.Count > 0)
                        {
                            stack.Push(container.Children.GetEnumerator());
                            skipPop = true;
                            break;
                        }
                    }
                }

                if (!skipPop)
                {
                    stack.Pop();
                }
            }
        }
    }
}
