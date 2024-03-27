// ==========================================================================
// Copyright (C) 2019 by Genetec, Inc.
// All rights reserved.
// May be used only in accordance with a valid Source Code License Agreement.
// ==========================================================================

using Genetec.Sdk.Workspace.Components;
using System;
using System.Windows;
using System.Windows.Media;
using Size = System.Windows.Size;

namespace ModuleSample.Components.CustomWidget
{
    public sealed class CustomWidget : DashboardWidget
    {

        #region Private Fields

        private CustomWidgetOptionsView m_optionsView;

        #endregion Private Fields

        #region Public Properties

        /// <summary>
        /// Default background color.
        /// </summary>
        public override Color DefaultBackgroundColor => Color.FromRgb(51, 204, 51);

        /// <summary>
        /// Title of the widget's configuration panel.
        /// </summary>
        public override string WidgetName => "Custom Widget";

        /// <summary>
        /// Maximum size (number of cells) of the widget.
        /// </summary>
        public override Size WidgetSize { get; set; } = new Size(5, 5);
        /// <summary>
        /// Unique id of the widget type
        /// </summary>
        public override Guid WidgetTypeId => CustomWidgetBuilder.CustomWidgetTypeId;

        #endregion Public Properties

        #region Public Methods

        public override UIElement CreateOptionsView()
        {
            var option = new CustomWidgetOptions { BackgroundColor = new SolidColorBrush(Colors.Cyan), ForegroundColor = new SolidColorBrush(Colors.DeepPink) };
            m_optionsView = new CustomWidgetOptionsView { DataContext = option };
            return m_optionsView;
        }

        public override UIElement CreateView() => new CustomWidgetView();
       
        #endregion Public Methods

    }
}