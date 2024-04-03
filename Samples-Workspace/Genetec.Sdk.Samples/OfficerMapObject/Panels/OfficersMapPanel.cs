// ==========================================================================
// Copyright (C) 2020 by Genetec, Inc.
// All rights reserved.
// May be used only in accordance with a valid Source Code License Agreement.
// ==========================================================================

using Genetec.Sdk.Workspace;
using Genetec.Sdk.Workspace.Components.MapPanel;
using System;
using System.Windows;
using System.Windows.Media;
using System.Windows.Media.Imaging;

namespace OfficerMapObject.Panels
{
    public class OfficersMapPanel : MapPanel
    {
        #region Public Properties

        public override Size MaxDockedSize => new Size(800, 800);
        public override Size MinDockedSize => new Size(200, 100);
        public override Guid UniqueId => new Guid("{1641FF76-3026-46DE-B507-D6E708C7D1A2}");

        #endregion Public Properties

        #region Public Constructors

        public OfficersMapPanel()
        {
            IsFloating = true;
            Title = "Officers";
        }

        #endregion Public Constructors

        #region Public Methods

        public override void Initialize(Workspace workspace)
        {
        }

        #endregion Public Methods

        #region Protected Methods

        protected override ImageSource CreateIcon() 
            => new BitmapImage(new Uri(@"pack://application:,,,/OfficerMapObject;Component/Resources/Officer.png", UriKind.RelativeOrAbsolute));
        
        protected override UIElement CreateView() 
            => new OfficerMapPanelView(this);

        #endregion Protected Methods
    }
}