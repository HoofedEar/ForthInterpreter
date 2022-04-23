using ForthInterpreter.LexicalScan.Tokens;

namespace ForthInterpreter.LexicalScan;

internal class QuoteEndedStringTokenReader : TokenReader
{
    protected override string TokenRegexPattern => @"\A \s? ([^""\n]*) ( "" | $ )";

    public override Token ReadToken(TextBuffer textBuffer)
    {
        return new QuoteEndedStringToken(GetFirstMatchGroup(textBuffer));
    }
}