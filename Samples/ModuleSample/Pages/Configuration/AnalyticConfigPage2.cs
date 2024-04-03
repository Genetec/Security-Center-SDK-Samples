// ==========================================================================
// Copyright (C) 2020 by Genetec, Inc.
// All rights reserved.
// May be used only in accordance with a valid Source Code License Agreement.
// ==========================================================================

using Genetec.Sdk;
using Genetec.Sdk.Entities;
using Genetec.Sdk.Workspace.Pages;
using System;
using System.Windows;

namespace ModuleSample.Pages.Configuration
{

    public class AnalyticConfigPage2 : ConfigPage
    {

        #region Public Fields

        public static readonly DependencyProperty CameraNameProperty =
                    DependencyProperty.Register("CameraName", typeof(string), typeof(AnalyticConfigPage2));

        #endregion Public Fields

        #region Private Fields

        private Camera m_entity;

        #endregion Private Fields

        #region Public Properties

        public string CameraName
        {
            get => (string)GetValue(CameraNameProperty);
            set => SetValue(CameraNameProperty, value);
        }

        // This means to bundle up the configuration page in the Analytic category.
        public override Guid Category => SystemCategories.Analytics;

        #endregion Public Properties

        #region Protected Properties

        protected override Guid Entity
        {
            set
            {
                m_entity = Workspace.Sdk.GetEntity(value) as Camera;
                IsVisible = m_entity != null;
            }
        }
        protected override EntityType EntityType => EntityType.Camera;

        protected override string Name => "Config Page 2";

        #endregion Protected Properties

        #region Public Constructors

        public AnalyticConfigPage2()
        {
            View = new AnalyticConfigPage2View(this);
        }

        #endregion Public Constructors

        #region Protected Methods

        protected override void Refresh()
        {
            CameraName = string.Empty;
            if (m_entity != null)
            {
                CameraName = m_entity.Name;
            }

            IsDirty = false;
        }

        #endregion Protected Methods

    }

}