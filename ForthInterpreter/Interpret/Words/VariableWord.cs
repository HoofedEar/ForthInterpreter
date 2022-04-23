﻿namespace ForthInterpreter.Interpret.Words;

public class VariableWord : InstanceWord
{
    public VariableWord(string name, int allocatedAddress)
        : base(name, "variable", allocatedAddress,
            e => e.DataStack.Push(allocatedAddress))
    {
    }


    public bool HidesSeeNodeDescription { get; set; }

    public override string SeeNodeDescription => !HidesSeeNodeDescription ? Name : "";

    public override string SeeRootDescription => $"Variable {Name}";
}