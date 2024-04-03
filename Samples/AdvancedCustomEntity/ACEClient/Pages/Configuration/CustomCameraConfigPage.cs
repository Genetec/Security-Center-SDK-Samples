using ACECommon;
using Genetec.Sdk;
using Genetec.Sdk.Diagnostics.Logging.Core;
using Genetec.Sdk.Entities;
using Genetec.Sdk.Workspace.Pages;
using System;
using System.Collections.Generic;
using System.Collections.ObjectModel;
using System.ComponentModel;
using System.Data;
using System.Linq;
using System.Runtime.CompilerServices;
using Genetec.Sdk.Queries;

// ==========================================================================
// Copyright (C) 2017 by Genetec, Inc.
// All rights reserved.
// May be used only in accordance with a valid Source Code License Agreement.
//
// Ephemerides for September 25:
//  1915 – World War I: The Second Battle of Champagne begins.
//  1955 – The Royal Jordanian Air Force is founded.
//  1981 – Belize joins the United Nations.
// ==========================================================================
namespace ACEClient.Pages.Configuration
{
    #region Classes

    public class CustomCameraConfigPage : ConfigPage, IDisposable
    {
        private ChildEntityModel m_selectedChildModel;

        public ObservableCollection<ChildEntityModel> AvailableChildren { get; set; }

        private readonly Logger m_logger;
        private Guid m_customEntityGuid;
        private bool m_disposed;

        #region Properties

        // Entity for which the config page is shown
        protected override Guid Entity
        {
            set
            {
                m_customEntityGuid = value;
                
                // Set the config page to visible only if the entity is an instance of the "Custom Camera" CustomEntityTypeDescriptor
                CustomEntity customEntity = Workspace.Sdk.GetEntity(m_customEntityGuid) as CustomEntity;
                IsVisible = (customEntity != null) && customEntity.CustomEntityType.Equals(CustomCamera.TypeGuid);
            }
        }

        /// <summary>
        /// Return the type for which this config page is displayed
        /// </summary>
        protected override EntityType EntityType
        {
            get
            {
                return EntityType.CustomEntity;
            }
        }

        protected override string Name
        {
            get { return "Custom Camera Configs"; }
        }

        #endregion

        #region Constructors

        public CustomCameraConfigPage()
        {
            m_logger = Logger.CreateInstanceLogger(this);
            View = new CustomCameraConfigPageView(this) { DataContext = this };
        }

        #endregion

        #region Destructors and Dispose Methods

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

        #endregion

        #region Event Handlers

        #endregion

        #region Protected Methods

        protected override void Activate()
        {
            // This is called when the config page becomes visible.
            m_logger.TraceDebug("Showing CustomCamera config page");
            base.Activate();
        }

        protected override void Deactivate()
        {
            // This is called when the config page becomes hidden.
            m_logger.TraceDebug("Hiding CustomCamera config page");
            base.Deactivate();
        }

        protected override void Initialize()
        {
            // This is where you first setup the page.
            m_logger.TraceDebug("Setting up CustomCamera config page");
            ((CustomCameraConfigPageView)View).Initialize(Workspace);

            AvailableChildren = new ObservableCollection<ChildEntityModel>();
        }

        /// <summary>
        /// Refresh SelectedChild by scanning the custom entity's hierarchical children.
        /// </summary>
        protected override void Refresh()
        {
            var thisCustomCamera = Workspace.Sdk.GetEntity(m_customEntityGuid) as CustomEntity;
            if (thisCustomCamera == null)
                return;

            if (thisCustomCamera.HierarchicalChildren == null || thisCustomCamera.HierarchicalChildren.Count < 1)
                return;

            // If there is more than one child, the first will be considered the SelectedChild
            Entity firstChild = Workspace.Sdk.GetEntity(thisCustomCamera.HierarchicalChildren[0]);

            if (firstChild != null)
                m_selectedChildModel = AvailableChildren.FirstOrDefault(child => child.ModelGuid == firstChild.Guid);
            else
                m_selectedChildModel = null;
            OnPropertyChanged("SelectedChild");
        }

        protected override void Save()
        {
            var thisCustomCamera = Workspace.Sdk.GetEntity(m_customEntityGuid) as CustomEntity;
            if (thisCustomCamera == null)
                return;

            // Only allow this Custom Camera entity to have one child. Remove existing children
            foreach (Guid child in thisCustomCamera.HierarchicalChildren)
                thisCustomCamera.RemoveHierarchicalChild(child);

            if(SelectedChild != null && Workspace.Sdk.GetEntity(SelectedChild.ModelGuid) != null )
                thisCustomCamera.AddHierarchicalChild(SelectedChild.ModelGuid);
        }

        #endregion

        public ChildEntityModel SelectedChild
        {
            get
            {
                return m_selectedChildModel;
            }
            set
            {
                if (value == null || m_selectedChildModel != null && m_selectedChildModel.ModelGuid == value.ModelGuid)
                    return;
                m_selectedChildModel = value;
                IsDirty = true;
                OnPropertyChanged();
            }
        }

        public void RefreshAvailableChildren()
        {
            Dispatcher.BeginInvoke(new Action(() => AvailableChildren.Clear()));

            // Query the database for available cameras, use Workspace.Sdk to get the currently connected SDK Engine
            var query = (EntityConfigurationQuery)Workspace.Sdk.ReportManager.CreateReportQuery(ReportType.EntityConfiguration);

            // Set filters to EntityTypes that are supported by the the "CustomCamera" CustomEntityTypeDescriptor
            CustomEntity customEntity = Workspace.Sdk.GetEntity(m_customEntityGuid) as CustomEntity;
            foreach(EntityType supportedType in customEntity.TypeDescriptor.HierarchicalChildTypes)
                query.EntityTypeFilter.Add(supportedType);

            // Perform the query
            QueryCompletedEventArgs results = query.Query();

            if (results.Data != null)
            {
                List<Guid> supportedEntities = results.Data.Rows.Cast<DataRow>().Select(row => (Guid)row[0]).ToList();

                // Add entities found by the SDK engine to the AvailableChildren list
                foreach (Guid guid in supportedEntities)
                {
                    var childEntity = Workspace.Sdk.GetEntity(guid);
                    if (childEntity == null) continue;

                    Dispatcher.BeginInvoke(new Action(() =>
                    {
                        AvailableChildren.Add(new ChildEntityModel
                        {
                            ModelGuid = childEntity.Guid,
                            ModelName = childEntity.Name,
                            ModelIcon = childEntity.GetIcon(true)
                        });
                    }));
                }
            }


            Dispatcher.BeginInvoke(new Action(() =>
            {
                // Add a None option
                AvailableChildren.Add(new ChildEntityModel
                {
                    ModelGuid = Guid.Empty,
                    ModelName = "None",
                    ModelIcon = null
                });
            }));
        }
    }

    #endregion
}

