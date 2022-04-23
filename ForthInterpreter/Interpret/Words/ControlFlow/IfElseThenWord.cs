using ForthInterpreter.DataTypes;

namespace ForthInterpreter.Interpret.Words.ControlFlow;

public class IfElseThenWord : ControlFlowWord
{
    public IfElseThenWord(Environment env)
        : base("if", env)
    {
        TrueBranchWord = new ControlFlowWord("branch", env);
        FalseBranchWord = new ControlFlowWord("branch", env);

        PrimitiveExecuteAction = ExecuteBranch;
    }

    public ControlFlowWord TrueBranchWord { get; }
    public ControlFlowWord FalseBranchWord { get; }


    protected override string SeeNodeStartDelimiter => "if";
    protected override string SeeNodeMidDelimiter => "else";
    protected override string SeeNodeEndDelimiter => "then";

    protected override string SeeNodeFrontBodyDescription => TrueBranchWord.SeeNodeDescription;
    protected override string SeeNodeRearBodyDescription => FalseBranchWord.SeeNodeDescription;

    private void ExecuteBranch(Environment env)
    {
        if (BoolType.IsTrue(env.DataStack.Pop()))
            TrueBranchWord.Execute(env);
        else
            FalseBranchWord.Execute(env);
    }
}