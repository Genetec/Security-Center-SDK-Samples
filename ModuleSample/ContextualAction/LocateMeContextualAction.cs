// ==========================================================================
// Copyright (C) 2019 by Genetec, Inc.
// All rights reserved.
// May be used only in accordance with a valid Source Code License Agreement.
// ==========================================================================

using Genetec.Sdk;
using Genetec.Sdk.Entities;
using Genetec.Sdk.Workspace.ContextualAction;
using Genetec.Sdk.Workspace.Services;
using System;
using System.Linq;
using System.Windows.Media.Imaging;

namespace ModuleSample.ContextualAction
{
    #region Classes

    public sealed class LocateMeContextualAction : Genetec.Sdk.Workspace.ContextualAction.ContextualAction
    {
        #region Properties

        /// <summary>
        /// Gets the action's unique identifier
        /// </summary>
        public override Guid Id => new Guid("{25A8518A-4B0D-45D2-B8A8-FC5F444004BA}");

        /// <summary>
        /// Gets the priority of the component, lowest is better
        /// </summary>
        public override int Priority => 0;

        #endregion Properties

        #region Constructors

        public LocateMeContextualAction()
        {
            Name = "Locate Entity";
            Icon = new BitmapImage(new Uri(@"pack://application:,,,/ModuleSample;Component/Resources/Map.png", UriKind.RelativeOrAbsolute));
        }

        #endregion Constructors

        #region Public Methods

        /// <summary>
        /// Return true if the contextual action can execute
        /// </summary>
        /// <param name="context">Current action's context</param>
        public override bool CanExecute(ContextualActionContext context)
        {
            var canExecute = context is EntityBrowserContextualActionContext && Workspace.ApplicationType == ApplicationType.SecurityDesk;
            if (!canExecute)
            {
                return false;
            }
            var entityBrowserContext = (EntityBrowserContextualActionContext)context;
            var selectedEntityGuid = entityBrowserContext.SelectedEntities.FirstOrDefault();
            var selectedEntity = Workspace.Sdk.GetEntity(selectedEntityGuid);
            var validEntity = selectedEntity != null && selectedEntity.LinkedMaps.Count > 0;
            return validEntity;
        }

        /// <summary>
        /// Executes the contextual action
        /// </summary>
        /// <param name="context">Current action's context</param>
        public override bool Execute(ContextualActionContext context)
        {
            var actionContext = context as EntityBrowserContextualActionContext;
            if (actionContext == null)
            {
                return false;
            }
            var entityBrowserContext = actionContext;
            var selectedEntityGuid = entityBrowserContext.SelectedEntities.FirstOrDefault();
            var selectedEntity = Workspace.Sdk.GetEntity(selectedEntityGuid);
            var mapGuid = selectedEntity.LinkedMaps.First();

            // This will show the map in a popup and in the monitoring task.
            // The two last boolean are use to choose how to display the map.
            var mapService = Workspace.Services.Get<IMapService>();
            mapService?.LocateEntityOnMap(mapGuid, selectedEntityGuid, true, true);

            return false;
        }

        #endregion Public Methods
    }

    #endregion Classes
}