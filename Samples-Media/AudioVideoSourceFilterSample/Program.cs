
using SdkHelpers.Common;
using System;

namespace VideoSourceFilterSample
{
    class Program
    {
        [STAThread]
        static void Main(string[] args)
        {
            try
            {
                SdkAssemblyLoader.Start();
                var form = new AudioVideoSourceFilterSampleForm();
                form.ShowDialog();
            }
            finally
            {
                SdkAssemblyLoader.Stop();
            }
        }
    }
}
