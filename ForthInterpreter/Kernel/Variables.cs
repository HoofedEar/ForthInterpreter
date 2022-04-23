using ForthInterpreter.Interpret;
using ForthInterpreter.Interpret.Words;

namespace ForthInterpreter.Kernel;

public static class Variables
{
    public static Word[] Primitives { get; } =
    {
        new("variable",
            env =>
            {
                var wordName = Validate.ReadMandatoryWordName(env);
                var variableAddress = env.Memory.AllocateCell();

                env.LastCompiledWord = new VariableWord(wordName, variableAddress);
                env.Words.AddOrUpdate(env.LastCompiledWord);
            }),
        new("value",
            env =>
            {
                var wordName = Validate.ReadMandatoryWordName(env);
                var initialValue = env.DataStack.Pop();
                var variableAddress = env.Memory.AllocateAndStoreCell(initialValue);

                env.LastCompiledWord = new ConstantWord(wordName, variableAddress, env);
                env.Words.AddOrUpdate(env.LastCompiledWord);
            }),
        new("to",
            env =>
            {
                var instanceWord = Validate.ReadExistingInstanceWord(env, "constant");
                var variableAddress = instanceWord.AllocatedAddress;
                var value = env.DataStack.Pop();

                env.Memory.StoreCell(value, variableAddress);
            }),
        new("addr",
            env =>
            {
                var instanceWord = Validate.ReadExistingInstanceWord(env, "constant");
                var variableAddress = instanceWord.AllocatedAddress;

                env.DataStack.Push(variableAddress);
            }),
        new("create",
            env =>
            {
                var wordName = Validate.ReadMandatoryWordName(env);
                var variableAddress = env.Memory.FirstFreeAddress;

                env.LastCompiledWord = new VariableWord(wordName, variableAddress);
                env.Words.AddOrUpdate(env.LastCompiledWord);
            })
    };
}