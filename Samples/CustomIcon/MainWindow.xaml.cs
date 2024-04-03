using Genetec.Sdk;
using Genetec.Sdk.Diagnostics;
using Genetec.Sdk.Diagnostics.Logging.Core;
using System;
using System.Collections.Generic;
using System.Data;
using System.Drawing;
using System.IO;
using System.Windows;
using System.Windows.Media.Imaging;
using Genetec.Sdk.Entities;
using Genetec.Sdk.Queries;

// ==========================================================================
// Copyright (C) 2016 by Genetec, Inc.
// All rights reserved.
// May be used only in accordance with a valid Source Code License Agreement.
// ==========================================================================
namespace CustomIcon
{
    #region Classes

    /// <summary>
    /// Interaction logic for MainWindow.xaml
    /// </summary>
    public partial class MainWindow : Window
    {
        #region Constants

        private readonly Engine m_sdkEngine = new Engine();

        #endregion

        #region Fields

        private readonly Logger m_logger;

        #endregion

        #region Constructors

        static MainWindow()
        {
            ActivateLogging();
        }

        public MainWindow()
        {
            InitializeComponent();
            m_logger = Logger.CreateInstanceLogger(this);
            // Register to important events
            m_sdkEngine.LoginManager.LoggedOff += OnEngineLoggedOff;
            m_sdkEngine.LoginManager.LoggedOn += OnEngineLoggedOn;
            m_sdkEngine.LoginManager.LogonFailed += OnEngineLogonFailed;
            m_sdkEngine.LoginManager.LogonStatusChanged += OnEngineLogonStatusChanged;

            // Logon to Sdk engine
            string server = "";
            string username = "admin";
            string password = "";
            m_sdkEngine.LoginManager.LogOn(server, username, password);
        }

        #endregion

        #region Properties

        public Entity AreaEntity
        {
            get;
            set;
        }

        public Guid CustomIconId
        {
            get;
            set;
        }

        #endregion

        #region Event Handlers

        private void OnEngineLoggedOff(object sender, LoggedOffEventArgs e)
        {
            Console.WriteLine("Sdk has logged off");
            m_logger.TraceDebug("Sdk has logged off");
        }

        private void OnEngineLoggedOn(object sender, LoggedOnEventArgs e)
        {
            Console.WriteLine(e.UserName + " has logged on to " + e.ServerName);
            m_logger.TraceDebug(e.UserName + " has logged on to " + e.ServerName);

            AreaEntity = m_sdkEngine.CreateEntity("Entity", EntityType.Area);

        }

        private void OnEngineLogonFailed(object sender, LogonFailedEventArgs e)
        {
            MessageBox.Show(e.FormattedErrorMessage);
            m_logger.TraceDebug(e.FormattedErrorMessage);
        }

        private void OnEngineLogonStatusChanged(object sender, LogonStatusChangedEventArgs e)
        {
            Console.WriteLine("Server : " + e.ServerName + ". Status changed to " + e.Status);
            m_logger.TraceDebug("Server : " + e.ServerName + ". Status changed to " + e.Status);
        }

        #endregion

        #region Public Methods

        public static void ActivateLogging()
        {
            // In this template, the logger are activated via code.
            // An alternate way is to activate them in the LogTargets.gconfig file found in the Security Center installation folder
            // Logs can be found in the Logs subfolder of that directory.

            DiagnosticServer logServer = DiagnosticServer.Instance;
            logServer.AddFileTracing(new[] { new LoggerTraces("Genetec.Sdk.Workspace*", LogSeverity.Full) });
            logServer.AddFileTracing(new[] { new LoggerTraces("WPFApplication2*", LogSeverity.Full) });
        }

        #endregion

        private void AddIconToDirectory_Click(object sender, RoutedEventArgs e)
        {
            var icon = new BitmapImage(new Uri(@"pack://application:,,,/CustomIcon;Component/Resources/Icon.png"));

            using (var memoryStream = new MemoryStream())
            {
                var pngBitmapEncoder = new PngBitmapEncoder();
                pngBitmapEncoder.Frames.Add(BitmapFrame.Create(icon));
                pngBitmapEncoder.Save(memoryStream);

                var iconId = AreaEntity.AddCustomIconToDirectory(Image.FromStream(memoryStream), true);
            }
        }

        private void GetCustomIconFromDirectory_Click(object sender, RoutedEventArgs e)
        {
            var iconId = AreaEntity.GetCustomIconId();
            var icon = AreaEntity.GetCustomIconFromDirectory(iconId, true);
        }

        private void RemoveIconFromDirectory_Click(object sender, RoutedEventArgs e)
        {
            var customIconGuid = AreaEntity.GetCustomIconId();
            
            var areaEntitiesWithCustomIcons = new List<Entity>();

            EntityConfigurationQuery query = m_sdkEngine.ReportManager.CreateReportQuery(ReportType.EntityConfiguration) as EntityConfigurationQuery;
            QueryCompletedEventArgs result = null;
            if (query != null)
            {
                query.EntityTypeFilter.Add(EntityType.Area);
                result = query.Query();
            }
            if (result != null && result.Success)
            {
                foreach (DataRow dr in result.Data.Rows)
                {
                    var area = m_sdkEngine.GetEntity((Guid)dr[0]) as Area;
                    if (area != null && area.GetCustomIconId() == customIconGuid && area.OwnerRole == Guid.Empty)
                    {
                        areaEntitiesWithCustomIcons.Add(area);
                    }
                }
            }

            AreaEntity.RemoveCustomIconFromDirectory(customIconGuid);

            foreach (var area in areaEntitiesWithCustomIcons)
            {
                area.ResetCustomIcon(false);
            }
            areaEntitiesWithCustomIcons.Clear();

        }

        private void GetCustomIcon_Click(object sender, RoutedEventArgs e)
        {
            var customIconGuid = AreaEntity.GetCustomIconId();
        }

        private void SetCustomIcon_Click(object sender, RoutedEventArgs e)
        {
            var customIconGuid = AreaEntity.GetCustomIconId();
            AreaEntity.SetCustomIcon(customIconGuid);
        }

        private void ResetCustomIcon_Click(object sender, RoutedEventArgs e)
        {
            AreaEntity.ResetCustomIcon(true);
        }

        private void CreateEntityAndSetCustomIcon_Click(object sender, RoutedEventArgs e)
        {
            var area = m_sdkEngine.CreateEntity("Entity2", EntityType.Area);
            var guid = AreaEntity.GetCustomIconId();
            area.SetCustomIcon(guid);
        }
    }

    #endregion
}

