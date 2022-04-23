namespace ForthInterpreter.Interpret.Words.ControlFlow;

public abstract class BasicLoopWord : ControlFlowWord
{
    protected BasicLoopWord(string definingWordName, Environment env)
        : base(definingWordName, env)
    {
        CycleWord = new ControlFlowWord("cycle", env);

        PrimitiveExecuteAction = ExecuteLoop;
    }

    public ControlFlowWord CycleWord { get; }

    protected virtual bool BeforeAction(Environment env)
    {
        return true;
    }

    protected virtual bool BeforeEachCycleAction(Environment env)
    {
        return true;
    }

    protected virtual bool AfterEachCycleAction(Environment env)
    {
        return true;
    }

    protected virtual bool AfterAction(Environment env)
    {
        return true;
    }

    private void ExecuteLoop(Environment env)
    {
        if (!BeforeAction(env))
            return;

        while (BeforeEachCycleAction(env))
        {
            if (env.IsExitMode) return;

            CycleWord.Execute(env);

            if (env.IsExitMode) return;

            if (!AfterEachCycleAction(env))
                break;
        }

        AfterAction(env);
    }
}