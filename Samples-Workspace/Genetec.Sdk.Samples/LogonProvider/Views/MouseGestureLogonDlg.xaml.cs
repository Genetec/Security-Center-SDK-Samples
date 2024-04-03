// ==========================================================================
// Copyright (C) 2019 by Genetec, Inc.
// All rights reserved.
// May be used only in accordance with a valid Source Code License Agreement.
// ==========================================================================

using System;
using System.Collections.Generic;
using System.Threading;
using System.Windows;
using System.Windows.Controls;
using System.Windows.Ink;
using System.Windows.Threading;

namespace LogonProvider.Views
{

    public delegate void RaiseReadyForLogon(object sender, EventArgs args);

    /// <summary>
    /// Interaction logic for MouseGestureLogonDlg.xaml
    /// </summary>
    public partial class MouseGestureLogonDlg
    {

        #region Public Fields

        // Dependency properties
        public static readonly DependencyProperty DirectoryProperty =
                    DependencyProperty.Register
                    ("Directory", typeof(string), typeof(MouseGestureLogonDlg),
                    new PropertyMetadata(Environment.MachineName));

        public static readonly DependencyProperty MessageProperty = DependencyProperty.Register(
                    "Message", typeof(string), typeof(MouseGestureLogonDlg), new PropertyMetadata(default(string)));

        #endregion Public Fields

        #region Private Fields

        private static readonly DependencyPropertyKey UsernamePropertyKey =
                    DependencyProperty.RegisterReadOnly
                    ("Username", typeof(string), typeof(MouseGestureLogonDlg),
                    new PropertyMetadata(null, OnPropertyUsernameChanged));

        private readonly Dictionary<ApplicationGesture, string> m_supportedGestures = new Dictionary<ApplicationGesture, string>();

        #endregion Private Fields

        #region Public Events

        /// <summary>
        /// Event to trigger the logon attempt.
        /// </summary>
        public event EventHandler ReadyForLogon;

        #endregion Public Events

        #region Public Properties

        public string Directory
        {
            get => (string)GetValue(DirectoryProperty);
            set => SetValue(DirectoryProperty, value);
        }

        public string Message
        {
            get => (string)GetValue(MessageProperty);
            set => SetValue(MessageProperty, value);
        }

        public string Username
        {
            get => (string)GetValue(UsernamePropertyKey.DependencyProperty);
            private set => SetValue(UsernamePropertyKey, value);
        }

        #endregion Public Properties

        #region Public Constructors

        public MouseGestureLogonDlg()
        {
            InitializeComponent();

            m_supportedGestures.Add(ApplicationGesture.Circle, "Admin");
            m_supportedGestures.Add(ApplicationGesture.Check, "User1");
            m_supportedGestures.Add(ApplicationGesture.Square, "User2");
            m_supportedGestures.Add(ApplicationGesture.Triangle, "User3");

            m_canvas.EditingMode = InkCanvasEditingMode.InkAndGesture;
            m_canvas.SetEnabledGestures(m_supportedGestures.Keys);
        }

        #endregion Public Constructors

        #region Public Methods

        public virtual void RaiseReadyForLogon(EventArgs e) => ReadyForLogon?.Invoke(this, e);

        /// <summary>
        /// Resets the dialog to the initial state.
        /// Does not reset the message.
        /// </summary>
        public void Reset()
        {
            m_usernameLabel.Visibility = Visibility.Collapsed;
            m_canvas.Strokes.Clear();
            Username = string.Empty;
            Show();
        }

        #endregion Public Methods

        #region Private Methods

        private static void OnPropertyUsernameChanged(DependencyObject d, DependencyPropertyChangedEventArgs e)
        {
            var instance = d as MouseGestureLogonDlg;
            instance?.OnUsernameChanged();
        }

        /// <summary>
        /// Event called when the button clear is clicked.
        /// </summary>
        /// <param name="sender">The button</param>
        /// <param name="e">The eventArgs</param>
        private void OnButtonClearClick(object sender, RoutedEventArgs e) => m_canvas.Strokes.Clear();

        /// <summary>
        /// This event is called when a gesture is detected on the ink canvas.
        /// </summary>
        /// <param name="sender">The ink canvas</param>
        /// <param name="e">The event args</param>
        private void OnInkCanvasGestureDetected(object sender, InkCanvasGestureEventArgs e)
        {
            string username = null;

            var results = e.GetGestureRecognitionResults();
            foreach (var gesture in results)
            {
                if (m_supportedGestures.TryGetValue(gesture.ApplicationGesture, out username))
                {
                    Console.WriteLine(username + " -> " + gesture.RecognitionConfidence);
                    break;
                }
            }

            Username = username;
        }

        /// <summary>
        /// This event is called when the username has been changed.
        /// </summary>
        private void OnUsernameChanged()
        {
            if (!string.IsNullOrEmpty(Username))
            {
                m_usernameLabel.Visibility = Visibility.Visible;
                Message = "Connecting..";

                Action pFunc = delegate
                {
                    Thread.Sleep(1000);
                    RaiseReadyForLogon(EventArgs.Empty);
                    Hide();
                };
                Dispatcher.BeginInvoke(DispatcherPriority.Background, pFunc);
            }
            else
            {
                m_usernameLabel.Visibility = Visibility.Collapsed;
            }
        }

        #endregion Private Methods
    }

}