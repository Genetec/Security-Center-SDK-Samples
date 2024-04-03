// ==========================================================================
// Copyright (C) 2019 by Genetec, Inc.
// All rights reserved.
// May be used only in accordance with a valid Source Code License Agreement.
// ==========================================================================

using GalaSoft.MvvmLight.CommandWpf;
using Genetec.Sdk.Workspace.Components;
using ModuleSample.Annotations;
using System;
using System.ComponentModel;
using System.Runtime.CompilerServices;
using System.Windows;
using System.Windows.Input;
using System.Windows.Media;
using System.Windows.Threading;
using Size = System.Windows.Size;

namespace ModuleSample.Components.ClockWidget
{
    public class ClockWidget : DashboardWidget, INotifyPropertyChanged
    {
        #region Private Fields

        private CustomWidgetSettings m_customSettings;

        private ClockWidgetView m_customWidgetView;

        private bool m_showDigitalTime;

        private string m_time;

        private RelayCommand m_viewDigitalTimeCommand;

        #endregion Private Fields

        #region Public Events

        public event PropertyChangedEventHandler PropertyChanged;

        #endregion Public Events

        #region Public Properties

        /// <summary>
        /// Default background color.
        /// </summary>
        public override Color DefaultBackgroundColor => Color.FromRgb(0, 170, 255);

        /// <summary>
        /// If the widget is configured to refresh automatically using a timer. Default value is false.
        /// </summary>
        public override bool DefaultRefreshAutomatically => true;

        /// <summary>
        /// Used with RefreshAutomatically, interval to refresh the widget. Default value is 1 minute.
        /// </summary>
        public override TimeSpan DefaultRefreshInterval => TimeSpan.FromMinutes(1);

        /// <summary>
        /// Indicates if we should show the title of the widget on top of the widget view. Default value is false.
        /// </summary>
        public override bool DefaultShowTitle => true;

        /// <summary>
        /// Title of the widget to show on top of the widget view if the option DefaultShowTitle is enabled. Default value is string.Empty.
        /// </summary>
        public override string DefaultTitle => "Clock Widget";

        /// <summary>
        /// Indicates if the widget has data that can be refreshed.
        /// </summary>
        public override bool IsRefreshable => true;

        /// <summary>
        /// Indicates if the user can change the width of the widget.
        /// </summary>
        public override bool IsResizableHorizontally => false;

        /// <summary>
        /// Indicates if the user can change the height of the widget.
        /// </summary>
        public override bool IsResizableVertically => false;

        /// <summary>
        /// Maximum size (number of cells) of the widget.
        /// </summary>
        public override Size MaxWidgetSize => new Size(10, 10);

        /// <summary>
        /// Minimum size (number of cells) of the widget.
        /// </summary>
        public override Size MinWidgetSize => new Size(3, 3);

        public bool ShowDigitalTime
        {
            get => m_showDigitalTime;
            set
            {
                m_showDigitalTime = value;
                OnPropertyChanged();
            }
        }

        public string Time
        {
            get => m_time;
            set
            {
                m_time = value;
                OnPropertyChanged(nameof(Time));
            }
        }

        public ICommand ViewDigitalTimeCommand => m_viewDigitalTimeCommand ?? (m_viewDigitalTimeCommand = new RelayCommand(DigitalTimeCommand));

        /// <summary>
        /// Title of the widget's configuration panel.
        /// </summary>
        public override string WidgetName => "Custom Clock Widget";

        /// <summary>
        /// Default height/width (number of cells) of the widget.
        /// </summary>
        public override Size WidgetSize { get; set; } = new Size(8, 8);

        /// <summary>
        /// Unique id of the widget type.
        /// </summary>
        /// <remarks>
        /// It's really important that the Id matches the <see cref="DashboardWidgetBuilder.UniqueId"/>.
        /// </remarks>
        public override Guid WidgetTypeId => ClockWidgetBuilder.ClockWidgetTypeId;

        #endregion Public Properties

        #region Public Methods

        /// <summary>
        /// Creates the custom settings section on the settings panel.
        /// If you do not want custom settings for your widget, remove this method.
        /// </summary>
        public override UIElement CreateOptionsView()
        {
            m_customSettings = new CustomWidgetSettings { DataContext = this };
            return m_customSettings;
        }

        /// <summary>
        /// Creates the custom widget.
        /// </summary>
        public override UIElement CreateView()
        {
            m_customWidgetView = new ClockWidgetView { DataContext = this };
            Refresh();
            return m_customWidgetView;
        }

        public override void Deserialize(string value) => ShowDigitalTime = new ClockWidgetSerializationData(value).ShowDigitalTime;

        public override void Refresh()
        {
            Dispatcher?.BeginInvoke(DispatcherPriority.Render, (Action)(() =>
            {
                var hourRotateValue = Convert.ToDouble(DateTime.Now.Hour.ToString());
                var minuteRotateValue = Convert.ToDouble(DateTime.Now.Minute.ToString());
                var secondRotateValue = Convert.ToDouble(DateTime.Now.Second.ToString());
                hourRotateValue = (hourRotateValue + minuteRotateValue / 60) * 30;
                minuteRotateValue = (minuteRotateValue + secondRotateValue / 60) * 6;
                m_customWidgetView.MinuteRotate.Angle = minuteRotateValue;
                m_customWidgetView.HourRotate.Angle = hourRotateValue;
                Time = DateTime.Now.ToString("h:mm tt");
            }));
        }

        public override string Serialize() => new ClockWidgetSerializationData(ShowDigitalTime).ToString();

        #endregion Public Methods

        #region Protected Methods

        [NotifyPropertyChangedInvocator]
        protected virtual void OnPropertyChanged([CallerMemberName] string propertyName = null)
            => PropertyChanged?.Invoke(this, new PropertyChangedEventArgs(propertyName));

        protected override void OnWidgetEndDrag(EventArgs e) => Refresh();

        #endregion Protected Methods

        #region Private Methods

        private void DigitalTimeCommand()
        {
            ShowDigitalTime = !m_showDigitalTime;
            Refresh();
        }

        #endregion Private Methods

        #region Private Classes

        /// <summary>
        /// The only real data we have to persist in this example
        /// </summary>
        private class ClockWidgetSerializationData
        {
            #region Public Properties

            public bool ShowDigitalTime { get; private set; }

            #endregion Public Properties

            #region Public Constructors

            public ClockWidgetSerializationData(bool showDigitalTime) 
                => ShowDigitalTime = showDigitalTime;

            public ClockWidgetSerializationData(string data)
                : this(bool.Parse(data))
            {
            }

            #endregion Public Constructors

            #region Public Methods

            public override string ToString()
                => $"{ShowDigitalTime}";

            #endregion Public Methods
        }

        #endregion Private Classes
    }
}