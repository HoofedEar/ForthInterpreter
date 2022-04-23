using System.Text.RegularExpressions;
using ForthInterpreter.Interpret;
using ForthInterpreter.LexicalScan.Tokens;

namespace ForthInterpreter.LexicalScan;

internal class ParanEndedStringTokenReader : TokenReader
{
    public ParanEndedStringTokenReader()
    {
        StartsWithNonWhitespaceRegex = new Regex(@"\A \S",
            RegexOptions.Compiled | RegexOptions.Singleline | RegexOptions.Multiline |
            RegexOptions.IgnoreCase | RegexOptions.IgnorePatternWhitespace);
    }

    protected override string TokenRegexPattern => @"\A \s? ([^)\n]*) ( \) | $ )";

    protected Regex StartsWithNonWhitespaceRegex { get; }

    public override Token ReadToken(TextBuffer textBuffer)
    {
        var match = GetMatch(textBuffer);

        if (match.Groups[2].Value == ")" && StartsWithNonWhitespaceRegex.IsMatch(textBuffer.Remainder))
            throw new InvalidWordException(textBuffer, "Comment not properly ended.", false);

        return new ParanEndedStringToken(match.Groups[1].Value, match.Groups[2].Value == ")");
    }
}