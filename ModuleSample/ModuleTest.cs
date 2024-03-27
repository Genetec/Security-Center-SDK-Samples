// ==========================================================================
// Copyright (C) 2020 by Genetec, Inc.
// All rights reserved.
// May be used only in accordance with a valid Source Code License Agreement.
// ==========================================================================

using Genetec.Sdk;
using Genetec.Sdk.Entities;
using Genetec.Sdk.Workspace;
using Genetec.Sdk.Workspace.Commands;
using Genetec.Sdk.Workspace.Components.MapObjectViewBuilder;
using Genetec.Sdk.Workspace.Pages;
using Genetec.Sdk.Workspace.Services;
using Genetec.Sdk.Workspace.Tasks;
using ModuleSample.Components;
using ModuleSample.Components.ClockWidget;
using ModuleSample.Components.CustomWidget;
using ModuleSample.ContextualAction;
using ModuleSample.Events;
using ModuleSample.Maps.MapObjects.Accidents;
using ModuleSample.Maps.Panels.Alarms;
using ModuleSample.Maps.Panels.Incidents;
using ModuleSample.Notifications;
using ModuleSample.Pages;
using ModuleSample.Pages.Configuration;
using ModuleSample.Tasks;
using System;
using System.Collections.Generic;
using System.Data;
using System.Windows.Media.Imaging;
using System.Windows.Threading;
using Module = Genetec.Sdk.Workspace.Modules.Module;

namespace ModuleSample
{

    public sealed class ModuleTest : Module
    {

        #region Public Fields

        public static readonly Guid CustomCategoryId = new Guid("{C29A5259-B51F-4812-918E-CE200B33DD1A}");

        public static readonly Guid CustomDashboardWidgetCategoryId = new Guid("{A1A1B52D-07C4-460B-99BB-3A785E87B01B}");

        #endregion Public Fields

        #region Private Fields

        private static readonly BitmapImage CustomCategoryIcon;

        private readonly AddFaceContextualAction m_addFacesContextualAction = new AddFaceContextualAction();

        private readonly LocateMeContextualAction m_caLocateMe = new LocateMeContextualAction();

        private readonly CustomEventExtender m_eventExtender = new CustomEventExtender();
        private readonly FaceContentBuilder m_faceContentBuilder = new FaceContentBuilder();
        private readonly FacesContextualActionGroup m_facesGroup = new FacesContextualActionGroup();
        private readonly List<Task> m_tasks = new List<Task>();
        // List containing the Alarm entities.
        private readonly List<Alarm> SelectedAlarmGuids = new List<Alarm>();

        private AckAlarmsMapPanelBuilder m_ackAlarmsMapPanelBuilder;
        private ClockWidgetBuilder m_clockWidgetBuilder;
        private AnalyticConfigPage m_configPage1;

        private AnalyticConfigPage2 m_configPage2;

        private ConfigToolStartTray m_configToolStartTray;
        private CustomConfigPage m_customConfigPage;
        private CustomWidgetBuilder m_customWidgetBuilder;
        private AccidentMapObjectProvider m_incidentMapObjectProvider;
        private IncidentsMapPanelBuilder m_incidentsMapPanelBuilder;

        private OverlayTileViewBuilder m_overlayTileViewer;

        private DispatcherTimer m_timer;

        #endregion Private Fields

        #region Public Constructors

        static ModuleTest()
        {
            CustomCategoryIcon = new BitmapImage(new Uri(@"pack://application:,,,/ModuleSample;Component/Resources/Category.png", UriKind.RelativeOrAbsolute));
        }

        #endregion Public Constructors

        #region Public Methods

        /// <summary>
        /// Loads the module in the workspace and register it's workspace extensions and shared components
        /// </summary>
        public override void Load()
        {
            var engine = Workspace.Sdk;
            var defaultMonitor = Workspace.DefaultMonitor;

            if (defaultMonitor != null) 
                defaultMonitor.Activated += OnDefaultMonitorActivated;

            SubscribeToSdkEvents(engine);
            SubscribeToWorkspaceEvents();

            RegisterTaskExtensions();

            // Register widget categories
            var widgetService = Workspace.Services.Get<IWidgetService>();
            widgetService?.RegisterCategory(CustomDashboardWidgetCategoryId, "Custom Category", 1500);

            if (Workspace.ApplicationType == ApplicationType.SecurityDesk)
            {
                m_configToolStartTray = new ConfigToolStartTray();
                m_configToolStartTray.Initialize(Workspace);
                Workspace.DefaultMonitor.Notifications.Add(m_configToolStartTray);
            }

            m_facesGroup.Initialize(Workspace);
            m_addFacesContextualAction.Initialize(Workspace);

            var contextualActionsService = Workspace.Services.Get<IContextualActionsService>();
            if (contextualActionsService != null)
            {
                m_caLocateMe.Initialize(Workspace);
                contextualActionsService.Register(m_caLocateMe);
                contextualActionsService.Register(m_facesGroup);
                contextualActionsService.Register(m_addFacesContextualAction);
            }

            if (Workspace.ApplicationType == ApplicationType.SecurityDesk)
            {
                var eventExtenderService = Workspace.Services.Get<IEventService>();
                if (eventExtenderService != null)
                {
                    eventExtenderService.Initialize(Workspace);
                    eventExtenderService.RegisterEventExtender(m_eventExtender);
                }

                m_faceContentBuilder.Initialize(Workspace);
                Workspace.Components.Register(m_faceContentBuilder);
            }

            m_timer = new DispatcherTimer(TimeSpan.FromSeconds(2), DispatcherPriority.Normal, OnTimerTick, Dispatcher);

            m_overlayTileViewer = new OverlayTileViewBuilder();
            m_overlayTileViewer.Initialize(Workspace);
            Workspace.Components.Register(m_overlayTileViewer);

            m_incidentMapObjectProvider = new AccidentMapObjectProvider();
            m_incidentMapObjectProvider.Initialize(Workspace);
            Workspace.Components.Register(m_incidentMapObjectProvider);

            m_ackAlarmsMapPanelBuilder = new AckAlarmsMapPanelBuilder();
            m_ackAlarmsMapPanelBuilder.Initialize(Workspace);
            Workspace.Components.Register(m_ackAlarmsMapPanelBuilder);

            m_incidentsMapPanelBuilder = new IncidentsMapPanelBuilder();
            m_incidentsMapPanelBuilder.Initialize(Workspace);
            Workspace.Components.Register(m_incidentsMapPanelBuilder);

            var service = Workspace?.Services.Get<IConfigurationService>();
            if (service != null)
            {
                m_configPage1 = new AnalyticConfigPage();
                m_configPage1.Initialize(Workspace);
                m_configPage2 = new AnalyticConfigPage2();
                m_configPage2.Initialize(Workspace);
                m_customConfigPage = new CustomConfigPage();
                m_customConfigPage.Initialize(Workspace);

                service.Register(m_configPage1);
                service.Register(m_configPage2);
                service.Register(m_customConfigPage);
            }

            m_customWidgetBuilder = new CustomWidgetBuilder();
            m_customWidgetBuilder.Initialize(Workspace);
            Workspace.Components.Register(m_customWidgetBuilder);

            m_clockWidgetBuilder = new ClockWidgetBuilder();
            m_clockWidgetBuilder.Initialize(Workspace);
            Workspace.Components.Register(m_clockWidgetBuilder);

            // Hooking on the commands. You will receive every commands the systems can do.
            // There is a bunch of commands to receive. Check them out in WorkspaceCommands class.
            Workspace.Commands.Executing += Commands_Executing;
        }

        /// <summary>
        /// Unloads the module in the workspace by unregistering it's workspace extensions and shared components
        /// </summary>
        public override void Unload()
        {
            if (Workspace != null)
            {
                m_timer.Stop();
                if (m_configToolStartTray != null)
                {
                    Workspace.DefaultMonitor.Notifications.Remove(m_configToolStartTray);
                }

                UnregisterTaskExtensions();

                UnsubscribeFromWorkspaceEvents();
                UnsubscribeFromSdkEvents(Workspace.Sdk);

                if (m_overlayTileViewer != null)
                {
                    Workspace.Components.Unregister(m_overlayTileViewer);
                    m_overlayTileViewer = null;
                }

                var contextualActionsService = Workspace.Services.Get<IContextualActionsService>();
                if (contextualActionsService != null)
                {
                    contextualActionsService.Unregister(m_caLocateMe);
                    contextualActionsService.Unregister(m_facesGroup);
                    contextualActionsService.Unregister(m_addFacesContextualAction);
                }

                if (m_incidentMapObjectProvider != null)
                {
                    Workspace.Components.Unregister(m_incidentMapObjectProvider);
                    m_incidentMapObjectProvider = null;
                }

                if (Workspace.ApplicationType == ApplicationType.SecurityDesk)
                {
                    var eventExtenderService = Workspace.Services.Get<IEventService>();
                    eventExtenderService?.UnregisterEventExtender(m_eventExtender);

                    if (m_faceContentBuilder != null)
                    {
                        Workspace.Components.Unregister(m_faceContentBuilder);
                    }
                }

                if (m_ackAlarmsMapPanelBuilder != null)
                {
                    Workspace.Components.Unregister(m_ackAlarmsMapPanelBuilder);
                    m_ackAlarmsMapPanelBuilder = null;
                }

                if (m_incidentsMapPanelBuilder != null)
                {
                    Workspace.Components.Unregister(m_incidentsMapPanelBuilder);
                    m_incidentsMapPanelBuilder = null;
                }

                var service = Workspace.Services.Get<IConfigurationService>();
                if (service != null)
                {
                    service.Unregister(m_configPage1);
                    service.Unregister(m_configPage2);
                    service.Unregister(m_customConfigPage);
                }

                if (m_customWidgetBuilder != null)
                {
                    Workspace.Components.Unregister(m_customWidgetBuilder);
                    m_customWidgetBuilder = null;
                }

                if (m_clockWidgetBuilder != null)
                {
                    Workspace.Components.Unregister(m_clockWidgetBuilder);
                    m_clockWidgetBuilder = null;
                }

                // Unhooking so that it stays clean.
                Workspace.Commands.Executing -= Commands_Executing;
            }
        }

        #endregion Public Methods

        #region Private Methods

        /// <summary>
        /// This method is called when a command is triggered.
        /// Here, you can intercept every single command.
        /// </summary>
        /// <param name="sender">The sender</param>
        /// <param name="e">The Argument of the cancel execution. It contains the command guid.</param>
        private void Commands_Executing(object sender, CommandCancelExecutionEventArgs e)
        {
            // Making sure it's the command we are expecting.
            if (e.Command.Id != WorkspaceCommands.AckAlarm) return;

            // For this command specifically, it is essential to be in AlarmMonitoringPage to be able
            // to retrieve the selected alarms. It is not possible to get the selected alarms in the reports.
            if (!(Workspace.ActiveMonitor.ActivePage is AlarmMonitoringPage alarmMonitoringPage)) return;

            // This allows you to manage the call yourself.
            // If you set this to True, you manage the command yourself.
            e.Cancel = false;

            // This is where we get the selected active alarms in the AlarmMonitoringPage.
            var alarms = alarmMonitoringPage.GetSelectedActiveAlarmsAsync().Result;

            // Check that the alarms DataTable is not null.
            if (alarms != null)
            {
                // Iteration throw the data table to retrieve the rows and select their Guid.
                // There are many more columns which are not shown here.
                foreach (DataRow row in alarms.Rows)
                {
                    // Getting the alarm Guid from the DataRow.
                    var alarmGuid = new Guid(row["AlarmGuid"].ToString());
                    // Getting the Entity from the sdk with the Guid.
                    var alarm = Workspace.Sdk.GetEntity(alarmGuid) as Alarm;
                    // Adding the Alarm to the list.
                    SelectedAlarmGuids.Add(alarm);
                }
            }
        }

        private static void OnDefaultMonitorActivated(object sender, EventArgs e)
        {
            Console.WriteLine("Default monitor activated");
        }

        private static void OnLoggedOn(object sender, LoggedOnEventArgs e)
        {
        }

        private static void OnTimerTick(object sender, EventArgs e)
        {
        }

        private void OnWorkspaceInitialized(object sender, InitializedEventArgs e)
        {
            var mapService = Workspace.Services.Get<IMapService>();

            if (mapService != null)
            {
                mapService.RegisterLayer(new LayerDescriptor(AccidentMapObject.AccidentsLayerId, AccidentMapObject.AccidentLayerName));
            }
        }
        /// <summary>
        /// Here you can register your custom tasks and task group.
        /// </summary>
        private void RegisterTaskExtensions()
        {
            // Create custom group to group our custom tasks.
            // This group will contain Tasks that override CategoryId and return a Guid matching the TaskGroup uniqueId (CustomCategoryId)
            var taskGroup = new TaskGroup(CustomCategoryId, Guid.Empty, "SDK ModuleSample", CustomCategoryIcon, 1500);
            taskGroup.Initialize(Workspace);
            m_tasks.Add(taskGroup);

            //Register task extensions
            Task task = new CreatePageTask<PageSample>();
            task.Initialize(Workspace);
            m_tasks.Add(task);

            task = new CreatePageTask<PageSdkSample>(true); // This page is singleton
            task.Initialize(Workspace);
            m_tasks.Add(task);

            // This task uses serialization to preserve page's content after closing the task
            task = new CreatePageTask<PagePersistenceSample>();
            task.Initialize(Workspace);
            m_tasks.Add(task);

            // Creates a task with browser page
            task = new CreatePageTask<WebPageSample>();
            task.Initialize(Workspace);
            m_tasks.Add(task);

            // Creates a task with no page assigned to it
            task = new NotepadTask(Workspace.Sdk);
            task.Initialize(Workspace);
            m_tasks.Add(task);

            task = new CreatePageTask<SdkControlsPage>(true);
            task.Initialize(Workspace);
            m_tasks.Add(task);

            // Register them to the workspace
            foreach (var pageExtension in m_tasks)
            {
                Workspace.Tasks.Register(pageExtension);
            }
        }

        private static void SubscribeToSdkEvents(IEngine engine)
        {
            if (engine != null)
            {
                engine.LoginManager.LoggedOn += OnLoggedOn;
            }
        }

        private void SubscribeToWorkspaceEvents()
        {
            if (Workspace != null)
            {
                Workspace.Initialized += OnWorkspaceInitialized;
            }
        }

        private void UnregisterTaskExtensions()
        {
            // Register them to the workspace
            foreach (var task in m_tasks)
            {
                Workspace.Tasks.Unregister(task);
            }

            m_tasks.Clear();
        }

        private static void UnsubscribeFromSdkEvents(IEngine engine)
        {
            if (engine != null)
            {
                engine.LoginManager.LoggedOn -= OnLoggedOn;
            }
        }

        private void UnsubscribeFromWorkspaceEvents()
        {
            if (Workspace != null)
            {
                Workspace.Initialized -= OnWorkspaceInitialized;
            }
        }

        #endregion Private Methods

    }

}