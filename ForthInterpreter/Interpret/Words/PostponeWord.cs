namespace ForthInterpreter.Interpret.Words;

public class PostponeWord : InstanceWord
{
    public PostponeWord(Word postponedWord)
        : base("postpone")
    {
        PostponedWordName = postponedWord.Name;

        ExecuteWords.Add(postponedWord.IsImmediate ? postponedWord : new Word("", postponedWord.CompileOrExecute));
    }

    private string PostponedWordName { get; }

    public override string SeeNodeDescription => $"postpone {PostponedWordName}";
}