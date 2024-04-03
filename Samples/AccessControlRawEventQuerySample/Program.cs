using System.Threading;
using System.Threading.Tasks;

// ==========================================================================
// Copyright (C) by Genetec, Inc.
// All rights reserved.
// May be used only in accordance with a valid Source Code License Agreement.
// ==========================================================================
namespace AccessControl.Sample.RawEventQuery
{
    class Program
    {
        static async Task Main(string[] args)
        {
            using (var cts = new CancellationTokenSource())
            {
                var sample = new Sample();
                await sample.Run(cts.Token);

                cts.Cancel();
            }
        }
    }
}
