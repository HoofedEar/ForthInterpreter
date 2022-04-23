using System;
using System.Collections.Generic;
using ForthInterpreter.DataTypes;

namespace ForthInterpreter.Interpret.Words;

public class ExitWord : Word
{
    public ExitWord(string name, bool readsFlag = false)
        : this(name, readsFlag, null)
    {
    }

    public ExitWord(string name, Action<Environment> beforeAction)
        : this(name, false, beforeAction)
    {
    }

    public ExitWord(string name, bool readsFlag, Action<Environment> beforeAction)
        : base(name, false, true, null)
    {
        ReadsFlag = readsFlag;
        BeforeAction = beforeAction;

        PrimitiveExecuteAction = DoExit;
    }

    protected override IEnumerable<string> RecognizedExitWordNames => Array.Empty<string>();

    private bool ReadsFlag { get; }

    private Action<Environment> BeforeAction { get; }

    private void DoExit(Environment env)
    {
        if (!ReadsFlag || BoolType.IsTrue(env.DataStack.Pop()))
        {
            if (BeforeAction != null)
                BeforeAction(env);

            env.ActiveExitWordName = Name;
        }
    }
}