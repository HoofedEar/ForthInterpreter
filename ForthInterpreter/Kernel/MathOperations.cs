﻿using System;
using ForthInterpreter.DataTypes;
using ForthInterpreter.Interpret.Words;

namespace ForthInterpreter.Kernel;

public static class MathOperations
{
    private static readonly Random random = new();

    public static Word[] Primitives { get; } =
    {
        new StackIntegerOpWord("+", (n1, n2) => n1 + n2),
        new StackIntegerOpWord("-", (n1, n2) => n1 - n2),
        new StackIntegerOpWord("*", (n1, n2) => n1 * n2),
        new StackIntegerOpWord("/", (n1, n2) => n1 / n2),
        new StackIntegerOpWord("mod", (n1, n2) => n1 % n2),
        new("/mod", env =>
        {
            int n2 = env.DataStack.Pop(), n1 = env.DataStack.Pop();
            env.DataStack.Push(n1 % n2);
            env.DataStack.Push(n1 / n2);
        }),
        new StackIntegerOpWord("negate", n1 => -1 * n1),
        new StackIntegerOpWord("abs", Math.Abs),
        new StackIntegerOpWord("min", Math.Min),
        new StackIntegerOpWord("max", Math.Max),
        new StackIntegerOpWord("1+", n1 => n1 + 1),
        new StackIntegerOpWord("1-", n1 => n1 - 1),
        new StackIntegerOpWord("2+", n1 => n1 + 2),
        new StackIntegerOpWord("2-", n1 => n1 - 2),
        new("random", env =>
        {
            var bytes = new byte[sizeof(int)];
            random.NextBytes(bytes);
            env.DataStack.Push(BitConverter.ToInt32(bytes, 0));
        }),


        // Comparisons
        new StackIntegerOpWord("<", (n1, n2) => BoolType.Flag(n1 < n2)),
        new StackIntegerOpWord(">", (n1, n2) => BoolType.Flag(n1 > n2)),
        new StackIntegerOpWord("<=", (n1, n2) => BoolType.Flag(n1 <= n2)),
        new StackIntegerOpWord(">=", (n1, n2) => BoolType.Flag(n1 >= n2)),
        new StackIntegerOpWord("=", (n1, n2) => BoolType.Flag(n1 == n2)),
        new StackIntegerOpWord("<>", (n1, n2) => BoolType.Flag(n1 != n2)),
        new StackIntegerOpWord("0=", n1 => BoolType.Flag(n1 == 0)),
        new StackIntegerOpWord("0<>", n1 => BoolType.Flag(n1 != 0)),
        new StackIntegerOpWord("0<", n1 => BoolType.Flag(n1 < 0)),
        new StackIntegerOpWord("0>", n1 => BoolType.Flag(n1 > 0)),
        new StackIntegerOpWord("within", (x, n1, n2) => BoolType.Flag(n1 <= x && x < n2)),


        // Bitwise logic
        new StackIntegerOpWord("and", (n1, n2) => n1 & n2),
        new StackIntegerOpWord("or", (n1, n2) => n1 | n2),
        new StackIntegerOpWord("xor", (n1, n2) => n1 ^ n2),
        new StackIntegerOpWord("not", n1 => ~n1),
        new StackIntegerOpWord("invert", n1 => ~n1),
        new StackIntegerOpWord("lshift", (n1, n2) => n1 << n2),
        new StackIntegerOpWord("rshift", (n1, n2) => n1 >> n2),
        new("true", env => { env.DataStack.Push(BoolType.TrueFlag); }),
        new("false", env => { env.DataStack.Push(BoolType.FalseFlag); })
    };
}