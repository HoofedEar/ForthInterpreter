using System;
using ForthInterpreter.DataTypes;
using ForthInterpreter.Interpret.Words;
using ForthInterpreter.LexicalScan;

namespace ForthInterpreter.Kernel;

public static class StringOperations
{
    public static Word[] Primitives { get; } =
    {
        new(".\"", true, false,
            env =>
            {
                var quoteEndedStringToken = TokenReader.ReadQuoteEndedStringToken(env.TextBuffer);
                if (env.IsCompileMode)
                {
                    var charString = env.Memory.AllocateAndStoreCharString(quoteEndedStringToken.Text);
                    new PrintStringLiteralWord(charString, env).Compile(env);
                }
                else
                {
                    Console.Write(quoteEndedStringToken.Text);
                }
            }),
        new("s\"", true, false,
            env =>
            {
                var quoteEndedStringToken = TokenReader.ReadQuoteEndedStringToken(env.TextBuffer);
                var charString = env.Memory.AllocateAndStoreCharString(quoteEndedStringToken.Text);
                new CharStringLiteralWord(charString, env).Interpret(env);
            }),
        new("c\"", true, false,
            env =>
            {
                var quoteEndedStringToken = TokenReader.ReadQuoteEndedStringToken(env.TextBuffer);
                var countedString = env.Memory.AllocateAndStoreCountedString(quoteEndedStringToken.Text);
                new CountedStringLiteralWord(countedString, env).Interpret(env);
            }),
        new("count", env =>
        {
            var charString = env.Memory.ToCharString(new CountedString(env.DataStack.Pop()));
            env.DataStack.Push(charString.CharAddress);
            env.DataStack.Push(charString.Length);
        }),
        new("type", env =>
        {
            int length = env.DataStack.Pop(), charAddress = env.DataStack.Pop();
            var text = env.Memory.FetchCharString(new CharString(charAddress, length));
            Console.Write(text);
        }),
        new("accept", env =>
        {
            env.DataStack.Pop();
            var bufferAddress = env.DataStack.Pop();
            var inputText = Console.ReadLine();
            env.Memory.StoreCharString(inputText, bufferAddress);
            if (inputText != null) env.DataStack.Push(inputText.Length);
        })
    };
}