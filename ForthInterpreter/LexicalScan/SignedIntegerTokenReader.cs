using ForthInterpreter.LexicalScan.Tokens;

namespace ForthInterpreter.LexicalScan;

public class SignedIntegerTokenReader : TokenReader
{
    protected override string TokenRegexPattern => @"\A \s* (-?\d{1,10}) (\s|$)";

    public override Token ReadToken(TextBuffer textBuffer)
    {
        var firstMatchGroup = GetFirstMatchGroup(textBuffer);

        if (firstMatchGroup != null)
        {
            int value;
            if (int.TryParse(firstMatchGroup, out value))
                return new SignedIntegerToken(value);
            textBuffer.UndoRead();
        }

        return null;
    }
}