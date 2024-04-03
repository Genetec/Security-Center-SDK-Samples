using Genetec.Sdk;
using Genetec.Sdk.Actions;
using Genetec.Sdk.Actions.Alarm;
using Genetec.Sdk.Actions.Schedules;
using Genetec.Sdk.Actions.Video;
using Genetec.Sdk.Actions.Video.Ptz;
using Genetec.Sdk.Entities;
using Genetec.Sdk.EventsArgs;
using System;
using Action = System.Action;
using System.Collections.Generic;
using System.Windows;
using System.Windows.Threading;

// ==========================================================================
// Copyright (C) 2016 by Genetec, Inc.
// All rights reserved.
// May be used only in accordance with a valid Source Code License Agreement.
// ==========================================================================
namespace RoutableSample
{
    #region Classes

    /// <summary>
    /// Interaction logic for MainWindow.xaml
    /// </summary>
    public partial class MainWindow : Window
    {
        #region Constants

        private readonly Engine m_sdkEngine;

        #endregion

        #region Fields

        private IAsyncResult m_loggingOnResult;

        #endregion

        #region Constructors

        public MainWindow()
        {
            InitializeComponent();

            m_sdkEngine = new Engine();
            m_sdkEngine.LoginManager.LoggedOn += OnEngineLoggedOn;
            m_sdkEngine.LoginManager.LoggedOff += OnEngineLoggedOff;
            m_sdkEngine.LoginManager.LogonFailed += OnEngineLogonFailed;
            m_sdkEngine.LoginManager.RequestDirectoryCertificateValidation += OnEngineDirectoryCertificateValidation;
        }

        #endregion

        #region Event Handlers

        /// <summary>
        /// Demonstrates the AddBookmarkAction
        /// </summary>
        private void OnButtonAddBookmarkClick(object sender, RoutedEventArgs e)
        {
            // ****** Important: this method is for demonstration purposes but not the preferred method to create a bookmark *****
            //
            // to create a bookmark use :
            //
            // m_sdkEngine.ActionManager.AddCameraBookmark(...);
            // or
            // ((Genetec.Sdk.Entities.Camera)camera_entity).AddBookmark(...);
            
            
            // Verify that the user is connected before attempting to use the SDK
            if (!m_sdkEngine.LoginManager.IsConnected)
            {
                AppendTextToOutput("You must be logged in to run do this.");
                return;
            }

            //Creates an hourly schedule that runs on the 30th minute and 10th second of every hour.
            ScheduleBase schedule = new HourlySchedule(30, 10);

            //Create a camera for demonstration purposes. 
            //This will be the camera that is bookmarked when the scheduled task runs.
            Entity camera = m_sdkEngine.CreateEntity("Camera_01", EntityType.Camera);

            //Creates the AddBookmarkAction with a blank message.
            Genetec.Sdk.Actions.Action action = CreateAddBookmarkAction(camera.Guid, Guid.Empty, String.Empty);

            //Create the ScheduledTask and associate the given schedule and action to it.
            ScheduledTask scheduledTask = CreateAndSetScheduledTask(schedule, action);

            //This is what is required to create and set a scheduled task.
            AppendScheduleInfoToOutput(scheduledTask);

            //Deletes the camera and scheduled task created for this sample
            CleanupSampleEntities(camera, scheduledTask);
        }

        /// <summary>
        /// Demonstrates the ArmZoneAction
        /// </summary>
        private void OnButtonArmZoneClick(object sender, RoutedEventArgs e)
        {
            // Verify that the user is connected before attempting to use the SDK
            if (!m_sdkEngine.LoginManager.IsConnected)
            {
                AppendTextToOutput("You must be logged in to run do this.");
                return;
            }

            //Creates an on startup schedule that runs on startup.
            ScheduleBase schedule = new OnStartupSchedule();

            //Create an intrusion area for demonstration purposes. 
            //This will be the zone that is armed by the scheduled task.
            Entity zone = m_sdkEngine.CreateEntity("Zone_01", EntityType.Zone);

            //Creates the ArmZoneAction
            Genetec.Sdk.Actions.Action action = CreateArmZoneAction(zone.Guid, Guid.Empty);

            //Creates the ScheduledTask and assosciates the given schedule and action to it.
            ScheduledTask scheduledTask = CreateAndSetScheduledTask(schedule, action);

            //This is what is required to create and set a scheduled task.
            AppendScheduleInfoToOutput(scheduledTask);

            //Deletes the camera and scheduled task created for this sample
            CleanupSampleEntities(zone, scheduledTask);
        }

        private void OnButtonConnectClick(object sender, RoutedEventArgs e)
        {
            if (m_loggingOnResult != null)
            {
                m_connect.Content = "Connect";
                m_connect.IsEnabled = true;
                m_sdkEngine.LoginManager.EndLogOn(m_loggingOnResult);
                m_loggingOnResult = null;
                return;
            }

            if (!m_sdkEngine.LoginManager.IsConnected)
            {
                m_connect.Content = "Connecting... Click to cancel";
                m_loggingOnResult = m_sdkEngine.LoginManager.BeginLogOn(m_directory.Text, m_userName.Text, passwordBox.Password);
            }
            else
            {
                m_sdkEngine.LoginManager.LogOff();
            }
        }

        /// <summary>
        /// Demonstrates the GoToPreset PTZ Action
        /// </summary>
        private void OnButtonGoToPresetClick(object sender, RoutedEventArgs e)
        {
            // Verify that the user is connected before attempting to use the SDK
            if (!m_sdkEngine.LoginManager.IsConnected)
            {
                AppendTextToOutput("You must be logged in to run do this.");
                return;
            }

            //Creates a schedule that will run on the 30th second of every minute
            ScheduleBase schedule = new ByMinuteSchedule(30);

            //Create a camera for demonstration purposes. 
            //This will be the camera to go to the given preset when the scheduled task runs
            Entity camera = m_sdkEngine.CreateEntity("Camera_01", EntityType.Camera);

            //Creates the PresetAction set to Go To Preset 1
            Genetec.Sdk.Actions.Action action = CreateGoToPresetPtzCommand(camera.Guid, Guid.Empty, 1);

            //Creates the ScheduledTask and assosciates the given schedule and action to it.
            ScheduledTask scheduledTask = CreateAndSetScheduledTask(schedule, action);

            //The scheduled task for the StartRecordingAction is now done.
            AppendScheduleInfoToOutput(scheduledTask);

            //Deletes the camera and scheduled task created for this sample
            CleanupSampleEntities(camera, scheduledTask);
        }

        /// <summary>
        /// Demonstrates the RunPattern PTZ action
        /// </summary>
        private void OnButtonRunPatternClick(object sender, RoutedEventArgs e)
        {
            // Verify that the user is connected before attempting to use the SDK
            if (!m_sdkEngine.LoginManager.IsConnected)
            {
                AppendTextToOutput("You must be logged in to run do this.");
                return;
            }

            //Creates a daily schedule that executes on the 8th hour of every day
            ScheduleBase schedule = new DailySchedule(8, 0, 0);

            //Create a camera for demonstration purposes. 
            //This will be the camera to run the given pattern when the scheduled task runs
            Entity camera = m_sdkEngine.CreateEntity("Camera_01", EntityType.Camera);

            //Creates the PatternAction set to Run Pattern 1
            Genetec.Sdk.Actions.Action action = CreateRunPatternPtzCommand(camera.Guid, Guid.Empty, 1);

            //Creates the ScheduledTask and assosciates the given schedule and action to it.
            ScheduledTask scheduledTask = CreateAndSetScheduledTask(schedule, action);

            //The scheduled task for the StartRecordingAction is now done.
            AppendScheduleInfoToOutput(scheduledTask);

            //Deletes the camera and scheduled task created for this sample
            CleanupSampleEntities(camera, scheduledTask);
        }

        /// <summary>
        /// Demonstrates the StartRecordingAction.
        /// </summary>
        private void OnButtonStartRecordingClick(object sender, RoutedEventArgs e)
        {
            // Verify that the user is connected before attempting to use the SDK
            if (!m_sdkEngine.LoginManager.IsConnected)
            {
                AppendTextToOutput("You must be logged in to run do this.");
                return;
            }

            //Creates a custom schedule that runs every 1 day 12 hours.
            ScheduleBase schedule = new CustomIntervalSchedule(1, 12, 0, 0);

            //Create a camera for demonstration purposes. 
            //This will be the camera that starts recording when the scheduled task runs
            Entity camera = m_sdkEngine.CreateEntity("Camera_01", EntityType.Camera);

            //Creates the StartRecordingAction with a recording length of TimeSpan.Zero, which means default recording time.
            Genetec.Sdk.Actions.Action action = CreateStartRecordingAction(camera.Guid, Guid.Empty, TimeSpan.Zero);

            //Creates the ScheduledTask and assosciates the given schedule and action to it.
            ScheduledTask scheduledTask = CreateAndSetScheduledTask(schedule, action);


            //This is what is required to create and set a scheduled task.
            AppendScheduleInfoToOutput(scheduledTask);

            //Deletes the camera and scheduled task created for this sample
            CleanupSampleEntities(camera, scheduledTask);
        }

        /// <summary>
        /// Demonstrates the ProtectLiveVideoAction
        /// </summary>
        private void OnButtonStartVideoProtectionClick(object sender, RoutedEventArgs e)
        {
            // Verify that the user is connected before attempting to use the SDK
            if (!m_sdkEngine.LoginManager.IsConnected)
            {
                AppendTextToOutput("You must be logged in to run do this.");
                return;
            }

            //Creates a weekly schedule that runs on the 12th hour every Monday, Wednesday, and Friday.
            var daysOfWeek = new HashSet<DayOfWeek>() { DayOfWeek.Monday, DayOfWeek.Wednesday, DayOfWeek.Friday };
            ScheduleBase schedule = new WeeklySchedule(daysOfWeek, 12, 0, 0);

            //Create a camera for demonstration purposes. 
            //This will be the camera that video protection is enabled on using the scheduled task.
            Entity camera = m_sdkEngine.CreateEntity("Camera_01", EntityType.Camera);

            //Creates the ProtectLiveVideoAction that protects video recorded in the next hour for seven days from the time the scheduled task executes.
            Genetec.Sdk.Actions.Action action = CreateProtectLiveVideoAction(camera.Guid, Guid.Empty, new TimeSpan(1, 0, 0), new TimeSpan(7, 0, 0, 0));

            //Creates the ScheduledTask and assosciates the given schedule and action to it.
            ScheduledTask scheduledTask = CreateAndSetScheduledTask(schedule, action);

            //This is what is required to create and set a scheduled task.
            AppendScheduleInfoToOutput(scheduledTask);

            //Deletes the camera and scheduled task created for this sample
            CleanupSampleEntities(camera, scheduledTask);
        }

        private void OnButtonTriggerAlarmClick(object sender, RoutedEventArgs e)
        {
            // Verify that the user is connected before attempting to use the SDK
            if (!m_sdkEngine.LoginManager.IsConnected)
            {
                AppendTextToOutput("You must be logged in to do this.");
                return;
            }

            //Creates a one time schedule that runs at this time tomorrow.
            ScheduleBase schedule = new OneTimeSchedule(DateTime.Now.AddDays(1));

            //Create an alarm for demonstration purposes. 
            //This will be the alarm that is triggered by the scheduled task.
            Entity alarm = m_sdkEngine.CreateEntity("Alarm_01", EntityType.Alarm);

            //Creates the StartRecordingAction with a recording length of TimeSpan.Zero, which means default recording time.
            Genetec.Sdk.Actions.Action action = CreateTriggerAlarmAction(alarm.Guid, Guid.Empty);

            //Creates the ScheduledTask and assosciates the given schedule and action to it.
            ScheduledTask scheduledTask = CreateAndSetScheduledTask(schedule, action);

            //This is what is required to create and set a scheduled task.
            AppendScheduleInfoToOutput(scheduledTask);

            //Deletes the camera and scheduled task created for this sample
            CleanupSampleEntities(alarm, scheduledTask);
        }

        private void OnEngineDirectoryCertificateValidation(object sender, DirectoryCertificateValidationEventArgs e)
        {
            MessageBoxResult result = MessageBox.Show("The identity of the Directory server cannot be verified. \n" +
                                                      "The certificate is not from a trusted certifying authority. \n" +
                                                      "Do you trust this server?", "Secure Communication", MessageBoxButton.YesNoCancel, MessageBoxImage.Question);
            if (result == MessageBoxResult.Yes)
            {
                e.AcceptDirectory = true;
            }
        }

        private void OnEngineLoggedOff(object sender, LoggedOffEventArgs e)
        {
            Dispatcher.BeginInvoke(DispatcherPriority.Normal, new Action(() =>
            {
                m_loggingOnResult = null;
                m_connect.Content = "Connect";
                m_connect.IsEnabled = true;
                EnableButtonForOptions(false);
            }));
        }

        private void OnEngineLoggedOn(object sender, LoggedOnEventArgs e)
        {
            Dispatcher.BeginInvoke(DispatcherPriority.Normal, new Action(() =>
            {
                LogOnSuccess();
                EnableButtonForOptions(true);
            }));
        }

        private void OnEngineLogonFailed(object sender, LogonFailedEventArgs e)
        {
            Dispatcher.BeginInvoke(DispatcherPriority.Normal, new Action(() =>
            {
                m_loggingOnResult = null;
                m_connect.Content = "Connect";
                m_connect.IsEnabled = true;
                AppendTextToOutput(e.FormattedErrorMessage);
            }));
        }

        #endregion

        #region Private Methods

        /// <summary>
        /// Appends the schedule info to output.
        /// </summary>
        /// <param name="task">The task to output.</param>
        private void AppendScheduleInfoToOutput(ScheduledTask task)
        {
            AppendTextToOutput("Created New Scheduled Task.");
            AppendTextToOutput("     Name: " + task.Name);
            AppendTextToOutput("     Guid: " + task.Guid);
        }

        private void AppendTextToOutput(string text)
        {
            OutputTextBox.Text = OutputTextBox.Text + text + "\r\n";
            OutputTextBox.ScrollToEnd();
        }

        /// <summary>
        /// Deletes the given entities
        /// </summary>
        /// <param name="entities">The entities to delete.</param>
        private void CleanupSampleEntities(params Entity[] entities)
        {
            foreach (var entity in entities)
            {
                m_sdkEngine.DeleteEntity(entity);
            }
        }

        /// <summary>
        /// Creates the add bookmark action and assigns a message.
        /// </summary>
        /// <param name="camera">The camera that the action will be associated to.</param>
        /// <param name="schedule">The schedule of the action.</param>
        /// <param name="message">The message to assign to the bookmark.</param>
        /// <returns></returns>
        private AddBookmarkAction CreateAddBookmarkAction(Guid camera, Guid schedule, string message)
        {
            AddBookmarkAction action = m_sdkEngine.ActionManager.BuildAction(ActionType.AddBookmark, camera, schedule) as AddBookmarkAction;
            if (action != null)
            {
                action.Message = message;
            }

            return action;
        }

        /// <summary>
        /// Creates the <see cref="ScheduledTask"/> and sets the given Schedule and Action to it.
        /// </summary>
        /// <param name="schedule">The schedule to associate with this ScheduledTask.</param>
        /// <param name="action">The action to associate with this ScheduledTask.</param>
        /// <exception cref="Genetec.Sdk.SdkException">An error occurred when creating the ScheduledTask</exception>
        private ScheduledTask CreateAndSetScheduledTask(ScheduleBase schedule, Genetec.Sdk.Actions.Action action)
        {
            Random random = new Random();
            ScheduledTask scheduledTask = m_sdkEngine.CreateEntity("ScheduledTask_" + random.Next(1, 100), EntityType.ScheduledTask) as ScheduledTask;

            if (scheduledTask == null)
            {
                throw new SdkException(SdkError.SdkRequestError, "An error occurred when creating the ScheduledTask");
            }

            scheduledTask.SetAction(action);
            scheduledTask.SetSchedule(schedule);

            return scheduledTask;
        }

        /// <summary>
        /// Creates the arm intrusion area action.
        /// </summary>
        /// <param name="intrusionArea">The intrusion area that the action will be associated to.</param>
        /// <param name="schedule">The schedule of the action.</param>
        /// <param name="delay">The delay before the intrusion area is armed from the time the action runs.</param>
        /// <param name="isMasterArm">if set to <c>true</c> it is the master arm, otherwise it is a perimeter arm</param>
        private ArmIntrusionAreaAction CreateArmIntrusionAreaAction(Guid intrusionArea, Guid schedule, TimeSpan delay, bool isMasterArm)
        {
            ArmIntrusionAreaAction action = m_sdkEngine.ActionManager.BuildAction(ActionType.ArmIntrusionArea, intrusionArea, schedule) as ArmIntrusionAreaAction;
            if (action != null)
            {
                action.ArmingData = new ArmingData(isMasterArm ? ArmingData.ArmScope.Master : ArmingData.ArmScope.Perimeter, ArmingData.ArmTiming.CustomDelay, delay, ArmingData.ArmType.Normal);
            }

            return action;
        }

        /// <summary>
        /// Creates the arm zone action.
        /// </summary>
        /// <param name="zone">The zone that the action will be associated to.</param>
        /// <param name="schedule">The schedule of the action.</param>
        /// <returns></returns>
        private ArmZoneAction CreateArmZoneAction(Guid zone, Guid schedule)
        {
            return m_sdkEngine.ActionManager.BuildAction(ActionType.ArmZone, zone, schedule) as ArmZoneAction;
        }

        /// <summary>
        /// Creates the disarm intrusion area action.
        /// </summary>
        /// <param name="intrusionArea">The intrusion area that the action will be associated to.</param>
        /// <param name="schedule">The schedule of the action.</param>
        /// <returns></returns>
        private DisarmIntrusionAreaAction CreateDisarmIntrusionAreaAction(Guid intrusionArea, Guid schedule)
        {
            return m_sdkEngine.ActionManager.BuildAction(ActionType.DisarmIntrusionArea, intrusionArea, schedule) as DisarmIntrusionAreaAction;
        }

        /// <summary>
        /// Creates the disarm zone action.
        /// </summary>
        /// <param name="zone">The zone that the action will be associated to.</param>
        /// <param name="schedule">The schedule of the action.</param>
        /// <returns></returns>
        private DisarmZoneAction CreateDisarmZoneAction(Guid zone, Guid schedule)
        {
            return m_sdkEngine.ActionManager.BuildAction(ActionType.DisarmZone, zone, schedule) as DisarmZoneAction;
        }

        /// <summary>
        /// Creates the display on analog monitor action.
        /// </summary>
        /// <param name="monitor">The monitor that the action will be associated to.</param>
        /// <param name="schedule">The schedule of the action.</param>
        /// <param name="camera">The camera that will be displayed on the monitor.</param>
        /// <returns></returns>
        private DisplayOnAnalogMonitorAction CreateDisplayOnAnalogMonitorAction(Guid monitor, Guid schedule, Guid camera)
        {
            DisplayOnAnalogMonitorAction action = m_sdkEngine.ActionManager.BuildAction(ActionType.DisplayOnAnalogMonitor, monitor, schedule) as DisplayOnAnalogMonitorAction;

            if (action != null)
            {
                action.Camera = camera;
            }

            return action;
        }

        /// <summary>
        /// Creates the execute macro action.
        /// </summary>
        /// <param name="macro">The macro that the action will be associated to.</param>
        /// <param name="schedule">The schedule of the action.</param>
        /// <param name="executionParameters">The execution parameters of the macro.</param>
        /// <returns></returns>
        private ExecuteMacroAction CreateExecuteMacroAction(Guid macro, Guid schedule, IEnumerable<MacroExecutionParameter> executionParameters)
        {
            ExecuteMacroAction action = m_sdkEngine.ActionManager.BuildAction(ActionType.RunMacro, macro, schedule) as ExecuteMacroAction;
            if (action != null)
            {
                foreach (var macroExecutionParameter in executionParameters)
                {
                    action.AddExecutionParameter(macroExecutionParameter);
                }
            }

            return action;
        }

        /// <summary>
        /// Creates the go to preset PTZ command action.
        /// </summary>
        /// <param name="camera">The camera that the action will be associated to.</param>
        /// <param name="schedule">The schedule of the action.</param>
        /// <param name="presetId">The preset id the camera will go to.</param>
        /// <returns></returns>
        private PresetAction CreateGoToPresetPtzCommand(Guid camera, Guid schedule, int presetId)
        {
            PresetAction action = m_sdkEngine.ActionManager.BuildAction(PtzCommandType.GoToPreset, camera, schedule) as PresetAction;
            if (action != null)
            {
                action.PresetId = presetId;
            }

            return action;
        }

        /// <summary>
        /// Creates the home action.
        /// </summary>
        /// <param name="camera">The camera that the action will be associated to.</param>
        /// <param name="schedule">The schedule of the action.</param>
        /// <returns></returns>
        private HomeAction CreateHomeAction(Guid camera, Guid schedule)
        {
            return m_sdkEngine.ActionManager.BuildAction(PtzCommandType.Home, camera, schedule) as HomeAction;
        }

        /// <summary>
        /// Creates the protect live video action.
        /// </summary>
        /// <param name="camera">The camera that the action will be associated to.</param>
        /// <param name="schedule">The schedule of the action.</param>
        /// <param name="applyProtection">How long the camera will be archiving video with video protection enabled.</param>
        /// <param name="protectionLength">The length of time the archived recorded video will be protected for.</param>
        /// <returns></returns>
        private ProtectLiveVideoAction CreateProtectLiveVideoAction(Guid camera, Guid schedule, TimeSpan applyProtection, TimeSpan protectionLength)
        {
            ProtectLiveVideoAction action = m_sdkEngine.ActionManager.BuildAction(ActionType.StartFileProtection, camera, schedule) as ProtectLiveVideoAction;
            if (action != null)
            {
                action.ApplyProtectionTime = applyProtection;
                action.ProtectionLength = protectionLength;
            }

            return action;
        }

        /// <summary>
        /// Creates the run pattern PTZ command.
        /// </summary>
        /// <param name="camera">The camera that the action will be associated to.</param>
        /// <param name="schedule">The schedule of the action.</param>
        /// <param name="patternId">The pattern id the camera will run.</param>
        /// <returns></returns>
        private PatternAction CreateRunPatternPtzCommand(Guid camera, Guid schedule, int patternId)
        {
            PatternAction action = m_sdkEngine.ActionManager.BuildAction(PtzCommandType.RunPattern, camera, schedule) as PatternAction;

            if (action != null)
            {
                action.PatternId = patternId;
            }

            return action;
        }

        /// <summary>
        /// Creates the set threat level action.
        /// </summary>
        /// <param name="area">The area that the action will be associated to.</param>
        /// <param name="schedule">The schedule of the action.</param>
        /// <param name="threatLevel">The threat level the area will be se to.</param>
        /// <param name="recursive">if set to <c>true</c> the threat level will be set recursively in child areas.</param>
        /// <returns></returns>
        private SetThreatLevelAction CreateSetThreatLevelAction(Guid area, Guid schedule, Guid threatLevel, bool recursive)
        {
            SetThreatLevelAction action = m_sdkEngine.ActionManager.BuildAction(ActionType.ActivateThreatLevel, area, schedule) as SetThreatLevelAction;
            if (action != null)
            {
                action.ThreatLevel = threatLevel;
                action.Recursive = recursive;
            }

            return action;
        }

        /// <summary>
        /// Creates the start recording action.
        /// </summary>
        /// <param name="camera">The camera that the action will be associated to.</param>
        /// <param name="schedule">The schedule of the action.</param>
        /// <param name="recordingLength">Length of the recording.</param>
        /// <returns></returns>
        private StartRecordingAction CreateStartRecordingAction(Guid camera, Guid schedule, TimeSpan recordingLength)
        {
            StartRecordingAction action = m_sdkEngine.ActionManager.BuildAction(ActionType.StartRecording, camera, schedule) as StartRecordingAction;
            if (action != null)
            {
                action.RecordingLength = recordingLength;
            }

            return action;
        }

        /// <summary>
        /// Creates the stop protect live video action.
        /// </summary>
        /// <param name="camera">The camera that the action will be associated to.</param>
        /// <param name="schedule">The schedule of the action.</param>
        /// <param name="applyProtection">The amount of time until protection is disabled after the action runs.</param>
        /// <returns></returns>
        private StopProtectLiveVideoAction CreateStopProtectLiveVideoAction(Guid camera, Guid schedule, TimeSpan applyProtection)
        {
            StopProtectLiveVideoAction action = m_sdkEngine.ActionManager.BuildAction(ActionType.StopFileProtection, camera, schedule) as StopProtectLiveVideoAction;
            if (action != null)
            {
                action.StopApplyingProtectionLength = applyProtection;
            }

            return action;
        }

        /// <summary>
        /// Creates the stop recording action.
        /// </summary>
        /// <param name="camera">The camera that the action will be associated to.</param>
        /// <param name="schedule">The schedule of the action.</param>
        /// <param name="lengthBeforeStop">The time before stopping recording.</param>
        /// <returns></returns>
        private StopRecordingAction CreateStopRecordingAction(Guid camera, Guid schedule, TimeSpan lengthBeforeStop)
        {
            StopRecordingAction action = m_sdkEngine.ActionManager.BuildAction(ActionType.StopRecording, camera, schedule) as StopRecordingAction;
            if (action != null)
            {
                action.RecordingLengthBeforeStop = lengthBeforeStop;
            }

            return action;
        }

        /// <summary>
        /// Creates the trigger alarm action.
        /// </summary>
        /// <param name="alarm">The alarm that the action will be associated to.</param>
        /// <param name="schedule">The schedule of the action.</param>
        /// <returns></returns>
        private TriggerAlarmAction CreateTriggerAlarmAction(Guid alarm, Guid schedule)
        {
            return m_sdkEngine.ActionManager.BuildAction(ActionType.TriggerAlarm, alarm, schedule) as TriggerAlarmAction;
        }

        /// <summary>
        /// Creates the trigger output action.
        /// </summary>
        /// <param name="outputDevice">The output device that the action will be associated to.</param>
        /// <param name="schedule">The schedule of the action.</param>
        /// <param name="outputBehaviour">The output behaviour that will run on the output device.</param>
        /// <returns></returns>
        private TriggerOutputAction CreateTriggerOutputAction(Guid outputDevice, Guid schedule, Guid outputBehaviour)
        {
            TriggerOutputAction action = m_sdkEngine.ActionManager.BuildAction(ActionType.TriggerOutput, outputDevice, schedule) as TriggerOutputAction;
            if (action != null)
            {
                action.OutputBehavior = outputBehaviour;
            }

            return action;
        }

        private void EnableButtonForOptions(bool enabled)
        {
            StartRecordingButton.IsEnabled = enabled;
            GoToPresetButton.IsEnabled = enabled;
            RunPatternButton.IsEnabled = enabled;
            TriggerAlarmButton.IsEnabled = enabled;
            AddBookmarkButton.IsEnabled = enabled;
            ArmZoneButton.IsEnabled = enabled;
            StartVideoProtectionButton.IsEnabled = enabled;

            m_directory.IsEnabled = !enabled;
            m_userName.IsEnabled = !enabled;
            passwordBox.IsEnabled = !enabled;
        }

        private void LogOnSuccess()
        {
            AppendTextToOutput("Logged in successfully");
            m_loggingOnResult = null;
            m_connect.Content = "Disconnect";
            m_connect.IsEnabled = true;
        }

        #endregion
    }

    #endregion
}

