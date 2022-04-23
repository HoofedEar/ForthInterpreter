using System;
using System.Collections.Generic;
using System.Linq;

namespace ForthInterpreter.Interpret;

public class StackUnderflowException : InvalidOperationException
{
    private StackUnderflowException(string stackName)
        : base($"{stackName} underflow.")
    {
    }

    public static InvalidOperationException SubstituteNewIfStackEmpty(InvalidOperationException ex, string stackName)
    {
        return ex.Message == "Stack empty." ? new StackUnderflowException(stackName) : ex;
    }
}

public class DataStack : Stack<int>
{
    public DataStack()
        : this("Stack")
    {
    }

    public DataStack(string stackName)
    {
        StackName = stackName;
    }

    private string StackName { get; }

    public int this[int index]
    {
        get
        {
            return this.FirstOrDefault(_ => index-- == 0);
        }
    }

    public new int Peek()
    {
        try
        {
            return base.Peek();
        }
        catch (InvalidOperationException ex)
        {
            throw StackUnderflowException.SubstituteNewIfStackEmpty(ex, StackName);
        }
    }

    public new int Pop()
    {
        try
        {
            return base.Pop();
        }
        catch (InvalidOperationException ex)
        {
            throw StackUnderflowException.SubstituteNewIfStackEmpty(ex, StackName);
        }
    }

    public new void Push(int item)
    {
        if (Count < 1048576)
            base.Push(item);
        else
            throw new StackOverflowException("Stack overflow.");
    }

    public void Print(int maxCount)
    {
        Console.Write("<{0}> ", Count);
        foreach (var cell in this.Take(maxCount).Reverse())
            Console.Write("{0} ", cell);
    }
}