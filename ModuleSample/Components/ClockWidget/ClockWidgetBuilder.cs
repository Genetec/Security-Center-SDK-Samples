// ==========================================================================
// Copyright (C) 2019 by Genetec, Inc.
// All rights reserved.
// May be used only in accordance with a valid Source Code License Agreement.
// ==========================================================================

using Genetec.Sdk.Workspace.Components;
using System;
using System.Windows.Media.Imaging;

namespace ModuleSample.Components.ClockWidget
{
    public sealed class ClockWidgetBuilder : DashboardWidgetBuilder
    {
        #region Public Fields

        public static Guid ClockWidgetTypeId = new Guid("{fbafebdc-25aa-47de-b0c5-ededaccffc5c}");

        #endregion Public Fields

        #region Public Properties

        public override Guid Category => ModuleTest.CustomDashboardWidgetCategoryId;

        /// <summary>
        /// Description of the widget to show when hovering over the widget in the widget panel.
        /// </summary>
        public override string Description => "This is a sample showing how to create a custom widget.";

        /// <summary>
        /// Name of the widget to show in the Widget panel.
        /// </summary>
        public override string Name => "Clock Widget";

        public override int Priority => 1;

        /// <summary>
        /// Thumbnail of the widget to show in the Widget panel.
        /// </summary>
        public override BitmapSource Thumbnail => new BitmapImage(new Uri(@"pack://application:,,,/ModuleSample;Component/Resources/AlarmClock.png", UriKind.RelativeOrAbsolute));

        /// <summary>
        /// Gets the <see cref="DashboardWidgetBuilder"/> UniqueId to map the <see cref="DashboardWidget"/>.
        /// </summary>
        public override Guid UniqueId => ClockWidgetTypeId;

        #endregion Public Properties

        #region Public Methods

        public override DashboardWidget CreateWidget(DashboardWidgetBuilderContext context) => new ClockWidget();
        
        public override bool IsSupported(DashboardWidgetBuilderContext context) => context.Type == ClockWidgetTypeId;
        
        #endregion Public Methods
    }
}