using ForthInterpreter.DataTypes;

namespace ForthInterpreter.Interpret.Words.ControlFlow;

public class BeginWhileRepeatWord : BasicLoopWord
{
    public BeginWhileRepeatWord(Environment env)
        : base("begin-while", env)
    {
        WhileTestWord = new ControlFlowWord("while", env);
    }

    public ControlFlowWord WhileTestWord { get; }


    protected override string SeeNodeStartDelimiter => "begin";
    protected override string SeeNodeMidDelimiter => "while";
    protected override string SeeNodeEndDelimiter => "repeat";

    protected override string SeeNodeFrontBodyDescription => WhileTestWord.SeeNodeDescription;
    protected override string SeeNodeRearBodyDescription => CycleWord.SeeNodeDescription;

    protected override bool BeforeEachCycleAction(Environment env)
    {
        WhileTestWord.Execute(env);

        if (env.IsExitMode) return false;

        return BoolType.IsTrue(env.DataStack.Pop());
    }
}