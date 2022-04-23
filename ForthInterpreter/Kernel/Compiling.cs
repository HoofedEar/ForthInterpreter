using System.Collections.Generic;
using ForthInterpreter.Interpret;
using ForthInterpreter.Interpret.Words;

namespace ForthInterpreter.Kernel;

public static class Compiling
{
    private static readonly Word[] primitives =
    {
        new(":",
            env =>
            {
                var wordName = Validate.ReadMandatoryWordName(env);
                env.CompilingWord = new Word(wordName);
                env.IsCompileMode = true;

                env.ControlFlowStack.Push(env.CompilingWord);
            }),
        new(";", true, true,
            env =>
            {
                Word compilingWord;
                if (env.ControlFlowStack.Count > 1)
                {
                    Validate.PopControlFlowStack(env, false);
                    compilingWord = Validate.PopControlFlowStack(env, true, null, typeof(DefiningWord));
                }
                else
                {
                    compilingWord = Validate.PopControlFlowStack(env, true);
                }

                env.Words.AddOrUpdate(compilingWord);
                env.LastCompiledWord = compilingWord;
                env.CompilingWord = null;
                env.IsCompileMode = false;
            }),
        new("[", true, false,
            env => { env.IsCompileMode = false; }),
        new("]",
            env => { env.IsCompileMode = true; }),
        new("postpone", true, false,
            env =>
            {
                var word = Validate.ReadExistingWord(env);
                new PostponeWord(word).Compile(env);
            }),
        new("immediate",
            env =>
            {
                if (env.LastCompiledWord != null)
                    env.LastCompiledWord.IsImmediate = true;
            }),
        new("literal", true, true,
            env =>
            {
                var cell = env.DataStack.Pop();
                new LiteralWord(cell).CompileOrExecute(env);
            }),


        new("does>", true, true,
            env =>
            {
                var compilingWord = Validate.PopControlFlowStack(env, true);

                var definingWord = new DefiningWord(compilingWord.Name);
                definingWord.ChildDefineWord.ExecuteWords.AddRange(compilingWord.ExecuteWords);

                env.CompilingWord = definingWord.ChildExecuteWord;

                env.ControlFlowStack.Push(definingWord);
                env.ControlFlowStack.Push(definingWord.ChildExecuteWord);
            })
    };

    public static IEnumerable<Word> Primitives => primitives;
}