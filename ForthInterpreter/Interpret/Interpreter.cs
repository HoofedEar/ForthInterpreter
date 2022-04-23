using ForthInterpreter.Interpret.Words;
using ForthInterpreter.IO;
using ForthInterpreter.Kernel;
using ForthInterpreter.LexicalScan;

namespace ForthInterpreter.Interpret;

public class Interpreter
{
    public Interpreter()
    {
        Environment = new Environment();

        Environment.Words.AddRange(StackOperations.Primitives);
        Environment.Words.AddRange(MathOperations.Primitives);
        Environment.Words.AddRange(MemoryOperations.Primitives);
        Environment.Words.AddRange(StringOperations.Primitives);
        Environment.Words.AddRange(IoOperations.Primitives);
        Environment.Words.AddRange(Variables.Primitives);
        Environment.Words.AddRange(Compiling.Primitives);
        Environment.Words.AddRange(ControlFlow.Primitives);
        Environment.Words.AddRange(Comments.Primitives);
        Environment.Words.AddRange(DevEnvironment.Primitives);

        Interpret(KernelSourceCode);
        Environment.LastCompiledWord = null;
    }

    public Environment Environment { get; }

    private static string KernelSourceCode => FileLoader.ReadResource("ForthInterpreter.Kernel.Kernel.fth");

    public void InterpretLine(string line)
    {
        try
        {
            Environment.TextBuffer = new TextBuffer(line);

            while (!Environment.TextBuffer.EndOfBuffer)
            {
                if (isInsideMultilineComment(Environment) ||
                    TokenReader.ReadEmptyLineToken(Environment.TextBuffer) != null)
                    continue;

                var wordToken = TokenReader.ReadWordToken(Environment.TextBuffer);

                if (Environment.Words.ContainsKey(wordToken.Name))
                {
                    Environment.Words[wordToken.Name].Interpret(Environment);
                }
                else
                {
                    Environment.TextBuffer.UndoRead();

                    var signedIntegerToken = TokenReader.ReadSignedIntegerToken(Environment.TextBuffer);
                    if (signedIntegerToken != null)
                        new LiteralWord(signedIntegerToken.Value).Interpret(Environment);
                    else
                        throw new InvalidWordException(Environment.TextBuffer, false);
                }
            }

            if (Environment.ActiveExitWordName == "abort")
                throw new InvalidWordException(Environment.TextBuffer, "Aborted.");
            Environment.ActiveExitWordName = null;
        }
        catch
        {
            Environment.Reset();
            throw;
        }
    }

    private bool isInsideMultilineComment(Environment env)
    {
        if (env.IsMultilineCommentMode)
        {
            var paranEndedStringToken = TokenReader.ReadParanEndedStringToken(env.TextBuffer);
            env.IsMultilineCommentMode = !paranEndedStringToken.IsEndingInParan;
            return true;
        }

        return false;
    }

    private void Interpret(string text)
    {
        foreach (var line in FileLoader.GetTextLines(text))
            InterpretLine(line);
    }
}