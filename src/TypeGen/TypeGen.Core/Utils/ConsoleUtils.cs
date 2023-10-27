using System;

namespace TypeGen.Core.Utils;

internal class ConsoleUtils
{
    public static void WithColor(ConsoleColor color, Action action)
    {
        var oldColor = Console.ForegroundColor;
        Console.ForegroundColor = color;
        action();
        Console.ForegroundColor = oldColor;
    }
}