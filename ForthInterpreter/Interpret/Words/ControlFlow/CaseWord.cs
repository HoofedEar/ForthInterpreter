using System.Collections.Generic;

namespace ForthInterpreter.Interpret.Words.ControlFlow;

public class CaseWord : ControlFlowWord
{
    public CaseWord(Environment env)
        : base("case", env)
    {
        CaseBodyWord = new ControlFlowWord("case-body", env);

        PrimitiveExecuteAction = ExecuteCase;
    }

    protected override IEnumerable<string> RecognizedExitWordNames
    {
        get { return new[] {"leave-case"}; }
    }

    public ControlFlowWord CaseBodyWord { get; }


    protected override string SeeNodeStartDelimiter => "case";
    protected override string SeeNodeEndDelimiter => "endcase";

    protected override string SeeNodeFrontBodyDescription => CaseBodyWord.SeeNodeDescription;

    private void ExecuteCase(Environment env)
    {
        CaseBodyWord.Execute(env);

        if (env.IsExitMode) return;

        env.DataStack.Pop();
    }
}