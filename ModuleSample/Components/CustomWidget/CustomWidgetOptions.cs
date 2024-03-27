// ==========================================================================
// Copyright (C) 2019 by Genetec, Inc.
// All rights reserved.
// May be used only in accordance with a valid Source Code License Agreement.
// ==========================================================================

using GalaSoft.MvvmLight.CommandWpf;
using ModuleSample.Annotations;
using System.ComponentModel;
using System.Runtime.CompilerServices;
using System.Windows.Input;
using System.Windows.Media;

namespace ModuleSample.Components.CustomWidget
{
    public class CustomWidgetOptions : INotifyPropertyChanged
    {

        #region Private Fields

        private SolidColorBrush m_backgroundColor;
        private RelayCommand m_changeButtonColorCommand;
        private SolidColorBrush m_foregroundColor;

        #endregion Private Fields

        #region Public Events

        public event PropertyChangedEventHandler PropertyChanged;

        #endregion Public Events

        #region Public Properties

        public SolidColorBrush BackgroundColor
        {
            get => m_backgroundColor;
            set
            {
                m_backgroundColor = value;
                OnPropertyChanged();
            }
        }

        public ICommand ChangeButtonColorCommand => m_changeButtonColorCommand ?? (m_changeButtonColorCommand = new RelayCommand(ChangeButtonColor));

        public SolidColorBrush ForegroundColor
        {
            get => m_foregroundColor;
            set
            {
                m_foregroundColor = value;
                OnPropertyChanged();
            }
        }

        #endregion Public Properties

        #region Protected Methods

        [NotifyPropertyChangedInvocator]
        protected virtual void OnPropertyChanged([CallerMemberName] string propertyName = null) 
            => PropertyChanged?.Invoke(this, new PropertyChangedEventArgs(propertyName));
        
        #endregion Protected Methods

        #region Private Methods

        private void ChangeButtonColor()
        {
            var color = BackgroundColor.Color;
            BackgroundColor.Color = ForegroundColor.Color;
            ForegroundColor.Color = color;
        }

        #endregion Private Methods
    }
}