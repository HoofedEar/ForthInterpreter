using ForthInterpreter.LexicalScan.Tokens;

namespace ForthInterpreter.LexicalScan;

internal class LineCommentTokenReader : TokenReader
{
    protected override string TokenRegexPattern => @"\A \s? ([^\n]*) $";

    public override Token ReadToken(TextBuffer textBuffer)
    {
        return new LineCommentToken(GetFirstMatchGroup(textBuffer));
    }
}