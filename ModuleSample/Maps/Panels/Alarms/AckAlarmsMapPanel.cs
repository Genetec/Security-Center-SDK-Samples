// ==========================================================================
// Copyright (C) 2020 by Genetec, Inc.
// All rights reserved.
// May be used only in accordance with a valid Source Code License Agreement.
// ==========================================================================

using Genetec.Sdk;
using Genetec.Sdk.Workspace;
using Genetec.Sdk.Workspace.Components.MapPanel;
using System;
using System.Threading.Tasks;
using System.Windows;
using System.Windows.Media;
using System.Windows.Media.Imaging;

namespace ModuleSample.Maps.Panels.Alarms
{
    /// <summary>
    /// This panel adds an icon on the top right corner of the Map Page in Security Desk
    /// The panel will display a green check mark within a borderless button
    /// The button will be enabled if the system has active alarms
    /// Clicking the button will launch the acknowledge all alarms command
    /// </summary>
    public class AckAlarmsMapPanel : MapPanel
    {

        #region Public Properties

        public override Size MaxDockedSize => new Size(100, 100);
        public override Size MinDockedSize => new Size(100, 100);
        public override Guid UniqueId => new Guid("{0A91A30D-7CCC-4CE4-B473-4AF8D0111EF7}");

        #endregion Public Properties

        #region Public Constructors

        public AckAlarmsMapPanel()
        {
            Title = "Force acknowledge all active alarms";
        }

        #endregion Public Constructors

        #region Public Methods

        public override void Initialize(Workspace workspace)
        {
            SubscribeAlarms();
        }

        public override void OnPanelClicked()
        {
            if (Workspace == null || !Workspace.Sdk.AlarmManager.ForceAcknowledgeAllAlarms())
            {
                MessageBox.Show("Error while trying to acknowledge alarms..", "Error");
            }
        }

        #endregion Public Methods

        #region Protected Methods

        /// <summary>
        /// Creates the icon.
        /// </summary>
        protected override ImageSource CreateIcon()
        {
            return new BitmapImage(new Uri(@"pack://application:,,,/ModuleSample;Component/Resources/acknowledgealarms.png", UriKind.RelativeOrAbsolute));
        }

        /// <summary>
        /// Creates the view.
        /// </summary>
        protected override UIElement CreateView()
        {
            return null;
        }

        #endregion Protected Methods

        #region Private Methods

        private int GetNumberOfActiveAlarms()
        {
            return Workspace.Sdk.AlarmManager.GetActiveAlarms().Rows.Count;
        }

        private void OnAlarmAcknowledged(object sender, AlarmAcknowledgedEventArgs e)
        {
            Task.Factory.StartNew(new Action(() =>
            {
                var numberOfActivealarms = GetNumberOfActiveAlarms();
                IsEnabled = numberOfActivealarms > 0;
            }));
        }

        private void OnAlarmTriggered(object sender, AlarmTriggeredEventArgs e)
        {
            IsEnabled = true;
        }

        private void SubscribeAlarms()
        {
            if (Workspace != null)
            {
                Workspace.Sdk.AlarmTriggered += OnAlarmTriggered;
                Workspace.Sdk.AlarmAcknowledged += OnAlarmAcknowledged;

                IsEnabled = Workspace.Sdk.AlarmManager.GetActiveAlarms().Rows.Count > 0;
            }
        }

        #endregion Private Methods
    }
}