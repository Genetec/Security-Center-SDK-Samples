using Genetec.Sdk.Workspace.Maps;
using System;
using System.Windows;
using System.Windows.Input;

// ==========================================================================
// Copyright (C) 2016 by Genetec, Inc.
// All rights reserved.
// May be used only in accordance with a valid Source Code License Agreement.
// ==========================================================================
namespace DynamicMapObjects.Maps
{
    #region Classes

    /// <summary>
    /// Interaction logic for OfficerMapObjectView.xaml
    /// </summary>
    public partial class OfficerMapObjectView
    {
        #region Constants

        public const string OfficerLayerName = "Officers";

        #endregion

        #region Fields

        public bool CanDragDrop;

        #endregion

        #region Constructors

        public OfficerMapObjectView()
        {
            InitializeComponent();
        }

        #endregion

        #region Event Handlers

        private void OnOfficerMapObjectViewMouseDown(object sender, MouseButtonEventArgs e)
        {
            CanDragDrop = true;
        }

        private void OnOfficerMapObjectViewMouseMove(object sender, MouseEventArgs e)
        {
            MapObjectView officerMapObject = sender as MapObjectView;
            if (officerMapObject != null && CanDragDrop && e.LeftButton == MouseButtonState.Pressed)
            {
                if (DragDrop.DoDragDrop(this, officerMapObject.MapObject.LinkedEntity, DragDropEffects.Copy) == DragDropEffects.None)
                {
                    CanDragDrop = false;
                }
            }
        }

        #endregion
    }

    #endregion
}

