// ==========================================================================
// Copyright (C) 2020 by Genetec, Inc.
// All rights reserved.
// May be used only in accordance with a valid Source Code License Agreement.
// ==========================================================================

using Genetec.Sdk;
using Genetec.Sdk.Diagnostics.Logging.Core;
using Genetec.Sdk.Entities;
using Genetec.Sdk.Workspace.Pages;
using System;
using System.ComponentModel;
using System.Linq;

namespace ModuleSample.Pages.Configuration
{

    public class CustomConfigPage : ConfigPage, IDisposable, INotifyPropertyChanged
    {

        #region Private Fields

        private readonly Logger m_logger;

        private bool m_disposed;
        private bool m_shouldBeAdministrator;
        private User m_user;

        #endregion Private Fields

        #region Public Properties

        public bool ShouldBeAdministrator
        {
            get => m_shouldBeAdministrator;
            set
            {
                if (m_shouldBeAdministrator == value) return;
                m_shouldBeAdministrator = value;
                IsDirty = true;
                OnPropertyChanged();
            }
        }

        #endregion Public Properties

        #region Protected Properties

        // Entity for which the config page is shown
        protected override Guid Entity
        {
            set
            {
                m_user = Workspace.Sdk.GetEntity(value) as User; ;
                // Set the config page to visible only if the specified role is a User
                IsVisible = m_user != null;
            }
        }

        protected override EntityType EntityType => EntityType.User;

        protected override string Name => "Custom Config Page";

        #endregion Protected Properties

        #region Public Constructors

        public CustomConfigPage()
        {
            m_logger = Logger.CreateInstanceLogger(this);
            View = new CustomConfigPageView(this) { DataContext = this };
        }

        #endregion Public Constructors

        #region Public Methods

        public void Dispose()
        {
            Dispose(true);
            GC.SuppressFinalize(this);
        }

        public void Dispose(bool disposing)
        {
            if (!m_disposed)
            {
                if (disposing)
                {
                    m_logger.Dispose();
                }

                m_disposed = true;
            }
        }

        #endregion Public Methods

        #region Protected Methods

        protected override void Activate()
        {
            // This is called when the config page becomes visible.
            m_logger.TraceDebug("Showing config page");
            base.Activate();
        }

        protected override void Deactivate()
        {
            // This is called when the config page becomes hidden.
            m_logger.TraceDebug("Hiding config page");
            base.Deactivate();
        }

        protected override void Initialize()
        {
            // This is where you first setup the page.
            m_logger.TraceDebug("Setting up config page");
            ((CustomConfigPageView)View).Initialize(Workspace);
        }

        protected override void Refresh()
        {
            m_shouldBeAdministrator = m_user.IsAdministrator;
            OnPropertyChanged("ShouldBeAdministrator");
        }

        protected override void Save()
        {
            var adminUserGroup = Workspace.Sdk.GetEntity(SdkGuids.Administrators) as UserGroup;
            if (m_shouldBeAdministrator && !adminUserGroup.Children.ToList().Contains(m_user.Guid))
            {
                adminUserGroup.AddChild(m_user.Guid);
            }
            else if (!m_shouldBeAdministrator && adminUserGroup.Children.ToList().Contains(m_user.Guid))
                adminUserGroup.RemoveChild(m_user.Guid);
        }

        #endregion Protected Methods
    }

}