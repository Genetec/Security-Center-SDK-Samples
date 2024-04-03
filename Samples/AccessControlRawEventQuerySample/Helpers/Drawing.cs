using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

// ==========================================================================
// Copyright (C) by Genetec, Inc.
// All rights reserved.
// May be used only in accordance with a valid Source Code License Agreement.
// ==========================================================================
namespace AccessControl.Sample.RawEventQuery.Helpers
{
    internal static class Drawing
    {
        private const int MaximumHeaderWidth = 119;

        public static void Header(string title, bool isMain = false, bool isSub = false)
        {
            var startIndex = (MaximumHeaderWidth - title.Length) / 2;

            var borderColor = isMain ? ConsoleColor.Green : isSub ? ConsoleColor.Green : ConsoleColor.Cyan;
            var titleColor = isMain ? ConsoleColor.Yellow : ConsoleColor.White;

            Console.ForegroundColor = borderColor;
            Console.WriteLine(new string('-', MaximumHeaderWidth));
            WriteBlankLines(isMain ? 1 : 0);
            Console.ForegroundColor = titleColor;
            Console.WriteLine($"{new string(' ', startIndex)}{title}");
            WriteBlankLines(isMain ? 1 : 0);
            Console.ForegroundColor = borderColor;
            Console.WriteLine(new string('-', MaximumHeaderWidth));
            Console.WriteLine();
        }

        public static void MainHeader()
        {
            Console.Clear();
            Console.WriteLine();

            Header("Access Control Raw Event Query Sample © 2022", true);
        }

        public static void Write(string text, ConsoleColor color = ConsoleColor.White)
        {
            Console.ForegroundColor = color;
            Console.Write(text);
        }

        public static void Write(string text, int cursorLeft, int cursorTop, ConsoleColor color = ConsoleColor.White)
        {
            Console.ForegroundColor = color;
            Console.CursorLeft = cursorLeft;
            Console.CursorTop = cursorTop;
            Console.Write(text);
        }

        public static void WriteBlankLine() => WriteBlankLines(1);
        public static void WriteBlankLines(int count)
        {
            for (int i = 0; i < count; i++)
            {
                Console.WriteLine();
            }
        }

        public static void WriteErrorLine(string text) => WriteLine(text, ConsoleColor.Red);

        public static void WriteLine(string text, ConsoleColor color = ConsoleColor.White)
        {
            Console.ForegroundColor = color;
            Console.WriteLine(text);
        }

        public static void WriteList(IList<string> list, ConsoleColor color = ConsoleColor.White)
        {
            Console.ForegroundColor = color;

            foreach (var item in list)
            {
                WriteLine($"  {item}");
            }
        }

        public static void WriteSuccessLine(string text) => WriteLine(text, ConsoleColor.Green);
        public static void WriteWarningLine(string text) => WriteLine(text, ConsoleColor.Yellow);
    }
}
