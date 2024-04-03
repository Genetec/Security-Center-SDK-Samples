// ==========================================================================
// Copyright (C) 2020 by Genetec, Inc.
// All rights reserved.
// May be used only in accordance with a valid Source Code License Agreement.
// ==========================================================================

using Genetec.Sdk.Workspace;
using Genetec.Sdk.Workspace.Components.MapPanel;
using System;
using System.Windows;
using System.Windows.Controls;
using System.Windows.Media;
using System.Windows.Media.Imaging;

namespace ModuleSample.Maps.Panels.Incidents
{
    public class IncidentsMapPanel : MapPanel
    {
        #region Public Properties

        public override Size MaxDockedSize => new Size(800, 800);
        public override Size MinDockedSize => new Size(200, 100);
        public override Guid UniqueId => new Guid("{1641FF76-3026-46DE-B507-D6E708C7D1A2}");

        #endregion Public Properties

        #region Public Constructors

        public IncidentsMapPanel()
        {
            DockPosition = Dock.Bottom;
            IsFloating = false;
            IsOpen = true;
            Title = "Incidents";
        }

        #endregion Public Constructors

        #region Public Methods

        public override void Initialize(Workspace workspace)
        {
        }

        #endregion Public Methods

        #region Protected Methods

        protected override ImageSource CreateIcon()
        {
            return new BitmapImage(new Uri(@"pack://application:,,,/ModuleSample;Component/Resources/incident.png", UriKind.RelativeOrAbsolute));
            //return new BitmapImage(new Uri(@"pack://application:,,,/ModuleSample;Component/Resources/acknowledgealarms.png", UriKind.RelativeOrAbsolute));
        }

        protected override UIElement CreateView()
        {
            return new IncidentMapPanelView(this);
        }

        #endregion Protected Methods
    }
}