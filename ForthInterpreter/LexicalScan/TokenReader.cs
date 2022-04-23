using System.Text.RegularExpressions;
using ForthInterpreter.LexicalScan.Tokens;

namespace ForthInterpreter.LexicalScan;

public abstract class TokenReader
{
    private static readonly WordTokenReader wordTokenReader = new();
    private static readonly SignedIntegerTokenReader signedIntegerTokenReader = new();
    private static readonly QuoteEndedStringTokenReader quoteEndedStringTokenReader = new();
    private static readonly ParanEndedStringTokenReader paranEndedStringTokenReader = new();
    private static readonly LineCommentTokenReader lineCommentTokenReader = new();
    private static readonly EmptyLineTokenReader emptyLineTokenReader = new();

    protected TokenReader()
    {
        TokenRegex = new Regex(TokenRegexPattern,
            RegexOptions.Compiled | RegexOptions.Singleline | RegexOptions.Multiline |
            RegexOptions.IgnoreCase | RegexOptions.IgnorePatternWhitespace);
    }

    private Regex TokenRegex { get; }
    protected abstract string TokenRegexPattern { get; }

    protected Match GetMatch(TextBuffer textBuffer)
    {
        var match = TokenRegex.Match(textBuffer.Remainder);

        if (match.Success)
        {
            textBuffer.MoveForward(match.Value.Length);
            return match;
        }

        return null;
    }

    protected string GetFirstMatchGroup(TextBuffer textBuffer)
    {
        var match = GetMatch(textBuffer);
        return match?.Groups[1].Value;
    }

    public abstract Token ReadToken(TextBuffer textBuffer);


    public static WordToken ReadWordToken(TextBuffer textBuffer)
    {
        return (WordToken) wordTokenReader.ReadToken(textBuffer);
    }

    public static SignedIntegerToken ReadSignedIntegerToken(TextBuffer textBuffer)
    {
        return (SignedIntegerToken) signedIntegerTokenReader.ReadToken(textBuffer);
    }

    public static QuoteEndedStringToken ReadQuoteEndedStringToken(TextBuffer textBuffer)
    {
        return (QuoteEndedStringToken) quoteEndedStringTokenReader.ReadToken(textBuffer);
    }

    public static ParanEndedStringToken ReadParanEndedStringToken(TextBuffer textBuffer)
    {
        return (ParanEndedStringToken) paranEndedStringTokenReader.ReadToken(textBuffer);
    }

    public static LineCommentToken ReadLineCommentToken(TextBuffer textBuffer)
    {
        return (LineCommentToken) lineCommentTokenReader.ReadToken(textBuffer);
    }

    public static EmptyLineToken ReadEmptyLineToken(TextBuffer textBuffer)
    {
        return (EmptyLineToken) emptyLineTokenReader.ReadToken(textBuffer);
    }
}