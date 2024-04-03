// ==========================================================================
// Copyright (C) 2020 by Genetec, Inc.
// All rights reserved.
// May be used only in accordance with a valid Source Code License Agreement.
// ==========================================================================

using Genetec.Sdk;
using Genetec.Sdk.Scripting;
using Genetec.Sdk.Scripting.Interfaces.Attributes;

namespace MacroStudio.Macros.Specific
{
    [MacroParameters(KeepRunningAfterExecute = true)]
    public sealed class AlarmAckMacro : UserMacro
    {

        #region Public Methods

        /// <summary>
        /// Called when the macro is executed.
        /// </summary>
        public override void Execute()
            => Sdk.AlarmTriggered += OnSdkAlarmTriggered;

        #endregion Public Methods

        #region Protected Methods

        /// <summary>
        /// Called when the macro needs to clean up.
        /// </summary>
        protected override void CleanUp()
            => Sdk.AlarmTriggered -= OnSdkAlarmTriggered;

        #endregion Protected Methods

        #region Private Methods

        private void OnSdkAlarmTriggered(object sender, AlarmTriggeredEventArgs e)
        {
            Trace("Alarm was acknowledged");
            Sdk.AlarmManager.AcknowledgeAlarm(e.InstanceId, e.AlarmGuid, AcknowledgementType.Ack);
        }

        #endregion Private Methods

    }
}