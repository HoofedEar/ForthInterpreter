using System;

namespace ForthInterpreter.Interpret.Words.ControlFlow;

public class ControlFlowWord : InstanceWord
{
    // Can't be made protected, other stuff freaks out
    public ControlFlowWord(string definingWordName, Environment env)
        : this("", definingWordName, -1, null, env)
    {
    }

    public ControlFlowWord(string name, string definingWordName, int allocatedAddress,
        Action<Environment> primitiveExecuteAction, Environment env)
        : base(name, definingWordName, allocatedAddress, primitiveExecuteAction)
    {
        SeeNodeIndentLevel = env.ControlFlowStack.Count / 2;
    }


    protected int SeeNodeIndentLevel { get; }

    protected virtual string SeeNodeStartDelimiter => "";
    protected virtual string SeeNodeMidDelimiter => "";
    protected virtual string SeeNodeEndDelimiter => "";

    protected virtual string SeeNodeFrontBodyDescription => "";
    protected virtual string SeeNodeRearBodyDescription => "";

    public override string SeeNodeDescription
    {
        get
        {
            if (SeeNodeStartDelimiter == "")
                return ExecuteWords.Count > 0 ? "\t" + SeeEnumerateChildNodes : "";
            return SeeNewLine(SeeNodeStartDelimiter, SeeNodeFrontBodyDescription, true) +
                   SeeNewLine(SeeNodeMidDelimiter, SeeNodeRearBodyDescription, false) +
                   SeeNewLine(SeeNodeEndDelimiter) +
                   SeeNewLine();
        }
    }

    private string SeeNewLine(string delimiter, string bodyDescription, bool mandatory)
    {
        return bodyDescription.Trim() != "" || mandatory ? SeeNewLine(delimiter) + bodyDescription : "";
    }

    private string SeeNewLine(string delimiter)
    {
        return delimiter != "" ? SeeNewLine() + delimiter.ToUpper() : "";
    }

    private string SeeNewLine()
    {
        return "\n" + new string('\t', SeeNodeIndentLevel);
    }
}