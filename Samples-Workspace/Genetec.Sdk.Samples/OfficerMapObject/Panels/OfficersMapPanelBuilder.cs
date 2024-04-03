// ==========================================================================
// Copyright (C) 2020 by Genetec, Inc.
// All rights reserved.
// May be used only in accordance with a valid Source Code License Agreement.
// ==========================================================================

using Genetec.Sdk.Entities;
using Genetec.Sdk.Workspace.Components.MapPanel;
using System;

namespace OfficerMapObject.Panels
{
    public class OfficersMapPanelBuilder : MapPanelBuilder
    {
        #region Public Properties

        public override string Name => "Incidents map panel builder";

        public override int Priority => 0;
        public override Guid UniqueId => new Guid("{4A82E72F-B48C-4A33-AF4D-EDBEE92AB658}");

        #endregion Public Properties

        #region Public Methods

        public override MapPanel CreateView() 
            => new OfficersMapPanel();
        
        public override bool IsSupported(MapPanelContext context)
        {
            Map map = null;

            var entity = Workspace.Sdk.GetEntity(context.MapId);
            if (entity != null)
            {
                if (entity.EntityType == Genetec.Sdk.EntityType.Area)
                {
                    var area = entity as Area;
                    if (area != null)
                    {
                        map = Workspace.Sdk.GetEntity(area.MapId) as Map;
                        return map != null && map.IsGeoReferenced;
                    }
                }
                map = entity as Map;
            }

            return map != null && map.IsGeoReferenced;
        }

        #endregion Public Methods
    }
}