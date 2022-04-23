namespace ForthInterpreter.Interpret.Words;

public class DefinedChildWord : InstanceWord
{
    public DefinedChildWord(string name, string definingWordName, int allocatedAddress)
        : base(name, definingWordName, allocatedAddress, null)
    {
    }


    public override string SeeRootDescription =>
        $"create {Name}   \\ {DefiningWordName}\nDOES>{CleanFormat(SeeEnumerateChildNodes)};";
}