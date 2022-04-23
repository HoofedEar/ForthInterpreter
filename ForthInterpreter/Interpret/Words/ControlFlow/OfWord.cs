namespace ForthInterpreter.Interpret.Words.ControlFlow;

public class OfWord : ControlFlowWord
{
    public OfWord(Environment env)
        : base("of", env)
    {
        OfBodyWord = new ControlFlowWord("of-body", env);

        PrimitiveExecuteAction = ExecuteOf;
    }

    public ControlFlowWord OfBodyWord { get; }


    protected override string SeeNodeStartDelimiter => "of";
    protected override string SeeNodeEndDelimiter => "endof";

    protected override string SeeNodeFrontBodyDescription => OfBodyWord.SeeNodeDescription;

    private void ExecuteOf(Environment env)
    {
        int ofParam = env.DataStack.Pop(), caseParam = env.DataStack.Peek();

        if (ofParam == caseParam)
        {
            env.DataStack.Pop();
            OfBodyWord.Execute(env);

            if (env.IsExitMode) return;

            env.ActiveExitWordName = "leave-case";
        }
    }
}