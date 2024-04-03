// ==========================================================================
// Copyright (C) 2020 by Genetec, Inc.
// All rights reserved.
// May be used only in accordance with a valid Source Code License Agreement.
// ==========================================================================

using Genetec.Sdk.Entities.Maps;
using Genetec.Sdk.Workspace.Components;
using Genetec.Sdk.Workspace.Components.MapObjectViewBuilder;
using Genetec.Sdk.Workspace.Maps;
using System;
using System.Collections.Generic;
using System.Linq;

namespace OfficerMapObject.MapObjects.Officers
{
    public class OfficerMapObjectBuilder : MapObjectViewBuilder
    {
        #region Public Properties

        public override string Name => "Custom OfficerMapObjectView Builder";

        /// <summary>
        /// Gets the priority of the component, lowest is better
        /// </summary>
        public override int Priority => 0;

        public override Guid UniqueId => new Guid("{21BA880B-6136-4F72-9E01-5102ED45724D}");

        #endregion Public Properties

        #region Public Methods

        /// <summary>
        /// Create views for the map objects
        /// </summary>
        /// <param name="mapObjects"></param>
        /// <param name="context"></param>
        /// <returns></returns>
        public override IEnumerable<IMapObjectView> CreateViews(IEnumerable<MapObject> mapObjects, MapContext context) 
            => mapObjects.OfType<OfficerMapObject>().Select(mapObject => new OfficerMapObjectView()).Cast<IMapObjectView>().ToList();
        
        #endregion Public Methods
    }
}