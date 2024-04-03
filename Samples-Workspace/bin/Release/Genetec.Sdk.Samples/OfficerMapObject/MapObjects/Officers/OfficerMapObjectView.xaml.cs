// ==========================================================================
// Copyright (C) 2020 by Genetec, Inc.
// All rights reserved.
// May be used only in accordance with a valid Source Code License Agreement.
// ==========================================================================

using System.Windows.Input;

namespace OfficerMapObject.MapObjects.Officers
{
    /// <summary>
    /// Interaction logic for OfficerMapObjectView.xaml
    /// </summary>
    public partial class OfficerMapObjectView
    {
        #region Public Fields

        public const string OfficerLayerName = "Officers";

        #endregion Public Fields

        #region Public Constructors

        public OfficerMapObjectView() 
            => InitializeComponent();

        #endregion Public Constructors

        #region Private Methods

        private void OnImagePreviewMouseRightButtonDown(object sender, MouseButtonEventArgs e)
        {
            if (OfficerImage.ContextMenu != null) OfficerImage.ContextMenu.IsOpen = true;
        }

        #endregion Private Methods
    }
}