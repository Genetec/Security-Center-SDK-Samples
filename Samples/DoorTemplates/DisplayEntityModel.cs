using DoorTemplates.Annotations;
using System;
using System.ComponentModel;
using System.Runtime.CompilerServices;
using System.Windows.Media;

namespace DoorTemplates
{
    public class DisplayEntityModel : INotifyPropertyChanged
    {
        public Guid EntityGuid { get; set; }

        private string m_entityName;
        public string EntityName
        {
            get { return m_entityName; }
            set
            {
                m_entityName = value;
                OnPropertyChanged();
            }
        }

        private ImageSource m_icon;
        public ImageSource Icon
        {
            get { return m_icon; }
            set
            {
                m_icon = value;
                OnPropertyChanged();
            }
        }

        public event PropertyChangedEventHandler PropertyChanged;

        [NotifyPropertyChangedInvocator]
        protected virtual void OnPropertyChanged([CallerMemberName] string propertyName = null)
        {
            var handler = PropertyChanged;
            if (handler != null) handler(this, new PropertyChangedEventArgs(propertyName));
        }
    }
}
