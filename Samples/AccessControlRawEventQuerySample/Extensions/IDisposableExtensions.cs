using System;

// ==========================================================================
// Copyright (C) by Genetec, Inc.
// All rights reserved.
// May be used only in accordance with a valid Source Code License Agreement.
// ==========================================================================
namespace AccessControl.Sample.RawEventQuery.Extensions
{
    internal static class IDisposableExtensions
    {
        public static void SafeDispose(this IDisposable source)
        {
            try
            {
                source?.Dispose();
            }
            catch
            {
                // Swallow exception
            }
        }
    }
}
