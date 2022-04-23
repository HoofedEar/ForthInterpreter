using System;
using System.Collections.Generic;
using ForthInterpreter.Interpret;
using ForthInterpreter.Interpret.Words;
using ForthInterpreter.IO;
using Environment = System.Environment;

namespace ForthInterpreter.Kernel;

public static class DevEnvironment
{
    private static readonly Word[] primitives =
    {
        new("words", env =>
        {
            Console.WriteLine("({0})", env.Words.Count);

            var csb = new ConsoleStringBuilder(78);
            foreach (var name in env.Words.Keys)
                csb.AppendFormat("{0}  ", name);

            Console.WriteLine(csb);
        }),
        new(".s", env => { env.DataStack.Print(20); }),
        new(".rs", env =>
        {
            Console.Write("Return stack: ");
            env.ReturnStack.Print(20);
        }),
        new(".free", env =>
        {
            Console.WriteLine(" Free memory = {0:#,##0} bytes [{1:#,##0} kb]",
                env.Memory.FreeMemory, env.Memory.FreeMemory / 1024);
        }),
        new("dump", env =>
        {
            int count = env.DataStack.Pop(), fromAddress = env.DataStack.Pop();
            env.Memory.PrintDump(fromAddress, count);
        }),
        new("see", env =>
        {
            var word = Validate.ReadExistingWord(env);
            Console.Write("\n{0}{1}", word.SeeRootDescription, word.IsImmediate ? " immediate" : "");
        }),
        new("bye", _ =>
        {
            Console.WriteLine();
            Environment.Exit(0);
        })
    };

    public static IEnumerable<Word> Primitives => primitives;
}