using System;
using System.Collections.Generic;
using System.Linq;
using Genetec.Sdk.Entities.Maps;
using Genetec.Sdk.Workspace.Components;
using Genetec.Sdk.Workspace.Components.MapObjectViewBuilder;
using Genetec.Sdk.Workspace.Maps;

// ==========================================================================
// Copyright (C) 1989-2016 by Genetec Information Systems, Inc.
// All rights reserved.
// May be used only in accordance with a valid Source Code License Agreement.
// Developer: JD Trepanier
// ==========================================================================
namespace DronesTracker.Maps
{
    public sealed class DemoMapObjectViewBuilder : MapObjectViewBuilder
    {
        #region Properties

        public override string Name => "Demo MapObjectView Builder";

        public override int Priority => -10;

        public override Guid UniqueId => new Guid("{36F853DE-089D-4C8F-B8E2-561A3391E681}");

        #endregion

        #region Public Methods

        /// <summary>
        /// Create a view for the corresponding map object
        /// </summary>
        /// <param name="mapObjects">List of map objects to build</param>
        /// <param name="context">Current map context</param>
        /// <returns>List of views representing the map objects</returns>
        public override IEnumerable<IMapObjectView> CreateViews(IEnumerable<MapObject> mapObjects, MapContext context)
        {
            return mapObjects.OfType<DemoMapObject>().Select(mapObject => new DemoMapObjectView(mapObject)).Cast<IMapObjectView>().ToList();
        }

        #endregion
    }
}