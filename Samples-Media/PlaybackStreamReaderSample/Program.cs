
using SdkHelpers.Common;
using System;

namespace PlaybackStreamReaderSample
{
    class Program
    {
        [STAThread]
        static void Main(string[] args)
        {
            try
            {
                SdkAssemblyLoader.Start();
                var form = new PlaybackStreamReaderSampleForm();
                form.ShowDialog();
            }
            finally
            {
                SdkAssemblyLoader.Stop();
            }
        }
    }
}
