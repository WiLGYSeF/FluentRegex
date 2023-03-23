using System.Text.RegularExpressions;
using Wilgysef.FluentRegex.Exceptions;
using static Wilgysef.FluentRegex.CharacterSetPattern;

namespace Wilgysef.FluentRegex.Tests;

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

    [Fact]
    public void Traverse()
    {
        var builder = new PatternBuilder()
            .CapturingGroup("z", new PatternBuilder().Group(new LiteralPattern("a")).Literal("b"))
            .Group(new LiteralPattern("c"))
            .Literal("asdf")
            .Between(1, 6);

        builder.ToString().ShouldBe("(?<z>(a)b)(c)(?:asdf){1,6}");

        var traversed = builder.Traverse().ToList();
        traversed.Count.ShouldBe(9);

        traversed[0].GetType().ShouldBe(typeof(GroupPattern));
        traversed[1].GetType().ShouldBe(typeof(PatternBuilder));
        traversed[2].GetType().ShouldBe(typeof(GroupPattern));
        traversed[3].GetType().ShouldBe(typeof(LiteralPattern));
        traversed[4].GetType().ShouldBe(typeof(LiteralPattern));
        traversed[5].GetType().ShouldBe(typeof(GroupPattern));
        traversed[6].GetType().ShouldBe(typeof(LiteralPattern));
        traversed[7].GetType().ShouldBe(typeof(QuantifierPattern));
        traversed[8].GetType().ShouldBe(typeof(LiteralPattern));
    }

    [Fact]
    public void Traverse_Recursive()
    {
        var group = new GroupPattern(new LiteralPattern("a"));
        group.WithPattern(new ConcatPattern(new LiteralPattern("b"), group));

        var builder = new PatternBuilder().CapturingGroup("z", group);

        Should.Throw<PatternRecursionException>(() => builder.Build());

        var exceptionThrown = false;
        try
        {
            group.ToString();
        }
        catch (PatternRecursionException ex)
        {
            ex.GetPatternPath().ShouldBe("ConcatPattern -> *GroupPattern");
            exceptionThrown = true;
        }

        exceptionThrown.ShouldBeTrue();
    }

    [Fact]
    public void EmailRegex()
    {
        #region Reference

        // https://www.rfc-editor.org/rfc/rfc5234

        // ALPHA          =  %x41-5A / %x61-7A   ; A-Z / a-z
        //
        // BIT            =  "0" / "1"
        //
        // CHAR           =  %x01-7F
        //                        ; any 7-bit US-ASCII character,
        //                        ;  excluding NUL
        //
        // CR             =  %x0D
        //                        ; carriage return
        //
        // CRLF           =  CR LF
        //                        ; Internet standard newline
        //
        // CTL            =  %x00-1F / %x7F
        //                        ; controls
        //
        // DIGIT          =  %x30-39
        //                        ; 0-9
        //
        // DQUOTE         =  %x22
        //                        ; " (Double Quote)
        //
        // HEXDIG         =  DIGIT / "A" / "B" / "C" / "D" / "E" / "F"
        //
        // HTAB           =  %x09
        //                        ; horizontal tab
        //
        // LF             =  %x0A
        //                        ; linefeed
        //
        // LWSP           =  *(WSP / CRLF WSP)
        //                        ; Use of this linear-white-space rule
        //                        ;  permits lines containing only white
        //                        ;  space that are no longer legal in
        //                        ;  mail headers and have caused
        //                        ;  interoperability problems in other
        //                        ;  contexts.
        //                        ; Do not use when defining mail
        //                        ;  headers and use with caution in
        //                        ;  other contexts.
        //
        // OCTET          =  %x00-FF
        //                        ; 8 bits of data
        //
        // SP             =  %x20
        //
        // VCHAR          =  %x21-7E
        //                        ; visible (printing) characters
        //
        // WSP            =  SP / HTAB
        //                        ; white space

        // https://www.rfc-editor.org/rfc/rfc5322

        // quoted-pair     =   ("\" (VCHAR / WSP)) / obs-qp

        // FWS             =   ([*WSP CRLF] 1*WSP) /  obs-FWS
        //                                        ; Folding white space
        //
        // ctext           =   %d33-39 /          ; Printable US-ASCII
        //                     %d42-91 /          ;  characters not including
        //                     %d93-126 /         ;  "(", ")", or "\"
        //                     obs-ctext
        //
        // ccontent        =   ctext / quoted-pair / comment
        //
        // comment         =   "(" *([FWS] ccontent) [FWS] ")"
        //
        // CFWS            =   (1*([FWS] comment) [FWS]) / FWS

        // atext           =   ALPHA / DIGIT /    ; Printable US-ASCII
        //                     "!" / "#" /        ;  characters not including
        //                     "$" / "%" /        ;  specials.  Used for atoms.
        //                     "&" / "'" /
        //                     "*" / "+" /
        //                     "-" / "/" /
        //                     "=" / "?" /
        //                     "^" / "_" /
        //                     "`" / "{" /
        //                     "|" / "}" /
        //                     "~"
        //
        // atom            =   [CFWS] 1*atext [CFWS]
        //
        // dot-atom-text   =   1*atext *("." 1*atext)
        //
        // dot-atom        =   [CFWS] dot-atom-text [CFWS]
        //
        // specials        =   "(" / ")" /        ; Special characters that do
        //                     "<" / ">" /        ;  not appear in atext
        //                     "[" / "]" /
        //                     ":" / ";" /
        //                     "@" / "\" /
        //                     "," / "." /
        //                     DQUOTE

        // qtext           =   %d33 /             ; Printable US-ASCII
        //                     %d35-91 /          ;  characters not including
        //                     %d93-126 /         ;  "\" or the quote character
        //                     obs-qtext
        //
        // qcontent        =   qtext / quoted-pair
        // 
        // quoted-string   =   [CFWS]
        //                     DQUOTE *([FWS] qcontent) [FWS] DQUOTE
        //                     [CFWS]

        // word            =   atom / quoted-string

        // addr-spec       =   local-part "@" domain
        //
        // local-part      =   dot-atom / quoted-string / obs-local-part
        //
        // domain          =   dot-atom / domain-literal / obs-domain
        //
        // domain-literal  =   [CFWS] "[" *([FWS] dtext) [FWS] "]" [CFWS]
        //
        // dtext           =   %d33-90 /          ; Printable US-ASCII
        //                     %d94-126 /         ;  characters not including
        //                     obs-dtext          ;  "[", "]", or "\"

        // obs-NO-WS-CTL   =   %d1-8 /            ; US-ASCII control
        //                     %d11 /             ;  characters that do not
        //                     %d12 /             ;  include the carriage
        //                     %d14-31 /          ;  return, line feed, and
        //                     %d127              ;  white space characters
        //
        // obs-ctext       =   obs-NO-WS-CTL
        //
        // obs-qtext       =   obs-NO-WS-CTL
        //
        // obs-utext       =   %d0 / obs-NO-WS-CTL / VCHAR
        //
        // obs-qp          =   "\" (%d0 / obs-NO-WS-CTL / LF / CR)
        //
        // obs-body        =   *((*LF *CR *((%d0 / text) *LF *CR)) / CRLF)
        //
        // obs-unstruct    =   *((*LF *CR *(obs-utext *LF *CR)) / FWS)
        //
        // obs-phrase      =   word *(word / "." / CFWS)
        //
        // obs-phrase-list =   [phrase / CFWS] *("," [phrase / CFWS])

        // obs-FWS         =   1*WSP *(CRLF 1*WSP)

        // obs-angle-addr  =   [CFWS] "<" obs-route addr-spec ">" [CFWS]
        //
        // obs-route       =   obs-domain-list ":"
        // 
        // obs-domain-list =   *(CFWS / ",") "@" domain
        //                     *("," [CFWS] ["@" domain])
        // 
        // obs-mbox-list   =   *([CFWS] ",") mailbox *("," [mailbox / CFWS])
        // 
        // obs-addr-list   =   *([CFWS] ",") address *("," [address / CFWS])
        // 
        // obs-group-list  =   1*([CFWS] ",") [CFWS]
        // 
        // obs-local-part  =   word *("." word)
        // 
        // obs-domain      =   atom *("." atom)
        // 
        // obs-dtext       =   obs-NO-WS-CTL / quoted-pair

        #endregion

        var alpha = new CharacterSetPattern(
            new CharacterRange('A', 'Z'),
            new CharacterRange('a', 'z'));

        var bit = new CharacterSetPattern("01");

        var @char = new CharacterSetPattern(CharacterRange.Hexadecimal("01", "7F"));

        var cr = CharacterPattern.Character('\r');

        var ctl = new CharacterSetPattern(
            new[] { CharacterRange.Hexadecimal("01", "1F") },
            new[] { CharacterPattern.Hexadecimal("7F") });

        var digit = new CharacterSetPattern(new CharacterRange('0', '9'));

        var dquote = CharacterPattern.Character('\"');

        var hexDig = new CharacterSetPattern(
            new[]
            {
                new CharacterRange('0', '9'),
                new CharacterRange('A', 'F'),
            });

        var htab = CharacterPattern.Character('\t');

        var lf = CharacterPattern.Character('\n');

        var octet = new CharacterSetPattern(CharacterRange.Hexadecimal("00", "FF"));

        var sp = CharacterPattern.Character(' ');

        var vchar = new CharacterSetPattern(CharacterRange.Hexadecimal("21", "7E"));

        var wsp = new CharacterSetPattern(sp, htab);

        var crLf = new ConcatPattern(cr, lf);

        var lwsp = new PatternBuilder()
            .NonCapturingGroup(new PatternBuilder().ZeroOrOne(crLf).Concat(wsp))
            .ZeroOrMore();

        var obsNoWsCtl = new CharacterSetPattern(
            new[]
            {
                CharacterRange.Hexadecimal("01", "08"),
                CharacterRange.Hexadecimal("14", "1F"),
            },
            new[]
            {
                CharacterPattern.Hexadecimal("11"), 
                CharacterPattern.Hexadecimal("12"), 
                CharacterPattern.Hexadecimal("7F"),
            });

        var obsQp = new PatternBuilder().Character('\\')
            .Or(obsNoWsCtl, lf, cr);

        var quotedPair = new OrPattern(
            new PatternBuilder().Character('\\').Or(vchar, wsp),
            obsQp);

        var obsFws = new PatternBuilder().OneOrMore(wsp)
            .ZeroOrMore(new PatternBuilder(crLf).OneOrMore(wsp));

        var fws = new OrPattern(
            new PatternBuilder(new PatternBuilder(wsp).ZeroOrMore().Concat(crLf)).ZeroOrOne()
                .Concat(wsp).OneOrMore(),
            obsFws);

        var obsCtext = obsNoWsCtl;

        var ctext = new OrPattern(
            new CharacterSetPattern(
                new[]
                {
                    CharacterRange.Hexadecimal("21", "27"),
                    CharacterRange.Hexadecimal("2A", "5B"),
                    CharacterRange.Hexadecimal("5D", "7E"),
                }),
            obsCtext);

        var ccontent = new OrPattern(ctext, quotedPair); // OR comment

        var comment = new PatternBuilder().Character('(')
            .ZeroOrMore(new PatternBuilder().ZeroOrOne(fws).Concat(ccontent))
            .ZeroOrOne(fws)
            .Character(')');

        var cfws = new OrPattern(
            new PatternBuilder(
                new PatternBuilder().ZeroOrOne(fws).Concat(comment)).OneOrMore()
                .Concat(fws).ZeroOrOne(),
            fws);

        var atext = new OrPattern(
            alpha,
            digit,
            new CharacterSetPattern("!#$%&'*+-/=?^_`{|}"));

        var atom = new PatternBuilder().ZeroOrOne(cfws).OneOrMore(atext).ZeroOrOne(cfws);

        var dotAtomText = new PatternBuilder().OneOrMore(atext)
            .ZeroOrMore(new PatternBuilder().Character('.').OneOrMore(atext));
        var dotAtom = new PatternBuilder().ZeroOrOne(cfws).Concat(dotAtomText).ZeroOrOne(cfws);

        var obsQtext = obsNoWsCtl;

        var qtext = new OrPattern(
            new CharacterSetPattern(
                new[]
                {
                    CharacterRange.Hexadecimal("23", "5B"),
                    CharacterRange.Hexadecimal("5D", "7E"),
                },
                new[] { CharacterPattern.Hexadecimal("21") }),
            obsQtext);

        var qcontent = new OrPattern(qtext, quotedPair);

        var quotedString = new PatternBuilder()
            .ZeroOrOne(cfws)
            .Concat(dquote)
            .ZeroOrMore(new PatternBuilder().ZeroOrOne(fws).Concat(qcontent))
            .ZeroOrOne(fws)
            .Concat(dquote)
            .ZeroOrOne(cfws);

        var word = new OrPattern(atom, quotedString);

        var obsLocalPart = new PatternBuilder(word).ZeroOrMore(new PatternBuilder().Character('.').Concat(word));

        var obsDtext = new OrPattern(obsNoWsCtl, quotedPair);

        var dtext = new OrPattern(
            new CharacterSetPattern(
                new[]
                {
                    CharacterRange.Hexadecimal("21", "5A"),
                    CharacterRange.Hexadecimal("5E", "7E"),
                }),
            obsDtext);

        var domainLiteral = new PatternBuilder(cfws).ZeroOrOne()
            .Character('[')
            .ZeroOrMore(new PatternBuilder(fws).ZeroOrOne().Concat(dtext))
            .ZeroOrOne(fws)
            .Character(']')
            .ZeroOrOne(cfws);

        var obsDomain = new PatternBuilder(atom).ZeroOrMore(new PatternBuilder().Character('.').Concat(atom));

        var localPart = new OrPattern(dotAtom, quotedString, obsLocalPart);
        var domain = new OrPattern(dotAtom, domainLiteral, obsDomain);

        var addrSpec = new PatternBuilder()
            .BeginLine
            .CapturingGroup("local", localPart)
            .Character('@')
            .CapturingGroup("domain", domain)
            .EndLine;

        var expected = @"^(?<local>(?:(?:(?:(?:[ \t]*\r\n)?[ \t]+|[ \t]+(?:\r\n[ \t]+)*)?\((?:(?:(?:[ \t]*\r\n)?[ \t]+|[ \t]+(?:\r\n[ \t]+)*)?(?:[\x21-\x27\x2A-\x5B\x5D-\x7E]|[\x01-\x08\x14-\x1F\x11\x12\x7F]|\\(?:[\x21-\x7E]|[ \t])|\\(?:[\x01-\x08\x14-\x1F\x11\x12\x7F]|\n|\r)))*(?:(?:[ \t]*\r\n)?[ \t]+|[ \t]+(?:\r\n[ \t]+)*)?\))+(?:(?:[ \t]*\r\n)?[ \t]+|[ \t]+(?:\r\n[ \t]+)*)?|(?:[ \t]*\r\n)?[ \t]+|[ \t]+(?:\r\n[ \t]+)*)?(?:[A-Za-z]|[0-9]|[!#$%&'*+\-/=?\^_`{|}])+(?:\.(?:[A-Za-z]|[0-9]|[!#$%&'*+\-/=?\^_`{|}])+)*(?:(?:(?:(?:[ \t]*\r\n)?[ \t]+|[ \t]+(?:\r\n[ \t]+)*)?\((?:(?:(?:[ \t]*\r\n)?[ \t]+|[ \t]+(?:\r\n[ \t]+)*)?(?:[\x21-\x27\x2A-\x5B\x5D-\x7E]|[\x01-\x08\x14-\x1F\x11\x12\x7F]|\\(?:[\x21-\x7E]|[ \t])|\\(?:[\x01-\x08\x14-\x1F\x11\x12\x7F]|\n|\r)))*(?:(?:[ \t]*\r\n)?[ \t]+|[ \t]+(?:\r\n[ \t]+)*)?\))+(?:(?:[ \t]*\r\n)?[ \t]+|[ \t]+(?:\r\n[ \t]+)*)?|(?:[ \t]*\r\n)?[ \t]+|[ \t]+(?:\r\n[ \t]+)*)?|(?:(?:(?:(?:[ \t]*\r\n)?[ \t]+|[ \t]+(?:\r\n[ \t]+)*)?\((?:(?:(?:[ \t]*\r\n)?[ \t]+|[ \t]+(?:\r\n[ \t]+)*)?(?:[\x21-\x27\x2A-\x5B\x5D-\x7E]|[\x01-\x08\x14-\x1F\x11\x12\x7F]|\\(?:[\x21-\x7E]|[ \t])|\\(?:[\x01-\x08\x14-\x1F\x11\x12\x7F]|\n|\r)))*(?:(?:[ \t]*\r\n)?[ \t]+|[ \t]+(?:\r\n[ \t]+)*)?\))+(?:(?:[ \t]*\r\n)?[ \t]+|[ \t]+(?:\r\n[ \t]+)*)?|(?:[ \t]*\r\n)?[ \t]+|[ \t]+(?:\r\n[ \t]+)*)?""(?:(?:(?:[ \t]*\r\n)?[ \t]+|[ \t]+(?:\r\n[ \t]+)*)?(?:[\x23-\x5B\x5D-\x7E\x21]|[\x01-\x08\x14-\x1F\x11\x12\x7F]|\\(?:[\x21-\x7E]|[ \t])|\\(?:[\x01-\x08\x14-\x1F\x11\x12\x7F]|\n|\r)))*(?:(?:[ \t]*\r\n)?[ \t]+|[ \t]+(?:\r\n[ \t]+)*)?""(?:(?:(?:(?:[ \t]*\r\n)?[ \t]+|[ \t]+(?:\r\n[ \t]+)*)?\((?:(?:(?:[ \t]*\r\n)?[ \t]+|[ \t]+(?:\r\n[ \t]+)*)?(?:[\x21-\x27\x2A-\x5B\x5D-\x7E]|[\x01-\x08\x14-\x1F\x11\x12\x7F]|\\(?:[\x21-\x7E]|[ \t])|\\(?:[\x01-\x08\x14-\x1F\x11\x12\x7F]|\n|\r)))*(?:(?:[ \t]*\r\n)?[ \t]+|[ \t]+(?:\r\n[ \t]+)*)?\))+(?:(?:[ \t]*\r\n)?[ \t]+|[ \t]+(?:\r\n[ \t]+)*)?|(?:[ \t]*\r\n)?[ \t]+|[ \t]+(?:\r\n[ \t]+)*)?|(?:(?:(?:(?:(?:[ \t]*\r\n)?[ \t]+|[ \t]+(?:\r\n[ \t]+)*)?\((?:(?:(?:[ \t]*\r\n)?[ \t]+|[ \t]+(?:\r\n[ \t]+)*)?(?:[\x21-\x27\x2A-\x5B\x5D-\x7E]|[\x01-\x08\x14-\x1F\x11\x12\x7F]|\\(?:[\x21-\x7E]|[ \t])|\\(?:[\x01-\x08\x14-\x1F\x11\x12\x7F]|\n|\r)))*(?:(?:[ \t]*\r\n)?[ \t]+|[ \t]+(?:\r\n[ \t]+)*)?\))+(?:(?:[ \t]*\r\n)?[ \t]+|[ \t]+(?:\r\n[ \t]+)*)?|(?:[ \t]*\r\n)?[ \t]+|[ \t]+(?:\r\n[ \t]+)*)?(?:[A-Za-z]|[0-9]|[!#$%&'*+\-/=?\^_`{|}])+(?:(?:(?:(?:[ \t]*\r\n)?[ \t]+|[ \t]+(?:\r\n[ \t]+)*)?\((?:(?:(?:[ \t]*\r\n)?[ \t]+|[ \t]+(?:\r\n[ \t]+)*)?(?:[\x21-\x27\x2A-\x5B\x5D-\x7E]|[\x01-\x08\x14-\x1F\x11\x12\x7F]|\\(?:[\x21-\x7E]|[ \t])|\\(?:[\x01-\x08\x14-\x1F\x11\x12\x7F]|\n|\r)))*(?:(?:[ \t]*\r\n)?[ \t]+|[ \t]+(?:\r\n[ \t]+)*)?\))+(?:(?:[ \t]*\r\n)?[ \t]+|[ \t]+(?:\r\n[ \t]+)*)?|(?:[ \t]*\r\n)?[ \t]+|[ \t]+(?:\r\n[ \t]+)*)?|(?:(?:(?:(?:[ \t]*\r\n)?[ \t]+|[ \t]+(?:\r\n[ \t]+)*)?\((?:(?:(?:[ \t]*\r\n)?[ \t]+|[ \t]+(?:\r\n[ \t]+)*)?(?:[\x21-\x27\x2A-\x5B\x5D-\x7E]|[\x01-\x08\x14-\x1F\x11\x12\x7F]|\\(?:[\x21-\x7E]|[ \t])|\\(?:[\x01-\x08\x14-\x1F\x11\x12\x7F]|\n|\r)))*(?:(?:[ \t]*\r\n)?[ \t]+|[ \t]+(?:\r\n[ \t]+)*)?\))+(?:(?:[ \t]*\r\n)?[ \t]+|[ \t]+(?:\r\n[ \t]+)*)?|(?:[ \t]*\r\n)?[ \t]+|[ \t]+(?:\r\n[ \t]+)*)?""(?:(?:(?:[ \t]*\r\n)?[ \t]+|[ \t]+(?:\r\n[ \t]+)*)?(?:[\x23-\x5B\x5D-\x7E\x21]|[\x01-\x08\x14-\x1F\x11\x12\x7F]|\\(?:[\x21-\x7E]|[ \t])|\\(?:[\x01-\x08\x14-\x1F\x11\x12\x7F]|\n|\r)))*(?:(?:[ \t]*\r\n)?[ \t]+|[ \t]+(?:\r\n[ \t]+)*)?""(?:(?:(?:(?:[ \t]*\r\n)?[ \t]+|[ \t]+(?:\r\n[ \t]+)*)?\((?:(?:(?:[ \t]*\r\n)?[ \t]+|[ \t]+(?:\r\n[ \t]+)*)?(?:[\x21-\x27\x2A-\x5B\x5D-\x7E]|[\x01-\x08\x14-\x1F\x11\x12\x7F]|\\(?:[\x21-\x7E]|[ \t])|\\(?:[\x01-\x08\x14-\x1F\x11\x12\x7F]|\n|\r)))*(?:(?:[ \t]*\r\n)?[ \t]+|[ \t]+(?:\r\n[ \t]+)*)?\))+(?:(?:[ \t]*\r\n)?[ \t]+|[ \t]+(?:\r\n[ \t]+)*)?|(?:[ \t]*\r\n)?[ \t]+|[ \t]+(?:\r\n[ \t]+)*)?)(?:\.(?:(?:(?:(?:(?:[ \t]*\r\n)?[ \t]+|[ \t]+(?:\r\n[ \t]+)*)?\((?:(?:(?:[ \t]*\r\n)?[ \t]+|[ \t]+(?:\r\n[ \t]+)*)?(?:[\x21-\x27\x2A-\x5B\x5D-\x7E]|[\x01-\x08\x14-\x1F\x11\x12\x7F]|\\(?:[\x21-\x7E]|[ \t])|\\(?:[\x01-\x08\x14-\x1F\x11\x12\x7F]|\n|\r)))*(?:(?:[ \t]*\r\n)?[ \t]+|[ \t]+(?:\r\n[ \t]+)*)?\))+(?:(?:[ \t]*\r\n)?[ \t]+|[ \t]+(?:\r\n[ \t]+)*)?|(?:[ \t]*\r\n)?[ \t]+|[ \t]+(?:\r\n[ \t]+)*)?(?:[A-Za-z]|[0-9]|[!#$%&'*+\-/=?\^_`{|}])+(?:(?:(?:(?:[ \t]*\r\n)?[ \t]+|[ \t]+(?:\r\n[ \t]+)*)?\((?:(?:(?:[ \t]*\r\n)?[ \t]+|[ \t]+(?:\r\n[ \t]+)*)?(?:[\x21-\x27\x2A-\x5B\x5D-\x7E]|[\x01-\x08\x14-\x1F\x11\x12\x7F]|\\(?:[\x21-\x7E]|[ \t])|\\(?:[\x01-\x08\x14-\x1F\x11\x12\x7F]|\n|\r)))*(?:(?:[ \t]*\r\n)?[ \t]+|[ \t]+(?:\r\n[ \t]+)*)?\))+(?:(?:[ \t]*\r\n)?[ \t]+|[ \t]+(?:\r\n[ \t]+)*)?|(?:[ \t]*\r\n)?[ \t]+|[ \t]+(?:\r\n[ \t]+)*)?|(?:(?:(?:(?:[ \t]*\r\n)?[ \t]+|[ \t]+(?:\r\n[ \t]+)*)?\((?:(?:(?:[ \t]*\r\n)?[ \t]+|[ \t]+(?:\r\n[ \t]+)*)?(?:[\x21-\x27\x2A-\x5B\x5D-\x7E]|[\x01-\x08\x14-\x1F\x11\x12\x7F]|\\(?:[\x21-\x7E]|[ \t])|\\(?:[\x01-\x08\x14-\x1F\x11\x12\x7F]|\n|\r)))*(?:(?:[ \t]*\r\n)?[ \t]+|[ \t]+(?:\r\n[ \t]+)*)?\))+(?:(?:[ \t]*\r\n)?[ \t]+|[ \t]+(?:\r\n[ \t]+)*)?|(?:[ \t]*\r\n)?[ \t]+|[ \t]+(?:\r\n[ \t]+)*)?""(?:(?:(?:[ \t]*\r\n)?[ \t]+|[ \t]+(?:\r\n[ \t]+)*)?(?:[\x23-\x5B\x5D-\x7E\x21]|[\x01-\x08\x14-\x1F\x11\x12\x7F]|\\(?:[\x21-\x7E]|[ \t])|\\(?:[\x01-\x08\x14-\x1F\x11\x12\x7F]|\n|\r)))*(?:(?:[ \t]*\r\n)?[ \t]+|[ \t]+(?:\r\n[ \t]+)*)?""(?:(?:(?:(?:[ \t]*\r\n)?[ \t]+|[ \t]+(?:\r\n[ \t]+)*)?\((?:(?:(?:[ \t]*\r\n)?[ \t]+|[ \t]+(?:\r\n[ \t]+)*)?(?:[\x21-\x27\x2A-\x5B\x5D-\x7E]|[\x01-\x08\x14-\x1F\x11\x12\x7F]|\\(?:[\x21-\x7E]|[ \t])|\\(?:[\x01-\x08\x14-\x1F\x11\x12\x7F]|\n|\r)))*(?:(?:[ \t]*\r\n)?[ \t]+|[ \t]+(?:\r\n[ \t]+)*)?\))+(?:(?:[ \t]*\r\n)?[ \t]+|[ \t]+(?:\r\n[ \t]+)*)?|(?:[ \t]*\r\n)?[ \t]+|[ \t]+(?:\r\n[ \t]+)*)?))*)@(?<domain>(?:(?:(?:(?:[ \t]*\r\n)?[ \t]+|[ \t]+(?:\r\n[ \t]+)*)?\((?:(?:(?:[ \t]*\r\n)?[ \t]+|[ \t]+(?:\r\n[ \t]+)*)?(?:[\x21-\x27\x2A-\x5B\x5D-\x7E]|[\x01-\x08\x14-\x1F\x11\x12\x7F]|\\(?:[\x21-\x7E]|[ \t])|\\(?:[\x01-\x08\x14-\x1F\x11\x12\x7F]|\n|\r)))*(?:(?:[ \t]*\r\n)?[ \t]+|[ \t]+(?:\r\n[ \t]+)*)?\))+(?:(?:[ \t]*\r\n)?[ \t]+|[ \t]+(?:\r\n[ \t]+)*)?|(?:[ \t]*\r\n)?[ \t]+|[ \t]+(?:\r\n[ \t]+)*)?(?:[A-Za-z]|[0-9]|[!#$%&'*+\-/=?\^_`{|}])+(?:\.(?:[A-Za-z]|[0-9]|[!#$%&'*+\-/=?\^_`{|}])+)*(?:(?:(?:(?:[ \t]*\r\n)?[ \t]+|[ \t]+(?:\r\n[ \t]+)*)?\((?:(?:(?:[ \t]*\r\n)?[ \t]+|[ \t]+(?:\r\n[ \t]+)*)?(?:[\x21-\x27\x2A-\x5B\x5D-\x7E]|[\x01-\x08\x14-\x1F\x11\x12\x7F]|\\(?:[\x21-\x7E]|[ \t])|\\(?:[\x01-\x08\x14-\x1F\x11\x12\x7F]|\n|\r)))*(?:(?:[ \t]*\r\n)?[ \t]+|[ \t]+(?:\r\n[ \t]+)*)?\))+(?:(?:[ \t]*\r\n)?[ \t]+|[ \t]+(?:\r\n[ \t]+)*)?|(?:[ \t]*\r\n)?[ \t]+|[ \t]+(?:\r\n[ \t]+)*)?|(?:(?:(?:(?:[ \t]*\r\n)?[ \t]+|[ \t]+(?:\r\n[ \t]+)*)?\((?:(?:(?:[ \t]*\r\n)?[ \t]+|[ \t]+(?:\r\n[ \t]+)*)?(?:[\x21-\x27\x2A-\x5B\x5D-\x7E]|[\x01-\x08\x14-\x1F\x11\x12\x7F]|\\(?:[\x21-\x7E]|[ \t])|\\(?:[\x01-\x08\x14-\x1F\x11\x12\x7F]|\n|\r)))*(?:(?:[ \t]*\r\n)?[ \t]+|[ \t]+(?:\r\n[ \t]+)*)?\))+(?:(?:[ \t]*\r\n)?[ \t]+|[ \t]+(?:\r\n[ \t]+)*)?|(?:[ \t]*\r\n)?[ \t]+|[ \t]+(?:\r\n[ \t]+)*)?\[(?:(?:(?:[ \t]*\r\n)?[ \t]+|[ \t]+(?:\r\n[ \t]+)*)?(?:[\x21-\x5A\x5E-\x7E]|[\x01-\x08\x14-\x1F\x11\x12\x7F]|\\(?:[\x21-\x7E]|[ \t])|\\(?:[\x01-\x08\x14-\x1F\x11\x12\x7F]|\n|\r)))*(?:(?:[ \t]*\r\n)?[ \t]+|[ \t]+(?:\r\n[ \t]+)*)?\](?:(?:(?:(?:[ \t]*\r\n)?[ \t]+|[ \t]+(?:\r\n[ \t]+)*)?\((?:(?:(?:[ \t]*\r\n)?[ \t]+|[ \t]+(?:\r\n[ \t]+)*)?(?:[\x21-\x27\x2A-\x5B\x5D-\x7E]|[\x01-\x08\x14-\x1F\x11\x12\x7F]|\\(?:[\x21-\x7E]|[ \t])|\\(?:[\x01-\x08\x14-\x1F\x11\x12\x7F]|\n|\r)))*(?:(?:[ \t]*\r\n)?[ \t]+|[ \t]+(?:\r\n[ \t]+)*)?\))+(?:(?:[ \t]*\r\n)?[ \t]+|[ \t]+(?:\r\n[ \t]+)*)?|(?:[ \t]*\r\n)?[ \t]+|[ \t]+(?:\r\n[ \t]+)*)?|(?:(?:(?:(?:[ \t]*\r\n)?[ \t]+|[ \t]+(?:\r\n[ \t]+)*)?\((?:(?:(?:[ \t]*\r\n)?[ \t]+|[ \t]+(?:\r\n[ \t]+)*)?(?:[\x21-\x27\x2A-\x5B\x5D-\x7E]|[\x01-\x08\x14-\x1F\x11\x12\x7F]|\\(?:[\x21-\x7E]|[ \t])|\\(?:[\x01-\x08\x14-\x1F\x11\x12\x7F]|\n|\r)))*(?:(?:[ \t]*\r\n)?[ \t]+|[ \t]+(?:\r\n[ \t]+)*)?\))+(?:(?:[ \t]*\r\n)?[ \t]+|[ \t]+(?:\r\n[ \t]+)*)?|(?:[ \t]*\r\n)?[ \t]+|[ \t]+(?:\r\n[ \t]+)*)?(?:[A-Za-z]|[0-9]|[!#$%&'*+\-/=?\^_`{|}])+(?:(?:(?:(?:[ \t]*\r\n)?[ \t]+|[ \t]+(?:\r\n[ \t]+)*)?\((?:(?:(?:[ \t]*\r\n)?[ \t]+|[ \t]+(?:\r\n[ \t]+)*)?(?:[\x21-\x27\x2A-\x5B\x5D-\x7E]|[\x01-\x08\x14-\x1F\x11\x12\x7F]|\\(?:[\x21-\x7E]|[ \t])|\\(?:[\x01-\x08\x14-\x1F\x11\x12\x7F]|\n|\r)))*(?:(?:[ \t]*\r\n)?[ \t]+|[ \t]+(?:\r\n[ \t]+)*)?\))+(?:(?:[ \t]*\r\n)?[ \t]+|[ \t]+(?:\r\n[ \t]+)*)?|(?:[ \t]*\r\n)?[ \t]+|[ \t]+(?:\r\n[ \t]+)*)?(?:\.(?:(?:(?:(?:[ \t]*\r\n)?[ \t]+|[ \t]+(?:\r\n[ \t]+)*)?\((?:(?:(?:[ \t]*\r\n)?[ \t]+|[ \t]+(?:\r\n[ \t]+)*)?(?:[\x21-\x27\x2A-\x5B\x5D-\x7E]|[\x01-\x08\x14-\x1F\x11\x12\x7F]|\\(?:[\x21-\x7E]|[ \t])|\\(?:[\x01-\x08\x14-\x1F\x11\x12\x7F]|\n|\r)))*(?:(?:[ \t]*\r\n)?[ \t]+|[ \t]+(?:\r\n[ \t]+)*)?\))+(?:(?:[ \t]*\r\n)?[ \t]+|[ \t]+(?:\r\n[ \t]+)*)?|(?:[ \t]*\r\n)?[ \t]+|[ \t]+(?:\r\n[ \t]+)*)?(?:[A-Za-z]|[0-9]|[!#$%&'*+\-/=?\^_`{|}])+(?:(?:(?:(?:[ \t]*\r\n)?[ \t]+|[ \t]+(?:\r\n[ \t]+)*)?\((?:(?:(?:[ \t]*\r\n)?[ \t]+|[ \t]+(?:\r\n[ \t]+)*)?(?:[\x21-\x27\x2A-\x5B\x5D-\x7E]|[\x01-\x08\x14-\x1F\x11\x12\x7F]|\\(?:[\x21-\x7E]|[ \t])|\\(?:[\x01-\x08\x14-\x1F\x11\x12\x7F]|\n|\r)))*(?:(?:[ \t]*\r\n)?[ \t]+|[ \t]+(?:\r\n[ \t]+)*)?\))+(?:(?:[ \t]*\r\n)?[ \t]+|[ \t]+(?:\r\n[ \t]+)*)?|(?:[ \t]*\r\n)?[ \t]+|[ \t]+(?:\r\n[ \t]+)*)?)*)$";

        addrSpec.ToString().ShouldBe(expected);

        // https://en.wikipedia.org/wiki/Email_address

        var validEmails = new[]
        {
            "simple@example.com",
            "very.common@example.com",
            "disposable.style.email.with+symbol@example.com",
            "other.email-with-hyphen@example.com",
            "fully-qualified-domain@example.com",
            "user.name+tag+sorting@example.com",
            "x@example.com",
            "example-indeed@strange-example.com",
            "test/test@test.com",
            "admin@mailserver1",
            "example@s.example",
            "\" \"@example.org",
            "\"john..doe\"@example.org",
            "mailhost!username@example.org",
            @"""very.(),:;<>[]\"".VERY.\""very@\\ \""very\"".unusual""@strange.example.com",
            "user%example.com@example.org",
            "user-@example.org",
            "postmaster@[123.123.123.123]",
            "postmaster@[IPv6:2001:0db8:85a3:0000:0000:8a2e:0370:7334]",
        };

        var invalidEmails = new[]
        {
            "Abc.example.com",
            "A@b@c@example.com",
            @"a""b(c)d,e:f;g<h>i[j\k]l@example.com",
            @"just""not""right@example.com",
            @"this is""not\allowed@example.com",
            @"this\ still\""not\\allowed@example.com",
            // these are valid under RFC5322 grammar
            // "1234567890123456789012345678901234567890123456789012345678901234+x@example.com",
            // "i_like_underscore@but_its_not_allowed_in_this_part.example.com",
            "QA[icon]CHOCOLATE[icon]@test.com",
        };

        var regex = addrSpec.Compile();

        foreach (var email in validEmails)
        {
            regex.IsMatch(email).ShouldBeTrue();
        }

        foreach (var email in invalidEmails)
        {
            regex.IsMatch(email).ShouldBeFalse();
        }
    }
}
