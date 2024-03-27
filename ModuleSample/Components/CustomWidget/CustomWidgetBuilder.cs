// ==========================================================================
// Copyright (C) 2019 by Genetec, Inc.
// All rights reserved.
// May be used only in accordance with a valid Source Code License Agreement.
// ==========================================================================

using Genetec.Sdk.Workspace.Components;
using System;
using System.Windows.Media.Imaging;

namespace ModuleSample.Components.CustomWidget
{
    public sealed class CustomWidgetBuilder : DashboardWidgetBuilder
    {
        #region Public Fields

        public static Guid CustomWidgetTypeId = new Guid("{2bafebdc-25aa-47de-a0c5-ededaccffc5c}");

        #endregion Public Fields

        #region Public Properties

        public override Guid Category => ModuleTest.CustomDashboardWidgetCategoryId;
        public override string Description => "Custom widget description";
        public override string Name => "Custom Widget name";
        /// <summary>
        /// Thumbnail of the widget.
        /// Displayed in the Widget panel.
        /// </summary>
        public override BitmapSource Thumbnail => new BitmapImage(new Uri(@"pack://application:,,,/ModuleSample;Component/Resources/Genetec.jpg", UriKind.RelativeOrAbsolute));

        public override Guid UniqueId => CustomWidgetTypeId;

        #endregion Public Properties

        #region Public Methods

        public override DashboardWidget CreateWidget(DashboardWidgetBuilderContext context) => new CustomWidget();
       
        public override bool IsSupported(DashboardWidgetBuilderContext context) => context.Type == CustomWidgetTypeId;
        
        #endregion Public Methods
    }
}