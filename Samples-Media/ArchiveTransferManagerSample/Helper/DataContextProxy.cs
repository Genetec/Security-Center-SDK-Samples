using System.Windows;
using ArchiveTransferManagerSample.Controls.TransferGroupControl;

namespace ArchiveTransferManagerSample.Helper
{
    /// <summary>
    /// This Class is to bind the DataContext of the CameraRestoreViewModel to correctly bind the StartTime and EndTime for the DatePicker Control.
    /// See: <see cref="CameraRestoreControl"/> xaml
    /// </summary>
    public class DataContextProxy : Freezable
    {
        #region Overrides of Freezable

        protected override Freezable CreateInstanceCore()
        {
            return new DataContextProxy();
        }

        #endregion

        public object DataSource
        {
            get { return (object)GetValue(DataProperty); }
            set { SetValue(DataProperty, value); }
        }

        public static readonly DependencyProperty DataProperty = DependencyProperty.Register("DataSource", typeof(object), typeof(DataContextProxy), new UIPropertyMetadata(null));
    }
}