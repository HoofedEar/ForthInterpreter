using ForthInterpreter.LexicalScan.Tokens;

namespace ForthInterpreter.LexicalScan;

public class EmptyLineTokenReader : TokenReader
{
    protected override string TokenRegexPattern => @"\A (\s*) $";

    public override Token ReadToken(TextBuffer textBuffer)
    {
        var firstMatchGroup = GetFirstMatchGroup(textBuffer);
        return firstMatchGroup != null ? new EmptyLineToken() : null;
    }
}