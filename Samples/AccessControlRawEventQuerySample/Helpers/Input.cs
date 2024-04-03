using System;
using System.Collections.Generic;
using System.Globalization;
using System.Threading;
using System.Threading.Tasks;

using DrawingHelper = AccessControl.Sample.RawEventQuery.Helpers.Drawing;

// ==========================================================================
// Copyright (C) by Genetec, Inc.
// All rights reserved.
// May be used only in accordance with a valid Source Code License Agreement.
// ==========================================================================
namespace AccessControl.Sample.RawEventQuery.Helpers
{
    internal class Input
    {
        public static ConsoleKey? AskAnyKey()
        {
            var keyInfo = Console.ReadKey(intercept: true);

            if (keyInfo.Key == ConsoleKey.Escape)
            {
                return (null);
            }

            return (keyInfo.Key);
        }

        public static async Task<int?> AskChoiceAsync(ICollection<int> accepted, CancellationToken token)
        {
            int result = -1;

            while (!token.IsCancellationRequested)
            {
                DrawingHelper.Write($"  > ");
                var input = await ReadStringAsync(false, token);

                if (input is null)
                {
                    return (null);
                }

                if (int.TryParse(input, out var converted))
                {
                    if (accepted.Contains(converted))
                    {
                        DrawingHelper.WriteSuccessLine(" (OK)");
                        result = converted;
                        break;
                    }
                }

                DrawingHelper.WriteErrorLine($" (Invalid)");
            }

            return (result);
        }

        public static async Task<DateTime?> AskDateTimeAsync(string name, CancellationToken token)
        {
            DateTime? result = null;

            while (!token.IsCancellationRequested)
            {
                DrawingHelper.Write($"  {name} ? ");
                var dateTimeStr = await ReadStringAsync(false, token);

                if (string.IsNullOrEmpty(dateTimeStr))
                {
                    DrawingHelper.WriteWarningLine("(NULL)");
                    break;
                }

                if (DateTime.TryParse(dateTimeStr, out var converted))
                {
                    DrawingHelper.WriteSuccessLine(" (OK)");
                    result = converted;
                    break;
                }

                DrawingHelper.WriteErrorLine(" (Invalid format: yyyy-MM-dd hh:MM:ss.fff)");
            }

            return (result);
        }

        public static async Task<object> AskEnumAsync(Type type, string name, CancellationToken token)
        {
            object result = null;

            while (result == null && !token.IsCancellationRequested)
            {
                DrawingHelper.Write($"  {name} ? ");
                var read = await ReadStringAsync(false, token);

                if (string.IsNullOrEmpty(read))
                {
                    return (null);
                }

                // Trying to convert string to enum
                if (!TryParseEnum(type, read, true, out result))
                {
                    // Trying to convert string to int and then int to enum
                    if (!int.TryParse(read, NumberStyles.None, CultureInfo.InvariantCulture, out var converted) ||
                        !Enum.IsDefined(type, converted))
                    {
                        DrawingHelper.WriteErrorLine(" (Invalid)");
                        continue;
                    }

                    result = Convert.ChangeType(converted, type);
                }
            }

            DrawingHelper.WriteSuccessLine(" (OK)");

            return (result);
        }

        public static Task<int?> AskIntegerAsync(string name, CancellationToken token)
            => AskIntegerAsync(name, null, token);

        public static Task<int?> AskIntegerAsync(string name, int? defaultValue, CancellationToken token)
            => AskIntegerAsync(name, int.MinValue, int.MaxValue, defaultValue, token);

        public static async Task<int?> AskIntegerAsync(string name, int min, int max, int? defaultValue, CancellationToken token)
        {
            var result = await AskLongAsync(name, min, max, defaultValue, token);
            return ((int?)result);
        }

        public static Task<long?> AskLongAsync(string name, CancellationToken token)
            => AskLongAsync(name, null, token);

        public static Task<long?> AskLongAsync(string name, long? defaultValue, CancellationToken token)
            => AskLongAsync(name, long.MinValue, long.MaxValue, defaultValue, token);

        public static async Task<long?> AskLongAsync(string name, long min, long max, long? defaultValue, CancellationToken token)
        {
            long? result = defaultValue;

            while (!token.IsCancellationRequested)
            {
                DrawingHelper.Write($"  {name} {(defaultValue != null ? $"(default: {defaultValue}) " : "")}? ");
                var input = await ReadStringAsync(false, token);

                if (string.IsNullOrEmpty(input))
                {
                    Drawing.WriteWarningLine($"({(defaultValue?.ToString() ?? "NULL")})");
                    break;
                }

                if (long.TryParse(input, NumberStyles.None, CultureInfo.InvariantCulture, out var converted))
                {
                    if (converted >= min && converted <= max)
                    {
                        Drawing.WriteSuccessLine(" (OK)");
                        result = converted;
                        break;
                    }
                }

                DrawingHelper.WriteErrorLine($" (Invalid - outside the range) [{min},{max}]");
            }

            return (result);
        }

        public static async Task<bool> AskQuestionAsync(string question, CancellationToken token)
        {
            var result = (bool?)null;
            
            DrawingHelper.Write($"  {question} (y/n) ?  ");
            while (!token.IsCancellationRequested)
            {
                var keyInfo = await Task.Run(() => Console.ReadKey(intercept: true), token);
                var key = keyInfo.Key;

                if (char.IsControl(keyInfo.KeyChar))
                {
                    if (key == ConsoleKey.Enter && result.HasValue)
                    {
                        return (result.Value);
                    }

                    if (key == ConsoleKey.Escape)
                    {
                        break;
                    }

                    if (key == ConsoleKey.Backspace && result.HasValue)
                    {
                        DrawingHelper.Write("\b \b");
                        result = null;
                    }

                    continue;
                }
                
                result = 
                key switch
                {
                    ConsoleKey.Y => true,
                    ConsoleKey.N => false,
                    _ => (bool?)null
                };

                if (result.HasValue)
                {
                    DrawingHelper.Write(result.Value ? "y" : "n");
                }
            }

            return (false);
        }

        public static Task<string> AskStringAsync(string name, CancellationToken token)
            => AskStringAsync(name, false, string.Empty, token);

        public static Task<string> AskStringAsync(string name, bool isSensitive, CancellationToken token)
            => AskStringAsync(name, isSensitive, string.Empty, token);

        public static Task<string> AskStringAsync(string name, string defaultValue, CancellationToken token)
            => AskStringAsync(name, false, defaultValue, token);

        public static async Task<string> AskStringAsync(string name, bool isSensitive, string defaultValue, CancellationToken token)
        {
            DrawingHelper.Write($"  {name} {(defaultValue == null ? "" : $"(default: '{defaultValue}') ")}? ");
            var result = await ReadStringAsync(isSensitive, token);

            if (result is null)
            {
                return (null);
            }

            if (!string.IsNullOrEmpty(result))
            {
                DrawingHelper.WriteSuccessLine(" (OK)");
                return (result);
            }

            DrawingHelper.WriteWarningLine(
                defaultValue is null ? 
                    "(NULL)" : 
                    string.Equals(defaultValue.Trim(), string.Empty) ? 
                        "(empty)" : 
                        defaultValue);

            return (defaultValue);
        }

        private static async Task<string> ReadStringAsync(bool isSensitive, CancellationToken token)
        {
            Console.ForegroundColor = isSensitive ? ConsoleColor.Magenta : ConsoleColor.Cyan;

            var result = string.Empty;

            ConsoleKey key;
            while (!token.IsCancellationRequested)
            {
                var keyInfo = await Task.Run(() => Console.ReadKey(intercept: true), token);
                key = keyInfo.Key;

                if (key == ConsoleKey.Escape)
                {
                    return (null);
                }

                if (key == ConsoleKey.Enter)
                {
                    return (result);
                }

                if (key == ConsoleKey.Backspace && result.Length > 0)
                {
                    Console.Write("\b \b");
                    result = result.Substring(0, result.Length - 1);
                }
                else if (!char.IsControl(keyInfo.KeyChar))
                {
                    Console.Write(isSensitive ? "*" : keyInfo.KeyChar.ToString());
                    result += keyInfo.KeyChar;
                }
            }

            return (result);
        }

        private static bool TryParseEnum(Type type, string value, bool ignoreCase, out object result)
        {
            result = null;

            try
            {
                result = Enum.Parse(type, value, ignoreCase);
            }
            catch
            {
                // Swallow the exception
            }

            return (result != null);
        }
    }
}
