namespace ForthInterpreter.LexicalScan.Tokens;

public class ParanEndedStringToken : Token
{
    public ParanEndedStringToken(string text, bool isEndingInParan)
    {
        Text = text;
        IsEndingInParan = isEndingInParan;
    }

    public string Text { get; }
    public bool IsEndingInParan { get; }
}