// ==========================================================================
// Copyright (C) 2020 by Genetec, Inc.
// All rights reserved.
// May be used only in accordance with a valid Source Code License Agreement.
// ==========================================================================

using Genetec.Sdk.Workspace.Components.MapPanel;
using System;

namespace ModuleSample.Maps.Panels.Alarms
{
    public class AckAlarmsMapPanelBuilder : MapPanelBuilder
    {
        #region Public Properties

        public override string Name => "AckAlarmsMapPanelBuilder";
        public override Guid UniqueId => new Guid("{B73D7422-A536-47C1-88B3-74EF5A84CF01}");

        #endregion Public Properties

        #region Public Methods

        public override MapPanel CreateView()
        {
            var panel = new AckAlarmsMapPanel();
            panel.Initialize(Workspace);
            return panel;
        }

        #endregion Public Methods
    }
}