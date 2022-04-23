using ForthInterpreter.LexicalScan.Tokens;

namespace ForthInterpreter.LexicalScan;

public class WordTokenReader : TokenReader
{
    protected override string TokenRegexPattern => @"\A \s* (\S+)";

    public override Token ReadToken(TextBuffer textBuffer)
    {
        var firstMatchGroup = GetFirstMatchGroup(textBuffer);
        return firstMatchGroup != null ? new WordToken(firstMatchGroup) : null;
    }
}