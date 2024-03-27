using VisualMapObject.Components;
using System;
using System.Windows;
using System.Windows.Input;

// ==========================================================================
// Copyright (C) 2016 by Genetec, Inc.
// All rights reserved.
// May be used only in accordance with a valid Source Code License Agreement.
// ==========================================================================
namespace VisualMapObject.Maps
{
    /// <summary>
    /// Interaction logic for AccidentMapObjectView.xaml
    /// </summary>
    public partial class AccidentMapObjectView
    {
        #region Constants

        /// <summary>
        /// The unique identifier of the AccidentMapObjectView class.
        /// </summary>
        public static readonly Guid AccidentsLayerId = new Guid("{4DBDF995-4818-4EC0-8DC4-315E78041234}");

        #endregion

        #region Fields

        /// <summary>
        /// The inner map object.
        /// This is the map object that holds the information.
        /// </summary>
        private readonly AccidentMapObject m_mapObject;

        /// <summary>
        /// The name of the layer.
        /// </summary>
        public const string AccidentLayerName = "Accidents";

        #endregion

        #region Properties

        /// <summary>
        /// Gets the layer's on which the item should appear
        /// </summary>
        public override Guid LayerId {get { return AccidentsLayerId; }}

        #endregion

        #region Constructors

        /// <summary>
        /// Creates a new instance of the <see cref="AccidentMapObjectView"/> class.
        /// </summary>
        /// <param name="mapObject"></param>
        public AccidentMapObjectView(AccidentMapObject mapObject)
        {
            if (mapObject == null)
            {
                throw new ArgumentNullException("mapObject");
            }

            m_mapObject = mapObject;
            InitializeComponent();

            Initialize(mapObject);
            m_txtDescription.Text = m_mapObject.Description;
        }

        #endregion

        #region Public Methods
        #endregion

        #region Event Handlers

        private void OnImageMouseRightButtonUp(object sender, MouseButtonEventArgs e)
        {
            var element = sender as FrameworkElement;
            if (element != null && element.ContextMenu != null)
            {
                element.ContextMenu.IsOpen = true;
                e.Handled = true;
            }
        }

        private void OnMenuRemoveClick(object sender, RoutedEventArgs e)
        {
           AccidentMapObjectProvider.RemoveAcccident(m_mapObject);
        }

        #endregion
    }
}
