using ForthInterpreter.Interpret.Words;

namespace ForthInterpreter.Kernel;

public static class StackOperations
{
    public static Word[] Primitives { get; } =
    {
        new("dup", env => { env.DataStack.Push(env.DataStack.Peek()); }),
        new("?dup", env =>
        {
            if (env.DataStack.Peek() != 0)
                env.DataStack.Push(env.DataStack.Peek());
        }),
        new("drop", env => { env.DataStack.Pop(); }),
        new("swap", env =>
        {
            int n2 = env.DataStack.Pop(), n1 = env.DataStack.Pop();
            env.DataStack.Push(n2);
            env.DataStack.Push(n1);
        }),
        new("over", env => { env.DataStack.Push(env.DataStack[1]); }),
        new("nip", env =>
        {
            var n2 = env.DataStack.Pop();
            env.DataStack.Pop();
            env.DataStack.Push(n2);
        }),
        new("tuck", env => { env.DataStack.Push(env.DataStack[1]); }),
        new("rot", env =>
        {
            int n3 = env.DataStack.Pop(),
                n2 = env.DataStack.Pop(),
                n1 = env.DataStack.Pop();
            env.DataStack.Push(n2);
            env.DataStack.Push(n3);
            env.DataStack.Push(n1);
        }),
        new("-rot", env =>
        {
            int n3 = env.DataStack.Pop(),
                n2 = env.DataStack.Pop(),
                n1 = env.DataStack.Pop();
            env.DataStack.Push(n3);
            env.DataStack.Push(n1);
            env.DataStack.Push(n2);
        }),
        new("pick", env =>
        {
            var n = env.DataStack.Pop();
            env.DataStack.Push(env.DataStack[n]);
        }),
        new("2dup", env =>
        {
            env.DataStack.Push(env.DataStack[1]);
            env.DataStack.Push(env.DataStack[1]);
        }),
        new("2drop", env =>
        {
            env.DataStack.Pop();
            env.DataStack.Pop();
        }),
        new("2swap", env =>
        {
            int n4 = env.DataStack.Pop(),
                n3 = env.DataStack.Pop(),
                n2 = env.DataStack.Pop(),
                n1 = env.DataStack.Pop();
            env.DataStack.Push(n3);
            env.DataStack.Push(n4);
            env.DataStack.Push(n1);
            env.DataStack.Push(n2);
        }),
        new("2over", env =>
        {
            env.DataStack.Push(env.DataStack[3]);
            env.DataStack.Push(env.DataStack[3]);
        }),
        new("depth", env => { env.DataStack.Push(env.DataStack.Count); }),


        // Return stack operations
        new(">r", env => { env.ReturnStack.Push(env.DataStack.Pop()); }),
        new("r@", env => { env.DataStack.Push(env.ReturnStack.Peek()); }),
        new("r>", env => { env.DataStack.Push(env.ReturnStack.Pop()); }),
        new("2>r", env =>
        {
            int n2 = env.DataStack.Pop(), n1 = env.DataStack.Pop();
            env.ReturnStack.Push(n1);
            env.ReturnStack.Push(n2);
        }),
        new("2r@", env =>
        {
            env.DataStack.Push(env.ReturnStack[1]);
            env.DataStack.Push(env.ReturnStack[0]);
        }),
        new("2r>", env =>
        {
            int n2 = env.ReturnStack.Pop(), n1 = env.ReturnStack.Pop();
            env.DataStack.Push(n1);
            env.DataStack.Push(n2);
        })
    };
}