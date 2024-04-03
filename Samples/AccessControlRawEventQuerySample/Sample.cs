using AccessControl.Sample.RawEventQuery.Extensions;
using Genetec.Sdk;
using Genetec.Sdk.Entities;
using Genetec.Sdk.Queries;
using System;
using System.Collections.Generic;
using System.Data;
using System.Linq;
using System.Threading;
using System.Threading.Tasks;

using DrawingHelper = AccessControl.Sample.RawEventQuery.Helpers.Drawing;
using InputHelper = AccessControl.Sample.RawEventQuery.Helpers.Input;

// ==========================================================================
// Copyright (C) by Genetec, Inc.
// All rights reserved.
// May be used only in accordance with a valid Source Code License Agreement.
// ==========================================================================
namespace AccessControl.Sample.RawEventQuery
{
    internal partial class Sample
    {
        public async Task Run(CancellationToken token)
        {
            DrawingHelper.MainHeader();

            DrawingHelper.WriteLine("  Starting (loading SDK) ...");

            var engine = CreateEngine();

            var isConnected = await LoginAsync(engine, token);
            if (!isConnected)
            {
                return;
            }

            await MainMenuAsync(engine, token);

            DrawingHelper.WriteBlankLine();

            engine.SafeDispose();
        }

        private Task DisplayQueryEventTypes(CancellationToken _)
        {
            DrawingHelper.MainHeader();

            DrawingHelper.Header("Supported EventTypes");

            var eventTypes = (Enum.GetValues(typeof(EventType)) as EventType[]).ToList();
            DrawingHelper.WriteList(eventTypes.Select(x => $"{((int)x)} - {x}").ToList());
            DrawingHelper.WriteBlankLine();

            DrawingHelper.WriteLine("  Press any key to return to main menu");
            
            InputHelper.AskAnyKey();

            return (Task.CompletedTask);
        }

        private async Task<bool> LoginAsync(IEngine engine, CancellationToken token)
        {
            bool exit = false;

            while (!exit && !token.IsCancellationRequested)
            {
                DrawingHelper.MainHeader();

                DrawingHelper.Header("Login to server");

                try
                {
                    var server = await InputHelper.AskStringAsync("Server", "localhost", token);

                    if (string.IsNullOrEmpty(server))
                    {
                        server = "localhost";
                    }

                    var username = await InputHelper.AskStringAsync("Username", "admin", token);
                    var password = await InputHelper.AskStringAsync("Password", true, token);

                    var isConnected = await LogOnAsync(engine, server, username, password, token);

                    if (!isConnected)
                    {
                        DrawingHelper.WriteErrorLine("  Unable to connect");
                        DrawingHelper.WriteBlankLine();
                    }

                    exit = isConnected;
                }
                catch (Exception ex)
                {
                    DrawingHelper.WriteErrorLine($"  An expection occurred: {ex.Message}");
                    DrawingHelper.WriteBlankLine();
                }
                finally
                {
                    if (!exit)
                    {
                        var retry = await InputHelper.AskQuestionAsync("Retry", token);

                        if (!retry)
                        {
                            exit = true;
                        }
                    }
                }
            }

            return (engine.IsConnected);
        }

        private async Task MainMenuAsync(IEngine engine, CancellationToken token)
        {
            while (!token.IsCancellationRequested)
            {
                DrawingHelper.MainHeader();

                DrawingHelper.WriteLine("  1- Query raw events");
                DrawingHelper.WriteLine("  2- Display EventTypes");
                DrawingHelper.WriteBlankLine();
                DrawingHelper.WriteLine("  ESC- Quit", ConsoleColor.Red);

                DrawingHelper.WriteBlankLine();
                var menuChoice = await InputHelper.AskChoiceAsync(new[] { 1, 2 }, token);

                if (menuChoice is null)
                {
                    DrawingHelper.WriteLine("Exiting ...");
                    DrawingHelper.WriteBlankLine();
                    break;
                }

                switch (menuChoice)
                {
                    case 1:
                    {
                        await QueryRawEvents(engine, token);
                        break;
                    }
                    case 2:
                    {
                        await DisplayQueryEventTypes(token);
                        break;
                    }
                }
            }
        }

        private async Task QueryRawEvents(IEngine engine, CancellationToken token)
        {
            DrawingHelper.MainHeader();
            DrawingHelper.WriteBlankLine();

            while (!token.IsCancellationRequested)
            {
                DrawingHelper.Header("Query raw events");

                var insertionStartTimeUtc = await InputHelper.AskDateTimeAsync("InsertionTime start", token);
                var insertionEndTimeUtc = await InputHelper.AskDateTimeAsync("InsertionTime end", token);
                var maxCount = await InputHelper.AskIntegerAsync("MaxCount", 1, 50000, 25, token);

                var startAfterIndexes = new List<RawEventIndex>();

                var accessManagerRoles = await RetrieveAccessManagerRolesAsync(engine);
                var accessManagerDictionary = accessManagerRoles.ToDictionary(x => x.Guid, x => x);

                List<EventType> filter = new List<EventType>();
                EventType? eventType = null;

                do
                {
                    eventType = (EventType?)await InputHelper.AskEnumAsync(typeof(EventType), "Event type filter (empty = no more)", token);
                    if (eventType.HasValue)
                    {
                        filter.Add(eventType.Value);
                    }
                }
                while (eventType != null);

                // ---------------------
                //  Start after indexes
                // ---------------------

                DrawingHelper.WriteBlankLine();
                var addIndex = await InputHelper.AskQuestionAsync("Add indexes on which to start from", token);

                if (addIndex)
                {
                    int cnt = 0;
                    DrawingHelper.WriteBlankLines(2);
                    DrawingHelper.WriteList(accessManagerRoles.Select(x => $"{++cnt} - {x.Name}").ToList());
                    DrawingHelper.WriteBlankLine();

                    while (!token.IsCancellationRequested)
                    {
                        var am = await InputHelper.AskIntegerAsync($"AccessManager (empty = no more)", 1, cnt, null, token);
                        if (am == null)
                        {
                            break;
                        }

                        var position = await InputHelper.AskLongAsync("Position", 0, token);

                        startAfterIndexes.Add(new RawEventIndex() { AccessManager = accessManagerRoles[am.Value - 1].Guid, Position = position.Value });
                    }
                }

                DrawingHelper.WriteBlankLines(2);

                // -----------
                //  The Query
                // -----------

                var results =
                    await QueryRawEventsAsync(
                        engine,
                        insertionStartTimeUtc,
                        insertionEndTimeUtc,
                        maxCount ?? 25,
                        filter,
                        startAfterIndexes, token);

                DrawingHelper.Header($"Query results (Total: {results.Data.Rows.Count})");

                if (results.SubQueryErrors.Any())
                {
                    foreach (var error in results.SubQueryErrors)
                    {
                        DrawingHelper.WriteErrorLine($"  {error.ErrorDetails}");
                        DrawingHelper.WriteBlankLine();
                    }
                }
                else
                { 
                    var currentAccessManager = Guid.Empty;
                    var count = 0;

                    foreach (DataRow row in results.Data.Rows)
                    {
                        var accessManager = row.GetAccessManager();
                        if (currentAccessManager != accessManager)
                        {
                            if (currentAccessManager != Guid.Empty)
                            {
                                DrawingHelper.WriteBlankLine();
                                DrawingHelper.WriteLine($"  {count} results", ConsoleColor.Magenta);
                                DrawingHelper.WriteBlankLine();
                                count = 0;
                            }
                            DrawingHelper.Header($"{accessManagerDictionary[accessManager].Name}", false, true);
                            currentAccessManager = accessManager;
                        }

                        count++;

                        var isEvenLine = count % 2 == 0;
                        WriteEventLine(row, isEvenLine);
                    }

                    DrawingHelper.WriteBlankLine();
                    DrawingHelper.WriteLine($"  {count} results", ConsoleColor.Magenta);
                    DrawingHelper.WriteBlankLine();
                }

                DrawingHelper.WriteLine("  -- Press ESC to go back, C to clear or any key to continue --");
                DrawingHelper.WriteBlankLine();

                var key = InputHelper.AskAnyKey();
                if (!key.HasValue)
                {
                    break;
                }

                if (key.Value == ConsoleKey.C)
                {
                    DrawingHelper.MainHeader();
                    DrawingHelper.WriteBlankLine();
                }
            }
        }

        private void WriteEventLine(DataRow row, bool isEvenLine)
        {
            const int maxPositionLength = 7;
            const int maxEventTypeLength = 30;
            const int maxEventTypeCompleteStr = 36;
            const string epsilon = " ...";

            var eventTypeStr = row.GetEventType().ToString();
            eventTypeStr = eventTypeStr.Length > maxEventTypeLength ? eventTypeStr.Substring(0, maxEventTypeLength - epsilon.Length) + epsilon : eventTypeStr;
            var eventTypeValueStr = ((int)row.GetEventType()).ToString();

            var padding = Math.Max(0, maxEventTypeCompleteStr - eventTypeStr.Length - eventTypeValueStr.Length);

            var baseColor = isEvenLine ? ConsoleColor.White : ConsoleColor.DarkGray;

            DrawingHelper.Write($"  {row.GetPosition().ToString().PadRight(maxPositionLength)} ", baseColor);
            DrawingHelper.Write($"{eventTypeStr} ", baseColor);
            DrawingHelper.Write($"(");
            DrawingHelper.Write($"{eventTypeValueStr}", ConsoleColor.Green);
            DrawingHelper.Write($"){new string(' ', padding)} ");
            DrawingHelper.Write("Occurred: ", ConsoleColor.Magenta);
            DrawingHelper.Write($"{row.GetEventTimestamp().ToString("yyyy-MM-dd HH:mm:ss.fff")} ", baseColor);
            DrawingHelper.Write("Inserted: ", ConsoleColor.Magenta);
            DrawingHelper.WriteLine($"{row.GetInsertionTimestamp().ToString("yyyy-MM-dd HH:mm:ss.fff")}", baseColor);
        }
    }
}
