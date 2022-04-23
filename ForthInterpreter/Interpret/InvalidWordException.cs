using System;
using System.Text.RegularExpressions;
using ForthInterpreter.LexicalScan;

namespace ForthInterpreter.Interpret;

public class InvalidWordException : ApplicationException
{
    public InvalidWordException(TextBuffer textBuffer, bool undoLastRead = true)
        : this(textBuffer, "Undefined word.", undoLastRead)
    {
    }

    public InvalidWordException(TextBuffer textBuffer, string errorDescription, bool undoLastRead = true)
        : base(BuildMessage(textBuffer, errorDescription, undoLastRead))
    {
    }

    private static string BuildMessage(TextBuffer textBuffer, string errorDescription, bool undoLastRead)
    {
        if (undoLastRead)
            textBuffer.UndoRead();

        var wordToken = TokenReader.ReadWordToken(textBuffer);

        var arrowsIndex = textBuffer.PreviousIndex + textBuffer.LastRead.Length - wordToken.Name.Length;
        var arrowsLine = Regex.Replace(textBuffer.Text.Substring(0, arrowsIndex), @"\S", " ", RegexOptions.Singleline);

        return $"{errorDescription}\n{textBuffer.Text}\n{arrowsLine + new string('^', wordToken.Name.Length)}";
    }
}