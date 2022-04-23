using System.Collections.Generic;
using ForthInterpreter.Interpret.Words;

namespace ForthInterpreter.Kernel;

public static class MemoryOperations
{
    private static readonly Word[] primitives =
    {
        new("@", env =>
        {
            var fromAddress = env.DataStack.Pop();
            env.DataStack.Push(env.Memory.FetchCell(fromAddress));
        }),
        new("!", env =>
        {
            int toAddress = env.DataStack.Pop(), value = env.DataStack.Pop();
            env.Memory.StoreCell(value, toAddress);
        }),
        new("+!", env =>
        {
            int address = env.DataStack.Pop(), valueToAdd = env.DataStack.Pop();
            var value = env.Memory.FetchCell(address);
            env.Memory.StoreCell(value + valueToAdd, address);
        }),
        new("c@", env =>
        {
            var fromAddress = env.DataStack.Pop();
            env.DataStack.Push(env.Memory.FetchByte(fromAddress));
        }),
        new("c!", env =>
        {
            int toAddress = env.DataStack.Pop(), value = env.DataStack.Pop();
            env.Memory.StoreByte((byte) value, toAddress);
        }),
        new("here", env => { env.DataStack.Push(env.Memory.FirstFreeAddress); }),
        new("allot", env =>
        {
            var n = env.DataStack.Pop();
            env.Memory.AllocateBytes(n);
        }),
        new("fill", env =>
        {
            int value = env.DataStack.Pop(), count = env.DataStack.Pop(), toAddress = env.DataStack.Pop();
            env.Memory.FillBytes(toAddress, count, (byte) value);
        }),
        new("cell", env => { env.DataStack.Push(sizeof(int)); }),
        new("cells", env =>
        {
            var n = env.DataStack.Pop();
            env.DataStack.Push(n * sizeof(int));
        }),
        new("chars", env =>
        {
            var n = env.DataStack.Pop();
            env.DataStack.Push(n);
        })
    };

    public static IEnumerable<Word> Primitives => primitives;
}