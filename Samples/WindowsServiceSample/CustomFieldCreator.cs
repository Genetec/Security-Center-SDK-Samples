// ==========================================================================
// Copyright (C) 2020 by Genetec, Inc.
// All rights reserved.
// May be used only in accordance with a valid Source Code License Agreement.
// ==========================================================================

using Genetec.Sdk;
using Genetec.Sdk.Entities;
using Genetec.Sdk.Entities.CustomFields;
using System;
using System.Data;
using System.Linq;
using System.Threading.Tasks;

namespace WindowsServiceSample
{
    public class CustomFieldCreator : IDisposable
    {

        #region Private Fields

        private readonly Guid m_systemConfiguration;
        private CustomField m_customField;
        private Engine m_sdkEngine;

        #endregion Private Fields

        #region Public Constructors

        public CustomFieldCreator(Engine engine)
        {
            m_sdkEngine = engine;
            m_systemConfiguration = m_sdkEngine.GetEntity<SystemConfiguration>(SystemConfiguration.SystemConfigurationGuid).Guid;
        }

        #endregion Public Constructors

        #region Public Methods

        public void CreateCustomFields()
        {
            var sysConfig = m_sdkEngine.GetEntity(m_systemConfiguration) as SystemConfiguration;
            if (sysConfig == null) return;
            var customFieldService = sysConfig.CustomFieldService;

            m_customField = customFieldService.CustomFields.FirstOrDefault(x => x.Name == "Is Ringing");

            if (m_customField != null)
            {
                RegisterAlarmEvents();
                return;
            }

            // Create a custom field builder
            var customFieldBuilder = customFieldService.CreateCustomFieldBuilder();

            // Set the custom field's attributes
            customFieldBuilder.SetEntityType(EntityType.Alarm);
            customFieldBuilder.SetGroupName("Alarms");
            customFieldBuilder.SetGroupPriority(1);
            customFieldBuilder.SetName("Is Ringing");
            customFieldBuilder.SetValueType(CustomFieldValueType.Boolean);
            customFieldBuilder.SetDefaultValue(false);

            // Build the custom field
            var customField = customFieldBuilder.Build();
            var cf = new[] { customField };
            customFieldService.AddCustomFields(cf);

            m_customField = customField;

            RegisterAlarmEvents();
        }

        public void Dispose()
        {
            m_sdkEngine.AlarmTriggered -= OnAlarmTriggered;
            m_sdkEngine.AlarmAcknowledged -= OnAlarmAcknowledged;
        }

        #endregion Public Methods

        #region Private Methods

        private void AcknowledgeAlarm(Entity alarm)
        {
            // Check that there are no other active instances of this alarm
            var activeAlarms = m_sdkEngine.AlarmManager.GetActiveAlarms();
            if (activeAlarms == null)
            {
                SetCustomField(alarm, false);
                return;
            }

            foreach (DataRow row in activeAlarms.Rows)
            {
                if (row[1].Equals(alarm.Guid))
                    return;
            }

            SetCustomField(alarm, false);
        }

        private void OnAlarmAcknowledged(object sender, AlarmAcknowledgedEventArgs e)
        {
            var alarm = m_sdkEngine.GetEntity(e.AlarmGuid);

            Task.Run(() => AcknowledgeAlarm(alarm));
        }

        private void OnAlarmTriggered(object sender, AlarmTriggeredEventArgs e)
        {
            var alarm = m_sdkEngine.GetEntity(e.AlarmGuid);
            // Set the alarm's custom field "Is Ringing" to true when the alarm is triggered
            SetCustomField(alarm, true);
        }

        private void RegisterAlarmEvents()
        {
            m_sdkEngine.AlarmTriggered += OnAlarmTriggered;
            m_sdkEngine.AlarmAcknowledged += OnAlarmAcknowledged;
        }
        private void SetCustomField(Entity alarm, bool value)
        {
            var customFields = alarm.GetCustomFields();
            var field = (from cf in customFields
                         where m_customField.Equals(cf.CustomField)
                         select cf.CustomField).FirstOrDefault();

            alarm.SetCustomField(field, value);
        }

        #endregion Private Methods
    }
}