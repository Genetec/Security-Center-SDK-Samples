using System;
using System.Windows.Media;

namespace ArchiveTransferManagerSample.ViewModels
{
    /// <summary>
    /// This class represent the Some Security center data we can use in the app.
    /// This can be changed to fill your need
    /// </summary>
    public class SecurityCenterEntityViewModel : ViewModelBase
    {
        public Guid EntityGuid { get; set; }
        public string EntityName { get; set; }
        public ImageSource EntityIcon { get; set; }
    }
}
