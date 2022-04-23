using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Text.RegularExpressions;

namespace ForthInterpreter.Interpret.Words;

public class Word
{
    public Word(string name, Action<Environment> primitiveExecuteAction = null)
        : this(name, false, false, primitiveExecuteAction)
    {
    }

    public Word(string name, bool isImmediate, bool isCompileOnly, Action<Environment> primitiveExecuteAction)
    {
        Name = name;
        IsImmediate = isImmediate;
        IsCompileOnly = isCompileOnly;

        PrimitiveExecuteAction = primitiveExecuteAction;
        ExecuteWords = new List<Word>();
    }

    public string Name { get; }
    public bool IsImmediate { get; set; }
    private bool IsCompileOnly { get; }

    protected Action<Environment> PrimitiveExecuteAction { get; init; }
    public List<Word> ExecuteWords { get; }

    private bool IsPrimitive => PrimitiveExecuteAction != null && ExecuteWords.Count == 0;

    protected virtual IEnumerable<string> RecognizedExitWordNames
    {
        get { return new[] {"exit"}; }
    }


    public string SeeEnumerateChildNodes
    {
        get
        {
            var sb = new StringBuilder();
            ExecuteWords.ForEach(word => sb.Append(word.SeeNodeDescription + " "));
            return sb.ToString();
        }
    }

    public virtual string SeeNodeDescription => Name;

    public virtual string SeeRootDescription
    {
        get
        {
            if (IsPrimitive)
                return $"Word {Name} is primitive";
            return CleanFormat($": {Name}\n{SeeEnumerateChildNodes};");
        }
    }

    public void Compile(Environment env)
    {
        if (env.IsCompileMode)
            env.CompilingWord.ExecuteWords.Add(this);
    }

    public void Execute(Environment env)
    {
        if (!env.IsExitMode)
        {
            if (IsPrimitive)
                PrimitiveExecuteAction(env);
            else
                ExecuteWords.ForEach(word =>
                {
                    if (!env.IsExitMode) word.Execute(env);
                });

            if (env.IsExitMode && RecognizedExitWordNames.Contains(env.ActiveExitWordName))
                env.ActiveExitWordName = null;
        }
    }

    public void CompileOrExecute(Environment env)
    {
        if (env.IsCompileMode)
        {
            if (!IsImmediate)
                Compile(env);
            else
                Execute(env);
        }
    }

    public void Interpret(Environment env)
    {
        if (env.IsCompileMode)
            CompileOrExecute(env);
        else if (!IsCompileOnly)
            Execute(env);
        else
            throw new ApplicationException("Interpreting a compile-only word.");
    }

    protected string CleanFormat(string description)
    {
        description = Regex.Replace(description, @"\n\s*\n", "\n");
        description = Regex.Replace(description, @"\t\n\s*", "\t");
        description = Regex.Replace(description, @"\n\s*;", " ;");
        description = description.Replace("\n ", "\n").Replace("\t ", "\t")
            .Replace("\n", "\n  ");

        return description;
    }
}