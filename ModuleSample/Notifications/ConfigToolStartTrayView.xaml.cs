using Genetec.Sdk;
using Genetec.Sdk.Workspace;
using Genetec.Sdk.Workspace.Services;
using System;
using System.Globalization;
using System.Windows;
using System.Windows.Controls;
using System.Windows.Data;
using System.Windows.Threading;

namespace ModuleSample.Notifications
{
    #region Classes

    /// <summary>
    /// Converts a boolean value to a Visibility.
    /// </summary>
    public class BooleanToCustomVisibilityConverter : IValueConverter
    {
        #region Constants

        private readonly BooleanToVisibilityConverter m_converter = new BooleanToVisibilityConverter();

        #endregion

        #region Properties

        /// <summary>
        /// Get or Sets if we should invert the input value.
        /// </summary>
        public bool Inverted { get; set; }

        #endregion

        #region Public Methods

        /// <summary>
        /// Converts a value. 
        /// </summary>
        /// <returns>
        /// A converted value. If the method returns null, the valid null value is used.
        /// </returns>
        /// <param name="value">The value produced by the binding source.</param>
        /// <param name="targetType">The type of the binding target property.</param>
        /// <param name="parameter">The converter parameter to use.</param>
        /// <param name="culture">The culture to use in the converter.</param>
        public object Convert(object value, Type targetType, object parameter, CultureInfo culture)
        {
            bool boolValue = false;
            if (value is bool b)
            {
                boolValue = b;
            }
            return m_converter.Convert(Inverted ^ boolValue, targetType, parameter, culture);
        }

        /// <summary>
        /// Converts a value. 
        /// </summary>
        /// <returns>
        /// A converted value. If the method returns null, the valid null value is used.
        /// </returns>
        /// <param name="value">The value that is produced by the binding target.</param>
        /// <param name="targetType">The type to convert to.</param>
        /// <param name="parameter">The converter parameter to use.</param>
        /// <param name="culture">The culture to use in the converter.</param>
        public object ConvertBack(object value, Type targetType, object parameter, CultureInfo culture)
        {
            throw new NotImplementedException();
        }

        #endregion
    }

    /// <summary>
    /// Interaction logic for GuardTourTrayView.xaml
    /// </summary>
    public partial class ConfigToolStartTrayView
    {
        #region Constants

        /// <summary>
        /// Identifies the IsConfigToolRunning dependency property.
        /// </summary>
        private static readonly DependencyPropertyKey IsConfigToolRunningPropertyKey =
                    DependencyProperty.RegisterReadOnly
                    ("IsConfigToolRunning", typeof(bool), typeof(ConfigToolStartTrayView),
                        new PropertyMetadata(false, OnPropertyIsConfigToolRunningChanged));

        /// <summary>
        /// A timer that checks if an instance of ConfigTool is currently running.
        /// </summary>
        private readonly DispatcherTimer m_timer;

        #endregion

        #region Properties

        /// <summary>
        /// Gets a value indicating if a ConfigTool instance is currently running. This is a dependency property.
        /// </summary>
        public bool IsConfigToolRunning
        {
            get => (bool)GetValue(IsConfigToolRunningPropertyKey.DependencyProperty);
            private set => SetValue(IsConfigToolRunningPropertyKey, value);
        }

        /// <summary>
        /// Gets the application's workspace.
        /// </summary>
        public Workspace Workspace { get; private set; }

        #endregion

        #region Constructors

        public ConfigToolStartTrayView()
        {
            InitializeComponent();

            // Initialize the timer
            m_timer = new DispatcherTimer(new TimeSpan(0, 0, 5), DispatcherPriority.Background, OnConfigToolTimerTick, Dispatcher);
        }

        #endregion

        #region Event Handlers

        private static void OnPropertyIsConfigToolRunningChanged(DependencyObject sender, DependencyPropertyChangedEventArgs e)
        {
            if (sender is ConfigToolStartTrayView instance)
            {
                instance.OnIsConfigToolRunningChanged();
            }
        }

        /// <summary>
        /// Occurs on timer tick.
        /// </summary>
        private void OnConfigToolTimerTick(object sender, EventArgs e)
        {
            Update();
        }

        private void OnIsConfigToolRunningChanged()
        {
            // If ConfigTool is started, start the "watchdog" timer. Otherwise, stop it.
            if (IsConfigToolRunning)
                m_timer.Start();
            else
                m_timer.Stop();
        }

        /// <summary>
        /// Occurs when the user left-click on the tray.
        /// </summary>
        private void OnMouseLeftButtonDown(object sender, System.Windows.Input.MouseButtonEventArgs e)
        {
            // On double-click only
            if (e.ClickCount == 2)
            {
                IShellService service = Workspace.Services.Get<IShellService>();
                if (service != null)
                {
                    // Start (or bring to focus) the ConfigTool
                    IsConfigToolRunning = service.Start(ApplicationType.ConfigTool);
                }

                e.Handled = true;
            }
        }

        #endregion

        #region Public Methods

        /// <summary>
        /// Initialize the tray and update its status.
        /// </summary>
        /// <param name="workspace">The application's workspace.</param>
        public void Initialize(Workspace workspace)
        {
            Workspace = workspace ?? throw new ArgumentNullException("workspace");

            Update();
        }

        #endregion

        #region Private Methods

        /// <summary>
        /// Update the ConfigTool's running status.
        /// </summary>
        private void Update()
        {
            var isrunning = false;

            var service = Workspace.Services.Get<IShellService>();
            if (service != null)
            {
                // Verify if the ConfigTool is running or not
                isrunning = service.IsRunning(ApplicationType.ConfigTool);
            }

            IsConfigToolRunning = isrunning;
        }

        #endregion
    }

    #endregion
}

