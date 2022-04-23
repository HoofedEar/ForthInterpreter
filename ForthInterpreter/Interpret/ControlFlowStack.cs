using System;
using System.Collections.Generic;
using ForthInterpreter.Interpret.Words;
using ForthInterpreter.Interpret.Words.ControlFlow;

namespace ForthInterpreter.Interpret;

public class ControlFlowStack : Stack<Word>
{
    public bool TopWordIsValid(bool countIsOne, string definingWordName, Type type)
    {
        if (Count <= 0) return false;
        var topWord = Peek();

        return Count == 1 == countIsOne &&
               (definingWordName == null ||
                topWord is ControlFlowWord word && word.DefiningWordName == definingWordName) &&
               (type == null || topWord.GetType() == type);
    }
}