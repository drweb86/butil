using System;
using System.Linq;

namespace butilc;

public static class ConsoleSelector
{
    public static int SelectWithArrowKeys(string title, string[] options)
    {
        if (options.Length == 0)
            throw new ArgumentException(nameof(options));

        Console.WriteLine(title);
        var selected = 0;
        var top = Console.CursorTop;
        var width = options.Max(x => x.Length) + 2;

        while (true)
        {
            Console.SetCursorPosition(0, top);
            for (var i = 0; i < options.Length; i++)
            {
                Console.WriteLine(((i == selected) ? "> " : "  ") + options[i].PadRight(width));
            }

            var key = Console.ReadKey(true).Key;
            switch (key)
            {
                case ConsoleKey.UpArrow:
                    selected = (selected + options.Length - 1) % options.Length;
                    break;
                case ConsoleKey.DownArrow:
                    selected = (selected + 1) % options.Length;
                    break;
                case ConsoleKey.Home:
                    selected = 0;
                    break;
                case ConsoleKey.End:
                    selected = options.Length - 1;
                    break;
                case ConsoleKey.Enter:
                    Console.WriteLine();
                    return selected;
            }
        }
    }
}
