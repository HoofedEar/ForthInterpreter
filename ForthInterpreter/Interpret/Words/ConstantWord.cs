using System;

namespace ForthInterpreter.Interpret.Words;

public class ConstantWord : InstanceWord
{
    public ConstantWord(string name, int allocatedAddress, Environment env)
        : base(name, "constant", allocatedAddress,
            e => e.DataStack.Push(e.Memory.FetchCell(allocatedAddress)))
    {
        GetValue = () =>
        {
            Execute(env);
            return env.DataStack.Pop();
        };
    }

    private Func<int> GetValue { get; }

    public override string SeeRootDescription => $"{GetValue()} Constant {Name}";
}