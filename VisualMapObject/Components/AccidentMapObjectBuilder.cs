using System;
using System.Collections.Generic;
using System.Linq;
using Genetec.Sdk.Entities.Maps;
using Genetec.Sdk.Workspace.Components;
using Genetec.Sdk.Workspace.Components.MapObjectViewBuilder;
using Genetec.Sdk.Workspace.Maps;
using VisualMapObject.Maps;

// ==========================================================================
// Copyright (C) 2016 by Genetec, Inc.
// All rights reserved.
// May be used only in accordance with a valid Source Code License Agreement.
// ==========================================================================
namespace VisualMapObject.Components
{
    /// <summary>
    /// This class represents the factory to create the accident map object. 
    /// </summary>
    public sealed class AccidentMapObjectBuilder : MapObjectViewBuilder
    {
        /// <summary>
        /// To avoid creating new Guid's each time the UniqueId.
        /// </summary>
        public static readonly Guid Identifier = new Guid("{2A7BBEBD-E911-4626-81E2-CC372136388E}");

        /// <summary>
        /// The name of the Component.
        /// </summary>
        public override string Name {get { return "Accident MapObjectView Builder"; }}

        /// <summary>
        /// Gets the priority of the component, lowest is better.
        /// Lower builder priority will be called before.
        /// </summary>
        public override int Priority { get { return 0; } }

        /// <summary>
        /// Gets the unique identifier of the component.
        /// </summary>
        public override Guid UniqueId { get { return Identifier;} }

        /// <summary>
        /// Create a view for the corresponding map objects.
        /// </summary>
        /// <param name="mapObjects">List of map objects in the current view(visible section of the current map).</param>
        /// <param name="context">Current map context</param>
        /// <returns>List of views representing the map objects</returns>
        public override IEnumerable<IMapObjectView> CreateViews(IEnumerable<MapObject> mapObjects, MapContext context)
        {
            //Here we simply gets all the the AccidentMapObject in the mapObjects collection and build it.
            if (mapObjects == null) return null;
            return mapObjects.OfType<AccidentMapObject>()
                             .Select(mapObj => new AccidentMapObjectView(mapObj))
                             .ToList();
        }
    }
}
