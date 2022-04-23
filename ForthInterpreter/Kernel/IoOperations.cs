using System;
using System.Collections.Generic;
using ForthInterpreter.DataTypes;
using ForthInterpreter.Interpret.Words;
using ForthInterpreter.LexicalScan;

namespace ForthInterpreter.Kernel;

public static class IoOperations
{
    private static readonly Word[] primitives =
    {
        new("key", env =>
        {
            var ch = Console.ReadKey(true).KeyChar;
            env.DataStack.Push(CharType.ToCell(ch));
        }),
        new("key?", env =>
        {
            var avail = Console.KeyAvailable;
            env.DataStack.Push(BoolType.Flag(avail));
        }),
        new("emit", env => { Console.Write(CharType.ToChar(env.DataStack.Pop())); }),
        new("char", env =>
        {
            var wordToken = TokenReader.ReadWordToken(env.TextBuffer);
            var ch = wordToken != null ? wordToken.Name.ToCharArray(0, 1)[0] : ' ';
            env.DataStack.Push(CharType.ToCell(ch));
        }),
        new("[char]", true, true,
            env =>
            {
                var wordToken = TokenReader.ReadWordToken(env.TextBuffer);
                var ch = wordToken != null ? wordToken.Name.ToCharArray(0, 1)[0] : ' ';
                new LiteralWord(ch).CompileOrExecute(env);
            }),
        new(".", env => { Console.Write("{0} ", env.DataStack.Pop()); }),
        new("cr", _ => { Console.WriteLine(); }),
        new("spaces", env =>
        {
            var n1 = env.DataStack.Pop();
            if (n1 > 0)
                Console.Write(new string(' ', n1));
        }),
        new("space", _ => { Console.Write(' '); }),
        new("bl", env => { env.DataStack.Push(CharType.ToCell(' ')); }),
        new("page", _ => { Console.Clear(); })
    };

    public static IEnumerable<Word> Primitives => primitives;
}