using System;
using ForthInterpreter.Interpret;
using ForthInterpreter.Interpret.Words;
using ForthInterpreter.LexicalScan;

namespace ForthInterpreter.Kernel;

public static class Comments
{
    public static Word[] Primitives { get; } =
    {
        new(@"\", true, false,
            env => { TokenReader.ReadLineCommentToken(env.TextBuffer); }),
        new("(", true, false,
            env =>
            {
                var paranEndedStringToken = TokenReader.ReadParanEndedStringToken(env.TextBuffer);
                env.IsMultilineCommentMode = !paranEndedStringToken.IsEndingInParan;
            }),
        new(")", true, false,
            env => throw new InvalidWordException(env.TextBuffer, "Closed parenthesis found outside comment.")),
        new(".(", true, false,
            env => { Console.Write(TokenReader.ReadParanEndedStringToken(env.TextBuffer).Text); })
    };
}