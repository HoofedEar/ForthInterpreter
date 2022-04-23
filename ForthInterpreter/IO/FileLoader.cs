using System;
using System.Collections.Generic;
using System.IO;
using System.Reflection;

namespace ForthInterpreter.IO;

public static class FileLoader
{
    public static string ReadResource(string resource)
    {
        return ReadResource(Assembly.GetAssembly(typeof(FileLoader)), resource);
    }

    private static string ReadResource(Assembly assembly, string resource)
    {
        try
        {
            var resourceStream = assembly.GetManifestResourceStream(resource);
            using var reader = new StreamReader(resourceStream ?? throw new InvalidOperationException());
            return reader.ReadToEnd();
        }
        catch
        {
            throw new ApplicationException($"Could not read resource '{resource}'.");
        }
    }

    public static IEnumerable<string> GetTextLines(string text)
    {
        using var reader = new StringReader(text);
        while (true)
        {
            var line = reader.ReadLine();
            if (line != null)
                yield return line;
            else
                break;
        }
    }
}