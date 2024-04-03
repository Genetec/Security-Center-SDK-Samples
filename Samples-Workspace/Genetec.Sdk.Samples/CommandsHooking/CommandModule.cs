// ==========================================================================
// Copyright (C) 2019 by Genetec, Inc.
// All rights reserved.
// May be used only in accordance with a valid Source Code License Agreement.
// ==========================================================================

using System;
using System.Collections.Generic;
using System.Data;
using System.Threading.Tasks;
using Genetec.Sdk.Entities;
using Genetec.Sdk.Workspace.Commands;
using Genetec.Sdk.Workspace.Modules;
using Genetec.Sdk.Workspace.Pages;

namespace CommandsHooking
{
    /// <summary>
    /// This Sample shows how to hook on a command and stop it from going through.
    /// It gets the alarms which are selected on the UI.
    /// </summary>
    public sealed class CommandModule : Module
    {

        #region Public Properties

        // List containing the Alarm entities.
        public List<Alarm> SelectedAlarmGuids { get; } = new List<Alarm>();

        #endregion Public Properties

        #region Public Methods

        // Hooking on the commands. You will receive every commands the systems can do.
        // There is a bunch of commands to receive. Check them out in WorkspaceCommands class.
        public override void Load() => Workspace.Commands.Executing += OnCommandExecuting;

        // Unhooking so that it stays clean.
        public override void Unload() => Workspace.Commands.Executing -= OnCommandExecuting;

        #endregion Public Methods

        #region Private Methods

        /// <summary>
        /// This method is called when a command is triggered.
        /// Here, you can intercept every single commands.
        /// </summary>
        /// <param name="sender">The sender</param>
        /// <param name="e">The Argument of the cancel execution. It contains the command guid.</param>
        private void OnCommandExecuting(object sender, CommandCancelExecutionEventArgs e)
        {
            // Making sure it's the command we are expecting.
            if (e.Command.Id != WorkspaceCommands.AckAlarm) return;

            // For this command specifically, it is essential to be in AlarmMonitoringPage to be able
            // to retrieve the selected alarms. It is not possible to get the selected alarms in the reports.
            if (!(Workspace.ActiveMonitor.ActivePage is AlarmMonitoringPage alarmMonitoringPage)) return;

            // This allows you to manage the call yourself.
            // If you set this to True, you manage the command yourself.
            // If this is set to true, it stops the default action from SecurityDesk or ConfigTool. Managing 
            // the call yourself will stop SD and CT from doing anything for that command.
            e.Cancel = true;

            // This is where we get the selected active alarms in the AlarmMonitoringPage.
            var alarms = GetActiveAlarms(alarmMonitoringPage).Result;

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

            Workspace.Sdk.ActionManager.SendMessage(
                Workspace.Sdk.LoggedUser.Guid,
                "Workspace Sample CommandsHooking is blocking the acknowledgement of alarms.", 10);
        }

        private async Task<DataTable> GetActiveAlarms(AlarmMonitoringPage alarmMonitoringPage)
            => await alarmMonitoringPage.GetSelectedActiveAlarmsAsync();

        #endregion Private Methods

    }
}