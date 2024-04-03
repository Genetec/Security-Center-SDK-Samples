// ==========================================================================
// Copyright (C) 2020 by Genetec, Inc.
// All rights reserved.
// May be used only in accordance with a valid Source Code License Agreement.
// ==========================================================================

using Genetec.Sdk;
using Genetec.Sdk.EventsArgs;
using Genetec.Sdk.Scripting;
using MacroStudio.Macros;
using System;
using System.Collections.Generic;
using System.Collections.ObjectModel;
using System.Linq;
using System.Windows;
using System.Windows.Controls;

namespace MacroStudio
{
    /// <summary>
    /// Interaction logic for MainWindow.xaml
    /// </summary>
    public partial class MainWindow
    {

        #region Public Fields

        public static readonly DependencyProperty ControlButtonsProperty = DependencyProperty.Register(
            "ControlButtons", typeof(bool), typeof(MainWindow), new PropertyMetadata(default(bool)));

        public static readonly DependencyProperty IsLoggedOnProperty = DependencyProperty.Register(
            "IsLoggedOn", typeof(bool), typeof(MainWindow), new PropertyMetadata(default(bool)));

        public static readonly DependencyProperty MainButtonsProperty = DependencyProperty.Register(
            "MainButtons", typeof(bool), typeof(MainWindow), new PropertyMetadata(default(bool)));

        public static readonly DependencyProperty SelectedMacroProperty = DependencyProperty.Register(
            "SelectedMacro", typeof(string), typeof(MainWindow), new PropertyMetadata(default(string)));

        #endregion Public Fields

        #region Private Fields

        private readonly List<MacroInfo> m_macroInfos = new List<MacroInfo>();

        private readonly Engine m_sdkEngine;

        #endregion Private Fields

        #region Public Properties

        public bool ControlButtons
        {
            get => (bool)GetValue(ControlButtonsProperty);
            set => SetValue(ControlButtonsProperty, value);
        }
        public bool IsLoggedOn
        {
            get => (bool)GetValue(IsLoggedOnProperty);
            set => SetValue(IsLoggedOnProperty, value);
        }

        public ObservableCollection<string> MacroNames { get; } = new ObservableCollection<string>();

        public bool MainButtons
        {
            get => (bool)GetValue(MainButtonsProperty);
            set => SetValue(MainButtonsProperty, value);
        }

        public string SelectedMacro
        {
            get => (string)GetValue(SelectedMacroProperty);
            set => SetValue(SelectedMacroProperty, value);
        }

        #endregion Public Properties

        #region Public Constructors

        public MainWindow()
        {
            // UI related stuff
            InitializeComponent();
            DataContext = this;

            // Create the engine and it's hooks for login
            m_sdkEngine = new Engine();
            m_sdkEngine.LoginManager.LoggedOn += OnEngineLoggedOn;
            m_sdkEngine.LoginManager.LoggedOff += OnEngineLoggedOff;
            m_sdkEngine.LoginManager.LogonFailed += OnEngineLogonFailed;
            m_sdkEngine.LoginManager.RequestDirectoryCertificateValidation += OnEngineDirectoryCertificateValidation;
        }

        #endregion Public Constructors

        #region Private Methods

        /// <summary>
        /// Simple method to enable the progress bar.
        /// </summary>
        /// <param name="enable">Do we enable it?</param>
        private void EnableProgressBar(bool enable = true)
            => ExecuteOnUIThread(() => ProgressBar.IsIndeterminate = enable);

        /// <summary>
        /// Forces the execution of an <see cref="Action"/> to be executed on the UI thread.
        /// </summary>
        /// <param name="action">The <see cref="Action"/> to perform on the UI thread.</param>
        private void ExecuteOnUIThread(Action action)
            => Application.Current.Dispatcher.Invoke(action);

        /// <summary>
        /// Fetches all the macros located in Macros/Specific folder.
        /// </summary>
        private void FetchAllMacros()
        {
            var arrTypes = System.Reflection.Assembly.GetExecutingAssembly().GetTypes();
            foreach (var type in arrTypes)
            {
                if (type.IsAbstract || !type.IsSubclassOf(typeof(UserMacro)))
                    continue;

                var macroInfo = new MacroInfo(m_sdkEngine, type);
                m_macroInfos.Add(macroInfo);
                MacroNames.Add(macroInfo.Name);
            }
        }

        /// <summary>
        /// Gets the currently selected macro.
        /// </summary>
        /// <returns>The currently selected macro.</returns>
        private MacroInfo GetCurrentSelectedMacro()
        {
            var macroInfo = m_macroInfos.FirstOrDefault(x => x.Name == SelectedMacro);
            if (macroInfo == null)
                MessageBox.Show("No Selected Macro.");
            return macroInfo;
        }

        /// <summary>
        /// When the button abort is clicked, we manually abort the macro.
        /// </summary>
        /// <param name="sender">The sender</param>
        /// <param name="e">The Event.</param>
        private void OnButtonAbortClick(object sender, RoutedEventArgs e)
            => GetCurrentSelectedMacro()?.Abort();

        /// <summary>
        /// When the button log off is clicked, we log off the user.
        /// </summary>
        /// <param name="sender">The sender.</param>
        /// <param name="e">The event</param>
        private void OnButtonLogoffClick(object sender, RoutedEventArgs e)
        {
            EnableProgressBar();
            m_macroInfos.Clear();
            MacroNames.Clear();
            m_sdkEngine.LogOff();
        }

        /// <summary>
        /// When the button log on is clicked, we log on the user.
        /// </summary>
        /// <param name="sender">The sender.</param>
        /// <param name="e">The event</param>
        private async void OnButtonLogonClick(object sender, RoutedEventArgs e)
        {
            EnableProgressBar();
            await m_sdkEngine.LogOnAsync(m_directory.Text, m_userName.Text, m_password.SecurePassword);
        }

        /// <summary>
        /// When the button run is clicked, we manually run the selected macro.
        /// </summary>
        /// <remarks>You should never call run yourself, Config Tool does it in production, this is only to debug a macro.</remarks>
        /// <param name="sender">The sender.</param>
        /// <param name="e">The event</param>
        private void OnButtonRunClick(object sender, RoutedEventArgs e)
            => GetCurrentSelectedMacro()?.Run();

        /// <summary>
        /// The engine might ask for a directory certificate validation.
        /// </summary>
        /// <param name="sender">The sender.</param>
        /// <param name="e">The event</param>
        private void OnEngineDirectoryCertificateValidation(object sender, DirectoryCertificateValidationEventArgs e)
        {
            var result = MessageBox.Show(
                "The identity of the Directory server cannot be verified. \n" +
                "The certificate is not from a trusted certifying authority. \n" +
                "Do you trust this server?",
                "Secure Communication",
                MessageBoxButton.YesNoCancel,
                MessageBoxImage.Question
            );

            e.AcceptDirectory = result == MessageBoxResult.Yes;
        }

        /// <summary>
        /// Called when the engine logs off.
        /// </summary>
        /// <param name="sender">The sender.</param>
        /// <param name="e">The event</param>
        private void OnEngineLoggedOff(object sender, LoggedOffEventArgs e)
        {
            IsLoggedOn = false;
            MainButtons = false;
            EnableProgressBar(false);
        }

        /// <summary>
        /// Called when the engine logs on.
        /// </summary>
        /// <param name="sender">The sender.</param>
        /// <param name="e">The event</param>
        private void OnEngineLoggedOn(object sender, LoggedOnEventArgs e)
        {
            IsLoggedOn = true;
            MainButtons = true;
            ControlButtons = false;
            FetchAllMacros();
            EnableProgressBar(false);
        }

        /// <summary>
        /// Called when the engine logon failed.
        /// </summary>
        /// <param name="sender">The sender.</param>
        /// <param name="e">The event</param>
        private void OnEngineLogonFailed(object sender, LogonFailedEventArgs e)
        {
            IsLoggedOn = false;
            MainButtons = false;
            EnableProgressBar(false);

            MessageBox.Show(
                e.FormattedErrorMessage,
                "Unable to connect",
                MessageBoxButton.OK,
                MessageBoxImage.Error
            );
        }

        /// <summary>
        /// Shows the different buttons when selection change.
        /// </summary>
        /// <param name="sender">The sender.</param>
        /// <param name="e">The event</param>
        private void OnListBoxSelectionChanged(object sender, SelectionChangedEventArgs e)
            => ControlButtons = true;

        #endregion Private Methods

    }
}