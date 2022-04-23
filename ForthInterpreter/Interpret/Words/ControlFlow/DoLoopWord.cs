using System.Collections.Generic;

namespace ForthInterpreter.Interpret.Words.ControlFlow;

public class DoLoopWord : BasicLoopWord
{
    public DoLoopWord(Environment env)
        : base("do", env)
    {
        PerformsInitialTest = false;
        ReadsIncrement = false;
    }

    protected override IEnumerable<string> RecognizedExitWordNames
    {
        get { return new[] {"leave", "?leave"}; }
    }

    // False for DO, True for ?DO
    public bool PerformsInitialTest { get; init; }

    // False for LOOP, True for +LOOP
    public bool ReadsIncrement { get; set; }


    protected override string SeeNodeStartDelimiter => PerformsInitialTest ? "?do" : "do";
    protected override string SeeNodeEndDelimiter => ReadsIncrement ? "+loop" : "loop";

    protected override string SeeNodeFrontBodyDescription => CycleWord.SeeNodeDescription;

    protected override bool BeforeAction(Environment env)
    {
        int startLimit = env.DataStack.Pop(), endLimit = env.DataStack.Pop();

        if (!PerformsInitialTest || startLimit != endLimit)
        {
            env.ReturnStack.Push(endLimit);
            env.ReturnStack.Push(startLimit);
            return true;
        }

        return false;
    }

    protected override bool AfterEachCycleAction(Environment env)
    {
        var increment = ReadsIncrement ? env.DataStack.Pop() : 1;
        int newIndex = env.ReturnStack.Pop() + increment, endLimit = env.ReturnStack.Peek();

        env.ReturnStack.Push(newIndex);

        return increment > 0 ? newIndex < endLimit : increment >= 0 || newIndex >= endLimit;
    }

    protected override bool AfterAction(Environment env)
    {
        env.ReturnStack.Pop();
        env.ReturnStack.Pop();
        return true;
    }
}