using System.ComponentModel;
using System.Runtime.CompilerServices;

namespace ArchiveTransferManagerSample.ViewModels
{
    public abstract class ViewModelBase : INotifyPropertyChanged
    {
        public event PropertyChangedEventHandler PropertyChanged;

        protected virtual void OnPropertyChanged([CallerMemberName] string propertyName = null)
            => PropertyChanged?.Invoke(this, new PropertyChangedEventArgs(propertyName));

        public void Set<T>(ref T current, T value, [CallerMemberName] string propertyName = null)
        {
            if(!value.Equals(current))
            {
                current = value;
                OnPropertyChanged(propertyName);
            }
        }
    }
}
