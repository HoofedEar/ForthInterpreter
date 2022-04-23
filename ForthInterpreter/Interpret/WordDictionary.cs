using System;
using System.Collections.Generic;
using ForthInterpreter.Interpret.Words;

namespace ForthInterpreter.Interpret;

public class CaseInsensitiveEqualityComparer : EqualityComparer<string>
{
    public override bool Equals(string x, string y)
    {
        return x != null && string.Equals(x, y, StringComparison.CurrentCultureIgnoreCase);
    }

    public override int GetHashCode(string obj)
    {
        return obj.ToLower().GetHashCode();
    }
}

public class WordDictionary : Dictionary<string, Word>
{
    public WordDictionary()
        : base(new CaseInsensitiveEqualityComparer())
    {
    }

    public void AddOrUpdate(Word word)
    {
        if (!ContainsKey(word.Name))
        {
            Add(word.Name, word);
        }
        else
        {
            this[word.Name] = word;
            Console.Write("redefined {0} ", word.Name);
        }
    }

    public void AddRange(IEnumerable<Word> words)
    {
        foreach (var word in words)
            Add(word.Name, word);
    }
}