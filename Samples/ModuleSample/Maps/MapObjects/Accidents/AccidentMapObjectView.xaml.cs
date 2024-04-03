// ==========================================================================
// Copyright (C) 2020 by Genetec, Inc.
// All rights reserved.
// May be used only in accordance with a valid Source Code License Agreement.
// ==========================================================================

using System.Windows;
using System.Windows.Input;

namespace ModuleSample.Maps.MapObjects.Accidents
{
    /// <summary>
    /// Interaction logic for AccidentMapObjectView.xaml
    /// </summary>
    public partial class AccidentMapObjectView
    {

        #region Private Fields

        private readonly AccidentMapObject m_mapObject;

        #endregion Private Fields

        #region Public Constructors

        public AccidentMapObjectView(AccidentMapObject mapObject)
        {
            m_mapObject = mapObject;
            InitializeComponent();

            Initialize(mapObject);
            m_txtDescription.Text = m_mapObject.Description;
        }

        #endregion Public Constructors

        #region Private Methods

        private void OnImagedMouseRightButtonUp(object sender, MouseButtonEventArgs e)
        {
            if ((sender is FrameworkElement element) && (element.ContextMenu != null))
            {
                element.ContextMenu.IsOpen = true;
                e.Handled = true;
            }
        }

        private void OnMenuRemoveClick(object sender, RoutedEventArgs e)
        {
            AccidentMapObjectProvider.RemoveAccident(m_mapObject);
        }

        #endregion Private Methods

    }
}