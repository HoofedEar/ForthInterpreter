using ForthInterpreter.DataTypes;

namespace ForthInterpreter.Interpret.Words.ControlFlow;

public class BeginWord : BasicLoopWord
{
    public BeginWord(Environment env)
        : base("begin", env)
    {
    }

    public bool TestsUntil { get; set; }


    protected override string SeeNodeStartDelimiter => "begin";
    protected override string SeeNodeEndDelimiter => TestsUntil ? "until" : "again";

    protected override string SeeNodeFrontBodyDescription => CycleWord.SeeNodeDescription;

    protected override bool AfterEachCycleAction(Environment env)
    {
        return !TestsUntil || BoolType.IsFalse(env.DataStack.Pop());
    }
}