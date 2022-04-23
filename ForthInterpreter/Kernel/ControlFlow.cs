using System.Collections.Generic;
using ForthInterpreter.Interpret;
using ForthInterpreter.Interpret.Words;
using ForthInterpreter.Interpret.Words.ControlFlow;

namespace ForthInterpreter.Kernel;

public static class ControlFlow
{
    private static readonly Word[] primitives =
    {
        new("if", true, true,
            env =>
            {
                var ifElseThenWord = new IfElseThenWord(env);
                ifElseThenWord.Compile(env);
                env.CompilingWord = ifElseThenWord.TrueBranchWord;

                env.ControlFlowStack.Push(ifElseThenWord);
                env.ControlFlowStack.Push(ifElseThenWord.TrueBranchWord);
            }),
        new("else", true, true,
            env =>
            {
                Validate.PopControlFlowStack(env, false, "branch", typeof(ControlFlowWord));
                var ifElseThenWord =
                    (IfElseThenWord) Validate.PeekControlFlowStack(env, false, "if", typeof(IfElseThenWord));

                env.CompilingWord = ifElseThenWord.FalseBranchWord;
                env.ControlFlowStack.Push(ifElseThenWord.FalseBranchWord);
            }),
        new("then", true, true,
            env =>
            {
                Validate.PopControlFlowStack(env, false, "branch", typeof(ControlFlowWord));
                Validate.PopControlFlowStack(env, false, "if", typeof(IfElseThenWord));

                env.CompilingWord = env.ControlFlowStack.Peek();
            }),


        new ExitWord("exit"),

        // 'quit' and 'abort' never caught by any word
        new ExitWord("quit", env => env.ReturnStack.Clear()),
        new ExitWord("abort", env =>
        {
            env.DataStack.Clear();
            env.ReturnStack.Clear();
        }),


        new("do", true, true,
            env =>
            {
                var doLoopWord = new DoLoopWord(env);
                doLoopWord.Compile(env);
                env.CompilingWord = doLoopWord.CycleWord;

                env.ControlFlowStack.Push(doLoopWord);
                env.ControlFlowStack.Push(doLoopWord.CycleWord);
            }),
        new("?do", true, true,
            env =>
            {
                var doLoopWord = new DoLoopWord(env)
                {
                    PerformsInitialTest = true
                };
                doLoopWord.Compile(env);
                env.CompilingWord = doLoopWord.CycleWord;

                env.ControlFlowStack.Push(doLoopWord);
                env.ControlFlowStack.Push(doLoopWord.CycleWord);
            }),
        new("loop", true, true,
            env =>
            {
                Validate.PopControlFlowStack(env, false, "cycle", typeof(ControlFlowWord));
                Validate.PopControlFlowStack(env, false, "do", typeof(DoLoopWord));

                env.CompilingWord = env.ControlFlowStack.Peek();
            }),
        new("+loop", true, true,
            env =>
            {
                Validate.PopControlFlowStack(env, false, "cycle", typeof(ControlFlowWord));
                var doLoopWord = (DoLoopWord) Validate.PopControlFlowStack(env, false, "do", typeof(DoLoopWord));
                doLoopWord.ReadsIncrement = true;

                env.CompilingWord = env.ControlFlowStack.Peek();
            }),
        new("i",
            env => { env.DataStack.Push(env.ReturnStack.Peek()); }),
        new("j",
            env => { env.DataStack.Push(env.ReturnStack[2]); }),
        new("unloop",
            env =>
            {
                env.ReturnStack.Pop();
                env.ReturnStack.Pop();
            }),
        new ExitWord("leave", env =>
        {
            env.ReturnStack.Pop();
            env.ReturnStack.Pop();
        }),
        new ExitWord("?leave", true, env =>
        {
            env.ReturnStack.Pop();
            env.ReturnStack.Pop();
        }),


        new("begin", true, true,
            env =>
            {
                var beginWord = new BeginWord(env);
                beginWord.Compile(env);
                env.CompilingWord = beginWord.CycleWord;

                env.ControlFlowStack.Push(beginWord);
                env.ControlFlowStack.Push(beginWord.CycleWord);
            }),
        new("again", true, true,
            env =>
            {
                Validate.PopControlFlowStack(env, false, "cycle", typeof(ControlFlowWord));
                Validate.PopControlFlowStack(env, false, "begin", typeof(BeginWord));

                env.CompilingWord = env.ControlFlowStack.Peek();
            }),
        new("until", true, true,
            env =>
            {
                Validate.PopControlFlowStack(env, false, "cycle", typeof(ControlFlowWord));
                var beginWord = (BeginWord) Validate.PopControlFlowStack(env, false, "begin", typeof(BeginWord));

                beginWord.TestsUntil = true;

                env.CompilingWord = env.ControlFlowStack.Peek();
            }),


        new("while", true, true,
            env =>
            {
                var oldCycleWord =
                    (ControlFlowWord) Validate.PopControlFlowStack(env, false, "cycle", typeof(ControlFlowWord));
                Validate.PopControlFlowStack(env, false, "begin", typeof(BeginWord));

                var parentWord = env.ControlFlowStack.Peek();
                var beginWhileRepeatWord = new BeginWhileRepeatWord(env);
                parentWord.ExecuteWords[^1] = beginWhileRepeatWord;

                beginWhileRepeatWord.WhileTestWord.ExecuteWords.AddRange(oldCycleWord.ExecuteWords);

                env.CompilingWord = beginWhileRepeatWord.CycleWord;

                env.ControlFlowStack.Push(beginWhileRepeatWord);
                env.ControlFlowStack.Push(beginWhileRepeatWord.CycleWord);
            }),
        new("repeat", true, true,
            env =>
            {
                Validate.PopControlFlowStack(env, false, "cycle", typeof(ControlFlowWord));
                Validate.PopControlFlowStack(env, false, "begin-while", typeof(BeginWhileRepeatWord));

                env.CompilingWord = env.ControlFlowStack.Peek();
            }),


        new("case", true, true,
            env =>
            {
                var caseWord = new CaseWord(env);
                caseWord.Compile(env);
                env.CompilingWord = caseWord.CaseBodyWord;

                env.ControlFlowStack.Push(caseWord);
                env.ControlFlowStack.Push(caseWord.CaseBodyWord);
            }),
        new("endcase", true, true,
            env =>
            {
                Validate.PopControlFlowStack(env, false, "case-body", typeof(ControlFlowWord));
                Validate.PopControlFlowStack(env, false, "case", typeof(CaseWord));

                env.CompilingWord = env.ControlFlowStack.Peek();
            }),
        new("of", true, true,
            env =>
            {
                Validate.PeekControlFlowStack(env, false, "case-body", typeof(ControlFlowWord));

                var ofWord = new OfWord(env);
                ofWord.Compile(env);
                env.CompilingWord = ofWord.OfBodyWord;

                env.ControlFlowStack.Push(ofWord);
                env.ControlFlowStack.Push(ofWord.OfBodyWord);
            }),
        new("endof", true, true,
            env =>
            {
                Validate.PopControlFlowStack(env, false, "of-body", typeof(ControlFlowWord));
                Validate.PopControlFlowStack(env, false, "of", typeof(OfWord));

                env.CompilingWord = env.ControlFlowStack.Peek();
            })
    };

    public static IEnumerable<Word> Primitives => primitives;
}