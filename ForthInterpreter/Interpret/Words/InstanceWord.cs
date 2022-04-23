using System;
using System.Collections.Generic;

namespace ForthInterpreter.Interpret.Words;

public class InstanceWord : Word
{
    protected InstanceWord(string definingWordName)
        : this("", definingWordName, -1, null)
    {
    }

    protected InstanceWord(string name, string definingWordName, int allocatedAddress,
        Action<Environment> primitiveExecuteAction)
        : base(name, primitiveExecuteAction)
    {
        DefiningWordName = definingWordName;
        AllocatedAddress = allocatedAddress;
    }

    protected override IEnumerable<string> RecognizedExitWordNames
    {
        get { return new string[] { }; }
    }

    public string DefiningWordName { get; }
    public int AllocatedAddress { get; }
}