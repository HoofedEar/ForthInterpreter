namespace ForthInterpreter.Interpret.Words;

public class DefiningWord : Word
{
    public DefiningWord(string name)
        : base(name)
    {
        ChildDefineWord = new Word("");
        ChildExecuteWord = new Word("");

        PrimitiveExecuteAction = DefineChild;
    }

    public Word ChildDefineWord { get; }
    public Word ChildExecuteWord { get; }

    public override string SeeRootDescription =>
        CleanFormat(
            $": {Name}\n{ChildDefineWord.SeeEnumerateChildNodes}\nDOES> {ChildExecuteWord.SeeEnumerateChildNodes};");

    private void DefineChild(Environment env)
    {
        ChildDefineWord.Execute(env);

        if (env.IsExitMode) return;

        if (env.LastCompiledWord is not VariableWord variableWord) return;
        var definedChildWord = new DefinedChildWord(variableWord.Name, Name, variableWord.AllocatedAddress);

        try
        {
            definedChildWord.ExecuteWords.Add(variableWord);
            definedChildWord.ExecuteWords.AddRange(ChildExecuteWord.ExecuteWords);
            variableWord.HidesSeeNodeDescription = true;

            env.Words.Remove(variableWord.Name);
            env.Words.AddOrUpdate(definedChildWord);
        }
        catch
        {
            // ignored
        }
    }
}