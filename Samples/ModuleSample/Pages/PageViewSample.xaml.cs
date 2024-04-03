// ==========================================================================
// Copyright (C) 2020 by Genetec, Inc.
// All rights reserved.
// May be used only in accordance with a valid Source Code License Agreement.
// ==========================================================================

using Genetec.Sdk.Workspace;
using Genetec.Sdk.Workspace.Modules;
using Genetec.Sdk.Workspace.Monitors;
using Genetec.Sdk.Workspace.Pages;
using Genetec.Sdk.Workspace.SharedComponents;
using System;
using System.Linq;
using System.Windows;

namespace ModuleSample.Pages
{

    /// <summary>
    /// Interaction logic for PageViewSample.xaml
    /// </summary>
    public partial class PageViewSample
    {

        #region Public Fields

        public static readonly DependencyProperty CurrentMonitorProperty =
                    DependencyProperty.Register
                    ("CurrentMonitor", typeof(string), typeof(PageViewSample),
                        new PropertyMetadata("Not evaluated yet."));

        #endregion Public Fields

        #region Private Fields

        private static readonly DependencyPropertyKey OwnerPropertyKey =
                    DependencyProperty.RegisterReadOnly
                    ("Owner", typeof(Page), typeof(PageViewSample),
                        new PropertyMetadata());

        #endregion Private Fields

        #region Public Properties

        public string CurrentMonitor
        {
            get => (string)GetValue(CurrentMonitorProperty);
            set => SetValue(CurrentMonitorProperty, value);
        }

        public Page Owner
        {
            get => (Page)GetValue(OwnerPropertyKey.DependencyProperty);
            private set => SetValue(OwnerPropertyKey, value);
        }

        #endregion Public Properties

        #region Protected Properties

        protected Workspace Workspace { get; private set; }

        #endregion Protected Properties

        #region Public Constructors

        public PageViewSample()
        {
            InitializeComponent();
        }

        #endregion Public Constructors

        #region Public Methods

        /// <summary>
        /// Initialize the view.
        /// </summary>
        /// <param name="workspace">The workspace.</param>
        /// <param name="owner">The page that owns the view.</param>
        public void Initialize(Workspace workspace, Page owner)
        {
            Workspace = workspace ?? throw new ArgumentNullException("workspace");
            Owner = owner;
        }

        #endregion Public Methods

        #region Private Methods

        private void OnButtonClick(object sender, RoutedEventArgs e)
        {
            foreach (var module in Workspace.Modules)
            {
                Console.WriteLine(module.ToString());
            }
            var alarmModule = Workspace.Modules[2];
            alarmModule.Unload();

            foreach (var monitor in Workspace.Monitors)
            {
                Console.WriteLine(monitor.ToString());
            }

            foreach (var page in Workspace.DefaultMonitor.Pages)
            {
                Console.WriteLine(page.ToString());
            }

            var sharedComponent = Workspace.DefaultMonitor.SharedComponents[SharedComponents.DashboardPane].FirstOrDefault();
            if (sharedComponent != null)
            {
                m_host.Content = sharedComponent;
                sharedComponent.Connect();
                m_host.Visibility = Visibility.Visible;
            }

            const string format = "<xml>";
            DragDrop.DoDragDrop(this, format, DragDropEffects.All);
        }

        private void OnButtonPreviewMouseLeftButtonDown(object sender, System.Windows.Input.MouseButtonEventArgs e)
        {
        }

        private void OnButtonRefreshMonitorClicked(object sender, RoutedEventArgs e)
        {
            foreach (var monitor in Workspace.Monitors)
            {
                if (monitor.Pages.Any(page => page == Owner))
                {
                    CurrentMonitor = $"{monitor} ({monitor.LogicalId})";
                    return;
                }
            }

            CurrentMonitor = "Page not found on any monitor.";
        }

        #endregion Private Methods
    }

}